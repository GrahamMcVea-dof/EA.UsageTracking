﻿using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using EA.UsageTracking.Core.DTOs;
using EA.UsageTracking.Core.Entities;

namespace EA.UsageTracking.Infrastructure.Features.Applications.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Application, ApplicationDTO>()
                .ForMember(d => d.ApplicationId, o => o.MapFrom(s => s.Id));
            CreateMap<ApplicationDTO, Application>();
        }
    }
}