using Cache_Aside_Pattern.Cache;
using Cache_Aside_Pattern.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cache_Aside_Pattern.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : ControllerBase
    {

        private readonly AppDbContext _appDbContext;
        private readonly ICacheService _cacheService;

        public EmployeeController(AppDbContext appDbContext, ICacheService cacheService)
        {
            _appDbContext = appDbContext;
            _cacheService = cacheService;
        }

         [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var record = _cacheService.Get<Employee>($"{id}");

            if (record == null)
            {
                record = _appDbContext.Employees.FirstOrDefault(x=>x.Id == id);
                if (record == null) return NotFound();

                _cacheService.Add(record, $"{id}");
            }

            return Ok(record);
        }

        [HttpPost()]
        public async Task<IActionResult> CreateAsync(Employee employee)
        {
            if (employee.Id == 0)
            {
                _appDbContext.Employees.Add(employee);
                await _appDbContext.SaveChangesAsync();
                _cacheService.Add(employee, $"{employee.Id}");
            }
            else
            {
                var record = _appDbContext.Employees
                    .AsNoTracking().FirstOrDefault(x => x.Id == employee.Id);
                if (record == null) return NotFound();
                _appDbContext.Employees.Update(employee);
                await _appDbContext.SaveChangesAsync();
                _cacheService.Add(employee, $"{employee.Id}");
            }

            return Ok(employee);
        }
    }

}
