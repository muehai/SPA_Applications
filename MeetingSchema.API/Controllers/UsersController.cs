using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MeetingSchema.Data.Abstract;
using MeetingSchema.API.ViewModels;
using MeetingSchema.Model.Entities;
using MeetingSchema.Model;
using AutoMapper;
using MeetingSchema.API.Core;
using System.Collections.ObjectModel;

namespace MeetingSchema.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Users")]
    public class UsersController : Controller
    {
        int page = 1;
        int pageSize = 12;
        private IMeetingSchemasRepository _meetingSchemasRepository;
        private IParticipantsRepository _participantsRepository;
        private IUserRepository _userRepository;


        public UsersController(IMeetingSchemasRepository mSchemasRepository,
                                        IParticipantsRepository participantsRepository,
                                        IUserRepository userRepository)
        {
            _meetingSchemasRepository = mSchemasRepository;
            _participantsRepository = participantsRepository;
            _userRepository = userRepository;
        }

        public IActionResult Get()
        {
            var pagination = Request.Headers["Pagination"];
            if (!string.IsNullOrEmpty(pagination))
            {
                string[] vals = pagination.ToString().Split(',');
                int.TryParse(vals[0], out page);
                int.TryParse(vals[1], out pageSize);
            }
                //Initialize the pages
                int currentPage = page;
                int currentPageSize = pageSize;
                int totalUsers = _userRepository.Count();
                int totalPages = (int)Math.Ceiling((double)totalUsers / pageSize);

            IEnumerable<User> users = _userRepository
                .AllIncluding(e => e.MeetingSchemaCreated)
                .OrderBy(e => e.Id)
                .Skip((currentPage - 1) * currentPageSize)
                .Take(currentPageSize)
                .ToList();

            IEnumerable<UserViewModel> userVM = Mapper.Map<IEnumerable<User>, IEnumerable<UserViewModel>> (users);
            Response.AddPagination(page, pageSize, totalUsers, totalPages);

            return new OkObjectResult(userVM);
        }

        [HttpGet("{id}", Name = "GetUser")]
        public IActionResult Get(int id)
        {
            User users = _userRepository.GetSingle(e => e.Id == id, e => e.MeetingSchemaCreated);

            if (users != null)
            {
                UserViewModel usersVM = Mapper.Map<User, UserViewModel>(users);

                return new OkObjectResult(usersVM);
            }
            else
            {
                //Add log file here
                return NotFound();
            }
        }

        [HttpGet("{id}/meetingschemas", Name = "GetMeetingSchemas")]
        public IActionResult GetMeetingSchemas(int id)
        {
            IEnumerable<MeetingSchemas> meetingSchema = _meetingSchemasRepository.FindBy(s => s.CreatorId == id);

            if (meetingSchema != null)
            {
                IEnumerable<MeetingSchemaViewModel> _userSchedulesVM = Mapper.Map<IEnumerable<MeetingSchemas>, IEnumerable<MeetingSchemaViewModel>>(meetingSchema);
                return new OkObjectResult(_userSchedulesVM);
            }
            else
            {
                return NotFound();
            }
        }
        
        [HttpPost]
        public IActionResult Create([FromBody]UserViewModel users)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            User newUsers = new User { Name = users.Name, Profession = users.Profession, Avatar = users.Avatar };
            //Add to DB and save
            _userRepository.Add(newUsers);
            _userRepository.Commit();
            //Mapp user to VM
            users = Mapper.Map<User, UserViewModel>(newUsers);
            CreatedAtRouteResult result = CreatedAtRoute("GetUser", new { controller = "Users", id = users.Id }, users);

            return result;
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UserViewModel userViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User usersdata = _userRepository.GetSingle(id);
            if (usersdata == null)
            {
                return NotFound();
            }
            else
            {
                usersdata.Name = userViewModel.Name;
                usersdata.Profession = userViewModel.Profession;
                usersdata.Avatar = userViewModel.Avatar;
                _userRepository.Commit();
            }

            //Mapp user to UsersVM
            userViewModel = Mapper.Map<User, UserViewModel>(usersdata);

            return new NoContentResult();

        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            User usersData = _userRepository.GetSingle(id);
            if (usersData == null)
            {
                return new NotFoundResult();
            }
            else
            {
                IEnumerable<Participants> participants = _participantsRepository.FindBy(e => e.UserId == id);
                IEnumerable<MeetingSchemas> meetingSchemas = _meetingSchemasRepository.FindBy(e => e.CreatorId == id);

                foreach (var participant in participants)
                {
                    _participantsRepository.Delete(participant);
                }

                foreach (var meetingSchmema in meetingSchemas)
                {
                    _participantsRepository.DeleteWhere(e => e.MeetingSchemaId == meetingSchmema.Id);
                    _meetingSchemasRepository.Delete(meetingSchmema);
                }
                _userRepository.Delete(usersData);

                return new NoContentResult();
            }
        }
    }//End UsersController
}