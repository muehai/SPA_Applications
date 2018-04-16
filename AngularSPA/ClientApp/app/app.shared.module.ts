import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';
import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';

//Add modules
import { PaginationModule } from 'ngx-bootstrap';
import { DatepickerModule } from 'ngx-bootstrap';
import { ModalModule } from 'ngx-bootstrap';
import { ProgressbarModule } from 'ngx-bootstrap';
import { ToastrModule } from 'toastr-ng2';
import { TimepickerModule } from 'ngx-bootstrap';


//MeetingSchema components
import { MeetingSchemaComponents } from '../app/components/meetingSchemas/MeetingSchema.Component';
import { EditMeetingSchemasComponent } from '../app/components/meetingSchemas/EditMeetingSchemas.Component';
import { userComponent } from '../app/components/users/user.component';

//Import MeetingSchemas services
import { MeetingSchemaService } from '../app/Services/index';
import { ConfigService} from '../app/Extensions/index';
import { ItemsService } from '../app/Extensions/index';
import { MappingService } from '../app/Extensions/index';
import { ToastrService } from 'toastr-ng2';
import { UrlSerializer } from '@angular/router';

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        //Add MeetingSchema Components
        MeetingSchemaComponents,
        EditMeetingSchemasComponent,
        userComponent,
       
      ],
    providers: [
        MeetingSchemaService,
        ConfigService,
        ItemsService,
        ToastrService,
        MappingService,
       
      ], 

    imports: [
        BrowserModule,
        CommonModule,
        HttpModule,
        FormsModule,
        ToastrModule.forRoot(),
        DatepickerModule.forRoot(),
        PaginationModule.forRoot(),
        TimepickerModule.forRoot(),
        ProgressbarModule.forRoot(),
        RouterModule.forRoot([
            
      { path: '', redirectTo: 'meetingSchemas', pathMatch: 'full' },
      { path: 'meetingSchemas', component: MeetingSchemaComponents },
      { path: 'meetingSchemas/:id', component: EditMeetingSchemasComponent },
      { path: 'users', component: userComponent },
      { path: '**', redirectTo: 'meetingSchemas'}

        ])
    ]
})
export class AppModuleShared {
}
