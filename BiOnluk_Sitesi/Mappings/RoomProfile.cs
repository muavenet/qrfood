using AutoMapper;
using HemenTanitim.Models;
using HemenTanitim.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HemenTanitim.Mappings
{
    public class RoomProfile : Profile
    {
        public RoomProfile()
        {
            CreateMap<Room, RoomViewModel>();

            CreateMap<RoomViewModel, Room>();
        }
    }
}