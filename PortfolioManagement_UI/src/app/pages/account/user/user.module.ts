import { NgModule } from '@angular/core';
import { UserListComponent } from './user-list.component';
import { UserFormComponent } from './user-form.component';
import { UserRoute } from './user.route';
import { UserComponent } from './user.component';
import { UserService } from './user.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NgbPagination, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';

@NgModule({
    imports: [
        UserRoute,
        CommonModule,
        FormsModule,
        NgbPaginationModule
    ],
    declarations: [
        UserComponent,
        UserListComponent,
        UserFormComponent,
    ],
    providers: [
        UserService
    ],
})
export class UserModule { }
