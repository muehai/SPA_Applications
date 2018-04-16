using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingSchema.Model
{
    //Using Id mapping to primary Id to databases 
    public interface IEntityBase
    {
       int Id { get; set; }
    }
}
