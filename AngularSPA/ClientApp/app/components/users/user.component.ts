import { Component, OnInit } from '@angular/core';
import { MeetingSchemas } from '../../Models/MeetingSchemas';
import { User } from '../../Models/User';
import { MeetingSchemaService } from '../../Services/MeetingSchema.Services';
import { ItemsService } from '../../Extensions/Items.Service';

import { ToastrService } from 'toastr-ng2';
import { Observable } from 'rxjs/Observable';
import "rxjs/Rx";



@Component({
    selector: 'user',
    templateUrl: './user.component.html'
})
 
export class userComponent implements OnInit {

    users: User[];
    IsAddingNewUser: boolean = false;

    constructor(private meetingSchemaservice: MeetingSchemaService,
        private itemservice: ItemsService,
        private toastrservice : ToastrService) { }

    ngOnInit() {
        this.meetingSchemaservice.getUser()
            .subscribe((user: User[]) => {
                this.users = user;
            },
            error => {
                this.toastrservice.error('Failed to load the users from database.' + error)
            });
    }

    //Remove users
    DeletecurrentUser(users: any)
    {
        var user: User = this.itemservice.getSerialized<User>(users.value);
        if (user != null) {
            this.itemservice.removeItemFromArray<User>(this.users, user);
        }
        else {
            this.toastrservice.error('The user is deleted successfully');
        }
    }

    //Create new user
    CreateNewUser(user: any){
        var users: User = this.itemservice.getSerialized<User>(user.value);
        this.toastrservice.success ('New user with name: ' + users.Name + 'has beeen created successfully.');
        this.itemservice.setItem<User>(this.users, (u) => u.Id == -1, users);
    }

    //Add new User 
    AddNewUser() {
        var newUser = { id: -1, name: '', avatar: 'avatar_01.png', profession: '', MeetingSchemaCreated: 0 };
        this.itemservice.addItemToStart<User>(this.users, newUser);
      
    }

   //Cancel an action to create new user
    CancelCurrentUser() {
        this.itemservice.removeItems<User>(this.users, e => e.Id < 0);
    }
}




