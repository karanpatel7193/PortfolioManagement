import { Component, OnInit } from '@angular/core';
import { AccessModel } from 'src/app/models/access.model';
import { SessionService } from 'src/app/services/session.service';
import { Router } from '@angular/router';
import { ToastService } from 'src/app/services/toast.service';
import { VolumeGridModel, VolumeModel } from './volume.model';
import { VolumeService } from './volume.service';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-stock-volume',
  templateUrl: './volume.component.html',
  styleUrls: ['./volume.component.scss']
})
export class VolumeComponent implements OnInit {
    public volumeModel: VolumeModel = new VolumeModel();
    public volumeGrid: VolumeGridModel = new VolumeGridModel();
    public access: AccessModel = new AccessModel();

    sortColumn: string = ''; 
    sortDirection: string = '';  

    constructor(private volumeService: VolumeService,
        private sessionService: SessionService,
        private router: Router,
        private toastr: ToastService,
        private commonService: CommonService) {
        this.setAccess();
    }

    ngOnInit() {
        this.getVolumeData();
    }

    public getVolumeData(): void {
        this.volumeService.getForVolume().subscribe(data => {
            this.volumeGrid.volumes = data.volumes;
        });
    }

    sortData(column: keyof VolumeModel) {
        this.volumeGrid.volumes = this.commonService.sortGrid(this.volumeGrid.volumes, column, this.sortColumn, this.sortDirection);
    
        if (this.sortColumn === column) {
            this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
        } else {
            this.sortColumn = column;
            this.sortDirection = 'asc'; 
        }
    }

    public redirect(id: number, symbol:string){
		this.commonService.redirectToPage(id,symbol);
	}

    public setAccess(): void {
        this.access = this.sessionService.getAccess('app/analysis');
    }

    public navigate(id: number) {
        this.router.navigate([`app/scriptView/${id}`]);
    }
}
