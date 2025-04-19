using Microsoft.EntityFrameworkCore;
using RIA.API.Data;
using RIA.API.Services;
using RIA.API.Validations;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddScoped<IDenominationService>(provider => new DenominationService(builder.Configuration.GetSection("Denominations:Cartridges").Get<int[]>() ?? [10, 50, 100]));
builder.Services.AddSingleton<IValidationService, ValidationService>();
builder.Services.AddScoped<ICustomerRepo, CustomerRepo>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers();
builder.Services.AddDbContextFactory<AppDbContext>(options => {
    options.UseSqlite(builder.Configuration.GetConnectionString("PrimaryConnStr"));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.SwaggerDoc("v1",
        new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "RIA.API - V1",
            Description = "Take home assessment",
            Version = "v1"
        }
     );

     var filePath = Path.Combine(System.AppContext.BaseDirectory, "ria.api.xml");
    options.IncludeXmlComments(filePath);
});

var app = builder.Build();

app.UseRouting();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
} 
else
{
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

PrepDatabase.PrepareDatabase(app, app.Environment);

app.Run();
