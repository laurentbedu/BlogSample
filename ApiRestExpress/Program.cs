using ApiRestExpress.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddInstantAPIs();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapInstantAPIs<AppDbContext>();

//List<Type> models = Assembly.GetExecutingAssembly().GetTypes()
//              .Where(t => string.Equals(t.Namespace, "ApiRestExpress.Models", StringComparison.Ordinal) && t.BaseType.Name == "ModelBase")
//              .ToList();



//foreach (Type model in models)
//{
//    app.MapGet($"/api/{model.Name}", async (AppDbContext _context) =>
//    {
//        var results = _context
//            .GetType().GetProperty(model.Name + "s")?.GetValue(_context);

//        return results;
//    });

//    app.MapGet($"/api/{model.Name}/" + "{id:int}", async (AppDbContext _context, int id) =>
//    {
//        var results = _context
//           .GetType().GetProperty(model.Name + "s")?.GetValue(_context);
//        return ((List<dynamic>)results).FirstOrDefault(item => item.Id == id);
//    });
//}

//var summaries = new[]
//{
//    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
//};

//app.MapGet("/weatherforecast", () =>
//{
//    var forecast = Enumerable.Range(1, 5).Select(index =>
//       new WeatherForecast
//       (
//           DateTime.Now.AddDays(index),
//           Random.Shared.Next(-20, 55),
//           summaries[Random.Shared.Next(summaries.Length)]
//       ))
//        .ToArray();
//    return forecast;
//})
//.WithName("GetWeatherForecast");

app.Run();

//internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
//{
//    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
//}