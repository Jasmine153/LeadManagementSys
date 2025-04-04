﻿using LeadManagementSys.Models.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManagementSys.Handlers.Admin
{
    public class GetAgentsQuery : IRequest<List<AgentResponse>>
    {
    }
}
