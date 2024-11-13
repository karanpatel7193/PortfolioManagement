import { Component, OnInit } from '@angular/core';
import { SessionService } from 'src/app/services/session.service';
import { DashboardService } from './dashboard.service';
import { ToastService } from 'src/app/services/toast.service';
import { SpinnerService } from 'src/app/components/spinner/spinner.service';
import { UserLoginModel, UserModel } from '../account/user/user.model';
import { ActivatedRoute } from '@angular/router';

@Component({
    selector: 'app-dashboard-page',
    templateUrl: './dashboard.component.html',
    styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
    public isLoading: boolean = false;
    public currentUser: UserLoginModel = new UserLoginModel();

    constructor(private sessionService: SessionService, private DashboardService: DashboardService, private toastService: ToastService, private spinnerService: SpinnerService,private route: ActivatedRoute) { }

    ngOnInit(): void {
        this.currentUser = this.sessionService.getUser();
    }

}
