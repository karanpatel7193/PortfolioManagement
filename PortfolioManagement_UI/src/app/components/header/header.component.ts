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
    showProfileModal: boolean = false;
    public swVersion: string = environment.softwareVersion;
    public modelVersion: string = '';

    public stockTransactionListModel: StockTransactionListModel = new StockTransactionListModel();
    public scriptParameter: ScriptParameterModel = new ScriptParameterModel();

    public headerGridModel: HeaderGridModel = new HeaderGridModel();
    public headerModel: HeaderModel = new HeaderModel();
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
        router.events.subscribe((val) => {
            if (val instanceof NavigationEnd) {
                this.selectedScript = new ScriptMainModel();
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
  
    public cancel():void{
        this.router.navigate(['auth/login'])
    }
    openProfileModal() {
        this.showProfileModal = true;
    }

    closeProfileModal() {
        this.showProfileModal = false;
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
    public redirect(id: number, symbol: string) {
        this.commonService.redirectToPage(id, symbol);
    }

    //code for password change
    showModal: boolean = false;

    goToChangePassword() {
        this.showModal = true;
    }

    closeModal() {
        this.showModal = false; 
    }
  onBackdropClick(event: MouseEvent) {
    const modalContent = document.querySelector('.modal-content');
    if (modalContent && !modalContent.contains(event.target as Node)) {
      this.closeModal(); 
      this.closeProfileModal();
    }
  }

  stopPropagation(event: MouseEvent) {
    event.stopPropagation(); 
  }
}


