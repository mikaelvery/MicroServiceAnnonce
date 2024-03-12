using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdService.DTOs;
using AdService.Models;
using AutoMapper;
using Events;

namespace AdService.Helpers
{
    public class ProfileMapper:Profile
    {
        public ProfileMapper(){
        CreateMap<Ad, AdDto>();
        CreateMap<CreateAdDto, Ad>();
        CreateMap<AdDto, AdCreated>();
        CreateMap<Ad, AdUpdated>();
        }
    }
}    