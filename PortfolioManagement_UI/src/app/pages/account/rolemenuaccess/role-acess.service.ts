import { Injectable } from '@angular/core';
import { BaseService } from '../../../services/base.service';
import { catchError, map, tap } from 'rxjs/operators';
import { RoleModel, RoleMainModel, RoleParameterModel, RoleGridModel} from '../role/role.model';
import { MenuMainModel } from '../menu/menu.model';
import { RoleMenuAccessModel } from '../rolemenuaccess/rolemenuaccess.model';
import { Observable } from 'rxjs';

@Injectable()
export class RoleAccessService {
    constructor(private httpBase: BaseService) {
    }

    public getRoleAccessById(roleMenu: RoleMenuAccessModel): Observable<RoleMenuAccessModel[]> {
        return this.httpBase.post('account/roleMenuAccess/getByRoleIdParentId', roleMenu).pipe(
            map((response: RoleMenuAccessModel[]) => {
                return response;
            }),
        );
    }

    public saveRoleAccess(role: RoleModel): Observable<void> {
        return this.httpBase.post('account/roleMenuAccess/bulkOperation', role).pipe(
            map((response: void) => {
                return response;
            }),
        );
    }
}
