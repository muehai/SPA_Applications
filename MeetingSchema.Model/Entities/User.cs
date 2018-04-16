using System;
using System.Collections.Generic;
using System.Text;
using MeetingSchema.Model;
namespace MeetingSchema.Model.Entities
{
    //Class user 
    public class User : IEntityBase
    {
        public User()
        {
            MeetingSchemaCreated = new List<MeetingSchemas>();
            MeetingSchemaAttended = new List<Participants>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Profession { get; set; }
        public ICollection<MeetingSchemas> MeetingSchemaCreated { get; set; }
        public ICollection<Participants> MeetingSchemaAttended { get; set; }
    }
}