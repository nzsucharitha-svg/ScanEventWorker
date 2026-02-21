CREATE TABLE WorkerStates (
    Id INT PRIMARY KEY,
    LastProcessedEventId BIGINT NOT NULL
);

INSERT INTO WorkerStates VALUES (1,0);

CREATE TABLE ParcelStates (
    ParcelId BIGINT PRIMARY KEY,
    LastEventId BIGINT NOT NULL,
    Type NVARCHAR(20),
    StatusCode NVARCHAR(20),
    RunId NVARCHAR(50),
    CreatedDateTimeUtc DATETIME2,
    PickupTimeUtc DATETIME2 NULL,
    DeliveryTimeUtc DATETIME2 NULL
);