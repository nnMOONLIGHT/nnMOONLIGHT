# FastAPI wrapper that exposes the trained MLP via /predict
from fastapi import FastAPI, File, UploadFile, HTTPException
from pydantic import BaseModel
from typing import Dict, List
import json
import io
import os
from PIL import Image
import numpy as np

from model import MLP
from features import extract_features
from styles import STYLES, FEATURE_NAMES

app = FastAPI(title="StyleAI Python ML Service")

MODEL_DIR = os.path.dirname(__file__)
WEB_MODEL_PATH = os.path.join(MODEL_DIR, "web_model.json")
WEIGHTS_PATH = os.path.join(MODEL_DIR, "weights.json")

class PredictionResponse(BaseModel):
    style_id: str
    style_name: str
    confidence: float
    features: List[float]
    all_probs: Dict[str, float]


def load_web_model():
    """Load web_model.json (preferred) or weights.json and return a callable predictor.
    Returns (predict_fn, mu, sigma, styles_list)
    predict_fn takes normalized feature numpy array shaped (n, NUM_FEATURES) and returns probs (n, C)
    """
    if os.path.exists(WEB_MODEL_PATH):
        with open(WEB_MODEL_PATH, "r", encoding="utf-8") as f:
            wm = json.load(f)
        meta = wm.get("meta", {})
        arch = meta.get("architecture")
        if not arch:
            raise RuntimeError("web_model.json missing architecture meta")
        mu = np.array(wm["norm"]["mu"]) if "norm" in wm else np.zeros(len(FEATURE_NAMES))
        sigma = np.array(wm["norm"]["sigma"]) if "norm" in wm else np.ones(len(FEATURE_NAMES))
        weights = wm.get("weights")
        if weights is None:
            raise RuntimeError("web_model.json missing weights")
        n_in, h1, h2, n_out = arch
        net = MLP.from_json(weights, n_in, h1, h2, n_out)
        styles = wm.get("styles", STYLES)
        def predict_fn(Xn: np.ndarray):
            return net.predict_proba(Xn)
        return predict_fn, mu, sigma, styles
    elif os.path.exists(WEIGHTS_PATH):
        with open(WEIGHTS_PATH, "r", encoding="utf-8") as f:
            weights = json.load(f)
        # assume architecture is the repository default
        from styles import NUM_FEATURES as NF, NUM_CLASSES as NC
        net = MLP.from_json(weights, NF, 32, 16, NC)
        mu = np.zeros(NF)
        sigma = np.ones(NF)
        def predict_fn(Xn: np.ndarray):
            return net.predict_proba(Xn)
        return predict_fn, mu, sigma, STYLES
    else:
        return None, None, None, STYLES

_predict_fn, _mu, _sigma, _styles = load_web_model()


@app.get("/health")
async def health():
    return {"status": "ok", "model_loaded": _predict_fn is not None}


@app.post("/predict", response_model=PredictionResponse)
async def predict(file: UploadFile = File(...)):
    if _predict_fn is None:
        raise HTTPException(status_code=500, detail="Model not available on server. Run train.py to produce web_model.json")
    content = await file.read()
    try:
        im = Image.open(io.BytesIO(content)).convert("RGB")
    except Exception:
        raise HTTPException(status_code=400, detail="Invalid image file")

    # extract features using the same code as training
    feats = extract_features(im)
    # normalise
    Xn = (feats - _mu) / _sigma
    Xn = Xn.reshape(1, -1)

    probs = _predict_fn(Xn)[0]
    idx = int(np.argmax(probs))
    style = _styles[idx] if idx < len(_styles) else {"id": str(idx), "name": "unknown"}

    all_probs = {s["id"]: float(probs[i]) for i, s in enumerate(_styles)}

    return PredictionResponse(
        style_id=style.get("id", str(idx)),
        style_name=style.get("name", ""),
        confidence=float(probs[idx]),
        features=[float(x) for x in feats.tolist()],
        all_probs=all_probs,
    )
