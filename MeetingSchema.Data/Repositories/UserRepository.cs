using MeetingSchema.Data.Abstract;
using MeetingSchema.Model.Entities;
using MeetingSchema.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingSchema.Data.Repositories
{
    public class UserRepository : EntityBaseRepository<User>, IUserRepository
    {
        public UserRepository(MeetingSchemaContext contex) :base(contex)
        {

        }
    }
}
