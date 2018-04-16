import { User } from './User';
import { MeetingSchemas } from './MeetingSchemas';

export interface Participants
{
    Id: number;
    UserId: number;
    User: User;
    MeetingSchemaId: number;
    MeetingSchemas: MeetingSchemas;
}