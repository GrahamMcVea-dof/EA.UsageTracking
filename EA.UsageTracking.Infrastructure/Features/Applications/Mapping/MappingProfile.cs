﻿using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using EA.UsageTracking.Core.DTOs;
using EA.UsageTracking.Core.Entities;
using EA.UsageTracking.Infrastructure.Features.Applications.Queries;

namespace EA.UsageTracking.Infrastructure.Features.Applications.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Application, ApplicationDTO>()
                .ForMember(d => d.ApplicationId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.ClientId, o => o.MapFrom(s => s.TenantId));
            CreateMap<ApplicationDTO, Application>();
            CreateMap<GetAllApplicationsQuery, Pagination.PaginationDetails>();
        }
    }
}
