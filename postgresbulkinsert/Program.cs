using BulkInsertAPI.Data.DbContexts;
using BulkInsertAPI.Data.Models;
using BulkInsertAPI.Services;
using BulkInsertAPI.Services.Helpers.Builders;
using BulkInsertAPI.Services.Helpers.Serializers;
using Microsoft.EntityFrameworkCore;

namespace BulkInsertAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IBulkInsertService, BulkInsertService>();
            builder.Services.AddScoped(typeof(IBulkInsertBinaryService<>), typeof(BulkInsertBinaryService<>));

            builder.Services.AddScoped(typeof(IBulkInsertBinaryStatementBuilder<>), typeof(BulkInsertBinaryStatementBuilder<>));

            builder.Services.AddSingleton<INpgsqlEntityBinarySerializer<Message>, MessageNpgsqlBinarySerializer>();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString));


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
        }
    }
}
