# Flora Management System

A distributed web application for greenhouse plant management built on microservice architecture using RabbitMQ message broker and PostgreSQL database. The system demonstrates patterns of building distributed systems with asynchronous message processing, where read operations are executed directly from the database for speed, and write operations are processed asynchronously through a message broker for scalability and reliability.

## System Components

### REST API
- Accepts HTTP requests from clients
- Validates input data
- Sends data modification commands through RabbitMQ
- Executes read requests directly from the database
- Swagger UI for documentation and testing

### Background Worker
- Listens to RabbitMQ queues
- Processes messages about creating/updating/deleting plants
- Stores changes in PostgreSQL database
- Ensures asynchronous command processing

### PostgreSQL Database
- Stores plant data
- Accessed through Entity Framework Core
- Automatic database schema migrations

### RabbitMQ Message Broker
- Direct exchange for message routing
- Separate queues for each operation type (create, update, delete)
- Ensures reliable message delivery

## Quick Start

### Running with Docker Compose (recommended)

```bash
# Clone repository
git clone <repository-url>
cd flora-management-system

# Start entire system (PostgreSQL, RabbitMQ, API, Worker)
docker-compose -f docker/docker-compose.yml up --build

# Check status
docker-compose -f docker/docker-compose.yml ps
```

**Note:** Database migrations are applied automatically on startup - no manual intervention required.

After starting:
- **API**: http://localhost:5000
- **Swagger UI**: http://localhost:5000/swagger
- **RabbitMQ Management UI**: http://localhost:15672 (login: `guest`, password: `guest`)

### Running Locally (without Docker)

#### Prerequisites
- .NET 8.0 SDK
- PostgreSQL 16
- RabbitMQ 3

#### Step 1: Start dependencies

```bash
# PostgreSQL
docker run -d --name flora-postgres \
  -e POSTGRES_DB=floradb \
  -e POSTGRES_USER=postgres \
  -e POSTGRES_PASSWORD=postgres \
  -p 5432:5432 \
  postgres:16

# RabbitMQ
docker run -d --name flora-rabbitmq \
  -p 5672:5672 \
  -p 15672:15672 \
  rabbitmq:3-management
```

#### Step 2: Database migrations

**Note:** Migrations are already created in the project. When you run the API or Worker, migrations will be applied automatically.

If you need to create new migrations after modifying entities:

```bash
# Install EF Core tools (first time only)
dotnet tool install --global dotnet-ef

# Create new migration
dotnet ef migrations add MigrationName \
  -p src/FloraManagement.Persistence \
  -s src/FloraManagement.API

# Migrations will be applied automatically on next startup
# Or apply manually:
dotnet ef database update \
  -p src/FloraManagement.Persistence \
  -s src/FloraManagement.API
```

#### Step 3: Start API and Worker

```bash
# Terminal 1: API
dotnet run --project src/FloraManagement.API

# Terminal 2: Worker
dotnet run --project src/FloraManagement.Worker
```

### Stopping the System

```bash
# Stop all containers
docker-compose -f docker/docker-compose.yml down

# Stop and remove volumes (database will be cleared)
docker-compose -f docker/docker-compose.yml down -v
```

### Viewing Logs

```bash
# Logs of all services
docker-compose -f docker/docker-compose.yml logs

# Logs of specific service
docker-compose -f docker/docker-compose.yml logs api
docker-compose -f docker/docker-compose.yml logs worker
docker logs flora-postgres
docker logs flora-rabbitmq

# Follow logs in real-time
docker-compose -f docker/docker-compose.yml logs -f
```

## API Usage Examples

### Create a new plant

```bash
curl -X POST http://localhost:5000/api/flowers \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Rose",
    "soil": 2,
    "origin": "Europe",
    "visualParameters": {
      "stemColor": "Green",
      "leafColor": "Dark Green",
      "averageSize": 50.5
    },
    "growingTips": {
      "temperatureCelsius": 20,
      "isPhotophilous": true,
      "wateringPerWeek": 500
    },
    "multiplying": 2
  }'
```

**Response:**
```json
{
  "message": "Plant creation request sent to processing queue"
}
```

### Get all plants

```bash
curl http://localhost:5000/api/flowers
```

### Get plant by ID

```bash
curl http://localhost:5000/api/flowers/{id}
```

### Update plant

```bash
curl -X PUT http://localhost:5000/api/flowers/{id} \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Red Rose",
    "soil": 2,
    "origin": "Europe",
    "visualParameters": {
      "stemColor": "Red",
      "leafColor": "Dark Green",
      "averageSize": 55.0
    },
    "growingTips": {
      "temperatureCelsius": 22,
      "isPhotophilous": true,
      "wateringPerWeek": 600
    },
    "multiplying": 2
  }'
```

### Delete plant

```bash
curl -X DELETE http://localhost:5000/api/flowers/{id}
```

## RabbitMQ Configuration

- **Exchange**: `flowers.exchange` (type: direct)
- **Queues**:
  - `flowers.create.queue` → routing key: `flower.create`
  - `flowers.update.queue` → routing key: `flower.update`
  - `flowers.delete.queue` → routing key: `flower.delete`
