# Metrics API

A RESTful API for querying and aggregating metric data, built with ASP.NET Core 10.0.

## Building the Docker Image

```bash
docker build -t metrics-api -f Dockerfile .
```

## Running the Docker Container

### Default Port (8080)
```bash
docker run -p 8080:8080 metrics-api
```

### Custom Port
```bash
docker run -p 9000:9000 -e METRICS_PORT=9000 metrics-api
```

The API will be available at `http://localhost:8080` (or your specified port).

## API Endpoints

### GET /metrics
Retrieve a list of metrics with optional filtering, pagination, and aggregation.

### GET /metrics/{id}
Retrieve metrics by ID with optional filtering, pagination, and aggregation.

## Parameters and Filters

### Route Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `id` | integer | No | Filter by specific metric ID |

### Query Parameters

| Parameter | Type | Required | Default | Validation | Description |
|-----------|------|----------|---------|------------|-------------|
| `client_id` | integer | **Yes** | - | - | Filter by client ID |
| `fund_id` | integer | No | - | - | Filter by fund ID |
| `start_date` | DateTime | No | - | Must be ≤ end_date | Filter by start date (inclusive) |
| `end_date` | DateTime | No | - | Must be ≥ start_date | Filter by end date (inclusive) |
| `aggregation` | string | No | - | "SUM" or "COUNT" | Aggregation type |
| `limit` | integer | No | 100 | 1-1000 | Maximum number of results to return |
| `offset` | integer | No | 0 | ≥ 0 | Number of results to skip (for pagination) |

## Example API Calls

### Get metrics for a specific client
```bash
curl "http://localhost:8080/metrics?client_id=105"
```

### Get metrics with filtering
```bash
curl "http://localhost:8080/metrics?client_id=105&fund_id=3&start_date=2024-01-01&end_date=2024-12-31"
```

### Get metrics with pagination
```bash
curl "http://localhost:8080/metrics?client_id=105&limit=50&offset=0"
```

### Get aggregated count
```bash
curl "http://localhost:8080/metrics?client_id=105&aggregation=COUNT"
```

### Get aggregated sum
```bash
curl "http://localhost:8080/metrics?client_id=105&aggregation=SUM"
```

### Get specific metric by ID
```bash
curl "http://localhost:8080/metrics/123?client_id=105"
```

## Response Format

### Standard Response (Non-Aggregated)
```json
{
  "isAggregation": false,
  "aggregationType": null,
  "aggregationValue": null,
  "metrics": [
    {
      "id": 1,
      "clientId": 105,
      "fundId": 3,
      "asOfDate": "2024-06-15T00:00:00",
      "metricName": "marketValue",
      "metricValue": 250.75
    }
  ]
}
```

### Aggregation Response
```json
{
  "isAggregation": true,
  "aggregationType": "SUM",
  "aggregationValue": 12500.50,
  "metrics": null
}
```

## Error Responses

### 400 Bad Request
Returned when validation fails (e.g., invalid parameter values, missing required parameters).

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "Limit": ["Limit must be between 1 and 1000"]
  }
}
```

### 404 Not Found
Returned when no metrics match the query filters.

## Data Seeding

The application automatically seeds the in-memory database with 1000 sample metric records on startup:
- **IDs**: Sequential (1-1000)
- **Client IDs**: Random (100-110)
- **Fund IDs**: Random (0-5)
- **Metric Names**: "marketValue" or "marketNotion"
- **Metric Values**: Random decimals (0-1000) with 2 decimal places
- **As Of Dates**: Random dates within the past year

## Technology Stack

- ASP.NET Core 10.0
- Entity Framework Core (In-Memory Database)
- Docker
