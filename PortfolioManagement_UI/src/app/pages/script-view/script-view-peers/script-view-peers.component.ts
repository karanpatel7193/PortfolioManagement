import { Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { ScriptViewPeersModel } from './script-view-peers.model';
import { ScriptViewPeersService } from './script-view-peers.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-script-view-peers',
  templateUrl: './script-view-peers.component.html',
  styleUrls:['./script-view-peers.component.scss']
})
export class ScriptViewPeersComponent implements OnInit {
@Input() id!:number; 

  public scriptViewPeersModel:ScriptViewPeersModel[] = [];
  constructor(private scriptViewPeersService: ScriptViewPeersService) { }

  ngOnInit(): void {
  }

  ngOnChanges(changes: SimpleChanges): void {
    if(changes['id'] && changes['id'].currentValue){
        this.getScriptData();
    }
}
  public getScriptData(){
      this.scriptViewPeersService.getRecord(this.id).subscribe((data)=>{
      this.scriptViewPeersModel = data;
      })
  }
}
