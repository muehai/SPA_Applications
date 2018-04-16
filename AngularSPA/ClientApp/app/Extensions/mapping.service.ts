import { Injectable } from '@angular/core';

import { MeetingSchemas} from '../Models/MeetingSchemas';
import { User } from '../Models/User';
import { MeetingSchemaDetails } from '../Models/MeetingSchemaDetails';
import { ItemsService } from './items.service';
import { Predicate } from './IPagination';


@Injectable()
export class MappingService {
    constructor(private itemsService: ItemsService) { }

    mapMeetingSchemaDetails(meetingSchemaDetails: MeetingSchemaDetails): MeetingSchemas {
        var ms: MeetingSchemas = {
            Id: meetingSchemaDetails.Id,
            Title: meetingSchemaDetails.Title,
            Description: meetingSchemaDetails.Description,
            TimeStart: meetingSchemaDetails.TimeStart,
            TimeEnd: meetingSchemaDetails.TimeEnd,
            Location: meetingSchemaDetails.Location,
            Type: meetingSchemaDetails.Type,
            Status: meetingSchemaDetails.Status,
            DateCreated: meetingSchemaDetails.DateCreated,
            DateUpdated: meetingSchemaDetails.DateUpdated,
            Creator: meetingSchemaDetails.Creator,
            CreatorId: meetingSchemaDetails.CreatorId,
            Participants: this.itemsService.getPropertyValues<User, number[]>(meetingSchemaDetails.Participants, 'id')
        }

        return ms;
    }
}