using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingSchema.Model.Entities
{   
    //Participants class for different activities
    public class Participants : IEntityBase
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public int MeetingSchemaId { get; set; }
        public MeetingSchemas MeetingSchemas { get; set; }
    }
}
