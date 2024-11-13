import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { ScriptViewOverviewService } from './script-view-overview.service';
import { ScriptViewOverviewModel } from './script-view-overview.model';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-script-view-overview',
  templateUrl: './script-view-overview.component.html',
  // styleUrls: ['./script-view-overview.component.scss'] 
})
export class ScriptViewOverviewComponent implements OnInit, OnChanges{
  @Input() id!: number;
  public scriptviewOverviewModel:ScriptViewOverviewModel = new ScriptViewOverviewModel();

  constructor(private scriptViewOverviewService: ScriptViewOverviewService, private route: ActivatedRoute) { }
  
  ngOnInit(): void {
  }

  ngOnChanges(changes: SimpleChanges): void {
    if(changes['id'] && changes['id'].currentValue){
      this.getScriptData();
    }
  }

  public getScriptData(){
    this.scriptViewOverviewService.getRecord(this.id).subscribe((data)=>{
      this.scriptviewOverviewModel = data;
    })
  }


}
