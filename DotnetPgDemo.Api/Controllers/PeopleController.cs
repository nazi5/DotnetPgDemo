using DotnetPgDemo.Api.models;
using DotnetPgDemo.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace DotnetPgDemo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly AppDbContext _context;
        public PeopleController(AppDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<ActionResult<Person>> CreatePerson([FromBody] Person person)
        {
            try            {
                _context.People.Add(person);
                await _context.SaveChangesAsync();
                return CreatedAtRoute("GetPerson", new { id = person.Id }, person);//200 status code with the created person object
                
            } catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating person: {ex.Message}");
            }
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetPeople()
        {
            try
            {
                var people = await _context.People.ToListAsync();
                return Ok(people);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving people: {ex.Message}");
            }
        }
        

        [HttpGet("{id}", Name = "GetPerson")]
        public async Task<ActionResult<Person>> GetPersonById(int id)
        {
            try
            {
                var person = await _context.People.FindAsync(id);
                if (person == null)
                {
                    return NotFound("Person not found.");
                }
                if (person.Id != id)
                {
                    return BadRequest("Person ID mismatch.");
                }
                return Ok(person);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving person: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Person>> UpdatePerson(int id, [FromBody] Person person)
        {
            try   {
                if (id != person.Id)
                {
                    return BadRequest("Person ID mismatch.");
                }
                if (!_context.People.Any(p => p.Id == id))
                {
                    return NotFound("Person not found.");
                }
                _context.People.Update(person);
                await _context.SaveChangesAsync();
                return NoContent();//200 status code with the created person object
                
            } catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating person: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Person>> DeletePerson(int id)
        {
            try
            {
                var person = await _context.People.FindAsync(id);
                if (person == null)
                {
                    return NotFound("Person not found.");
                }
                if(!_context.People.Any(p => p.Id == id))
                {
                    return BadRequest("Id mismatch.");
                }
                _context.People.Remove(person);
                await _context.SaveChangesAsync();
                return Ok(person);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting person: {ex.Message}");
            }
        }
    }   
}