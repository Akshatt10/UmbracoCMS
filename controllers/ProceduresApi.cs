using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PublishedCache;
using System;
using System.Collections.Generic;
using System.Linq;
using UmbracoProject.Models;

namespace UmbracoProject.Controllers
{
    [Route("umbraco/procedures")]
    [ApiController]
    public class ProceduresApi : ControllerBase
    {
        private readonly IUmbracoContextFactory _umbracoContextFactory;
        private readonly IContentService _contentService;

        public ProceduresApi(IUmbracoContextFactory umbracoContextFactory, IContentService contentService)
        {
            _umbracoContextFactory = umbracoContextFactory;
            _contentService = contentService;
        }

        [HttpGet]
        public ActionResult GetAllProcedures()
        {
            try
            {
                using (var umbracoContextReference = _umbracoContextFactory.EnsureUmbracoContext())
                {
                    var contentQuery = umbracoContextReference.UmbracoContext.Content;

                    // Get the root content node
                    var rootContent = contentQuery.GetAtRoot().FirstOrDefault();
                    if (rootContent == null)
                    {
                        return NotFound(new { message = "Root content not found" });
                    }

                    // Fetch all published procedures under the root content node
                    var procedures = rootContent
                        .DescendantsOrSelf("procedure") // Make sure 'procedures' matches the alias
                        .Where(x => x.IsVisible()) // Ensure the content is visible
                        .ToList();

                    if (procedures == null || !procedures.Any())
                    {
                        return NotFound(new { message = "No procedures found" });
                    }

                    var procedureList = new List<object>();

                    foreach (var procedure in procedures)
                    {
                        var procedureData = new
                        {
                            id = procedure.Value<string>("procedureID"),
                            version = procedure.Value<decimal>("version"),
                            icon = procedure.Value<string>("icon"),
                            description = procedure.Value<string>("description"),
                            chapters = new List<object>()
                        };

                        // Get the chapters (Nested content)
                        var chapters = procedure.Value<IEnumerable<IPublishedElement>>("chapters");
                        if (chapters != null)
                        {
                            foreach (var chapter in chapters)
                            {
                                procedureData.chapters.Add(new
                                {
                                    chapterId = chapter.Value<string>("chapterID"),
                                    description = chapter.Value<string>("description"),
                                    content = chapter.Value<string>("content")
                                });
                            }
                        }

                        procedureList.Add(procedureData);
                    }

                    return Ok(procedureList);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error retrieving procedures: {ex.Message}" });
            }
        }
    }}