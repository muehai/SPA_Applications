using System;
using System.Collections.Generic;
using System.Text;
using MeetingSchema.Data;
using MeetingSchema.Data.Abstract;
using MeetingSchema.Model.Entities;

namespace MeetingSchema.Data.Repositories
{
    public class ParticipantsReopsitory: EntityBaseRepository<Participants>, IParticipantsRepository
    {
        public ParticipantsReopsitory(MeetingSchemaContext contex): base(contex)
        {

        }
    }
}
