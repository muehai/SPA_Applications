import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { NgForm } from '@angular/forms';
import { MeetingSchemas } from '../../Models/MeetingSchemas';
import { User } from '../../Models/User';
import { MeetingSchemaDetails } from '../../Models/MeetingSchemaDetails';
import { Pagination } from '../../Extensions/IPagination';
import { PaginatedResult } from '../../Extensions/IPagination';

//Services for MeetingSchemas
import { MeetingSchemaService } from '../../Services/index'
import { ConfigService } from '../../Extensions/Config.Service';
import { ItemsService } from '../../Extensions/Items.Service';
import { MappingService } from '../../Extensions/mapping.service';

import { ToastrService } from 'toastr-ng2';

//Declare class of MeetingSchema instance
class MeetingSchemasEditComponentInfo implements MeetingSchemas {

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

    constructor(public id: number, public title: string, public description: string,
                public timeStart: Date, public timeEnd: Date, public location: string,
                public type: MeetingSchemaType, public status: MeetingSchemaStatus,
                public dateCreated: Date, public dateUpdated: Date, public creator: string, public creatorId: number, participants:number[]) {
    }
}


@Component({
    selector: 'EditMeetingSchemas',
    templateUrl: './EditMeetingSchemas.Component.html'
})

export class EditMeetingSchemasComponent implements OnInit {
   public router: any;
   private id: number;
   public meetingSchemaDetails: MeetingSchemaDetails;
   public meetingSchemasLoaded: boolean = false;
   public statuses: string[];
   public type: string[];
   private sub: any;

    constructor(private meetingSchemaService: MeetingSchemaService,
                private itemService: ItemsService,
                private configService: ConfigService,
                private toastrService: ToastrService,
                private mappingService: MappingService)
    { }

    //Load MeetingSchema Data
    ngOnInit() {
        this.LoadMeetingSchemaDetailsData();
    }
    //Load MeetingSchema obj to be edited
    LoadMeetingSchemaDetailsData() {
           this.meetingSchemaService.GetMeetingDetialsSchemaById(this.id)
            .subscribe((meetingSchemaDetails: MeetingSchemaDetails[]) => {
                this.meetingSchemaDetails = this.itemService.getSerialized<MeetingSchemaDetails>(meetingSchemaDetails)
                this.meetingSchemasLoaded = true;
                this.meetingSchemaDetails.TimeStart = new Date(this.meetingSchemaDetails.TimeStart.toString());
                this.meetingSchemaDetails.TimeEnd = new Date(this.meetingSchemaDetails.TimeEnd.toString());
                this.statuses = this.meetingSchemaDetails.Statuses;
                this.type = this.meetingSchemaDetails.Types;
              

            },
            error => { 
              
                this.toastrService.error('Failed to load MeetingSchemas obj. ' + error)
            });
    }
    //update the Details 
    UpdateMeetingSchemasData(editMeetingSchema: NgForm) {
        console.log(editMeetingSchema.value);
        var mappingMeetingSchemaObj = this.mappingService.mapMeetingSchemaDetails(this.meetingSchemaDetails);
       
        this.meetingSchemaService.UpdateMeetingSchemaData(mappingMeetingSchemaObj)
            .subscribe(() => {
                this.toastrService.success('MeetingSchemas has been updated. ');
                
            },
            error => {
                
                this.toastrService.error('Updating MeetigSchema is failed.' + error);
            });
    }
    //Delete meetings paricipants
    DeleteParicipants(participants: User) {
        this.toastrService.info('Are you sure to delete ?', participants.Name + 'from the current meeting.');
       
        this.meetingSchemaService.DeleteMSAndParticipanstById(this.meetingSchemaDetails.Id, participants.Id)
            .subscribe(() => {
                this.itemService.removeItemFromArray<User>(this.meetingSchemaDetails.Participants, participants);
                this.toastrService.success(participants.Name + 'is not participants for the current meetingSchema.')
               
            },
                error => {
                  
                    this.toastrService.error(participants.Name + 'Failed to move the current participanst.' + error);
                
            });
    }
    Back() {
        this.router.navigate(['/meetingSchemas']);
    }


}