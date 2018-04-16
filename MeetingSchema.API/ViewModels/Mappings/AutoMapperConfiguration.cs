using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace MeetingSchema.API.ViewModels.Mappings
{
    //This is automapping for initilialize the to from domain to view and viewmodel to domain
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize( v => 
            {
                v.AddProfile<DomainToViewModelMappingProfileForMetAndUser>();
                v.AddProfile<ViewModelToDomainMappingProfileForMetAndUser>();
            });
       }
    }
}
