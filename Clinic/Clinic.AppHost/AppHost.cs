var builder = DistributedApplication.CreateBuilder(args);

var password = builder.AddParameter("DatabasePassword");
var dbName = "clinic";

var clinicDb = builder
    .AddPostgres("clinic-db", password: password) 
    .AddDatabase(dbName);

builder.AddProject<Projects.Clinic_Api>("clinic-api")
    .WithReference(clinicDb, "Database")
    .WaitFor(clinicDb);

builder.Build().Run();
