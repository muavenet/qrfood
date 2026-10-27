using AutoMapper;
using HemenTanitim.Helpers;
using HemenTanitim.Models;
using HemenTanitim.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HemenTanitim.Mappings
{
    public class MessageProfile : Profile
    {
        public MessageProfile()
        {
            CreateMap<Message, MessageViewModel>()
                .ForMember(dst => dst.From, opt => opt.MapFrom(x => x.fromUser))
                .ForMember(dst => dst.To, opt => opt.MapFrom(x => x.toUser))
                //.ForMember(dst => dst.Avatar, opt => opt.MapFrom(x => x.FromUser.Avatar))
                .ForMember(dst => dst.Content, opt => opt.MapFrom(x => BasicEmojis.ParseEmojis(x.Content)))
                .ForMember(dst => dst.tarih, opt => opt.MapFrom(x => x.tarih));

            CreateMap<MessageViewModel, Message>();
        }
    }
}