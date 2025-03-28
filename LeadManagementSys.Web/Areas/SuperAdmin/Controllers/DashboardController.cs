using LeadManagementSys.Handlers.Features;
using LeadManagementSys.Models.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeadManagementSys.Web.Areas.SuperAdmin.Controllers
{
    [Area("SuperAdmin")]
    [Authorize(Roles = "SuperAdmin")]
    public class DashboardController : Controller
    {
        private readonly IMediator _mediator;

        public DashboardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            var summary = await _mediator.Send(new GetDashboardSummary());

            if (summary == null)
            {
                return View(new DashboardSummaryDto());
            }

            return View(summary);
        }
    }
}
