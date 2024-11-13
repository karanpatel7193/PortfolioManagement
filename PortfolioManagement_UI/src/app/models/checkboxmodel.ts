export class CheckBoxModel
{
    public name : string = "";
    public isSelected: boolean = false;

    constructor( name : string ,isSelected: boolean ){
        this.name=name;
        this.isSelected=isSelected;
    }
}

