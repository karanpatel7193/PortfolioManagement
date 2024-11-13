import { Component, OnInit } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { UserLoginModel } from 'src/app/pages/account/user/user.model';
import { SessionService } from 'src/app/services/session.service';

@Component({
    selector: 'app-sidebar',
    templateUrl: './sidebar.component.html',
    styleUrls: ['./sidebar.component.scss']
})
export class SidebarComponent implements OnInit {
    public isMouseOver: boolean = false;
    public isCollapsed: boolean = false;
    public activeParentMenu: string = ''; 
    public user: UserLoginModel;
    public menuList: any[] = [];

    constructor(private router: Router, private session: SessionService) {
        this.user = this.session.getUser();
        if (this.user == null) {
            this.router.navigate(['auth/login']);
        }
        this.SetMenuItems();
    }

    ngOnInit() {
        this.checkSidebarCollapse();
    }

    public toggleSidebar() {
        this.isCollapsed = !this.isCollapsed;
        this.session.setIsMenuCollapsed(this.isCollapsed);
    }

    private checkSidebarCollapse() {
        // You can add specific checks here if you want to collapse/expand the sidebar
        // based on the route. For now, this keeps the state consistent.
        this.isCollapsed = this.session.isMenuCollapsedValue;
    }

    public SetMenuItems() {
            
        let menuList = this.user.menus.filter(m => m.parentId == 0 && m.isMenu && !m.isClient);
        for (let i = 0; i < menuList.length; i++) {
            let menuItem = menuList[i];
            let menu: any = {};
            menu.name = menuItem.name;
            menu.pageTitle = menuItem.pageTitle;
            menu.icon = menuItem.icon;
            menu.url = menuItem.routing;
            menu.home = true;

            var childrenList = this.user.menus.filter(m => m.parentId == menuItem.id && m.isMenu && !m.isClient);
            if (childrenList.length > 0) {
                menu.subtabs = childrenList.map(m => {
                    let subtab: any = {};
                    subtab.name = m.name;
                    subtab.pageTitle = m.pageTitle;
                    subtab.icon = m.icon;
                    subtab.url = m.routing;
                    return (subtab);
                });
            }
            else
                menu.subtabs = [];
            this.menuList.push(menu);
        }
    }
}



