using Microsoft.EntityFrameworkCore;
using REST_API_CV_hantering.Classes;
using REST_API_CV_hantering.Data;
using REST_API_CV_hantering.Models;

namespace REST_API_CV_hantering.Services
{
    public class EducationService
    {
        private readonly CVDbContext context;

        public EducationService(CVDbContext _context)
        {
            context = _context;
        }

        // Can not have ServiceResponse due to being a list, not any validation needed though
        public async Task<ServiceResponse<List<Education>>> GetEducations()
        {
            try
            {
                var educations = await context.Educations.ToListAsync();
                return ServiceResponse<List<Education>>.CreateSuccess(educations, $"All {educations.Count} educations were fetched");
            }
            catch (Exception ex)
            {
                return ServiceResponse<List<Education>>.CreateFail($"An error occurred: {ex.Message}", 500);
            }
        }

        public async Task<ServiceResponse<Education>> GetEducation(int id)
        {
            if (id <= 0)
                return ServiceResponse<Education>.CreateFail("Id must be greater than 0.", 400);

            try
            {
                var education = await context.Educations.FindAsync(id);
                if (education == null)
                    return ServiceResponse<Education>.CreateFail($"Education with id:{id} not found.", 404);

                return ServiceResponse<Education>.CreateSuccess(education, $"Education with id:{education.Id} was fetched succesfully");
            }
            catch (Exception ex)
            {
                return ServiceResponse<Education>.CreateFail($"An error occurred: {ex.Message}", 500);
            }
        }

        public async Task<ServiceResponse<Education>> CreateEducation(string school, string degree, string fieldOfStudy, string startDate, string? endDate, int personId)
        {
            try
            {
                if (personId <= 0)
                    return ServiceResponse<Education>.CreateFail("Id must be greater than 0.", 400);

                if (!DateTime.TryParse(startDate, out DateTime parsedStartDate))
                    return ServiceResponse<Education>.CreateFail("Invalid start date format.", 400);

                DateTime? parsedEndDate = null;
                if (!string.IsNullOrWhiteSpace(endDate) && DateTime.TryParse(endDate, out DateTime tempEndDate))
                {
                    if (tempEndDate < parsedStartDate)
                        return ServiceResponse<Education>.CreateFail("End date must be after start date.", 400);

                    parsedEndDate = tempEndDate;
                }

                if (!await context.Persons.AnyAsync(p => p.Id == personId))
                    return ServiceResponse<Education>.CreateFail($"Person with id:{personId} not found.", 404);

                var education = new Education
                {
                    School = school,
                    Degree = degree,
                    FieldOfStudy = fieldOfStudy,
                    StartDate = parsedStartDate,
                    EndDate = parsedEndDate,
                    PersonId = personId
                };

                context.Educations.Add(education);
                await context.SaveChangesAsync();

                return ServiceResponse<Education>.CreateSuccess(education, $"Education with id:{education.Id} has been created");
            }
            catch (Exception ex)
            {
                return ServiceResponse<Education>.CreateFail($"{ex.Message}", 500);
            }
        }


        public async Task<ServiceResponse<Education>> UpdateEducation(int id, string school, string degree, string startDate, string? endDate)
        {
            try
            {
                if (id <= 0)
                    return ServiceResponse<Education>.CreateFail("Id must be greater than 0.", 400);

                if (!DateTime.TryParse(startDate, out DateTime parsedStartDate))
                    return ServiceResponse<Education>.CreateFail("Invalid start date format.", 400);

                DateTime? parsedEndDate = null;
                if (!string.IsNullOrEmpty(endDate))
                {
                    if (!DateTime.TryParse(endDate, out DateTime tempEndDate))
                        return ServiceResponse<Education>.CreateFail("Invalid end date format.", 400);

                    if (tempEndDate < parsedStartDate)
                        return ServiceResponse<Education>.CreateFail("End date must be after start date.", 400);

                    parsedEndDate = tempEndDate;
                }

                var education = await context.Educations.FindAsync(id);
                if (education == null)
                    return ServiceResponse<Education>.CreateFail($"Education with id:{id} not found.", 404);

                // Update entity
                education.School = school;
                education.Degree = degree;
                education.StartDate = parsedStartDate;
                education.EndDate = parsedEndDate;

                await context.SaveChangesAsync();
                return ServiceResponse<Education>.CreateSuccess(education, $"Education with id:{education.Id} has been edited");
            }
            catch (Exception ex)
            {
                return ServiceResponse<Education>.CreateFail($"{ex.Message}", 500);
            }
        }


        public async Task<ServiceResponse<bool>> DeleteEducation(int id)
        {
            if (id <= 0)
                return ServiceResponse<bool>.CreateFail("Id must be greater than 0.");

            try
            {
                var education = await context.Educations.FindAsync(id);
                if (education == null)
                    return ServiceResponse<bool>.CreateFail($"Education with id:{id} not found.", 404);

                context.Educations.Remove(education);
                await context.SaveChangesAsync();
                return ServiceResponse<bool>.CreateSuccess(true, $"Education with id:{education.Id} has been deleted");
            }
            catch (Exception ex)
            {
                return ServiceResponse<bool>.CreateFail($"{ex.Message}", 500);
            }
        }

    }
}
