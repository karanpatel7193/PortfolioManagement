import { PagingSortingModel } from '../../../models/pagingsorting.model';
import { RoleMainModel } from '../role/role.model';
import { RoleMenuAccessModel } from '../rolemenuaccess/rolemenuaccess.model';
import { MenuModel } from '../menu/menu.model';
import { MasterValuesModel } from 'src/app/models/mastervalue.model';

export class UserMainModel {
    public id: number = 0;
    public firstName: string = '';
    public lastName: string = '';
    public email: string = '';
}

export class UserModel extends UserMainModel {
    public middleName: string = '';
    public roleId: number = 0;
    public roleName: string = '';
    public username: string = '';
    public password: string = '';
    public passwordSalt: string = '';
    public phoneNumber: string = '';
    public birthDate: Date = new  Date(0);
    public gender: number = 0;
    public imageSrc: string = '';
    public lastUpdateDateTime: Date = new Date(0);
    public isActive: boolean = true;
    public confirmPassword: string = '';
    public mode: string = 'Admin';
    public type: string = 'Individual';
    public pmsName: string = '';
}

export class UserAddModel {
    public roles: RoleMainModel[] = [];
}

export class UserEditModel extends UserAddModel {
    public user: UserModel = new UserModel();
}

export class UserGridModel {
    public users: UserModel[] = [];
    public totalRecords: number = 0;
}

export class UserListModel extends UserGridModel {
    public roles: RoleMainModel[] = [];
}

export class UserParameterModel extends PagingSortingModel {
    public id: number = 0;
    public firstName: string = '';
    public lastName: string = '';
    public email: string = '';
    public roleId: number = 0;;
    public username: string = '';
    public phoneNumber: string = '';
    public parentUserId: number = 0;

}

export class UserLoginModel extends UserMainModel {
    public username: string = '';
    public token: string = '';
    public gender: number = 0;
    public roleId: number = 0;
    public roleName: string = '';
    public imageSrc: string = '';
    public roleMenuAccesss: RoleMenuAccessModel[] = [];
    public menus: MenuModel[] = [];
    public masterValues: MasterValuesModel[] = [];
    public pmsName: string = '';

}

