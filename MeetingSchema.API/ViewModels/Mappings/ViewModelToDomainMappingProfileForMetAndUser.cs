using AutoMapper;
using MeetingSchema.Model.Entities;
using MeetingSchema.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetingSchema.API.ViewModels.Mappings
{
    public class ViewModelToDomainMappingProfileForMetAndUser : Profile
    {
        protected void Configure()
        {
            CreateMap<MeetingSchemaViewModel, MeetingSchemas>()
               .ForMember(s => s.Creator, map => map.UseDestinationValue()) //map.UseValue(null))
               .ForMember(s => s.Participants, map => map.UseValue(new List<Participants>()));

            CreateMap<UserViewModel, User>();
        }
    }
}
