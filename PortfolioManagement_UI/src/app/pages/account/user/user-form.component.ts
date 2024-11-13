import { Component, OnInit, OnDestroy } from '@angular/core';
import { UserService } from './user.service';
import { UserModel, UserParameterModel, UserAddModel, UserEditModel, UserLoginModel } from './user.model';
import { Router, ActivatedRoute, UrlSegment } from '@angular/router';
import { SessionService } from '../../../services/session.service';
import { AccessModel } from '../../../models/access.model';
import { RoleMainModel } from '../role/role.model';
import { ToastService } from 'src/app/services/toast.service';
import { MasterValuesModel } from 'src/app/models/mastervalue.model';

@Component({
    selector: 'app-user-form',
    templateUrl: './user-form.component.html',
//     styles: [`
//     nb-card {
//       transform: translate3d(0, 0, 0);
//     }
//   `],
})
export class UserFormComponent implements OnInit, OnDestroy {
    public access: AccessModel = new AccessModel();
    public user: UserModel = new UserModel();
    public users: UserModel[] = [];
    public roles: RoleMainModel[] = [];
    public hasAccess: boolean = false;
    public mode: string = '';
    public id: number = 0;
    public userAdd: UserAddModel = new UserAddModel();
    public userEdit: UserEditModel = new UserEditModel();
    public userParameter: UserParameterModel = new UserParameterModel();
    private sub: any;
    public passwordsMatch: boolean = true;

    public masterValues: MasterValuesModel[] = [];

    constructor(private userService: UserService,
        private sessionService: SessionService,
        private router: Router,
        private route: ActivatedRoute,
        private toaster: ToastService) {
            this.setAccess();
        }

    ngOnInit() {
        this.getRouteData();
        this.loadMasterValues()
    }

    loadMasterValues() {
        this.masterValues = this.sessionService.getUser().masterValues.filter(x => x.masterId == 1);
    }

    ngOnDestroy() {
        this.sub.unsubscribe();
    }


    onPasswordChange() {
      this.checkPasswordsMatch();
    }
  
    onConfirmPasswordChange() {
      this.checkPasswordsMatch();
    }
  
    checkPasswordsMatch() {
      this.passwordsMatch = this.user.password === this.user.confirmPassword;
    }

    public getRouteData(): void {
        this.sub = this.route.params.subscribe(params => {
            const segments: UrlSegment[] = this.route.snapshot.url;
            if (segments.toString().toLowerCase() === 'add')
                this.id = 0;
            else
                this.id = +params['id']; // (+) converts string 'id' to a number
            this.setPageMode();
        });
    }

    public clearModels(): void {
        this.user = new UserModel();
    }

    public setPageMode(): void {
        if (this.id === undefined || this.id === 0)
            this.setPageAddMode();
        else
            this.setPageEditMode();

        if (this.hasAccess) {
        }
    }

    public setPageAddMode(): void {
        if (!this.access.canInsert) {
            this.toaster.warning('You do not have add access of this page.');
            return;
        }
        this.hasAccess = true;
        this.mode = 'Add';

        this.userService.getAddMode(this.userParameter).subscribe(data => {
            this.userAdd = data;
            this.roles = this.userAdd.roles;
        });
        this.clearModels();
    }

    public setPageEditMode(): void {
        if (!this.access.canUpdate) {
            this.toaster.warning('You do not have update access of this page.');
            return;
        }
        this.hasAccess = true;
        this.mode = 'Edit';
        this.userParameter.id = this.id;

        this.userService.getEditMode(this.userParameter).subscribe(data => {
            this.userEdit = data;
            this.user = this.userEdit.user;
            this.roles = this.userEdit.roles;
        });
    }

    public save(isFormValid: boolean): void {
        if (!this.access.canInsert && !this.access.canUpdate) {
            this.toaster.warning('You do not have add or edit access of this page.');
            return;
        }
        this.checkPasswordsMatch();
        if (isFormValid && this.passwordsMatch) {
            this.userService.save(this.user).subscribe(data => {
                if (data === 0)
                    this.toaster.warning('Record is already exist.');
                else if (data > 0) {
                    this.toaster.success('Record submitted successfully.');
                    this.router.navigate(['app/account/user/list']);
                }
            });
        } else {
            this.toaster.warning('Please provide valid input.');
        }
    }

    public cancel():void{
        this.router.navigate(['app/account/user/list'])
    }

    public setAccess(): void {
        this.access = this.sessionService.getAccess('account/user');
        // this.access.canInsert = true;
        // this.access.canUpdate = true;
        // this.access.canDelete = true;
        // this.access.canView = true;
    }
}
