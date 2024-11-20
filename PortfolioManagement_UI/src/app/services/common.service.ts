import { Injectable } from '@angular/core';
import { PageModel } from '../models/page.model';
import { RouteApiData } from '../data-constant';
import { environment } from 'src/environments/environment';

@Injectable({
    providedIn: 'root',
})  
export class CommonService {

    constructor() { }

    public getApiData(routePath: string): PageModel {
        let commonPageModel: PageModel[] = RouteApiData.filter(x => x.pageName.toLowerCase() == routePath.replace(' ', '-').toLowerCase());
        if (commonPageModel.length > 0)
            return commonPageModel[0];
        else
            return new PageModel();
    }

    public redirectToPage(id: number, symbol: string) {
        const targetUrl = environment.isOurScriptPage 
            ? `${environment.ourScriptPageUrl}${id}`  
            : `${environment.otherScriptPageUrl}${symbol}`; 
        
        window.open(targetUrl, '_blank');
    }

    public sortGrid<T>(items: T[], column: keyof T, sortExpression: string, sortDirection: string): T[] {
        if (sortExpression === column as string) {
            sortDirection = sortDirection === 'asc' ? 'desc' : 'asc';
        } else {
            sortExpression = column as string;
            sortDirection = 'asc';
        }
    
        return items.sort((a, b) => {
            const aValue = a[column];
            const bValue = b[column];
    
            const aStr = String(aValue).toLowerCase();
            const bStr = String(bValue).toLowerCase();
    
            if (aStr < bStr) {
                return sortDirection === 'asc' ? -1 : 1;
            }
            if (aStr > bStr) {
                return sortDirection === 'asc' ? 1 : -1;
            }
            return 0;
        });
    }
}
