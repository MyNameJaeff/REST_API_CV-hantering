using Microsoft.AspNetCore.Mvc;
using REST_API_CV_hantering.Services;

namespace REST_API_CV_hantering.Endpoints.EducationsEndpoints
{
    public class EducationsEndpoints
    {
        public static void RegisterEndpoints(WebApplication app)
        {
            // Get all educations
            app.MapGet("/api/educations", async (EducationService educationService) =>
            {
                try
                {
                    var response = await educationService.GetEducations();
                    if (response.Success)
                        return response.Data?.Count == 0 ? Results.Ok("No educations found") : Results.Ok(new { response.Message, response.Data });

                    var problemDetails = new ProblemDetails
                    {
                        Title = response.Message ?? "An error occurred.",
                        Status = response.StatusCode,
                        Type = $"https://http.cat/{response.StatusCode}"
                    };

                    return Results.Problem(problemDetails);
                }
                catch (Exception ex)
                {
                    var problemDetails = new ProblemDetails
                    {
                        Title = "An internal server error occurred",
                        Status = 500,
                        Type = "https://http.cat/500"
                    };

                    return Results.Problem(problemDetails);
                }
            })
            .WithTags("Educations")
            .WithName("GetAllEducation")
            .WithSummary("Hämtar alla Education");

            // Get a single education by id
            app.MapGet("/api/educations/{id}", async (EducationService educationService, int id) =>
            {
                var response = await educationService.GetEducation(id);
                if (response.Success)
                    return Results.Ok(new { response.Message, response.Data });

                var problemDetails = new ProblemDetails
                {
                    Title = response.Message ?? "An error occurred.",
                    Status = response.StatusCode,
                    Type = $"https://http.cat/{response.StatusCode}"
                };

                return Results.Problem(problemDetails);
            })
            .WithTags("Educations")
            .WithName("GetAEducation")
            .WithSummary("Hämtar en Education");

            // Create a new education
            app.MapPost("/api/educations", async (EducationService educationService, string school, string degree, string fieldOfStudy, string startDateInput, string? endDateInput, int personId) =>
            {
                var response = await educationService.CreateEducation(school, degree, fieldOfStudy, startDateInput, endDateInput, personId);
                if (response.Success)
                    return Results.Ok(new { response.Message, response.Data });

                var problemDetails = new ProblemDetails
                {
                    Title = response.Message ?? "An error occurred.",
                    Status = response.StatusCode,
                    Type = $"https://http.cat/{response.StatusCode}"
                };

                return Results.Problem(problemDetails);
            })
            .WithTags("Educations")
            .WithName("CreateEducation")
            .WithSummary("Skapar en ny Education");

            // Delete an education
            app.MapDelete("/api/educations/{id}", async (EducationService educationService, int id) =>
            {
                var response = await educationService.DeleteEducation(id);
                if (response.Success)
                    return Results.Ok(new { response.Message, response.Data });

                var problemDetails = new ProblemDetails
                {
                    Title = response.Message ?? "An error occurred.",
                    Status = response.StatusCode,
                    Type = $"https://http.cat/{response.StatusCode}"
                };

                return Results.Problem(problemDetails);
            })
            .WithTags("Educations")
            .WithName("DeleteEducation")
            .WithSummary("Tar bort en befintlig Education");

            // Update an education
            app.MapPut("/api/educations/{id}", async (EducationService educationService, int id, string school, string degree, string startDateInput, string? endDateInput) =>
            {
                var response = await educationService.UpdateEducation(id, school, degree, startDateInput, endDateInput);
                if (response.Success)
                    return Results.Ok(response.Data);

                var problemDetails = new ProblemDetails
                {
                    Title = response.Message ?? "An error occurred.",
                    Status = response.StatusCode,
                    Type = $"https://http.cat/{response.StatusCode}"
                };

                return Results.Problem(problemDetails);
            })
            .WithTags("Educations")
            .WithName("UpdateEducation")
            .WithSummary("Updaterar en befintlig Education");
        }
    }
}
