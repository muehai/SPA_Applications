using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MeetingSchema.Model.Entities;
using MeetingSchema.Model;

namespace MeetingSchema.API.ViewModels.Mappings
{
    public class DomainToViewModelMappingProfileForMetAndUser : Profile
    {
        protected void Configure()
        {
            CreateMap<MeetingSchemas, MeetingSchemaViewModel>()
               .ForMember(vm => vm.Creator,
                    map => map.MapFrom(s => s.Creator.Name))
               .ForMember(vm => vm.Participants, map =>
                    map.MapFrom(s => s.Participants.Select(a => a.UserId)));

            CreateMap<MeetingSchemas, MeetingSchemaDetailsViewModel>()
               .ForMember(vm => vm.Creator,
                    map => map.MapFrom(s => s.Creator.Name))
               .ForMember(vm => vm.Participants, map =>
                    map.UseValue(new List<UserViewModel>()))
                .ForMember(vm => vm.Status, map =>
                    map.MapFrom(s => ((MeetingSchemaStatus)s.Status).ToString()))
                .ForMember(vm => vm.Type, map =>
                   map.MapFrom(s => ((MeetingSchemaType)s.Type).ToString()))
               .ForMember(vm => vm.Statuses, map =>
                    map.UseValue(Enum.GetNames(typeof(MeetingSchemaStatus)).ToArray()))
               .ForMember(vm => vm.Types, map =>
                    map.UseValue(Enum.GetNames(typeof(MeetingSchemaType)).ToArray()));

            CreateMap<User, UserViewModel>()
                .ForMember(vm => vm.MeetingSchemaCreated,
                    map => map.MapFrom(u => u.MeetingSchemaCreated.Count()));
        }
    }
}
