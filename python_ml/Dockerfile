# для мл
FROM python:3.11-slim

WORKDIR /app

# загрузка системного
RUN apt-get update && apt-get install -y --no-install-recommends \
    build-essential \
    && rm -rf /var/lib/apt/lists/*

# копирка
COPY ./python_ml /app

# загрузка пипов
RUN python -m pip install --upgrade pip \
    && pip install --no-cache-dir -r requirements.txt

EXPOSE 8000

CMD ["uvicorn", "app:app", "--host", "0.0.0.0", "--port", "8000"]
