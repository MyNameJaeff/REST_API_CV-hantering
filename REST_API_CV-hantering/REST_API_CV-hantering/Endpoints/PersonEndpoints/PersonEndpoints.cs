using Microsoft.AspNetCore.Mvc;
using REST_API_CV_hantering.Services;

namespace REST_API_CV_hantering.Endpoints.PersonEndpoints
{
    public static class PersonEndpoints
    {
        public static void RegisterEndpoints(WebApplication app)
        {
            // Get all persons
            app.MapGet("/api/persons", async (PersonService personService) =>
            {
                try
                {
                    var response = await personService.GetPersons();
                    if (response.Success)
                        return response.Data?.Count == 0 ? Results.Ok("No persons found") : Results.Ok(new { response.Message, response.Data });

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
            .WithTags("Persons")
            .WithName("GetPersons")
            .WithSummary("Fetches all persons");

            // Get a single person by id
            app.MapGet("/api/persons/{id}", async (PersonService personService, int id) =>
            {
                var response = await personService.GetPerson(id);
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
            .WithTags("Persons")
            .WithName("GetPerson")
            .WithSummary("Fetches a person by ID");

            // Create a new person
            app.MapPost("/api/persons", async (PersonService personService, string name, string description, string contactInfo) =>
            {
                if (name.Length > 100 || contactInfo.Length > 500)
                {
                    return Results.BadRequest("Name cannot exceed 100 characters and contact information cannot exceed 500 characters.");
                }

                var response = await personService.CreatePerson(name, description, contactInfo);
                if (response.Success)
                    return Results.Created($"/api/persons/{response.Data.Id}", new { response.Message, response.Data });

                var problemDetails = new ProblemDetails
                {
                    Title = response.Message ?? "An error occurred.",
                    Status = response.StatusCode,
                    Type = $"https://http.cat/{response.StatusCode}"
                };

                return Results.Problem(problemDetails);
            })
            .WithTags("Persons")
            .WithName("CreatePerson")
            .WithSummary("Creates a new person");

            // Update an existing person
            app.MapPut("/api/persons/{id}", async (PersonService personService, int id, string name, string description, string contactInfo) =>
            {
                if (name.Length > 100 || contactInfo.Length > 500)
                {
                    return Results.BadRequest("Name cannot exceed 100 characters and contact information cannot exceed 500 characters.");
                }

                var response = await personService.UpdatePerson(id, name, description, contactInfo);
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
            .WithTags("Persons")
            .WithName("UpdatePerson")
            .WithSummary("Updates an existing person");

            // Delete a person
            app.MapDelete("/api/persons/{id}", async (PersonService personService, int id) =>
            {
                var response = await personService.DeletePerson(id);
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
            .WithTags("Persons")
            .WithName("DeletePerson")
            .WithSummary("Deletes an existing person");
        }
    }
}
