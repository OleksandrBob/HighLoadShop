using FluentValidation;
using HighLoadShop.Application;
using HighLoadShop.Infrastructure;
using HighLoadShop.Persistence;

namespace HighLoadShopApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers(options =>
            {
                // Add global filters if needed
            });

            builder.Services.AddAuthorization();

            builder.Services.AddApplication();
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddPersistence(builder.Configuration);

            // Add FluentValidation
            builder.Services.AddValidatorsFromAssemblyContaining<Program>();

            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
