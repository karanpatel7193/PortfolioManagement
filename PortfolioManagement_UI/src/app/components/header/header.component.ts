import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { debounceTime, distinctUntilChanged, filter, map, Observable, OperatorFunction } from 'rxjs';
import { PageModel } from 'src/app/models/page.model';
import { UserLoginModel } from 'src/app/pages/account/user/user.model';
import { UserService } from 'src/app/pages/account/user/user.service';
import { HeaderGridModel, HeaderModel } from 'src/app/pages/index/index-header.model';
import { IndexHeaderService } from 'src/app/pages/index/index-header.service';
import { ScriptMainModel, ScriptParameterModel } from 'src/app/pages/master/script/script.model';
import { ScriptService } from 'src/app/pages/master/script/script.service';
import { StockTransactionListModel } from 'src/app/pages/transaction/stocktransaction/stocktransaction.model';
import { CommonService } from 'src/app/services/common.service';
import { SessionService } from 'src/app/services/session.service';
import { environment } from 'src/environments/environment';

@Component({
    selector: 'app-header',
    templateUrl: './header.component.html',
    styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
    public commonPageModel: PageModel = new PageModel();
    public currentUser: UserLoginModel = new UserLoginModel();

    public breadcrumbs: string[] = [];
    public analysisReady: boolean = false;
    public analysisInProcess: boolean = false;
    public analysisCompleteDate: string = '';
    public analysisFileName: string = '';
    public analysisMinDate: string = '';
    public analysisMaxDate: string = '';

    public forecastReady: boolean = false;
    public forecastToBeDefined: boolean = false;
    public forecastInProcess: boolean = false;
    public forecastCompleteDate: string = '';
    public forecastFileName: string = '';
    public forecastMinDate: string = '';
    public forecastMaxDate: string = '';

    public swVersion: string = environment.softwareVersion;
    public modelVersion: string = '';

    public stockTransactionListModel: StockTransactionListModel = new StockTransactionListModel();
    public scriptParameter: ScriptParameterModel = new ScriptParameterModel();

    public headerGridModel: HeaderGridModel = new HeaderGridModel();
    public headerModel: HeaderModel = new HeaderModel();
    //public niftyModel: ScriptViewNiftyModel = new ScriptViewNiftyModel();

    public selectedScript: ScriptMainModel = new ScriptMainModel();
    formatter = (script: ScriptMainModel) => script.name;
    search = (text$: Observable<string>) =>
        text$.pipe(
            debounceTime(200),
            distinctUntilChanged(),
            filter((term) => term.length >= 2),
            map((term) => this.stockTransactionListModel.scripts.filter((script) => new RegExp(term, 'mi').test(script.name)).slice(0, 10)),
        );

    constructor(private router: Router, private activatedRoute: ActivatedRoute, private commonService: CommonService,
        private sessionService: SessionService, private userService: UserService, private scriptService: ScriptService,
        private indexHeaderService: IndexHeaderService) {
        activatedRoute.url.subscribe(() => {
            let routeName: string = activatedRoute.snapshot.url.toString();
            this.breadcrumbs = routeName.split('-');
        });

        router.events.subscribe((val) => {
            if (val instanceof NavigationEnd) {
                let routeParts: string[] = val.url.toString().split('/');
                if (routeParts.length > 0) {
                    let routeName: string = routeParts[routeParts.length - 1];
                    this.commonPageModel = this.commonService.getApiData(routeName);
                    if (this.commonPageModel.breadcrumbs != '') {
                        this.breadcrumbs = this.commonPageModel.breadcrumbs.split('/');
                    }
                }
            }
        });
    }

    ngOnInit(): void {
        this.currentUser = this.sessionService.getUser();
        this.sessionService.userState.subscribe(data => {
            this.currentUser = this.sessionService.getUser();
        });
        this.fillDropdowns()
        this.getScriptData()
    }

    // public logout() {
    //     this.userService.logoutUser().subscribe({
    //         next: (result) => {
    //             this.sessionService.logout();
    //         },
    //         error: (error) => {
    //             console.log(error);
    //             this.sessionService.logout();
    //         }
    //     });
    // }

    public logout() {
        this.sessionService.logout()
    }

    private fillDropdowns(): void {
        this.scriptService.getForLOV(this.scriptParameter).subscribe((data) => {
            this.stockTransactionListModel.scripts = data
        },
            error => {
                console.error('Failed to load roles:', error);
            }
        )
    }

    public onScriptSelected(event: any) {
        if (event) {
            this.router.navigate([`app/scriptView/${event.item.id}`]);
        }
    }

    public getScriptData(): void {
        this.indexHeaderService.getForGrid().subscribe((data) => {
            this.headerGridModel.nifty50 = data.nifty50;
        });
        this.indexHeaderService.getForIndex().subscribe((data) => {
            this.headerModel = data;
        });
    }

    scrollLeft() {
		const container = document.querySelector('.ttape-inner');
		if (container) {
		    container.scrollBy({ left: -100, behavior: 'smooth' });
		}
	}
	  
	scrollRight() {
		const container = document.querySelector('.ttape-inner');
		if (container) {
		  container.scrollBy({ left: 100, behavior: 'smooth' });
		}
	}
 
    }


