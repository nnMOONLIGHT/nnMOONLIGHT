server:
uvicorn app:app --host 0.0.0.0 --port 8000

JSON:
{
  "style_id": "street",
  "style_name": "Street",
  "confidence": 0.92,
  "features": [...],
  "all_probs": {"minimal":0.01,...}
}
