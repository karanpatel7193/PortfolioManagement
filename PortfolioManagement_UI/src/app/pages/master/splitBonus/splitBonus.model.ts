
import { PagingSortingModel } from '../../../models/pagingsorting.model';
import { ScriptMainModel } from '../script/script.model';
export class SplitMainModel {
	public id: number = 0;

}

export class SplitModel extends SplitMainModel {
	public scriptID: number = 0;
    public nseCode: string = '';  
    public isSplit: boolean = false;
    public isBonus: boolean = false;
    public oldFaceValue?: number ;
    public newFaceValue?: number ;
    public fromRatio?: number ;
    public toRatio?: number ;
    public announceDate: string = '';  
    public rewardDate: string = '';
    public isApply: boolean = false;
    
}

export class SplitAddModel {

}

export class SplitEditModel extends SplitAddModel {
	public Split: SplitModel = new SplitModel();
}

export class SplitGridModel {
	public splitBonuss: SplitModel[] = [];
	public totalRecords: number = 0;
}

export class SplitListModel extends SplitGridModel {
    public scripts: ScriptMainModel[] = [];

}
export class SplitParameterModel extends PagingSortingModel {
	public id: number = 0;
    public scriptId: number = 0;
	public announceDate?: Date = new Date(0);
    public rewardDate?: Date= new Date(0);
}
