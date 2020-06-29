using System;
using System.Collections.Generic;
using System.Linq;
using DataLayerModernSQL;
using DataLayerModernSQL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace azureplaywebapi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AzureServicesController : ControllerBase
    {
        private DataService _asdService;
        ILogger<DataService> _logger;

        public AzureServicesController(ILogger<DataService> logger, DataService asdService)
        {
            _asdService = asdService;
            _logger = logger;
            if (_asdService.BadConnectionString())
            {
                _logger.LogError("No connection string configured");
                throw new System.Exception("connection string is bad");
            }
        }

        // GET: Services
        [HttpGet]
        public IEnumerable<ServiceDescription> Get()
        {
            var items = _asdService.Services.AsEnumerable();
            return items;
        }

        // GET: Services/5
        [HttpGet("{id}")]
        public ServiceDescription Get(Guid id)
        {
            return _asdService.Services.Find(new object[] { id });
        }

        // GET: Services/find/jamesclarke
        [HttpGet("find/{searchstring}")]
        public IEnumerable<ServiceDescription> Find(string searchstring)
        {
            return _asdService.Services.Where(f => f.ServiceName.ToLower().Contains(searchstring.ToLower())).AsEnumerable();
        }

        // POST: Services
        [HttpPost]
        public void Create([FromBody] ServiceDescription value)
        {
            _asdService.Services.Add(value);
        }

        // PUT: Services/5
        [HttpPut("{id}")]
        public void Update(string id, [FromBody] ServiceDescription value)
        {
            _asdService.Services.Update(value);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var foundItem = _asdService.Services.Find(id);
            if (foundItem == null)
            {
                return NotFound();
            }
            _asdService.Services.Remove(foundItem);
            return NoContent();
        }
    }
}
