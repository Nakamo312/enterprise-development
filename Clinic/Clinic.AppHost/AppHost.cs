var builder = DistributedApplication.CreateBuilder(args);

var password = builder.AddParameter("DatabasePassword");
var dbName = "clinic";

var clinicDb = builder
    .AddPostgres("clinic-db", password: password)
    .AddDatabase(dbName);

var rabbitMq = builder.AddRabbitMQ("rabbitmq")
    .WithManagementPlugin();

builder.AddProject<Projects.Clinic_Api>("clinic-api")
    .WithReference(clinicDb, "Database")
    .WaitFor(clinicDb);

builder.AddProject<Projects.Clinic_RabbitMq_Consumer>("clinic-consumer")
    .WithReference(rabbitMq)
    .WithReference(clinicDb, "Database")
    .WaitFor(rabbitMq)
    .WaitFor(clinicDb);

builder.AddProject<Projects.Clinic_RabbitMq_Producer>("clinic-producer")
    .WithReference(rabbitMq)
    .WaitFor(rabbitMq);

builder.Build().Run();