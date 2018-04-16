import { Injectable, Inject } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { MeetingSchemas} from '../Models/MeetingSchemas';
import { User } from '../Models/User';
import { MeetingSchemaDetails} from '../Models/MeetingSchemaDetails';
import { Participants } from '../Models/Participants';

import { ItemsService} from '../Extensions/Items.Service';
import { MappingService } from '../Extensions/mapping.service';
import { Pagination } from '../Extensions/IPagination';
import { ConfigService } from '../Extensions/Config.Service';
import { PaginatedResult } from '../Extensions/index';
import { Observable } from 'rxjs/Observable';
import "rxjs/Rx";

@Injectable()
export class MeetingSchemaService {

    //CRUD operations for User Controllers
    public _getUserAllUrl: string; 
    public _getUsersByIdUrl: string;
    public _getMeetingSchemasUrl: string;
    public _createUserUrl: string; 
    public _putUrl: string; 
    public _deleteUlr: string; 

    //Urls to operate the CRUD operations for MeetingSchemas Controller
    public _getMeetingSchemaUrl: string;
    public _getMeetingSchemaByIdUrl: string; 
    public _getMeetingDetialsSchemaById : string; 
    public _createMeetingSchemaUrl: string; 
    public _updateMeetingSchemaUrl: string; 
    public _deleteByIdUrl: string; 
    public _deletMeetingSchemaAndParticipanstByIdUrl: string;

 constructor(private http: Http, private itemservice: ItemsService, private configService: ConfigService) {

        //initialize the MeetingSchema url
        this._getUserAllUrl = '/User/Get';
        this._getUsersByIdUrl = '/User/Get/';
        this._getMeetingSchemasUrl = '/User/Details/';
        this._createUserUrl = '/User/Create/';
        this._putUrl = '/User/Put/';
        this._deleteUlr = '/User/Delete/';

        //initialize the User Url
        this._getMeetingSchemaUrl = '/MS/Get';
        this._getMeetingSchemaByIdUrl = '/MS/GetById/';
        this._getMeetingDetialsSchemaById = '/MS/Details/';
        this._createMeetingSchemaUrl = '/MS/Create/';
        this._updateMeetingSchemaUrl = '/MS/Put/';
        this._deleteByIdUrl = '/MS/Delete/';
        this._deletMeetingSchemaAndParticipanstByIdUrl = '/MS/Delete/';
    }

     //CRUD operation for Users obj
     //Get all users
    getUser(): Observable<User[]> {

        var headers = new Headers({'Content-Type': 'application/json'});
        var getUserAllUrl =  this._getUserAllUrl + 'users';

        return this.http.get(getUserAllUrl, {headers: headers})
            .map((res: Response) => {
                return res.json();
            })
             .catch(this.ErrorHandler);
     }

    //Get users meetingSchema by thiere id
    getUserMeetingSchemaById(id: number): Observable<MeetingSchemas> {

        var getMeetingSchemasUrl = this._getMeetingSchemasUrl + 'users/' + id + 'meetingSchemas';

        return this.http.get(getMeetingSchemasUrl)
            .map(res => res.json().message)
            .catch(this.ErrorHandler);
    }

    //Create new user and add to DB
    createUser(user: User): Observable<User> {

        let body = JSON.stringify(user);
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });

        return this.http.post(this._createUserUrl + 'users/', body, options)
            .map(res => res.json().message)
            .catch(this.ErrorHandler);
    }

    //Update user DB
    updateUser(user: User): Observable<any> {

        let headers = new Headers();
        headers.append('Content-Type', 'application/json');

        return this.http.put(this._putUrl + 'users/' + user.Id, JSON.stringify(user), {
            headers: headers
        })
            .map((res: Response) => {
                return;
            })
            .catch(this.ErrorHandler);
    }

    //Delete users
   deleteUser(id: number): Observable<User>
    {
        var deleteUrl = this._deleteUlr + 'users/' + id;

        return this.http.delete(deleteUrl)
            .map(res => res.json().message)
            .catch(this.ErrorHandler)
    }


    //CRUD operation for MeetingSchemas obj  
    GetMeetingSchema():Observable<MeetingSchemas[]>
    {
        var headers = new Headers();
        headers.append('Content-Type', 'application/json');
        var getMSUrl = this._getMeetingSchemaUrl;
        return this.http.get(getMSUrl, { headers: headers })
            .map(respones => <any>(<Response>respones).json());
    }

    GetMeetingSchemaById(id: number): Observable<MeetingSchemas[]>  {
        var getMeetingSchemaById = this._getMeetingSchemaByIdUrl + 'meetingSchemas/' + id ; //meetingSchemas

        return this.http.get(getMeetingSchemaById)
            .map(respones => respones.json().message)
            .catch(this.ErrorHandler);
    }
     //Get details MS based on thier Id
    GetMeetingDetialsSchemaById(id: number): Observable<MeetingSchemaDetails[]> {
   
        var getMeetingDetialsSchemaById = this._getMeetingDetialsSchemaById + '/meetingSchemas' + id + '/GetMeetingDetailsSchema' ;

        return this.http.get(getMeetingDetialsSchemaById)
            .map(respones => respones.json().messsage)
            .catch(this.ErrorHandler);
    }

     //Post MeetingSchemas data 
    CreateMeetingSchemaData(meetingSchema: MeetingSchemas): Observable<MeetingSchemas>
    {
        let body = JSON.stringify(meetingSchema);
        let headers = new Headers({ 'Content-Type': 'application/json' })
        let options = new RequestOptions({ headers: headers });

        return this.http.post(this._createMeetingSchemaUrl, body, options).
            map(res => res.json().message).catch(this.ErrorHandler);

    }

    //Update MS obj
    UpdateMeetingSchemaData(meetingSchema: MeetingSchemas): Observable<MeetingSchemas> { //Observable<string>

        let body = JSON.stringify(meetingSchema);
        let headers = new Headers({ 'Content-Type': 'application/json' })
        let options = new RequestOptions({ headers: headers });

        return this.http.put(this._updateMeetingSchemaUrl + 'meetingSchemas/' + meetingSchema.Id , body, options).
            map(res => res.json().message).catch(this.ErrorHandler);
    }

    //Delete MS obj by thier id
    DeletMeetingSchemaById(id: number): Observable<string> //Observable<string>
    {
        var deletedByIdUrl = this._deleteByIdUrl + '/meetingSchemas' + id; 

        return this.http.delete(deletedByIdUrl)
            .map(respones => respones.json().message)
            .catch(this.ErrorHandler);
    }

    //Delete MS paricipants by theier id
    DeleteMSAndParticipanstById(id: number, participants: number)
    {
        var deletedParticipantsByIdUrl = this._deletMeetingSchemaAndParticipanstByIdUrl + '/meetingSchemas' + id + '/removepaticipants/' + participants;

        return this.http.delete(deletedParticipantsByIdUrl)
            .map(respones => respones.json().message)
            .catch(this.ErrorHandler);
    }

    getMeetingSchemaNavPages(page?: number, itemsPerPage?: number): Observable<MeetingSchemas[]> {
        let headers = new Headers();
        if (page != null && itemsPerPage != null) {
            headers.append('Pagination', page + ',' + itemsPerPage);
        }

        return this.http.get(this._getMeetingSchemaUrl + 'meetingSchemas', {
            headers: headers
        })
            .map((res: Response) => {
                return res.json();
            })
            .catch(this.ErrorHandler);
    }
    
    //Handler server side errors
    private ErrorHandler(error: Response) {
        return Observable.throw(error.json().error || 'The server has generate this error.');
    }
}
