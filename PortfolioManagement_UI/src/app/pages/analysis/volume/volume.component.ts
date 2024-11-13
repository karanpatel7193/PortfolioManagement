import { Component, OnInit } from '@angular/core';
import { AccessModel } from 'src/app/models/access.model';
import { SessionService } from 'src/app/services/session.service';
import { Router } from '@angular/router';
import { ToastService } from 'src/app/services/toast.service';
import { VolumeGridModel, VolumeModel } from './volume.model';
import { VolumeService } from './volume.service';

@Component({
  selector: 'app-stock-volume',
  templateUrl: './volume.component.html',
  styleUrls: ['./volume.component.scss']
})
export class VolumeComponent implements OnInit {
  public volumeModel: VolumeModel = new VolumeModel();
  public volumeGrid: VolumeGridModel = new VolumeGridModel();
  public access: AccessModel = new AccessModel();
  public volumes: VolumeModel[] = [];

  constructor(private volumeService: VolumeService,
    private sessionService: SessionService,
    private router: Router,
    private toastr: ToastService) {
    this.setAccess();
  }

  ngOnInit() {
    this.fetchData();
  }


  public fetchData(): void {
    this.volumeService.getForVolume().subscribe(data => {
      this.volumeGrid = data;
    });
  }

  public setAccess(): void {
    //const currentUrl = this.router.url.substring(0, this.router.url.indexOf('/list'));
    this.access = this.sessionService.getAccess('report/stockTransactionreport');
  }

  public navigate(id: number) {
    this.router.navigate([`app/scriptView/${id}`]);
  }
}
