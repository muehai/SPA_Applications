using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace MeetingSchema.API.ViewModels.Validations
{
    public class MeetingSchemaViewModelValidator : AbstractValidator<MeetingSchemaViewModel>
    {
        public MeetingSchemaViewModelValidator()
        {
            RuleFor(s => s.TimeEnd).Must((start, end) =>
            {
                return DateTimeIsGreater(start.TimeStart, end);
            }).WithMessage("MeetingSchema's Start time must be less than the End time.");
        }

        private bool DateTimeIsGreater(DateTime start, DateTime end)
        {
            return end > start;
        }

    }
}
