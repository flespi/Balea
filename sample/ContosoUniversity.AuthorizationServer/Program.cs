var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("balea.json", optional: false, reloadOnChange: true);

// Add services to the container.
builder.Services.AddBalea().AddConfigurationStore(builder.Configuration);

builder.Services.AddBaleaEndpoints();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseBaleaEndpoints();

app.Run();
