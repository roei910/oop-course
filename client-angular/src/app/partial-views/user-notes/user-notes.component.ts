import { Component } from '@angular/core';
import { User } from 'src/models/users/user';
import { UserService } from 'src/services/user.service';

@Component({
  selector: 'app-user-notes',
  templateUrl: './user-notes.component.html',
  styleUrls: ['./user-notes.component.css']
})
export class UserNotesComponent {
  user?: User;

  constructor(
    private userService: UserService
  ){}

  ngOnInit(): void {
    this.userService.getUser().subscribe(user => this.user = user);    
  }

  getKeys(dictionary: any): string[]{
    return Object.keys(dictionary);
  }
}
