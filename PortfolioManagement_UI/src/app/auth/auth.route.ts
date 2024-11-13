import { ExtraOptions, RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { LoginComponent } from './login/login.component';
import { AuthLayoutComponent } from '../layouts/auth-layout/auth-layout.component';
import { RegistrationComponent } from './registration/registration.component';
import { ForgetPasswordComponent } from './forget-password/forget-password.component';

const routes: Routes = [
    {
        path: '',
        component: AuthLayoutComponent,
        children: [
            {
                path: 'login',
                component: LoginComponent,
            },
            {
                path: 'registration',
                component: RegistrationComponent
            },
            {
                path: 'forgetPassword',
                component: ForgetPasswordComponent
            },
            { 
                path: '', 
                redirectTo: 'login', 
                pathMatch: 'full' 
            }
        ],
    }
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class AuthRoute {
}
