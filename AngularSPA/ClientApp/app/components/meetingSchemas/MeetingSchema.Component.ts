import { Component, OnInit } from '@angular/core';
import { MeetingSchemas } from '../../Models/MeetingSchemas';
import { User } from '../../Models/User';
import { Participants } from '../../Models/Participants';
import { MeetingSchemaDetails } from '../../Models/MeetingSchemaDetails';
import { Pagination} from '../../Extensions/IPagination';
import { PaginatedResult } from '../../Extensions/IPagination';

//Services for MeetingSchemas
import { MeetingSchemaService } from '../../Services/index'
import { ConfigService } from '../../Extensions/Config.Service';
import { ItemsService } from '../../Extensions/Items.Service';
import { MappingService } from '../../Extensions/mapping.service';
import { ToastrService } from 'toastr-ng2';

@Component({
    
    selector: 'MeetingSchema',
    templateUrl: './Meetingschema.Component.html'
})

export class MeetingSchemaComponents implements OnInit {

    public meetingSchema: MeetingSchemas[];
    public currentPage: number = 1;
    public itemsToShowPage: number = 2;
    public totalItems: number = 5;
    public model: any;
    public selected: string;
    public output: string;
    public selectedMeetinghSchemaId: number;
    public meetingsSchemaDetails: MeetingSchemaDetails;
    public IsSelectedMeetingSchemaLoaded: boolean = false;
    public index: number = 0;

    //Constructor
    constructor(private meetingSchemaService: MeetingSchemaService,
                private itemService: ItemsService,
                private configService: ConfigService,
                private toastrService: ToastrService) { }

    //Load MeetingSchema Data
    ngOnInit() {
        
        this.loadMeetingSchemaData();
    }

    // Load MeetingSchema 
   loadMeetingSchemaData() {
       this.meetingSchemaService.getMeetingSchemaNavPages(this.currentPage, this.itemsToShowPage)
            .subscribe((res: MeetingSchemas[]) => {  //res: PaginatedResult < MeetingSchemas[] >) => {
            
                this.meetingSchema = res; //res.result;
                this.totalItems = this.totalItems; //res.pagination.TotalItems;
              
            },
             
            error => {
               
                this.toastrService.error('Failed to load meetingSchema data.' + error);
            })
    }

   TriggerPageChange(event: any): void {

       this.currentPage = event.page();
       this.loadMeetingSchemaData();
   }
   
   RemoveMeetingSchemasObj(meetingSchema: MeetingSchemas) {
       this.toastrService.warning('Are you sure you wanted to remove this meetingSchema ?');
      

       this.meetingSchemaService.DeletMeetingSchemaById(meetingSchema.Id)
           .subscribe(() => {
               this.itemService.removeItemFromArray<MeetingSchemas>(this.meetingSchema, meetingSchema);
               this.toastrService.success(meetingSchema.Title + 'has been removed.');
              
           },
           error => {
               this.toastrService.success(meetingSchema.Title + 'has been removed.')
             
           });
   }

   DisplayMeetingSchemaDetailsById(id: number) {
       this.selectedMeetinghSchemaId = id;

       this.meetingSchemaService.GetMeetingDetialsSchemaById(this.selectedMeetinghSchemaId)
           .subscribe((meetingSchemaDetail: MeetingSchemaDetails[]) => {
               this.meetingsSchemaDetails = this.itemService.getSerialized<MeetingSchemaDetails>(meetingSchemaDetail)
               this.meetingsSchemaDetails.TimeStart = new Date(this.meetingsSchemaDetails.TimeStart.toLocaleString()); 
               this.meetingsSchemaDetails.TimeEnd = new Date(this.meetingsSchemaDetails.TimeEnd.toLocaleString());
             
               this.IsSelectedMeetingSchemaLoaded = true;
           },
           error => {
             
               this.toastrService.error('MeetingSchema is failed to load and show data.' + error);
           });
   }

}
 

