using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.Common.Controllers;

namespace UmbracoProject.Controllers
{
    [ApiController]
    [Route("/umbraco/api/MyApi")] // Custom route for Umbraco APIs
    public class MyApiController : ControllerBase
    {
        private readonly IContentService _contentService;

        public MyApiController(IContentService contentService)
        {
            _contentService = contentService;
        }

        // GET: /umbraco/api/MyApi/greeting
        [HttpGet("greeting")]
        public IActionResult GetGreeting()
        {
            return Ok("Hello, this is your custom Umbraco API!");
        }

        // GET: /umbraco/api/MyApi/content/{id}
        [HttpGet("content/{id:int}")]
        public IActionResult GetContentById(int id)
        {
            var content = _contentService.GetById(id);
            if (content == null)
            {
                return NotFound($"Content with ID {id} was not found.");
            }

            return Ok(new
            {
                Id = content.Id,
                Name = content.Name,
                ContentType = content.ContentType.Alias
            });
        }
    }
}
