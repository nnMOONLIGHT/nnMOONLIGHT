# -*- coding: utf-8 -*-
"""
Style metadata and feature names used by the training / inference code.
This is a compact reconstruction so the Python ML service can run standalone
inside the repository.
"""

STYLES = [
    {"id": "minimal", "name": "Minimal", "summary": "Clean, simple palettes" , "palette": ["#f5f3fb","#ffffff","#eceaf7","#7a6df0"], "clothing": ["clean lines","monochrome"], "hobbies": ["design","reading"], "directions": ["reduce clutter"]},
    {"id": "street", "name": "Street", "summary": "Bold accents, urban textures" , "palette": ["#12142c","#3a8bef","#e0567c","#dc9a2e"], "clothing": ["hoodies","sneakers"], "hobbies": ["skateboarding","photography"], "directions": ["contrast","layers"]},
    {"id": "retro", "name": "Retro", "summary": "Warm, vintage tones" , "palette": ["#dc9a2e","#4a3527","#f0e0c0"], "clothing": ["vintage jackets","high-waist"], "hobbies": ["vinyl","film photography"], "directions": ["warm tones"]},
    {"id": "romantic", "name": "Romantic", "summary": "Soft pastels and blur" , "palette": ["#fce6ec","#efebfe","#e2dcfc"], "clothing": ["flowy dresses","lace"], "hobbies": ["flowers","poetry"], "directions": ["soft textures"]},
    {"id": "grunge", "name": "Grunge", "summary": "Rough textures, dark base" , "palette": ["#0c0d20","#8f83ff","#9598b8"], "clothing": ["distressed denim","boots"], "hobbies": ["music","DIY"], "directions": ["textures","distress"]},
    {"id": "avant", "name": "Avant‑garde", "summary": "Experimental shapes and colours" , "palette": ["#7a6df0","#26a677","#e0567c"], "clothing": ["asymmetric cuts","statement pieces"], "hobbies": ["art","installation"], "directions": ["bold shapes"]},
    {"id": "classic", "name": "Classic", "summary": "Timeless neutrals and structure" , "palette": ["#e7e5f3","#14162c","#63678a"], "clothing": ["tailoring","clean silhouettes"], "hobbies": ["culture","reading"], "directions": ["timeless pieces"]},
    {"id": "popart", "name": "Pop‑art", "summary": "Bright blocks and playful motifs" , "palette": ["#e0567c","#3a8bef","#f6c667"], "clothing": ["graphic prints","bright accessories"], "hobbies": ["collecting","crafts"], "directions": ["colour blocks"]},
]

NUM_CLASSES = len(STYLES)

# Feature names must match the order produced by features.extract_features()
FEATURE_NAMES = [
    "r_mean", "g_mean", "b_mean",
    "h_mean", "s_mean", "v_mean",
    "brightness_std", "saturation_std",
    "warm_cool", "colorfulness",
    "edge_density", "edge_std",
    "dark_ratio", "light_ratio",
    "symmetry",
]

NUM_FEATURES = len(FEATURE_NAMES)
