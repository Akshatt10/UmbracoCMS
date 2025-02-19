using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Cms.Core.Web;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UmbracoProject.Controllers
{
    [Route("umbraco/api/quiz")]
    [ApiController]
    public class QuizApiController : ControllerBase
    {
        private readonly IUmbracoContextFactory _contextFactory;

        public QuizApiController(IUmbracoContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        [HttpGet]
        public IActionResult GetQuiz()
        {
            using (var context = _contextFactory.EnsureUmbracoContext())
            {
                // Fetch the root content and locate the quiz page
                var quizPage = context.UmbracoContext.Content.GetAtRoot()
                    .FirstOrDefault(x => x.ContentType.Alias == "quizPage");

                if (quizPage == null)
                    return NotFound("Quiz page not found.");

                // Fetch the questions block (ensure 'Questions' is the correct alias of the questions field)
                var questionsBlock = quizPage.Value<IEnumerable<IPublishedElement>>("questions");
                if (questionsBlock == null || !questionsBlock.Any())
                {
                    return NotFound("No questions found in the quiz.");
                }

                // Prepare the list of questions and their answers
                var questions = questionsBlock
                    .Select(question =>
                    {
                        // Fetch the answers options (ensure 'answersOptions' is the correct alias)
                        var answersBlock = question.Value<IEnumerable<IPublishedElement>>("answersOptions");

                        return new
                        {
                            // Extract the text for the question
                            QuestionText = question.Value<string>("QuestionText"),

                            // Extract the answers for this question
                            Answers = answersBlock?.Select(answer => new
                            {
                                AnswerText = answer.Value<string>("Option1"), // Ensure this matches the alias
                                IsCorrect = answer.Value<bool>("IsCorrect")  // Ensure this matches the alias
                            }).ToList()
                        };
                    })
                    .ToList();

                return Ok(questions);
            }
        }
    }
}
