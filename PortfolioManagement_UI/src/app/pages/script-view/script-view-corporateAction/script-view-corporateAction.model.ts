
export class ScriptViewCorporateActionModel {
	// public scriptID: number = 0;
    // public isSplit: boolean = false;
    // public isBonus: boolean = false;
    // public oldFaceValue?: number ;
    // public newFaceValue?: number ;
    // public fromRatio?: number ;
    // public toRatio?: number ;
    // public announceDate?: string = '';  
    // public rewardDate?: string = '';
    // public isApply: boolean = false;
}
export class ScriptViewCorporateActionBonusModel {
    public fromRatio?: number;
    public toRatio?: number;
    public announceDate?: Date;  
    public rewardDate?: Date;
}

export class ScriptViewCorporateActionSplitModel {
    public oldFaceValue?: number;
    public newFaceValue?: number;
    public announceDate?: Date;  
    public rewardDate?: Date; 
    public isSplit?:boolean;
}