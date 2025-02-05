import { Component } from '@angular/core';
import { User } from 'src/models/users/user';
import { UserStockNote } from 'src/models/users/user-stock-note';
import { UserService } from 'src/services/user.service';

@Component({
  selector: 'app-user-notes',
  templateUrl: './user-notes.component.html',
  styleUrls: ['./user-notes.component.css']
})
export class UserNotesComponent {
  user?: User;
  stockSymbols: string[] = [];
  selectedStockSymbol?: string | undefined;

  constructor(
    private userService: UserService
  ){}

  ngOnInit(): void {
    this.userService.getUser().subscribe(user => {
      this.user = user;
      this.stockSymbols = Object.keys(user.userStockNotesBySymbol);
      this.selectedStockSymbol = this.stockSymbols[0];
    });    
  }

  getKeys(dictionary: any): string[]{
    return Object.keys(dictionary);
  }
}
