using MeetingSchema.Data.Abstract;
using MeetingSchema.Model.Entities;
using MeetingSchema.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingSchema.Data.Repositories
{
    public class MeetingSchemasRepository : EntityBaseRepository<MeetingSchemas>, IMeetingSchemasRepository
    {
        public MeetingSchemasRepository(MeetingSchemaContext contex) : base(contex)
        {

        }
    }
}
