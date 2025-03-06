using Microsoft.EntityFrameworkCore;
using REST_API_CV_hantering.Classes;
using REST_API_CV_hantering.Data;
using REST_API_CV_hantering.Models;

namespace REST_API_CV_hantering.Services
{
    public class WorkExperienceService
    {
        private readonly CVDbContext context;

        public WorkExperienceService(CVDbContext _context)
        {
            context = _context;
        }

        public async Task<ServiceResponse<List<WorkExperience>>> GetWorkExperiences()
        {
            try
            {
                var workExperiences = await context.WorkExperiences.ToListAsync();
                return ServiceResponse<List<WorkExperience>>.CreateSuccess(workExperiences, $"All {workExperiences.Count} work experiences were fetched.");
            }
            catch (Exception ex)
            {
                return ServiceResponse<List<WorkExperience>>.CreateFail($"An error occurred: {ex.Message}", 500);
            }
        }

        public async Task<ServiceResponse<WorkExperience>> GetWorkExperience(int id)
        {
            if (id <= 0)
                return ServiceResponse<WorkExperience>.CreateFail("Id must be greater than 0.", 400);

            try
            {
                var workExperience = await context.WorkExperiences.FindAsync(id);
                if (workExperience == null)
                    return ServiceResponse<WorkExperience>.CreateFail($"Work experience with id:{id} not found.", 404);

                return ServiceResponse<WorkExperience>.CreateSuccess(workExperience, $"Work experience with id:{workExperience.Id} was fetched successfully.");
            }
            catch (Exception ex)
            {
                return ServiceResponse<WorkExperience>.CreateFail($"An error occurred: {ex.Message}", 500);
            }
        }

        public async Task<ServiceResponse<WorkExperience>> CreateWorkExperience(string jobTitle, string company, string description, string startDate, string? endDate, int personId)
        {
            try
            {
                if (personId <= 0)
                    return ServiceResponse<WorkExperience>.CreateFail("Id must be greater than 0.", 400);

                if (!DateTime.TryParse(startDate, out DateTime parsedStartDate))
                    return ServiceResponse<WorkExperience>.CreateFail("Invalid start date format.", 400);

                DateTime? parsedEndDate = null;
                if (!string.IsNullOrEmpty(endDate))
                {
                    if (!DateTime.TryParse(endDate, out DateTime tempEndDate))
                        return ServiceResponse<WorkExperience>.CreateFail("Invalid end date format.", 400);

                    if (tempEndDate < parsedStartDate)
                        return ServiceResponse<WorkExperience>.CreateFail("End date must be after start date.", 400);

                    parsedEndDate = tempEndDate;
                }

                if (!await context.Persons.AnyAsync(p => p.Id == personId))
                    return ServiceResponse<WorkExperience>.CreateFail($"Person with id:{personId} not found.", 404);

                var workExperience = new WorkExperience
                {
                    JobTitle = jobTitle,
                    Company = company,
                    Description = description,
                    StartDate = parsedStartDate,
                    EndDate = parsedEndDate,
                    PersonId = personId
                };

                context.WorkExperiences.Add(workExperience);
                await context.SaveChangesAsync();

                return ServiceResponse<WorkExperience>.CreateSuccess(workExperience, $"Work experience with id:{workExperience.Id} has been created.");
            }
            catch (Exception ex)
            {
                return ServiceResponse<WorkExperience>.CreateFail($"{ex.Message}", 500);
            }
        }

        public async Task<ServiceResponse<WorkExperience>> UpdateWorkExperience(int id, string jobTitle, string company, string description, string startDate, string? endDate)
        {
            try
            {
                if (id <= 0)
                    return ServiceResponse<WorkExperience>.CreateFail("Id must be greater than 0.", 400);

                if (!DateTime.TryParse(startDate, out DateTime parsedStartDate))
                    return ServiceResponse<WorkExperience>.CreateFail("Invalid start date format.", 400);

                DateTime? parsedEndDate = null;
                if (!string.IsNullOrEmpty(endDate))
                {
                    if (!DateTime.TryParse(endDate, out DateTime tempEndDate))
                        return ServiceResponse<WorkExperience>.CreateFail("Invalid end date format.", 400);

                    if (tempEndDate < parsedStartDate)
                        return ServiceResponse<WorkExperience>.CreateFail("End date must be after start date.", 400);

                    parsedEndDate = tempEndDate;
                }

                var workExperience = await context.WorkExperiences.FindAsync(id);
                if (workExperience == null)
                    return ServiceResponse<WorkExperience>.CreateFail($"Work experience with id:{id} not found.", 404);

                // Update entity
                workExperience.JobTitle = jobTitle;
                workExperience.Company = company;
                workExperience.Description = description;
                workExperience.StartDate = parsedStartDate;
                workExperience.EndDate = parsedEndDate;

                await context.SaveChangesAsync();
                return ServiceResponse<WorkExperience>.CreateSuccess(workExperience, $"Work experience with id:{workExperience.Id} has been edited.");
            }
            catch (Exception ex)
            {
                return ServiceResponse<WorkExperience>.CreateFail($"{ex.Message}", 500);
            }
        }

        public async Task<ServiceResponse<string>> DeleteWorkExperience(int id)
        {
            if (id <= 0)
                return ServiceResponse<string>.CreateFail("Id must be greater than 0.");

            try
            {
                var workExperience = await context.WorkExperiences.FindAsync(id);
                if (workExperience == null)
                    return ServiceResponse<string>.CreateFail($"Work experience with id:{id} not found.", 404);

                context.WorkExperiences.Remove(workExperience);
                await context.SaveChangesAsync();
                return ServiceResponse<string>.CreateSuccess($"Work experience with id:{workExperience.Id} has been deleted.");
            }
            catch (Exception ex)
            {
                return ServiceResponse<string>.CreateFail($"{ex.Message}", 500);
            }
        }
    }
}
