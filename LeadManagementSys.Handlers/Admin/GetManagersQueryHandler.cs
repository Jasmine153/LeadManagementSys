using LeadManagementSys.Data;
using LeadManagementSys.Models.DTOs;
using LeadManagementSys.Models.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManagementSys.Handlers.Admin
{
    public class GetManagersQueryHandler : IRequestHandler<GetManagersQuery, List<ManagerResponse>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly LeadDbContext _context;

        public GetManagersQueryHandler(UserManager<ApplicationUser> userManager, LeadDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<List<ManagerResponse>> Handle(GetManagersQuery request, CancellationToken cancellationToken)
        {
            var managers = await _context.Users
                .OfType<Manager>()
                .ToListAsync(cancellationToken);

            var managerResponses = managers.Select(m => new ManagerResponse
            {
                Id = m.Id,
                Name = m.FullName,
                Email = m.Email,
                AdminName = m.AdminId != null
                            ? _context.Users
                                .Where(a => a.Id == m.AdminId)
                                .Select(a => a.FullName)
                                .FirstOrDefault() ?? "N/A"
                            : "N/A"
            }).ToList();

            return managerResponses;
        }

    }
}
