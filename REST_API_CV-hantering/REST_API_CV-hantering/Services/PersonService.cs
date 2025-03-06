using Microsoft.EntityFrameworkCore;
using REST_API_CV_hantering.Classes;
using REST_API_CV_hantering.Data;
using REST_API_CV_hantering.Models;

namespace REST_API_CV_hantering.Services
{
    public class PersonService
    {
        private readonly CVDbContext context;
        public PersonService(CVDbContext _context)
        {
            context = _context;
        }

        public async Task<ServiceResponse<List<Person>>> GetPersons()
        {
            try
            {
                var persons = await context.Persons.ToListAsync();
                return ServiceResponse<List<Person>>.CreateSuccess(persons, $"{persons.Count} persons were fetched");
            }
            catch (Exception ex)
            {
                return ServiceResponse<List<Person>>.CreateFail($"An error occurred: {ex.Message}", 500);
            }
        }

        public async Task<ServiceResponse<Person>> GetPerson(int id)
        {
            if (id <= 0)
                return ServiceResponse<Person>.CreateFail("Id must be greater than 0.", 400);

            try
            {
                var person = await context.Persons
                    .Include(p => p.Educations) // Eager load Educations
                    .Include(p => p.WorkExperiences) // Eager load WorkExperiences
                    .FirstOrDefaultAsync(p => p.Id == id); // Find person by ID

                if (person == null)
                    return ServiceResponse<Person>.CreateFail($"Person with id:{id} not found.", 404);

                return ServiceResponse<Person>.CreateSuccess(person, $"Person with id:{person.Id} was fetched successfully.");
            }
            catch (Exception ex)
            {
                return ServiceResponse<Person>.CreateFail($"An error occurred: {ex.Message}", 500);
            }
        }

        public async Task<ServiceResponse<Person>> CreatePerson(string name, string description, string contactInfo)
        {
            try
            {
                var person = new Person
                {
                    Name = name,
                    Description = description,
                    ContactInfo = contactInfo
                };

                context.Persons.Add(person);
                await context.SaveChangesAsync();

                return ServiceResponse<Person>.CreateSuccess(person, $"Person with id:{person.Id} has been created");
            }
            catch (Exception ex)
            {
                return ServiceResponse<Person>.CreateFail($"An error occurred: {ex.Message}", 500);
            }
        }

        public async Task<ServiceResponse<Person>> UpdatePerson(int id, string name, string description, string contactInfo)
        {
            if (id <= 0)
                return ServiceResponse<Person>.CreateFail("Id must be greater than 0.", 400);

            try
            {
                var person = await context.Persons.FindAsync(id);
                if (person == null)
                    return ServiceResponse<Person>.CreateFail($"Person with id:{id} not found.", 404);

                person.Name = name;
                person.Description = description;
                person.ContactInfo = contactInfo;

                await context.SaveChangesAsync();
                return ServiceResponse<Person>.CreateSuccess(person, $"Person with id:{person.Id} has been updated");
            }
            catch (Exception ex)
            {
                return ServiceResponse<Person>.CreateFail($"An error occurred: {ex.Message}", 500);
            }
        }

        public async Task<ServiceResponse<bool>> DeletePerson(int id)
        {
            if (id <= 0)
                return ServiceResponse<bool>.CreateFail("Id must be greater than 0.", 400);

            try
            {
                var person = await context.Persons.FindAsync(id);
                if (person == null)
                    return ServiceResponse<bool>.CreateFail($"Person with id:{id} not found.", 404);

                context.Persons.Remove(person);
                await context.SaveChangesAsync();

                return ServiceResponse<bool>.CreateSuccess(true, $"Person with id:{person.Id} has been deleted");
            }
            catch (Exception ex)
            {
                return ServiceResponse<bool>.CreateFail($"An error occurred: {ex.Message}", 500);
            }
        }
    }
}
