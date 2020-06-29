using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DataLayerModernSQL;
using DataLayerModernSQL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;

namespace azureplaywebapi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AzureLearningResourcesController : Controller
    {
        private DataService _lrService;
        ILogger<AzureLearningResourcesController> _logger;

        public AzureLearningResourcesController(ILogger<AzureLearningResourcesController> logger, DataService lrService)
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
            var items = _lrService.LearningResources.AsEnumerable();
            return items;
        }

        // GET: LearningResource/5
        [HttpGet("{id}")]
        public LearningResource Get(Guid id)
        {
            return _lrService.LearningResources.Find(id);
        }

        // GET: LearningResource/find/jamesclarke
        [HttpGet("find/{searchstring}")]
        public IEnumerable<LearningResource> Find(string searchstring)
        {
            return _lrService.LearningResources.Where(f => f.Name.ToLower().Contains(searchstring.ToLower())).AsEnumerable();
        }

        // POST: LearningResource
        [HttpPost]
        public void Create([FromBody] LearningResource value)
        {
            _lrService.LearningResources.Add(value);
            _lrService.SaveChanges();
        }

        // PUT: LearningResource/5
        [HttpPut("{id}")]
        public void Update(string id, [FromBody] LearningResource value)
        {
            _lrService.LearningResources.Update(value);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var item = _lrService.LearningResources.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            _lrService.LearningResources.Remove(item);
            return NoContent();
        }

        

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (!base.ModelState.IsValid)
            {
                var problemDetails = base.ProblemDetailsFactory.CreateValidationProblemDetails(base.HttpContext, base.ModelState);

                string errors = string.Join(";", problemDetails.Errors.Select(x => "key:" + x.Key + " error:" + x.Value[0]));

                _logger.LogWarning("Client API call failed with invalid model state: key {0} with problem ", errors);
                
            }
            base.OnActionExecuted(context);
        }
    }
}