import { Component, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { catchError, of } from 'rxjs';
import { ToastService } from 'src/services/toast.service';
import { UserService } from 'src/services/user.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  @ViewChild("connectionForm")
  form!: NgForm;

  visibleUpdatePasswordDialog: boolean = false;
  email: string = '';
  password: string = '';
  repeatPassword: string = '';

  constructor(private router: Router,
    private userService: UserService,
    private toastService: ToastService
  ){ }

  signIn() {
    this.userService
    .tryConnect(this.form.value.email, this.form.value.password)
    .pipe(catchError(() => {
      return of(false);
    }))
    .subscribe(isConnected => {
      if(!isConnected)
      {
        this.toastService.addErrorMessage("username or password was incorrect");
      }
      else
        this.router.navigate(['user', 'stocks']);
    }); 
  }

  updatePassword() {
    if(this.password !== this.repeatPassword){
      this.toastService.addErrorMessage("Passwords must be a match");

      return;
    }

    this.visibleUpdatePasswordDialog = false;
    this.userService.updatePassword(this.email, this.password)
      .pipe(catchError(() => of(false)))
      .subscribe(isUpdated => {
        if(isUpdated)
          this.toastService.addSuccessMessage("Updated password successfully");
        else
          this.toastService.addErrorMessage("There was an error while updating the password");
      });
  }
}
