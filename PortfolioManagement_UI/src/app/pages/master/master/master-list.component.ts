import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SessionService } from '../../../services/session.service';
import { AccessModel } from '../../../models/access.model';
import { ToastService } from 'src/app/services/toast.service';
import { MasterModel, MasterParameterModel } from './master.model';
import { MasterChildService } from './master.service';

@Component({
    selector: 'app-master-list',
    templateUrl: './master-list.component.html',
})

export class MasterListComponent implements OnInit {
    public access: AccessModel = new AccessModel();
    public masters: MasterModel[] = [];
    public masterModel: MasterModel = new MasterModel();
    public masterParameterModel: MasterParameterModel = new MasterParameterModel();

    constructor(private masterChildService: MasterChildService,
        private sessionService: SessionService,
        private router: Router,
        private toaster: ToastService) {
        this.setAccess();
    }

    ngOnInit() {
        this.masterChildService.getForGrid().subscribe(data => {
            this.masters = data;
        });
    }


    public add(): void {
        if (!this.access.canInsert) {
            this.toaster.warning('You do not have add access of this page.');
            return;
        }

        this.router.navigate(['app/master/master-values/add']);
    }

    public edit(id: number): void {
        if (!this.access.canUpdate) {
            this.toaster.warning('You do not have edit access of this page.');
            return;
        }

        this.router.navigate(['app/master/master-values/edit', id]);
    }

    public search(): void {
		if (!this.access.canView) {
			this.toaster.warning('You do not have view access of this page.');
			return;
		}

		this.masterChildService.getForGrid().subscribe(data => {
			this.masters = data;
		});
	}
    public delete(id: number): void {
        if (!this.access.canDelete) {
            this.toaster.warning('You do not have delete access of this page.');
            return;
        }

        if (window.confirm('Are you sure you want to delete?')) {
            this.masterChildService.delete(id).subscribe(data => {
                this.toaster.success('Record deleted successfully.');
                this.search();
            });
        }
    }
    public setAccess(): void {
        this.access = this.sessionService.getAccess('master/master-values');
    }
}
