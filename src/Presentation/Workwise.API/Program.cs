using Workwise.API.Extantions;
using Workwise.Application.ServiceRegistration;
using Workwise.Infrastructure.ServiceRegistration;
using Workwise.Persistance.ServiceRegistration;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAplicationServices();
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddCorsConfig();
builder.Services.IdentitySwagger();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.ContextInitalize();
app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin");
app.AddExceptionHandlerService();
app.UseAuthentication();
app.UseAuthorization();

app.AddSignalREndpoints();
app.MapControllers();

app.Run();