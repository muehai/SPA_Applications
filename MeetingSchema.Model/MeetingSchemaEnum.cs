using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingSchema.Model
{
    //Using to classified different type of services
    public enum MeetingSchemaType
    {
        Work = 1,
        Coffe = 2,
        Doctor = 3,
        Shopping = 4,
        Others = 5
    }

    public enum MeetingSchemaStatus
    {
        Valid = 1,
        Cancelled = 2
    }
}
