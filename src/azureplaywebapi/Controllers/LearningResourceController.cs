using System.Collections.Generic;
using System.Linq;
using DataLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace azureplaywebapi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LearningResourceController : Controller
    {
        private LearningResourceService _lrService;
        ILogger<LearningResourceController> _logger;

        public LearningResourceController(ILogger<LearningResourceController> logger, LearningResourceService lrService)
        {
            _lrService = lrService;
            _logger = logger;
            if (_lrService.BadConnectionString())
            {
                _logger.LogError("No connection string configured");
            }
        }

        // GET: LearningResource
        [HttpGet]
        public IEnumerable<LearningResource> Get()
        {
            var items = _lrService.List().ToArray();
            return items;
        }

        // GET: LearningResource/5
        [HttpGet("{id:length(24)}", Name = "GetLR")]
        public LearningResource Get(string id)
        {
            return _lrService.Get(id);
        }

        // POST: LearningResource
        [HttpPost]
        public void Create([FromBody] LearningResource value)
        {
            _lrService.Insert(value);
        }

        // PUT: LearningResource/5
        [HttpPut("{id:length(24)}")]
        public void Update(string id, [FromBody] LearningResource value)
        {
            _lrService.Update(id, value);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var item = _lrService.Get(id);
            if (item == null)
            {
                return NotFound();
            }
            _lrService.Delete(item);
            return NoContent();
        }
    }
}