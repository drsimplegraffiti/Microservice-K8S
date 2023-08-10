using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformService.Profiles
{
    public class PlatformsProfile: Profile
    {
        public PlatformsProfile()
        {
            // Source -> Target
            // Platform is the source and PlatformReadDto is the target
            CreateMap<Platform, PlatformReadDto>();
            CreateMap<PlatformCreateDto, Platform>();
            CreateMap<PlatformUpdateDto, Platform>().ReverseMap(); // ReverseMap() allows us to map from Platform to PlatformUpdateDto
        }
    }
}