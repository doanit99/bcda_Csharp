using ApiWebBanHang.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Cấu hình cors
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowReactApp", builder =>
	{
		builder
			.AllowAnyOrigin()
			.AllowAnyMethod()
			.AllowAnyHeader();
	});
});
//KN database
builder.Services.AddDbContext<DataContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
//Sử dụng cors
app.UseCors("AllowReactApp");

//Sử dụng tệp tĩnh
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
