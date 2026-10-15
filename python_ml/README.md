# Small README for the python ML microservice

This folder contains a lightweight FastAPI wrapper for the MLP model used by StyleAI.

How to run (virtualenv recommended):

pip install -r python_ml/requirements.txt
cd python_ml
# optionally train to produce weights/web_model.json:
python3 train.py
# then start the server:
uvicorn app:app --host 0.0.0.0 --port 8000

The endpoint /predict accepts a multipart/form-data POST with field `file` and returns JSON:
{
  "style_id": "street",
  "style_name": "Street",
  "confidence": 0.92,
  "features": [...],
  "all_probs": {"minimal":0.01,...}
}
