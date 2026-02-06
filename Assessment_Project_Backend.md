# Backend Developer Assessment – Candidate Instructions

## Overview

This assessment consists of **two parts** designed to evaluate your backend engineering skills and your understanding of analytical data systems and MPP databases.

* **Total Time Limit:** ~2 hours
* **Submission Format:** ZIP file
* **Languages/Tools:** .NET (C#), WebAPI, Docker
* **Style:** Text-based instructions and deliverables

You are not expected to perfectly finish everything. We value **clarity, reasoning, and trade-off awareness** over completeness.

---

## What to Submit

Please return a single ZIP file named:

```
<FirstName>_<LastName>_BackendAssessment.zip
```

ZIP contents:

```
/part1-api/
  ├── src/                     # .NET WebAPI project
  ├── Dockerfile
  ├── README.md                 # How to run the API
  └── k8s.yaml (optional)

/part2-starrocks/
  └── starrocks-design.md
```

---

# Part 1 – Backend API Assessment

**Estimated Time:** 75–90 minutes

### Objective

Build a simple **.NET WebAPI** that exposes analytical-style endpoints over a mocked dataset. This simulates an API layer sitting in front of an analytical database such as StarRocks.

---

## Requirements

### 1. Mock Dataset

* Generate or mock **at least 1,000 records** in memory or via seed logic
* Each record should contain:

  * `id`
  * `client_id`
  * `fund_id` (nullable)
  * `as_of_date`
  * `metric_name`
  * `metric_value` (numeric)

No real database is required. In-memory collections are acceptable.

---

### 2. API Endpoints

Implement endpoints that support:

* Filtering by:

  * `client_id` (required)
  * `fund_id` (optional)
  * Date range (`start_date`, `end_date`)
* Aggregations:

  * SUM and COUNT on `metric_value`
* Pagination:

  * `limit` and `offset`

Example endpoint shape (illustrative only):

```
GET /metrics?client_id=123&fund_id=456&start_date=2024-01-01&end_date=2024-03-31&limit=100&offset=0
```

---

### 3. API Design Expectations

We are evaluating:

* RESTful design
* Sensible defaults and validation
* Safe query patterns (avoid unbounded queries)
* Clear request/response models

Authentication is **not required**.

---

### 4. Docker

Provide a valid `Dockerfile` that:

* Builds the API
* Runs it on a configurable port

The API should be runnable via:

```
docker build .
docker run ...
```

---

### 5. (Bonus) Kubernetes

Optionally include a `k8s.yaml` manifest that deploys the API to a Kubernetes cluster.

This is a bonus and **not required** for a complete submission.

---

# Part 2 – StarRocks / MPP Design Challenge

**Estimated Time:** 30–40 minutes

### Objective

This design exercise evaluates your understanding of **MPP analytical databases**, **data lakehouse ingestion**, and **query performance at scale**.

You are not expected to have hands-on StarRocks experience. We are evaluating **how you think**, not product-specific syntax.

Submit your answers as:

```
part2-starrocks/starrocks-design.md
```

---

## Scenario

You are designing a data pipeline and query layer for a lakehouse-backed analytics platform.

* Source systems produce **daily data files**
* Data is landed in **S3-compatible object storage**
* Data is queried using an **MPP analytical database** such as StarRocks
* APIs sit on top of the analytical database to serve downstream applications

### Dataset Characteristics

* ~500 million rows per year
* Append-only (no updates or deletes)
* Columns:

  * `client_id`
  * `fund_id` (nullable)
  * `as_of_date`
  * `metric_name`
  * `metric_value`

### Query Characteristics

* Almost all queries filter by `client_id`
* Many queries also filter by `fund_id`
* Almost all queries filter by a date range
* Common queries aggregate data (SUM, COUNT, AVG)

---

## Questions

### 1. Object Storage Layout

How would you organize this data in object storage?

Consider:

* Directory / prefix structure
* Partitioning strategy
* File formats
* Handling late or reprocessed data

---

### 2. MPP Table Design

How would you design the analytical table(s)?

Consider:

* Partition keys
* Distribution / bucketing strategy
* Sort or clustering choices
* Duplicate vs primary key-style designs

---

### 3. API & Query Design

How should APIs be designed to work efficiently with an MPP system?

Consider:

* Safe vs unsafe query patterns
* Pagination strategies
* Avoiding full table scans
* Handling large aggregations

---

### 4. Failure Modes & Operations

What failure modes do you expect, and how would you mitigate them?

Examples:

* Data skew
* Partition explosion
* Small file problems
* Query latency regression
* Cost growth

---

### 5. Scale Thought Experiment

Assume data volume grows **10× over two years**.

* What breaks first?
* What would you change?

---

## Evaluation Criteria

Across both parts, we are evaluating:

* Backend API fundamentals
* Data modeling instincts
* MPP vs OLTP understanding
* Trade-off awareness
* Clarity of communication

There is no single correct answer. Explain your reasoning.

---

## Final Notes

* Focus on clarity over perfection
* State assumptions explicitly
* Partial solutions are acceptable
* This assessment is designed to be completed within **~2 hours**
