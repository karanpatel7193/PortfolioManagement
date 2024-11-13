import { Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { ScriptViewCorporateActionBonusModel,  ScriptViewCorporateActionSplitModel } from './script-view-corporateAction.model';
import { ScriptViewCorporateActionService } from './script-view-corporateAction.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-script-view-corporateAction',
  templateUrl: './script-view-corporateAction.component.html',
  styleUrls: ['./script-view-corporateAction.component.scss']
})
export class ScriptViewCorporateActionComponent implements OnInit {
  public corporateActionBonusModel: ScriptViewCorporateActionBonusModel[] = [];
  public corporateActionSplitModel: ScriptViewCorporateActionSplitModel[] = [];

  @Input() id!: number;
  selectedTab: string = 'bonus';
  // Flags to check if data has been loaded
  private bonusDataLoaded: boolean = false;
  private splitDataLoaded: boolean = false;
  constructor(private scriptViewCorporateActionService: ScriptViewCorporateActionService, private route: ActivatedRoute) { }

  ngOnInit(): void {
  }

  ngOnChanges(changes: SimpleChanges): void {
    if(changes['id'] && changes['id'].currentValue){
      this.selectTab(this.selectedTab); // Load data for the default tab
    }
  }
 
  selectTab(tab: string) {
    this.selectedTab = tab;
    if (tab === 'bonus') {
      if (!this.bonusDataLoaded) {
        this.getScriptBonusData();
      }
    } 
    else if (tab === 'split') {
      if(!this.splitDataLoaded){
        this.getScriptSplitData();
      }
    }
  }

  public getScriptBonusData() {
    this.scriptViewCorporateActionService.getRecordBonus(this.id).subscribe((data) => {
      this.corporateActionBonusModel = data;
      this.bonusDataLoaded = true;  // sure that data is loaded
    });
  }

  public getScriptSplitData() {
    this.scriptViewCorporateActionService.getRecordSplit(this.id).subscribe((data) => {
      this.corporateActionSplitModel= data;
      this.splitDataLoaded = true; 
    });
  }
}
