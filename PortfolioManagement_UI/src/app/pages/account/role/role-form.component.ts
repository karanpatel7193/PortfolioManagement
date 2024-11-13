import { Component, OnInit, OnDestroy } from '@angular/core';
import { RoleService } from './role.service';
import { RoleModel, RoleParameterModel } from './role.model';
import { Router, ActivatedRoute, UrlSegment } from '@angular/router';
import { SessionService } from '../../../services/session.service';
import { AccessModel } from '../../../models/access.model';
import { ToastService } from 'src/app/services/toast.service';

@Component({
    selector: 'role-form',
    templateUrl: './role-form.component.html',
    styles: [`
    nb-card {
      transform: translate3d(0, 0, 0);
    }
  `],
})
export class RoleFormComponent implements OnInit, OnDestroy {
    public access: AccessModel = new AccessModel();
    public role: RoleModel = new RoleModel();
    public roles: RoleModel[] = [];
    public roleParameter: RoleParameterModel = new RoleParameterModel();

    public hasAccess: boolean = false;
    public mode: string ='';
    public id: number=0;
    private sub: any;

    constructor(private roleService: RoleService,
        private sessionService: SessionService,
        private router: Router,
        private route: ActivatedRoute,
        private toaster: ToastService) {
            this.setAccess();
    }

    ngOnInit() {
        this.getRouteData();
    }

    ngOnDestroy() {
        this.sub.unsubscribe();
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
        this.role = new RoleModel();
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

        this.clearModels();
    }

    public setPageEditMode(): void {
        if (!this.access.canUpdate) {
            this.toaster.warning('You do not have update access of this page.');
            return;
        }
        this.hasAccess = true;
        this.mode = 'Edit';

        this.roleService.getRecord(this.id).subscribe(data => {
            this.role = data;
        });
    }

    public add(): void {
		if (!this.access.canInsert) {
			this.toaster.warning('You do not have add access of this page.');
			return;
		}

		this.router.navigate(['app/account/role/list']);
	}

    public edit(id: number): void {
        if (!this.access.canUpdate) {
            this.toaster.warning('You do not have edit access of this page.');
            return;
        }

        this.router.navigate(['app/account/role/edit', id]);
    }

    public save(isFormValid: boolean): void {
        if (!this.access.canInsert && !this.access.canUpdate) {
            this.toaster.warning('You do not have add or edit access of this page.');
            return;
        }
        if (isFormValid) {
            this.roleService.save(this.role).subscribe(data => {
                if (data === 0)
                    this.toaster.warning('Record is already exist.');
                else if (data > 0) {
                    this.toaster.success('Record submitted successfully.');
                    this.router.navigate(['app/account/role/list']);
                }
            });
        } else {
            this.toaster.warning('Please provide valid input.');
        }
    }

    public cancle():void{
        this.router.navigate(['app/account/role/list'])
    }
    public setAccess(): void {
        this.access = this.sessionService.getAccess('account/role');
   }
}
