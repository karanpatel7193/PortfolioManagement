import { PagingSortingModel } from '../../../models/pagingsorting.model';

export class RoleMenuAccessModel {
    constructor(data: any = null) {
        if (data != null) {
            this.id = data.id;
            this.roleId = data.roleId;
            this.menuId = data.menuId;
            this.canInsert = data.canInsert;
            this.canUpdate = data.canUpdate;
            this.canDelete = data.canDelete;
            this.canView = data.canView;
            this.parentIdName = data.parentIdName;
            this.menuIdName = data.menuIdName;
        }
    }

    public id: number = 0;
    public roleId: number = 0;
    public menuId: number = 0;
    public canInsert: boolean = false;
    public canUpdate: boolean = false;
    public canDelete: boolean = false;
    public canView: boolean = false;
    public parentIdName: string = '';
    public menuIdName:string = '';
}
