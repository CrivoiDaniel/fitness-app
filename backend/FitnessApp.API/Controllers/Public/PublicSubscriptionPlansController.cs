using System;
using FitnessApp.Application.Interfaces.Subscriptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessApp.API.Controllers.Public;

[ApiController]
[Route("api/public/subscription-plans")]
[AllowAnonymous]
public class PublicSubscriptionPlansController : ControllerBase
{
    private readonly ISubscriptionPlanService _planService;

    public PublicSubscriptionPlansController(ISubscriptionPlanService planService)
    {
        _planService = planService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PublicSubscriptionPlanDto>>> GetActive(CancellationToken cancellationToken)
    {
        // ia planuri active + benefit package + items (repo face Include)
        var plans = await _planService.GetActiveAsync(cancellationToken);

        var result = plans.Select(p => new PublicSubscriptionPlanDto
        {
            Id = p.Id,
            Type = p.Type, // string (ex: "Monthly")
            Price = p.Price,
            DurationInMonths = p.DurationInMonths,
            BenefitPackageId = p.BenefitPackageId,
            BenefitPackageName = p.BenefitPackageName,
            Benefits = p.BenefitPackage?.Items?
                .Select(i => new PublicBenefitDto
                {
                    Name = i.BenefitName,           // key stabil pt i18n (ex: "pool_access")
                    DisplayName = i.BenefitDisplayName,
                    Value = i.Value
                })
                .ToList() ?? new List<PublicBenefitDto>()
        });

        return Ok(result);
    }

    public class PublicSubscriptionPlanDto
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int DurationInMonths { get; set; }
        public int BenefitPackageId { get; set; }
        public string BenefitPackageName { get; set; } = string.Empty;
        public List<PublicBenefitDto> Benefits { get; set; } = new();
    }

    public class PublicBenefitDto
    {
        public string Name { get; set; } = string.Empty;        // ideal ca “translation key”
        public string DisplayName { get; set; } = string.Empty; // fallback text
        public string Value { get; set; } = string.Empty;
    }
}