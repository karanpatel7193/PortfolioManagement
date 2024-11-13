import { NgModule } from '@angular/core';
import { RoleListComponent } from './role-list.component';
import { RoleFormComponent } from './role-form.component';
import { RoleRoute } from './role.route';
import { RoleComponent } from './role.component';
import { RoleService } from './role.service';
import { RoleAccessComponent } from '../rolemenuaccess/role-access.component';
import { RoleAccessService } from '../rolemenuaccess/role-acess.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';

@NgModule({
    imports: [
        RoleRoute,
        FormsModule,
        CommonModule,
        NgbPaginationModule
    ],
    declarations: [
        RoleComponent,
        RoleListComponent,
        RoleFormComponent,
        RoleAccessComponent,
    ],
    providers: [
        RoleService,
        RoleAccessService,
    ],
})
export class RoleModule { }
