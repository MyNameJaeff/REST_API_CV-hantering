using Microsoft.AspNetCore.Mvc;
using REST_API_CV_hantering.Services;

namespace REST_API_CV_hantering.Endpoints.WorkExperiencesEndpoints
{
    public static class WorkExperiencesEndpoints
    {
        public static void RegisterEndpoints(WebApplication app)
        {
            // Get all work experiences
            app.MapGet("/api/workexperiences", async (WorkExperienceService workExperienceService) =>
            {
                try
                {
                    var response = await workExperienceService.GetWorkExperiences();
                    if (response.Success)
                        return response.Data?.Count == 0 ? Results.Ok("No work experiences found") : Results.Ok(new { response.Message, response.Data });

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
            .WithTags("WorkExperiences")
            .WithName("GetAllWorkExperiences")
            .WithSummary("Hämtar alla arbetserfarenheter");

            // Get a single work experience by id
            app.MapGet("/api/workexperiences/{id}", async (WorkExperienceService workExperienceService, int id) =>
            {
                var response = await workExperienceService.GetWorkExperience(id);
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
            .WithTags("WorkExperiences")
            .WithName("GetAWorkExperience")
            .WithSummary("Hämtar en arbetserfarenhet");

            // Create a new work experience
            app.MapPost("/api/workexperiences", async (WorkExperienceService workExperienceService, string jobTitle, string company, string description, string startDateInput, string? endDateInput, int personId) =>
            {
                var response = await workExperienceService.CreateWorkExperience(jobTitle, company, description, startDateInput, endDateInput, personId);
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
            .WithTags("WorkExperiences")
            .WithName("CreateWorkExperience")
            .WithSummary("Skapar en ny arbetserfarenhet");

            // Update an existing work experience
            app.MapPut("/api/workexperiences/{id}", async (WorkExperienceService workExperienceService, int id, string jobTitle, string company, string description, string startDateInput, string? endDateInput) =>
            {
                var response = await workExperienceService.UpdateWorkExperience(id, jobTitle, company, description, startDateInput, endDateInput);
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
            .WithTags("WorkExperiences")
            .WithName("UpdateWorkExperience")
            .WithSummary("Updaterar en befintlig arbetserfarenhet");

            // Delete a work experience
            app.MapDelete("/api/workexperiences/{id}", async (WorkExperienceService workExperienceService, int id) =>
            {
                var response = await workExperienceService.DeleteWorkExperience(id);
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
            .WithTags("WorkExperiences")
            .WithName("DeleteWorkExperience")
            .WithSummary("Tar bort en befintlig arbetserfarenhet");
        }
    }
}
