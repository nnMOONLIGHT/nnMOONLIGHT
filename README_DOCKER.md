# README — how to run the project with Docker Compose

This repository contains the StyleAI backend (ASP.NET Core) and a Python ML microservice (FastAPI). To run both services locally in containers, use Docker Compose.

Prerequisites
- Docker & Docker Compose installed
- Ports 5000 (API) and 8000 (ML service) free

Quick start

1) Build and run services:

   docker compose up --build

2) Check ML service health:

   curl http://localhost:8000/health

   Expected: {"status":"ok","model_loaded":true}

3) Call the API ML proxy (the API will forward to the python service):

   curl -F "file=@/path/to/photo.jpg" http://localhost:5000/api/ml/predict

Notes
- The Python service uses the included web_model.json / weights.json, so no training is necessary.
- The .NET API uses an on-disk SQLite database (styleai.db) created in the API container. For quick local testing the Program.cs will attempt migrations and fall back to EnsureCreated().

If you prefer to run services locally without Docker, see the project README and the python_ml/README.md for Python-specific instructions.
