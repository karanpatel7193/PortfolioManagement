import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbActiveModal, NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { UserService } from 'src/app/pages/account/user/user.service';
import { ToastService } from 'src/app/services/toast.service';
import { RegistrationSuccessComponent } from './registration-success.component';
import { UserLoginModel, UserModel } from 'src/app/pages/account/user/user.model';
import { SessionService } from 'src/app/services/session.service';

@Component({
  selector: 'app-registration-active',
  templateUrl: './registration-active.component.html',
})
export class RegistrationActiveComponent implements OnInit {

    public activation: string = '';
    // public Message: string = 'Hello! Your registration activation going on...';
    public user: UserModel = new UserModel();
    public userLoginModel: UserLoginModel = new UserLoginModel();
    
    public registrationStatus: string = ''; 
    public message: string = ''; 

   
    public ModalOption: NgbModalOptions = {
        backdrop: 'static',
        beforeDismiss: () => {
            this.router.navigate([{ outlets: { popup: null } }]);
            return true;
        }
    }
    constructor(private router: Router,
        private userService: UserService,
        public activeModal: NgbActiveModal,
        private myToaster: ToastService,
        private route: ActivatedRoute,
        private modalService: NgbModal,
        private sessionService: SessionService
        ) {
    }
   
    ngOnInit() {
        this.route.params.subscribe(x => {
            this.activation = x['activation']
        });
        this.ValidateRegistration();
    }

    goToLogin(): void {
        this.router.navigate(['/auth/login']);
    }

    public regenerateLink(): void {
        this.userService.RegenerateRegistrationActive(this.activation).subscribe(
            (data: string) => {
                if (data) {
                    this.myToaster.success("A new activation link has been generated successfully.", 'Link Regenerated');
                    console.log("Activation Link: ", data); 
                } else {
                    this.myToaster.error("Failed to regenerate the activation link. Please try again.", 'Error!');
                }
            },
            (error) => {
                this.myToaster.error("An error occurred while regenerating the activation link.", 'Error!');
                console.error("Error: ", error);
            }
        );
    }
    
    public ValidateRegistration(): any {
        this.userService.RegistrationActive(this.activation).subscribe(data => {
            if (data != null && data === 'expired') {
                this.registrationStatus = 'expired';
                this.message = "Your registration activation link is expired.";
                this.myToaster.error("Registration activation link is expired.", 'Link Expired');
            } else if (data != null && data === 'success') {
                this.registrationStatus = 'success';
                this.message = "Registration activation successful. Redirecting you to login...";
                this.myToaster.success("Registration activated successfully. Now you can log in.", 'Account Activated');
                setTimeout(() => {
                    this.activeModal.close('');
                    this.router.navigate([{ outlets: { popup: ['auth', 'login', 'Activated'] } }]);
                }, 3000);
            } else if (data != null && data === 'fail') {
                this.registrationStatus = 'fail';
                this.message = "User details are invalid. Contact admin or click the activation link again.";
                this.myToaster.error("User details are invalid. Please try again.", 'Error!');
            } else {
                this.registrationStatus = 'error';
                this.message = "Something went wrong. Please contact admin.";
                this.myToaster.error("An error occurred. Please contact admin.", 'Error!');
            }
        });
    }

    public OnRegistrationDone() {
        this.activeModal.close('');
        const successModal = this.modalService.open(RegistrationSuccessComponent, this.ModalOption);
    }
    
    public OnClosed(): void {
        this.activeModal.close('');
        this.router.navigate([{ outlets: { popup: null } }]);
    }
}
