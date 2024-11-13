import { NgModule } from '@angular/core';
import { AuthRoute } from './auth.route';
import { LoginComponent } from './login/login.component';
import { RoleService } from '../pages/account/role/role.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RegistrationComponent } from './registration/registration.component';
import { UserService } from '../pages/account/user/user.service';
import { ForgetPasswordComponent } from './forget-password/forget-password.component';

@NgModule({
    imports: [
        AuthRoute,
        FormsModule,
        CommonModule,
        FormsModule
    ],
    declarations: [
        LoginComponent,
        RegistrationComponent,
        ForgetPasswordComponent,
    ],
    providers: [
        RoleService,UserService
    ],
})
export class AuthModule { }
