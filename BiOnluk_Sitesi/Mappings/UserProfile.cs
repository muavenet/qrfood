using AutoMapper;
using HemenTanitim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HemenTanitim.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, HemenTanitim.Models.ViewModels.UserViewModel>()
                .ForMember(dst => dst.Username, opt => opt.MapFrom(x => x.UserName));
        }
    }
}