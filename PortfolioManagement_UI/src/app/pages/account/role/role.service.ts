import { Injectable } from '@angular/core';
import { BaseService } from '../../../services/base.service';
import { catchError, map, tap } from 'rxjs/operators';
import { RoleModel, RoleMainModel, RoleParameterModel, RoleGridModel} from './role.model';
import { MenuMainModel } from '../menu/menu.model';
import { Observable } from 'rxjs';

@Injectable()
export class RoleService {
    constructor(private httpBase: BaseService) {
    }

    public getRecord(id: number): Observable<RoleModel> {
        return this.httpBase.get('account/role/getRecord/' + id).pipe(
            map((response: RoleModel) => {
                return response;
            }),
        );
    }

    public getForLOV(roleParameter: RoleParameterModel): Observable<RoleMainModel[]> {
        return this.httpBase.post('account/role/getLovValue', roleParameter).pipe(
            map((response: RoleMainModel[]) => {
                return response;
            }),
        );
    }

    public getForGrid(roleParameter: RoleParameterModel): Observable<RoleGridModel> {
        return this.httpBase.post('account/role/getGridData', roleParameter).pipe(
            map((response: RoleGridModel) => {
                return response;
            }),
        );
    }

    public save(role: RoleModel): Observable<number> {
        if (role.id === 0)
            return this.httpBase.post('account/role/insert', role).pipe(
                map((response: number) => {
                    return response;
                }),
            );
        else
            return this.httpBase.post('account/role/update', role).pipe(
                map((response: number) => {
                    return response;
                }),
            );
    }

    public delete(id: number): Observable<void> {
        return this.httpBase.post('account/role/delete/' + id, null).pipe(
            map((response: void) => {
                return response;
            }),
        );
    }

    public fillParent(): Observable<MenuMainModel[]> {
        return this.httpBase.get('account/menu/getParent').pipe(
            map((response: MenuMainModel[]) => {
                return response;
            }),
        );
    }

}
