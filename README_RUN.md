# Simple instructions for running locally and via docker

## Run with Docker Compose

docker compose up --build

- ML service will be at http://localhost:8000
- API will be at http://localhost:5000

Default seeded manager credentials:
- email: admin@local
- password: password

## Run Python service locally

cd python_ml
python3 -m venv .venv
source .venv/bin/activate
pip install -r requirements.txt
uvicorn app:app --host 0.0.0.0 --port 8000

## Run .NET API locally

cd StyleAI.Api
# ensure .NET 8 SDK installed
dotnet run

