
import { PagingSortingModel } from '../../../models/pagingsorting.model';



export class ScriptMainModel {
	public id: number = 0;
	public name: string = '';

}

export class ScriptModel extends ScriptMainModel {
	public bseCode: number = 0;
	public nseCode: string = '';
	public isinCode: string = '';
	public moneyControlURL: string = '';
	public fetchURL: string = '';
	public isFetch: boolean = false;
	public isActive: boolean = false;
	public price: number = 0;
	public industryName: string = ''
	public faceValue: number = 0;
	public group: string = ''

}

export class ScriptAddModel {

}

export class ScriptEditModel extends ScriptAddModel {
	public script: ScriptModel = new ScriptModel();
}

export class ScriptGridModel {
	public scripts: ScriptModel[] = [];
	public totalRecords: number = 0;
}

export class ScriptListModel extends ScriptGridModel {

}

export class ScriptParameterModel extends PagingSortingModel {
	public id: number = 0;
	public name: string = '';
	public bseCode: number = 0;
	public nseCode: string = '';
	public faceValue: number = 0;

}
