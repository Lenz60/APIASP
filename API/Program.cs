using API.Context;
using API.Helper;
using API.Repositories;
using Microsoft.EntityFrameworkCore;
using Nest;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<MyContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("API")));
builder.Services.AddDbContext<MyContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Secret")));
builder.Services.AddScoped<DepartmentRepository>();
builder.Services.AddScoped<DepartmentRepository>();
builder.Services.AddScoped<EmployeeRepository>();
builder.Services.AddScoped<AccountRepository>();
builder.Services.AddScoped<JWTHelper>();
builder.Services.AddScoped<BcryptHelper>();

//builder.Services.AddCors(options =>
//            {
//                options.AddPolicy("AllowSpecificOrigin",
//                    builder => builder
//                        // Replace with your allowed origin
//                        .AllowAnyHeader()
//                        .AllowAnyMethod());
//            });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});
// Enable CORS



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigin");
app.UseAuthorization();

app.MapControllers();

app.Run();
