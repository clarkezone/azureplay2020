using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace azureplaywebapi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ASDController : ControllerBase
    {
        private AzureServiceDescriptionService _asdService;
        ILogger<ASDController> _logger;

        public ASDController(ILogger<ASDController> logger, AzureServiceDescriptionService asdService)
        {
            _asdService = asdService;
            _logger = logger;
            if (_asdService.BadConnectionString())
            {
                _logger.LogError("No connection string configured");
            }
        }

        // GET: ASD
        [HttpGet]
        public IEnumerable<ServiceDescription> Get()
        {
            var items = _asdService.List().ToArray();
            return items;
        }

        // GET: ASD/5
        [HttpGet("{id:length(24)}", Name = "Get")]
        public ServiceDescription Get(string id)
        {
            return _asdService.Get(id);
        }

        // POST: ASD
        [HttpPost]
        public void Create([FromBody] ServiceDescription value)
        {
            _asdService.Insert(value);
        }

        // PUT: ASD/5
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
