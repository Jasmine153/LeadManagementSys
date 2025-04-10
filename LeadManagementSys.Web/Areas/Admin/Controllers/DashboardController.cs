﻿using LeadManagementSys.Handlers.Admin;
using LeadManagementSys.Handlers.SuperAdmin;
using LeadManagementSys.Models.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeadManagementSys.Web.Areas.SuperAdmin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin, Admin")]
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

        public async Task<IActionResult> GetAgents()
        {
            var agents = await _mediator.Send(new GetAgentsQuery());
            return Json(agents);
        }

        public async Task<IActionResult> GetManagers()
        {
            var managers = await _mediator.Send(new GetManagersQuery());
            return Json(managers);
        }


    }
}
