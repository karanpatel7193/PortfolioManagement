import { NgModule } from '@angular/core';
import { AuthRoute } from './auth.route';
import { LoginComponent } from './login/login.component';
import { RoleService } from '../pages/account/role/role.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RegistrationComponent } from './registration/registration.component';
import { UserService } from '../pages/account/user/user.service';
import { ForgetPasswordComponent } from './forget-password/forget-password.component';
import { RegistrationActiveComponent } from './registration/registration-active.component';
import { RegistrationSuccessComponent } from './registration/registration-success.component';
import { ProfileComponent } from './profile/profile.component';
import { NgbActiveModal, NgbModule } from '@ng-bootstrap/ng-bootstrap';

@NgModule({
    imports: [
        AuthRoute,
        FormsModule,
        CommonModule,
        FormsModule,
        NgbModule
    ],
    declarations: [
        LoginComponent,
        RegistrationComponent,
        ForgetPasswordComponent,
        RegistrationActiveComponent,
        RegistrationSuccessComponent,
    ],
    providers: [
        RoleService,UserService,NgbActiveModal
    ],
})
export class AuthModule { }
