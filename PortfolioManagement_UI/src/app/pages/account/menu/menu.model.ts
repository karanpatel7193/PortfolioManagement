import { PagingSortingModel } from '../../../models/pagingsorting.model';

export class MenuMainModel {
    public id: number = 0;
    public name: string = '';
}

export class MenuModel extends MenuMainModel {
    public description: string = '';
    public parentId: number = 0;
    public parentIdName: string = '';
    public pageTitle: string = '';
    public icon: string = '';
    public routing: string = '';
    public orderBy: number = 0;
    public isMenu: boolean = false;
    public isClient: boolean = false;
    public isPublic: boolean = false;
}

export class MenuAddModel {
    public menus: MenuMainModel[] = [];

}

export class MenuEditModel extends MenuAddModel {
    public menu: MenuModel = new MenuModel;
}

export class MenuGridModel {
    public menus: MenuModel[] = [];
    public totalRecords: number = 0;
}

export class MenuParameterModel extends PagingSortingModel {
    public id: number = 0;
    public name: string = '';
    public parentId: number = 0;
}
