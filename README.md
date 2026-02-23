# Scan Event Worker Service (.NET)

## Overview

This project is a .NET Worker Service that continuously consumes a Scan Event API, processes parcel scan events, and persists the latest parcel state along with pickup and delivery timestamps.

The service is designed to be fault-tolerant, restart-safe, and production-ready, using batch processing, checkpoint persistence, structured logging, and clean layered architecture.


## Why .NET Worker Service?

This application uses .NET Worker Service because it is optimized for:
- Long-running background tasks
- Continuous polling workloads
- High-throughput processing
- Clean lifecycle management
- Graceful startup and shutdown

Worker Services are ideal for event ingestion pipelines, schedulers, and background processors.
---

## Key Features

- Continuous background processing using .NET Worker Service
- Batch-based API consumption for scalability and stability
- Persistent checkpointing for crash recovery
- Fault tolerant and idempotent processing
- Structured logging using Serilog
- Database schema versioning using EF Core migrations
- Clean layered architecture

---

## High-Level Architecture
Scan Event API -> .NET Worker Service -> SQL Server

## API Communication Strategy

The worker communicates with the Scan Event API using HttpClientFactory and typed clients.

Key benefits:
- Connection pooling
- DNS refresh handling
- Testability
- Resilience


### Processing Flow
Load last processed EventId from database

Fetch new events from API using FromEventId + Limit

Process each scan event

Persist latest parcel state

Save checkpoint

Repeat continuously


---

## Technical Architecture
Worker
   |
ScanApiClient
   |
ScanProcessor
    |
Entity Framework Core (DbContext)
    |
SQL Server


### Layer Responsibilities

| Layer       | Responsibility |
|----------|----------------|
Worker        | Orchestrates execution and scheduling 
ScanApiClient | Handles HTTP communication with API 
ScanProcessor | Contains business logic
DbContext     | Database persistence
Entities      | Domain data models 

---

## Database Design

### WorkerStates

Stores processing checkpoint for restart safety.

| Column             | Purpose |
|----------|-----------|
Id                   | Primary key 
LastProcessedEventId | Last successfully processed event 

### ParcelStates

Stores latest parcel state and lifecycle timestamps.

| Column          | Purpose |
|------------|----------|
ParcelId           | Primary key 
LastEventId        | Latest event ID 
Type               | Event type 
StatusCode         | Status 
RunId              | Run identifier 
CreatedDateTimeUtc | Event timestamp 
PickupTimeUtc      | Pickup time 
DeliveryTimeUtc    | Delivery time 

---

## Dependencies Installed

| Package                                | Purpose |
|------------|----------||------------|----------|
Microsoft.EntityFrameworkCore            | ORM 
Microsoft.EntityFrameworkCore.SqlServer  | SQL Server provider
Microsoft.EntityFrameworkCore.Design     | EF Core migrations 
Serilog.Extensions.Hosting               | Structured logging 
Serilog.Sinks.Console                    | Console logging output 

---

## Configuration

### appsettings.json 

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=ScanWorkerDb;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "ScanApi": {
    "BaseUrl": "http://localhost/v1/scans/scanevents"
  }
}

## Setup Instructions

1. Install Dependencies
dotnet restore

2. Create Database using EF Core or Run init-db.sql in ScanWorkerDb Database in Sql Server.
dotnet ef database update

3. Run Worker Service
dotnet run

## Fault Tolerance & Resilience Strategy

    - Persistent checkpointing using WorkerStates table
    - Batch-based event processing
    - Idempotent event handling
    - Structured error logging
    - Graceful shutdown support

## Assumptions

  - Events are immutable
  - EventId is strictly increasing
  - API supports pagination using FromEventId + Limit
  - Moderate Workload
  - At-least-once processing is acceptable

## Possible Improvements

  - Replace polling with event-driven architecture (Azure Service Bus)
  - Horizontal scaling using container orchestration
  - Health checks and metrics (Prometheus / OpenTelemetry)
  - Distributed locking
  - Dead-letter queues
  - API retry policies using Polly

##Scalable Architecture (Future Design)
Scan API -> Ingestion Worker -> Message Queue -> Multiple Workers

### Allows:

   - Horizontal scaling
   - Multiple consumers
   - Real-time processing
   - Fault isolation

### Why This Design?

   - Reliable event processing
   - Production-grade fault tolerance
   - Clean separation of concerns
   - Easy scalability
   - Easy observability