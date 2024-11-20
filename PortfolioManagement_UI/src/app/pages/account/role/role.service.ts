import { Injectable } from '@angular/core';
import { catchError, map, tap } from 'rxjs/operators';
import { RoleModel, RoleMainModel, RoleParameterModel, RoleGridModel} from './role.model';
import { MenuMainModel } from '../menu/menu.model';
import { Observable } from 'rxjs';
import { HttpService } from 'src/app/services/http.service';

@Injectable()
export class RoleService {
    constructor(private http: HttpService) {
    }

    public getRecord(id: number): Observable<RoleModel> {
        return this.http.get('account/role/getRecord/' + id).pipe(
            map((response: RoleModel) => {
                return response;
            }),
        );
    }

    public getForLOV(roleParameter: RoleParameterModel): Observable<RoleMainModel[]> {
        return this.http.post('account/role/getLovValue', roleParameter).pipe(
            map((response: RoleMainModel[]) => {
                return response;
            }),
        );
    }

    public getForGrid(roleParameter: RoleParameterModel): Observable<RoleGridModel> {
        return this.http.post('account/role/getGridData', roleParameter).pipe(
            map((response: RoleGridModel) => {
                return response;
            }),
        );
    }

    public save(role: RoleModel): Observable<number> {
        if (role.id === 0)
            return this.http.post('account/role/insert', role).pipe(
                map((response: number) => {
                    return response;
                }),
            );
        else
            return this.http.post('account/role/update', role).pipe(
                map((response: number) => {
                    return response;
                }),
            );
    }

    public delete(id: number): Observable<void> {
        return this.http.post('account/role/delete/' + id, null).pipe(
            map((response: void) => {
                return response;
            }),
        );
    }

    public fillParent(): Observable<MenuMainModel[]> {
        return this.http.get('account/menu/getParent').pipe(
            map((response: MenuMainModel[]) => {
                return response;
            }),
        );
    }

}
