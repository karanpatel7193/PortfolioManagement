import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-script-view',
  templateUrl: './script-view.component.html',
  styleUrls: ['./script-view.component.scss']
})
export class ScriptViewComponent {
    public id: number = 0;
    constructor(private route: ActivatedRoute) { }
    
    ngOnInit(): void {
        this.setId()
    }

    public setId(){
      this.route.params.subscribe(x=>{
          this.id = +x['id']
          console.log('id: ', this.id);
      })
  }

}
