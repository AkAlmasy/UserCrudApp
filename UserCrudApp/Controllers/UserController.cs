using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserCrudApp.DataAccess;
using UserCrudApp.Models;

namespace UserCrudApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        // TODO create dbProvider for long term use
        private readonly UserContext _context;

        public UserController(UserContext context)
        {
            _context = context;
        }

        [HttpGet("getUsers")]
        public async Task<ActionResult<List<User>>> Get()
        {
            try
            {
                var users = await _context.Users
                    .Include(T => T.Todos)
                    .ToListAsync();
                return Ok(users);            
            }
            catch (Exception e)
            {
                // TODO Log e somewhere
                return BadRequest(e);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<User>>> Get(int id)
        {
            try
            {
                User user = await _context.Users.Include(i => i.Todos).FirstOrDefaultAsync(i => i.Id == id);
                if (user == null)
                {
                    return BadRequest("User not found");
                }
                return Ok(user);
            }
            catch (Exception e)
            {
                // TODO Log e somewhere
                return BadRequest("An error occurred" +  e);
            }
        }

        [HttpPost("createUser")]
        public async Task<ActionResult<List<User>>> AddUser(User user)
        {
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return Ok(await _context.Users.Include(T => T.Todos).ToListAsync());
            }
            catch (Exception e)
            {
                // TODO Log e somewhere
                return BadRequest("An error occurred" + e);
            }
        }

        [HttpPut("modifyUser")]
        public async Task<ActionResult<List<User>>> UpdateUser(User modifiedUser)
        {
            try
            {
                var dbUser = await _context.Users.Include(user => user.Todos).Where(user => user.Id == modifiedUser.Id).SingleOrDefaultAsync();

                if (dbUser == null)
                {
                    return BadRequest("User not found");
                }

                _context.Entry(dbUser).CurrentValues.SetValues(modifiedUser);

                foreach (var existingChild in dbUser.Todos.ToList())
                {
                    if (!modifiedUser.Todos.Any(c => c.Id == existingChild.Id))
                        _context.Todos.Remove(existingChild);
                }

                // Update and Insert children
                foreach (var childModel in modifiedUser.Todos)
                {
                    var existingChild = dbUser.Todos
                        .Where(c => c.Id == childModel.Id)
                        .SingleOrDefault();

                    if (existingChild != null)
                        // Update child
                        _context.Entry(existingChild).CurrentValues.SetValues(childModel);
                    else
                    {
                        // Insert child
                        var newChild = new Todo
                        {
                            Id = childModel.Id, 
                            Name = childModel.Name,
                            Description = childModel.Description,
                        };
                        dbUser.Todos.Add(newChild);
                    }
                }

                await _context.SaveChangesAsync();
                return Ok(await _context.Users.Include(T => T.Todos).ToListAsync());
            }
            catch (Exception e)
            {
                // TODO Log e somewhere
                return BadRequest("An error occurred" + e);
            }

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<User>>> Delete(int id)
        {
            try
            {
                User user = await _context.Users.Include(i => i.Todos).FirstOrDefaultAsync(i => i.Id == id);
                if (user == null)
                {
                    return BadRequest("User not found");
                }
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return Ok(await _context.Users.Include(T => T.Todos).ToListAsync());
            }
            catch (Exception e)
            {
                // TODO Log e somewhere
                return BadRequest("An error occurred" + e);
            }
        }
    }
}
