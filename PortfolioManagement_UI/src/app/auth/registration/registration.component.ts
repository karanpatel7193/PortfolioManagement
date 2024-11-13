import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AccessModel } from 'src/app/models/access.model';
import { MasterValuesModel } from 'src/app/models/mastervalue.model';
import { RoleMainModel } from 'src/app/pages/account/role/role.model';
import { UserModel, UserAddModel, UserEditModel, UserParameterModel } from 'src/app/pages/account/user/user.model';
import { UserService } from 'src/app/pages/account/user/user.service';
import { SessionService } from 'src/app/services/session.service';
import { ToastService } from 'src/app/services/toast.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss']
})
export class RegistrationComponent implements OnInit {

    public user: UserModel = new UserModel();
    public access: AccessModel = new AccessModel();
    public users: UserModel[] = [];
    public roles: RoleMainModel[] = [];
    public hasAccess: boolean = false;

    public id: number = 0;
    public userAdd: UserAddModel = new UserAddModel();
    public userEdit: UserEditModel = new UserEditModel();
    public userParameter: UserParameterModel = new UserParameterModel();
    public masterValues: MasterValuesModel[] = [];
    
    constructor(private userService: UserService, private sessionService: SessionService,
        private router: Router,
        private route: ActivatedRoute,
        private toaster: ToastService) {
        }

    ngOnInit(): void {
        //this.loadMasterValues()
    }

    // loadMasterValues() {
    //     this.masterValues = this.sessionService.getUser().masterValues.filter(x => x.masterId == 1);
    // }

    public save(isFormValid: boolean): void {
        if (isFormValid) {
            if (!this.access.canInsert && !this.access.canUpdate) {
                this.toaster.warning('You do not have add or edit access of this page.');
                return;
            }
            this.userService.Registration(this.user).subscribe(data => {
                if (data === 0)
                    this.toaster.warning('Record is already exist.');
                else if (data > 0) {
                    this.toaster.success('Record submitted successfully.');
                    this.router.navigate(['auth/login'])
                }
            });
        } else {
            this.toaster.warning('Please provide valid input.');
        }
    }

public cancel():void{
    this.router.navigate(['auth/login'])
}

}
