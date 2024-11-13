import { PagingSortingModel } from '../../../models/pagingsorting.model';
import { RoleMenuAccessModel } from '../rolemenuaccess/rolemenuaccess.model';

export class RoleMainModel {
    public id: number = 0;
    public name: string = '';
}

export class RoleModel extends RoleMainModel {
    public isPublic: boolean = false;
    public roleMenuAccesss: RoleMenuAccessModel[] = [];
}

export class RoleGridModel {
    public roles: RoleModel[] = [];
    public totalRecords: number = 0;
}

export class RoleParameterModel extends PagingSortingModel {
    public id: number = 0;
    public name: string = '';
    public isPublic: boolean = true;
}
