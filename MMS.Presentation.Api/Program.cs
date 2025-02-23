
using Common.Proto;
using MMS.Presentation.Api.Controllers;
using MMS.Presentation.Api.Services;

namespace MMS.Presentation.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddGrpcClient<MessageExchange.MessageExchangeClient>(options =>
            {
                options.Address = new Uri("https://localhost:7085"); // Replace with the correct server address
            });

            builder.Services.AddTransient<MessageManagementServiceImpl>();

            builder.Services.AddControllers();
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
        }
    }
}
