import { Component, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { ConfirmationService } from 'primeng/api';
import { UserCreation } from 'src/models/users/user-creation';
import { ToastService } from 'src/services/toast.service';
import { UserService } from 'src/services/user.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  @ViewChild("registrationForm")
  form!: NgForm;

  constructor(private router: Router,
    private userService: UserService,
    private toastService: ToastService,
    private confirmationService: ConfirmationService
  ) { }

  createUser() {
    if (this.form.invalid) {
      this.confirmMissingInformation();

      return;
    }

    let password = this.form.value.password;
    let repeatPassword = this.form.value.repeatPassword;

    if (password !== repeatPassword) {
      this.confirmMissingPassword();

      return;
    }

    const user: UserCreation = {
      firstName: this.form.value.firstName,
      lastName: this.form.value.lastName,
      password: this.form.value.password,
      email: this.form.value.email,
    };

    this.userService.createUser(user)
      .subscribe(isCreated => {
        if (isCreated)
          this.router.navigate(['/login']);
        else
          this.toastService.addErrorMessage("couldn't create the user.");
      });
  }

  confirmMissingInformation(): void{
    this.confirmationService.confirm({
      message: 'Please finish the form and try again',
      header: 'Missing information',
      icon: 'pi pi-exclamation-triangle',
      acceptIcon: "none",
      rejectIcon: "none",
      rejectButtonStyleClass: "p-button-text",
      rejectVisible: false,
      acceptLabel: "Continue"
    });
  }

  confirmMissingPassword(): void{
    this.confirmationService.confirm({
      message: 'Password and Confirm Passwords must be a match',
      header: 'Password Confirmation Error',
      icon: 'pi pi-exclamation-triangle',
      acceptIcon: "none",
      rejectIcon: "none",
      rejectButtonStyleClass: "p-button-text",
      rejectVisible: false,
      acceptLabel: "Continue"
    });
  }
}