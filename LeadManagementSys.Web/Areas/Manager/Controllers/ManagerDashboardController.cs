using LeadManagementSys.Handlers.Admin;
using LeadManagementSys.Handlers.Managers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeadManagementSys.Web.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Roles = "Manager")]
    public class ManagerDashboardController : Controller
    {
        private readonly IMediator _mediator;

        public ManagerDashboardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            var dashboard = await _mediator.Send(new GetManagerDashboard());

            return View(dashboard);
        }
        public async Task<IActionResult> GetAgents()
        {
            var agents = await _mediator.Send(new GetAgentsQuery());
            return Json(agents);
        }
    }

}
