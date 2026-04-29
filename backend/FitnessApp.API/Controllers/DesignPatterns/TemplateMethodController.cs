using Microsoft.AspNetCore.Mvc;
using FitnessApp.Application.TemplateMethod;
using System.Threading.Tasks;

namespace FitnessApp.API.Controllers.DesignPatterns
{
    [ApiController]
    [Route("api/[controller]")]
    public class TemplateMethodController : ControllerBase
    {
        [HttpGet("generate/{type}")]
        public async Task<IActionResult> GenerateReport(string type, [FromQuery] int clientId)
        {
            ProgressReportGenerator generator;

            // Decidem ce "implementare" de algoritm folosim
            if (type.ToLower() == "weightloss")
            {
                generator = new WeightLossReportGenerator();
            }
            else if (type.ToLower() == "strength")
            {
                generator = new StrengthReportGenerator();
            }
            else
            {
                return BadRequest("Tip de raport necunoscut. Folosiți 'weightloss' sau 'strength'.");
            }

            // Rulăm algoritmul șablon (Template Method)
            var result = await generator.GenerateReportAsync(clientId);

            return Ok(result);
        }
    }
}
