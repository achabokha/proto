import { Component, OnInit } from '@angular/core';
import { UserService } from "../../../services/user.service";

@Component({
    selector: 'app-settings',
    templateUrl: './settings.component.html',
    styleUrls: ['./settings.component.css']
})
export class SettingsComponent implements OnInit
{
    user: any;

    constructor(private userService: UserService) { }

    ngOnInit(): void {
        this.userService.getSettings().subscribe(result => {
            this.user = result;
        });
    }
}
