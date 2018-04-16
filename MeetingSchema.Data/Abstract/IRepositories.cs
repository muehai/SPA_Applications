using System;
using System.Collections.Generic;
using System.Text;
using MeetingSchema.Model.Entities;

namespace MeetingSchema.Data.Abstract
{
    public interface IMeetingSchemasRepository : IEntityBaseRepository<MeetingSchemas> { }
    public interface IUserRepository : IEntityBaseRepository<User> { }
    public interface IParticipantsRepository : IEntityBaseRepository<Participants> { }
}
