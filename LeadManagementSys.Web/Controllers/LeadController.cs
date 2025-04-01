using LeadManagementSys.Data;
using LeadManagementSys.Handlers.Agents;
using LeadManagementSys.Handlers.Leads;
using LeadManagementSys.Models.DTOs;
using LeadManagementSys.Models.Enums;
using LeadManagementSys.Models.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LeadManagementSys.Web.Controllers
{
    [Authorize]
    public class LeadController : Controller
    {
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;

        public LeadController(IMediator mediator, UserManager<ApplicationUser> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        public async Task<IActionResult> Create()
        {
            if (User.IsInRole("Admin") || User.IsInRole("Manager"))
            {
                var agents = await _userManager.GetUsersInRoleAsync("Agent");
                ViewBag.AgentList = new SelectList(agents, "Id", "FullName");
            }
            else
            {
                ViewBag.AgentId = _userManager.GetUserId(User);
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeadRequest leadRequest)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                var command = new CreateLeadCommand(leadRequest, userId);
                var result = await _mediator.Send(command);
             
                if (result)
                {
                    TempData["success"] = "Lead created successfully!";
                    if (User.IsInRole("SuperAdmin"))
                        return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                    if (User.IsInRole("Admin"))
                        return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                    if (User.IsInRole("Agent"))
                        return RedirectToAction("Index", "Dashboard", new { area = "Agent" });
                    if (User.IsInRole("Manager"))
                        return RedirectToAction("Index", "Dashboard", new { area = "Manager" });
                }
                //else
                //{
                //    TempData["error"] = "Failed to delete the lead!";
                //}
            }

            return View(leadRequest);
        }

        public async Task<IActionResult> GetLeads()
        {
            var leads = await _mediator.Send(new GetAllLeadsQuery());

            if (leads == null || !leads.Any())
            {
                leads = new List<LeadResponse>();
            }

            return Json(leads);
        }

        [HttpGet]
        public async Task<IActionResult> GetAssignedLeads()
        {
            var assignedLeads = await _mediator.Send(new GetDashboard());

            if (assignedLeads == null)
            {
                return Json(new { message = "No leads found for this agent." });
            }

            return Json(assignedLeads.Leads); 
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin, Admin, Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteLeadCommand(id);
            var result = await _mediator.Send(command);

            if (result)
            {
                TempData["success"] = "Lead deleted successfully!";
            }
            else
            {
                TempData["error"] = "Failed to delete the lead!";
            }
            var currentUser = await _userManager.GetUserAsync(User);
            var userRoles = await _userManager.GetRolesAsync(currentUser);
            if (userRoles.Contains("SuperAdmin"))
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
            else if (userRoles.Contains("Admin"))
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
            else if (userRoles.Contains("Manager"))
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Manager" });
            }
            else if (userRoles.Contains("Agent"))
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Agent" });
            }

            return RedirectToAction("Index", "Home");
        }



        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var lead = await _mediator.Send(new GetLeadByIdQuery(id));
            if (lead == null)
            {
                TempData["error"] = "Lead not found.";
                return RedirectToAction("Index");
            }

            var agents = await _userManager.GetUsersInRoleAsync("Agent");
            ViewBag.AgentList = new SelectList(agents, "Id", "FullName", lead.AssignedToId);

            return View(lead);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(LeadUpdateRequest leadRequest)
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(new UpdateLeadCommand(leadRequest));
                if (result)
                {
                    TempData["success"] = "Lead updated successfully.";
                    if (User.IsInRole("SuperAdmin"))
                        return RedirectToAction("Index", "Dashboard", new { area = "Admin" });

                    if (User.IsInRole("Admin"))
                        return RedirectToAction("Index", "Dashboard", new { area = "Admin" });

                    if (User.IsInRole("Manager"))
                        return RedirectToAction("Index", "Dashboard", new { area = "Manager" });

                    if (User.IsInRole("Agent"))
                        return RedirectToAction("Index", "Dashboard", new { area = "Agent" });

                    return RedirectToAction("Index", "Home");
                }
            }
            
            var agents = await _userManager.GetUsersInRoleAsync("Agent");
            ViewBag.AgentList = new SelectList(agents, "Id", "FullName", leadRequest.AssignedToId);

            TempData["error"] = "Error updating lead.";
            return View(leadRequest);
        }


    }
}