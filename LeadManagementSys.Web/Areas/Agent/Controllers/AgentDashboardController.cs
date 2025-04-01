using LeadManagementSys.Handlers.Agents;
using LeadManagementSys.Handlers.SuperAdmin;
using LeadManagementSys.Models.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeadManagementSys.Web.Areas.Agent.Controllers
{
    [Area("Agent")]
    [Authorize(Roles = "Agent")]
    public class AgentDashboardController : Controller
    {
        private readonly IMediator _mediator;

        public AgentDashboardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetString("UserId");
            Console.WriteLine($"UserId in session: {userId}");
            var summary = await _mediator.Send(new GetDashboard());

            if (summary == null)
            {
                return View(new AgentDashboardDto());
            }

            return View(summary);
        }
    }
}
