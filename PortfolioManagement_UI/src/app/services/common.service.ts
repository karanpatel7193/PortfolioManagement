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


}
