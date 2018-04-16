//import { User } from './User';
//import { Participants } from './Participants';

export interface MeetingSchemas
{       
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
    Participants: number[]; 
}