using Service_Address.Service;

var builder = WebApplication.CreateBuilder(args);





#region Service

//--Nạp cấu hình kết nối với mogodb
builder.Services.Configure<List<Service_Address.Models.ConnectionInfo>>(
    builder.Configuration.GetSection("MongodbConnections"));
//--Service đọc các dữ liệu
builder.Services.AddScoped<CountriesService>();
builder.Services.AddScoped<CitiesService>();
builder.Services.AddScoped<RegionsServiec>();
builder.Services.AddScoped<StatesService>();
builder.Services.AddScoped<SubregionsService>();
builder.Services.AddScoped<FileProcessingService>();





#endregion

// Add services to the container.

builder.Services.AddControllers();
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

app.MapControllers();

app.Run();
