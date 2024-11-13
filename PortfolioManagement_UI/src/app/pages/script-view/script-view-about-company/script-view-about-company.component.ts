import { Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { ScriptViewAboutCompanyService } from './script-view-about-company.service';
import { ScriptViewAboutCompanyModel } from './script-view-about-company.model';

@Component({
  selector: 'app-script-view-about-company',
  templateUrl: './script-view-about-company.component.html',
  styleUrls: ['./script-view-about-company.component.scss']
})
export class ScriptViewAboutCompanyComponent implements OnInit {
  @Input() id!:number; 
  public scriptViewAboutCompanyModel:ScriptViewAboutCompanyModel = new ScriptViewAboutCompanyModel();

  constructor(private scriptViewAboutCompanyService: ScriptViewAboutCompanyService) { }


  ngOnInit(): void {
  }
  ngOnChanges(changes: SimpleChanges): void {
    if(changes['id'] && changes['id'].currentValue){
      this.getScriptData();
    }
  }
  public getScriptData(){
      this.scriptViewAboutCompanyService.getRecord(this.id).subscribe((data)=>{
      this.scriptViewAboutCompanyModel = data;
      })
  }

}
