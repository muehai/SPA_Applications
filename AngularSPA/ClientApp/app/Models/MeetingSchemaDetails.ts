import { User } from './User';

export interface MeetingSchemaDetails {
    Id: number;
    Title: string;
    Description: string;
    TimeStart: Date;
    TimeEnd: Date;
    Location: string;
    Type: string;
    Status: string;
    DateCreated: Date;
    DateUpdated: Date;
    Creator: string;
    CreatorId: number;
    Participants: User[];
    Statuses: string[];
    Types: string[];
}