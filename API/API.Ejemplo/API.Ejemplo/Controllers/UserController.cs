using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Ejemplo.Models;

namespace API.Ejemplo.Controllers
{    
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly UserContext _userContext;

        public UserController(UserContext context)
        {
            _userContext = context;
            if(_userContext.UserItems.Count() == 0)
            {
                _userContext.UserItems.Add(new Models.User { Name = "Test", Email = "Test@mail.com" });
                _userContext.SaveChanges();
            }
        }


        [HttpGet]
        public IEnumerable<User> GetAll()
        {
            return _userContext.UserItems.ToList();
        }

        [HttpGet("{id}", Name = "GetTodo")]
        public IActionResult GetById(int id)
        {
            var item = _userContext.UserItems.FirstOrDefault(t => t.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }


        [HttpPost]
        public IActionResult Create([FromBody] User item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            _userContext.UserItems.Add(item);
            _userContext.SaveChanges();

            return CreatedAtRoute("GetTodo", new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] User item)
        {
            if (item == null || item.Id != id)
            {
                return BadRequest();
            }

            var user = _userContext.UserItems.FirstOrDefault(t => t.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            user.Email = item.Email;
            user.Name = item.Name;

            _userContext.UserItems.Update(user);
            _userContext.SaveChanges();
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var todo = _userContext.UserItems.FirstOrDefault(t => t.Id == id);
            if (todo == null)
            {
                return NotFound();
            }

            _userContext.UserItems.Remove(todo);
            _userContext.SaveChanges();
            return new NoContentResult();
        }

    }
}