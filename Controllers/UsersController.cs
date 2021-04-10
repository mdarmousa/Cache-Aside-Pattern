using Cache_Aside_Pattern.Cache;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cache_Aside_Pattern.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private static readonly User[] Users = new []
        {
            new User (1,"Freezing"),
            new User (2,"Bracing"),
            new User (3,"Chilly"),
            new User (4,"Cool"),
            new User (5,"Mild"),
            new User (6,"Warm"),
            new User (7,"Balmy"),
            new User (8,"Hot"),
            new User (9,"Sweltering"),
            new User (10,"Scorching")
        };

        private readonly ILogger<UsersController> _logger;
        private readonly ICacheService _cacheService;

        public UsersController(ILogger<UsersController> logger, ICacheService cacheService)
        {
            _logger = logger;
            _cacheService = cacheService;
        }

        [HttpGet]
        public IEnumerable<User> Get()
        {
            return Users;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var record = _cacheService.Get<User>($"{id}");

            if (record == null)
            {
                record = Users.FirstOrDefault(x=>x.Id == id);
                if (record == null) return NotFound();

                _cacheService.Add(record, $"{id}");
            }

            return Ok(record);
        }
    }

    public class User
    {
        public User(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public int Id{ get; set; }
        public string Name{ get; set; }
    }
}
