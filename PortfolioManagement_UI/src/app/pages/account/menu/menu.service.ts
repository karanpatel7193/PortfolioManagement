import { Injectable } from '@angular/core';
import { catchError, map, tap } from 'rxjs/operators';
import { MenuModel, MenuMainModel, MenuParameterModel, MenuGridModel, MenuAddModel, MenuEditModel } from './menu.model';
import { Observable } from 'rxjs';
import { HttpService } from 'src/app/services/http.service';

@Injectable()
export class MenuService {
    constructor(private http: HttpService) {
    }

    public getRecord(id: number): Observable<MenuModel> {
        return this.http.get('account/menu/getRecord/' + id).pipe(
            map((response: MenuModel) => {
                return response;
            }),
        );
    }

    public getForLOV(menuParameter: MenuParameterModel): Observable<MenuMainModel[]> {
        return this.http.post('account/menu/getLovValue', menuParameter).pipe(
            map((response: MenuMainModel[]) => {
                return response;
            }),
        );
    }

    public getAddMode(menuParameter: MenuParameterModel): Observable<MenuAddModel> {
        return this.http.post('account/menu/getAddMode', menuParameter).pipe(
            map((response: MenuAddModel) => {
                return response;
            }),
        );
    }

    public getEditMode(menuParameter: MenuParameterModel): Observable<MenuEditModel> {
        return this.http.post('account/menu/getEditMode', menuParameter).pipe(
            map((response: MenuEditModel) => {
                return response;
            }),
        );
    }

    public getForGrid(menuParameter: MenuParameterModel): Observable<MenuGridModel> {
        return this.http.post('account/menu/getGridData', menuParameter).pipe(
            map((response: MenuGridModel) => {
                return response;
            }),
        );
    }

    public save(menu: MenuModel): Observable<number> {
        if (menu.id === 0)
            return this.http.post('account/menu/insert', menu).pipe(
                map((response: number) => {
                    return response;
                }),
            );
        else
            return this.http.post('account/menu/update', menu).pipe(
                map((response: number) => {
                    return response;
                }),
            );
    }

    public delete(id: number): Observable<void> {
        return this.http.post('account/menu/delete/' + id, null).pipe(
            map((response: void) => {
                return response;
            }),
        );
    }
}
