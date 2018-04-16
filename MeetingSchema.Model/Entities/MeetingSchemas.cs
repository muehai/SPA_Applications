using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingSchema.Model.Entities
{
    //MeetingSchema class for meetings plan
    public class MeetingSchemas : IEntityBase
    {
        public MeetingSchemas()
        {
            Participants = new List<Participants>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public string Location { get; set; }
        public MeetingSchemaType Type { get; set; }

        public  MeetingSchemaStatus Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public User Creator { get; set; }
        public int CreatorId { get; set; }
        public ICollection<Participants> Participants { get; set; }
    }
}
