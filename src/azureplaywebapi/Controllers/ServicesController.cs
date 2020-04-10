using System.Collections.Generic;
using System.Linq;
using DataLayerMongo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace azureplaywebapi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private ServiceDescriptionService _asdService;
        ILogger<ServicesController> _logger;

        public ServicesController(ILogger<ServicesController> logger, ServiceDescriptionService asdService)
        {
            _asdService = asdService;
            _logger = logger;
            if (_asdService.BadConnectionString())
            {
                _logger.LogError("No connection string configured");
            }
        }

        // GET: Services
        [HttpGet]
        public IEnumerable<ServiceDescription> Get()
        {
            var items = _asdService.List().ToArray();
            return items;
        }

        // GET: Services/5
        [HttpGet("{id:length(24)}", Name = "Get")]
        public ServiceDescription Get(string id)
        {
            return _asdService.Get(id);
        }

        // GET: Services/find/jamesclarke
        [HttpGet("find/{searchstring}")]
        public IEnumerable<ServiceDescription> Find(string searchstring)
        {
            return _asdService.ObjectCollection.Find(f => f.ServiceName.Contains(searchstring)).ToList();
        }

        // POST: Services
        [HttpPost]
        public void Create([FromBody] ServiceDescription value)
        {
            _asdService.Insert(value);
        }

        // PUT: Services/5
        [HttpPut("{id:length(24)}")]
        public void Update(string id, [FromBody] ServiceDescription value)
        {
            _asdService.Update(id, value);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var item = _asdService.Get(id);
            if (item == null)
            {
                return NotFound();
            }
            _asdService.Delete(item);
            return NoContent();
        }
    }
}
