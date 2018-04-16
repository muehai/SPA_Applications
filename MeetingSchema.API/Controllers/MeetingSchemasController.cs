using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MeetingSchema.Data.Abstract;
using MeetingSchema.Model.Entities;
using MeetingSchema.Model;
using MeetingSchema.API.ViewModels;
using MeetingSchema.API.Core;
using AutoMapper;

namespace MeetingSchema.API.Controllers
{
    [Produces("application/json")]
    [Route("api/MeetingSchemas")]
    public class MeetingSchemasController : Controller
    {
        private IMeetingSchemasRepository _meetingSchemasRepository;
        private IParticipantsRepository _participantsRepository;
        private IUserRepository _userRepository;

        //Pagenations 
        int page = 1;
        int pageSize = 4;

        public MeetingSchemasController(IMeetingSchemasRepository mSchemasRepository,
                                        IParticipantsRepository participantsRepository,
                                        IUserRepository userRepository)
        {
            _meetingSchemasRepository = mSchemasRepository;
            _participantsRepository = participantsRepository;
            _userRepository = userRepository;

        }

        public IActionResult Get()
        {
            var pagenation = Request.Headers["Pagination"];

            if (!String.IsNullOrEmpty(pagenation))
            {
                string[] vals = pagenation.ToString().Split(',');
                int.TryParse(vals[0], out page);
                int.TryParse(vals[1], out pageSize);
            }

            int currentPage = page;
            int currentPageSize = pageSize;
            var totalMeetingSchema = _meetingSchemasRepository.Count();
            var totalPages = (int)Math.Ceiling((double)totalMeetingSchema / pageSize);

            IEnumerable<MeetingSchemas> _meetingSchemas = _meetingSchemasRepository
              .AllIncluding(e => e.Creator, e => e.Participants)
              .OrderBy(e => e.Id)
              .Skip((currentPage - 1) * currentPageSize)
              .Take(currentPageSize)
              .ToList();

            Response.AddPagination(page, pageSize, totalMeetingSchema, totalPages);
            //Mapp MeetingSchema model to ViewModel
            IEnumerable<MeetingSchemaViewModel> _meetingSchemaVM = Mapper.Map<IEnumerable<MeetingSchemas>, IEnumerable<MeetingSchemaViewModel>>(_meetingSchemas);

            return new OkObjectResult(_meetingSchemaVM);
        }

        [HttpGet("{Id}", Name = "GetMeetingSchema")]
        public IActionResult Get(int id)
        {
            MeetingSchemas _meetingSchemas = _meetingSchemasRepository
                .GetSingle(e => e.Id == id, e => e.Creator, e => e.Participants);

            if (_meetingSchemas != null)
            {
                MeetingSchemaViewModel meetingSchemaVM = Mapper.Map<MeetingSchemas, MeetingSchemaViewModel>(_meetingSchemas);

                return new OkObjectResult(meetingSchemaVM);
            }
            else
            {
                return NotFound();

            }
        }

        [HttpGet("{Id}/Details", Name = "GetMeetingDetailsSchema")]
        public IActionResult GetMeetingDetialsSchema(int id)
        {
            MeetingSchemas _meetingSchemas = _meetingSchemasRepository
                .GetSingle(e => e.Id == id, e => e.Creator, e => e.Participants);

            if (_meetingSchemas != null)
            {
                MeetingSchemaDetailsViewModel meetingDatailsSchemaVM = Mapper.Map<MeetingSchemas, MeetingSchemaDetailsViewModel>(_meetingSchemas);
                foreach (var participants in _meetingSchemas.Participants)
                {
                    User users = _userRepository.GetSingle(participants.UserId);
                    meetingDatailsSchemaVM.Participants.Add(Mapper.Map<User, UserViewModel>(users));
                }

                return new OkObjectResult(meetingDatailsSchemaVM);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody] MeetingSchemaViewModel meetingSchema)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            MeetingSchemas NewMSchemas = Mapper.Map<MeetingSchemaViewModel, MeetingSchemas>(meetingSchema);
            NewMSchemas.DateCreated = DateTime.Now;
            _meetingSchemasRepository.Add(NewMSchemas);
            _meetingSchemasRepository.Commit();

            foreach (var item in meetingSchema.Participants)
            {
                NewMSchemas.Participants.Add(new Participants { UserId = item });

            }
            _meetingSchemasRepository.Commit();

            meetingSchema = Mapper.Map<MeetingSchemas, MeetingSchemaViewModel>(NewMSchemas);

            //Create 
            CreatedAtRouteResult result = CreatedAtRoute("GetMeetingSchema", new {controller = "MeetingSchemas", Id = meetingSchema.Id }, meetingSchema);

            return result;

        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] MeetingSchemaViewModel meetingSchemaViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            MeetingSchemas _meetingSchemas = _meetingSchemasRepository.GetSingle(id);

            if (_meetingSchemas == null)
            {
                return NotFound();
            }
            else
            {
                _meetingSchemas.Title = meetingSchemaViewModel.Title;
                _meetingSchemas.Location = meetingSchemaViewModel.Location;
                _meetingSchemas.Description = meetingSchemaViewModel.Description;
                _meetingSchemas.Status = (MeetingSchemaStatus)Enum.Parse(typeof(MeetingSchemaStatus), meetingSchemaViewModel.Status);
                _meetingSchemas.Type = (MeetingSchemaType)Enum.Parse(typeof(MeetingSchemaType), meetingSchemaViewModel.Type);
                _meetingSchemas.TimeStart = meetingSchemaViewModel.TimeStart;
                _meetingSchemas.TimeEnd = meetingSchemaViewModel.TimeEnd;

                _participantsRepository.DeleteWhere(e => e.MeetingSchemaId == id);

                foreach (var user_id in meetingSchemaViewModel.Participants)
                {
                    _meetingSchemas.Participants.Add(new Participants { MeetingSchemaId = id, UserId = id });
                }

                _meetingSchemasRepository.Commit();
            }

                //Mapp to ViewModel from DB
                meetingSchemaViewModel = Mapper.Map<MeetingSchemas, MeetingSchemaViewModel>(_meetingSchemas);

            return new NoContentResult();
        }

        [HttpDelete("{id}", Name = "RemoveMeetingSchema")]
        public IActionResult Delete(int id)
        {
            MeetingSchemas _meetingSchemas = _meetingSchemasRepository.GetSingle(id);
            if (_meetingSchemas == null)
            {
                return new NoContentResult();
            }
            else
            {
                _participantsRepository.DeleteWhere(e => e.MeetingSchemaId == id);
                _meetingSchemasRepository.Delete(_meetingSchemas);
                _meetingSchemasRepository.Commit();

                return new NoContentResult();
            }
        }

        [HttpDelete("{id}/removeparticipants/{participants}")]
        public IActionResult Delete(int id, int participants)
        {
            MeetingSchemas _meetingSchemas = _meetingSchemasRepository.GetSingle(id);

            if (_meetingSchemas == null)
            {
                return new NotFoundResult();
            }
            else
            {
                _participantsRepository.DeleteWhere(e => e.MeetingSchemaId == id && e.UserId == participants);
                _participantsRepository.Commit();

                return new NotFoundResult();
            }
        }
    }
}