import { Component, OnInit, OnDestroy } from '@angular/core';
import { MenuService } from './menu.service';
import { MenuModel, MenuAddModel, MenuEditModel, MenuParameterModel, MenuMainModel } from './menu.model';
import { Router, ActivatedRoute, UrlSegment } from '@angular/router';
import { SessionService } from '../../../services/session.service';
import { AccessModel } from '../../../models/access.model';
import { ToastService } from 'src/app/services/toast.service';

@Component({
    selector: 'app-menu-form',
    templateUrl: './menu-form.component.html',
    styles: [`
    nb-card {
      transform: translate3d(0, 0, 0);
    }
  `],
})
export class MenuFormComponent implements OnInit,OnDestroy {
    public access: AccessModel = new AccessModel();
    public menu: MenuModel = new MenuModel();
    public menus: MenuModel[] = [];
    public parentMenus: MenuMainModel[] = [];
    public menuAdd: MenuAddModel = new MenuAddModel();
    public menuEdit: MenuEditModel = new MenuEditModel();
    public menuParameter: MenuParameterModel = new MenuParameterModel();

    public hasAccess: boolean = false;
    public mode: string = "";
    public id: number = 0;
    private sub: any;

    constructor(private menuService: MenuService,
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
        this.menu = new MenuModel();
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

        this.menuService.getAddMode(this.menuParameter).subscribe((data: MenuAddModel) => {
            this.menuAdd = data;
            this.parentMenus = this.menuAdd.menus;
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
        this.menuParameter.id = this.id;

        this.menuService.getEditMode(this.menuParameter).subscribe(data => {
            this.menuEdit = data;
            this.parentMenus = this.menuEdit.menus;
            this.menu = this.menuEdit.menu;
        });

    }

    public save(isFormValid: boolean): void {
        if (isFormValid) {
            if (!this.access.canInsert && !this.access.canUpdate) {
                this.toaster.warning('You do not have add or edit access of this page.');
                return;
            }

            this.menuService.save(this.menu).subscribe(data => {
                if (data === 0)
                    this.toaster.warning('Record is already exist.');
                else if (data > 0) {
					this.toaster.success('Record submitted successfully.');
                    this.router.navigate(['app/account/menu/list']);
                }
            });
        } else {
			this.toaster.warning('Please provide valid input.');
        }
    }

    public cancle():void{
        this.router.navigate(['app/account/menu/list'])
    }
    public setAccess(): void {
        this.access = this.sessionService.getAccess('account/menu');
    }
}
