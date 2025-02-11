import { ChangeDetectorRef, Component } from '@angular/core';
import { User } from 'src/models/users/user';
import { UserStockNote } from 'src/models/users/user-stock-note';
import { AuthenticationService } from 'src/services/authentication.service';
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
  visibleDialog: boolean = false;
  note: string = "";
  stockSymbol: string = "";
  visibleStockSymbol: boolean = false;

  constructor(
    private userService: UserService,
    private authenticationService: AuthenticationService,
    private cdRef: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.userService.getUser().subscribe(user => {
      this.user = user;
      this.stockSymbols = Object.keys(user.userStockNotesBySymbol);
      this.selectedStockSymbol = this.stockSymbols[0];
    });
  }

  reloadComponent() {
    this.cdRef.detectChanges();
  }

  openNoteDialog(): void {
    this.visibleDialog = true;
    this.visibleStockSymbol = true;
  }

  updateUserNote(): void {
    let email = this.authenticationService.getUserEmail();
    if (!email)
      throw new Error();

    this.userService.addNote(email, this.stockSymbol, this.note)
      .subscribe(idResponse => {
        let userStockNote: UserStockNote = {
          id: idResponse.Id,
          note: this.note
        };

        this.insertNewNote(userStockNote);
      });

    this.visibleDialog = false;
  }

  private insertNewNote(userStockNote: UserStockNote) {
    if (!this.stockSymbols.find(symbol => symbol == this.stockSymbol))
      this.user!.userStockNotesBySymbol[this.stockSymbol] = [];

    this.stockSymbols.push(this.stockSymbol);
    this.user?.userStockNotesBySymbol[this.stockSymbol].push(userStockNote);

    this.reloadComponent();
  }

  addNoteToSymbol(): void {
    this.stockSymbol = this.selectedStockSymbol!;
    this.visibleDialog = true;
    this.visibleStockSymbol = false;
  }
}