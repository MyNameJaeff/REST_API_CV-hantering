using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using REST_API_CV_hantering.Data;
using REST_API_CV_hantering.Models;

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
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "CV API",
                    Version = "v1",
                    Description = "API för att hantera CV-data (personer, utbildningar och arbetserfarenhet)"
                });
            });
            builder.Services.AddDbContext<CVDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();

            RegisterRoutes(app);

            app.Run();
        }

        private static void RegisterRoutes(WebApplication app)
        {
            // Person endpoints
            app.MapGet("/api/persons", async (CVDbContext context) =>
            {
                var persons = await context.Persons.ToListAsync();
                return persons.Count == 0 ? Results.Ok("Inga personer registrerade än") : Results.Ok(persons);
            });

            app.MapPost("/api/persons", async (CVDbContext context, string name, string description, string contactInfo) =>
            {
                // I perferred this way compared to having to make a json body for each response as that wasnt great, also data validation
                var person = new Person { Name = name, Description = description, ContactInfo = contactInfo };
                await context.Persons.AddAsync(person);
                await context.SaveChangesAsync();
                return Results.Created($"/api/persons/{person.Id}", person);
            });

            // Education endpoints
            app.MapGet("/api/educations", async (CVDbContext context) =>
            {
                var educations = await context.Educations.ToListAsync();
                return educations.Count == 0 ? Results.Ok("Inga educations registrerade än") : Results.Ok(educations);
            });

            app.MapPost("/api/educations", async (CVDbContext context, string school, string degree, string startDateInput, string? endDateInput, int personId) =>
            {
                // Check if person exists first
                if (!await context.Persons.AnyAsync(p => p.Id == personId))
                {
                    return Results.NotFound($"Person with id {personId} not found");
                }

                if (!DateTime.TryParse(startDateInput, out DateTime startDate))
                {
                    return Results.BadRequest($"Invalid start date format: {startDateInput}");
                }

                DateTime? endDate = null;
                if (endDateInput != null)
                {
                    if (!DateTime.TryParse(endDateInput, out DateTime parsedEndDate))
                    {
                        return Results.BadRequest($"Invalid end date format: {endDateInput}");
                    }
                    if (parsedEndDate < startDate)
                    {
                        return Results.BadRequest("End date must be after start date");
                    }
                    endDate = parsedEndDate;
                }

                var education = new Education
                {
                    School = school,
                    Degree = degree,
                    StartDate = startDate,
                    EndDate = endDate,
                    PersonId = personId
                };

                await context.Educations.AddAsync(education);
                await context.SaveChangesAsync();
                return Results.Created($"/api/education/{education.Id}", education);
            });



            // Work Experience endpoints
            app.MapGet("/api/workexperiences", async (CVDbContext context) =>
            {
                var workExperiences = await context.WorkExperiences.ToListAsync();
                return workExperiences.Count == 0 ? Results.Ok("Inga workExperiences registrerade än") : Results.Ok(workExperiences);
            });

            app.MapPost("/api/workexperiences", async (CVDbContext context, string jobTitle, string company, string description, string startDateInput, string endDateInput, int personId) =>
            {
                // Check if person exists first
                if (!await context.Persons.AnyAsync(p => p.Id == personId))
                {
                    return Results.NotFound($"Person with id {personId} not found");
                }

                if (!DateTime.TryParse(startDateInput, out DateTime startDate))
                {
                    return Results.BadRequest($"Invalid start date format: {startDateInput}");
                }

                DateTime? endDate = null;
                if (endDateInput != null)
                {
                    if (!DateTime.TryParse(endDateInput, out DateTime parsedEndDate))
                    {
                        return Results.BadRequest($"Invalid end date format: {endDateInput}");
                    }
                    if (parsedEndDate < startDate)
                    {
                        return Results.BadRequest("End date must be after start date");
                    }
                    endDate = parsedEndDate;
                }

                var workExperience = new WorkExperience { JobTitle = jobTitle, Company = company, Description = description, StartDate = startDate, EndDate = endDate, PersonId = personId };
                await context.WorkExperiences.AddAsync(workExperience);
                await context.SaveChangesAsync();
                return Results.Created($"/api/workexperience/{workExperience.Id}", workExperience);
            });
        }
    }
}