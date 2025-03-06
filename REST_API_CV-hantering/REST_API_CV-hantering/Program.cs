using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using REST_API_CV_hantering.Data;
using REST_API_CV_hantering.Endpoints.EducationsEndpoints;
using REST_API_CV_hantering.Endpoints.GithubEndpoints;
using REST_API_CV_hantering.Endpoints.PersonEndpoints;
using REST_API_CV_hantering.Endpoints.WorkExperiencesEndpoints;
using REST_API_CV_hantering.Services;

namespace REST_API_CV_hantering
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.EnableAnnotations(); // Aktiverar annoteringar
                options.OrderActionsBy((apiDesc) => $"{apiDesc.ActionDescriptor.RouteValues["controller"]}");
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "CV API",
                    Version = "v1",
                    Description = "API för att hantera CV-data (personer, utbildningar och arbetserfarenhet)",
                });
            });
            builder.Services.AddDbContext<CVDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<PersonService>();
            builder.Services.AddScoped<EducationService>();
            builder.Services.AddScoped<WorkExperienceService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();

            // Endpoints
            GithubEndpoints.RegisterEndpoints(app);
            PersonEndpoints.RegisterEndpoints(app);
            EducationsEndpoints.RegisterEndpoints(app);
            WorkExperiencesEndpoints.RegisterEndpoints(app);

            app.Run();
        }
    }
}