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
    [Route("umbraco/api/questions")]
    [ApiController]
    public class QuestionsApiController : ControllerBase
    {
        private readonly IUmbracoContextFactory _umbracoContextFactory;

        public QuestionsApiController(IUmbracoContextFactory umbracoContextFactory)
        {
            _umbracoContextFactory = umbracoContextFactory;
        }

        [HttpGet]
        public ActionResult GetAllQuestions()
        {
            try
            {
                using (var umbracoContextReference = _umbracoContextFactory.EnsureUmbracoContext())
                {
                    var contentQuery = umbracoContextReference.UmbracoContext.Content;

                    // Get the root content node (replace with actual root node if needed)
                    var rootContent = contentQuery.GetAtRoot().FirstOrDefault();
                    if (rootContent == null)
                    {
                        return NotFound(new { message = "Root content not found" });
                    }

                    // Fetch all questions under a specific content type (replace 'questions' with your content type alias)
                    var questions = rootContent
                        .DescendantsOrSelf("questions")
                        .ToList();

                    if (questions == null || !questions.Any())
                    {
                        return NotFound(new { message = "No questions found" });
                    }

                    var questionList = new List<object>();

                    foreach (var question in questions)
                    {
                        var questionData = new
                        {
                            questionId = question.Value<string>("questionId"),
                            questionText = question.Value<string>("questionText"),
                            quizzType = question.Value<string>("quizzType"),
                            image = question.Value<string>("image"),
                            showToggle = question.Value<bool>("showToggle"),
                            essential = question.Value<bool>("essential"),
                            description = question.Value<string>("description"),
                            answers = question.Value<string>("answers") // Assuming answers is stored as a string, adjust as necessary
                        };

                        questionList.Add(questionData);
                    }

                    return Ok(questionList);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error retrieving questions: {ex.Message}" });
            }
        }
    }

}