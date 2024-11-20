import { Injectable } from '@angular/core';
import { catchError, map, tap } from 'rxjs/operators';
import { RoleModel, RoleMainModel, RoleParameterModel, RoleGridModel} from '../role/role.model';
import { MenuMainModel } from '../menu/menu.model';
import { RoleMenuAccessModel } from '../rolemenuaccess/rolemenuaccess.model';
import { Observable } from 'rxjs';
import { HttpService } from 'src/app/services/http.service';

@Injectable()
export class RoleAccessService {
    constructor(private http: HttpService) {
    }

    public getRoleAccessById(roleMenu: RoleMenuAccessModel): Observable<RoleMenuAccessModel[]> {
        return this.http.post('account/roleMenuAccess/getByRoleIdParentId', roleMenu).pipe(
            map((response: RoleMenuAccessModel[]) => {
                return response;
            }),
        );
    }

    public saveRoleAccess(role: RoleModel): Observable<void> {
        return this.http.post('account/roleMenuAccess/bulkOperation', role).pipe(
            map((response: void) => {
                return response;
            }),
        );
    }
}
