import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IndexHeaderService } from './index-header.service';
import { HeaderGridModel, HeaderModel } from './index-header.model';

@Component({
	selector: 'app-script-view-niftySensex',
	templateUrl: './index-header.component.html',
	styleUrls: ['./index-header.component.scss']
})
export class IndexHeaderComponent implements OnInit {
	@Input() id!: number;
	//public niftySensexGridModel: HeaderNifty50Model[] = [];
	public headerGridModel: HeaderGridModel = new HeaderGridModel();
	public headerModel: HeaderModel = new HeaderModel();
	//public niftyModel: ScriptViewNiftyModel = new ScriptViewNiftyModel();

	constructor(private indexHeaderService: IndexHeaderService, private route: ActivatedRoute) { }

	ngOnInit(): void {
		//this.setId()
		this.getScriptData()
	}

	public getScriptData(): void {
		this.indexHeaderService.getForGrid().subscribe((data) => {
			// this.headerGridModel = data; 
			this.headerGridModel.nifty50 = data.nifty50;
		});
		this.indexHeaderService.getForIndex().subscribe((data)=>{
			this.headerModel = data;
			});
			// this.scriptViewNiftySensexService.getnifty50().subscribe((data)=>{
			// 	this.niftyModel = data;
			// 	});	
	}
	scrollLeft() {
		const container = document.querySelector('.ttape-inner');
		if (container) {
		  container.scrollBy({ left: -100, behavior: 'smooth' });
		}
	  }
	  
	  scrollRight() {
		const container = document.querySelector('.ttape-inner');
		if (container) {
		  container.scrollBy({ left: 100, behavior: 'smooth' });
		}
	  }
}
