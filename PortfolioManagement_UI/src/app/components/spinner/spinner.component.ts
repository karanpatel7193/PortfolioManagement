import { Component, OnInit } from '@angular/core';
import { SpinnerService } from './spinner.service';

@Component({
  selector: 'app-spinner',
  templateUrl: './spinner.component.html',
  styleUrls: ['./spinner.component.scss']
})
export class SpinnerComponent implements OnInit {
  public isShow: boolean = false;

  constructor(private spinner: SpinnerService) { 
    this.spinner._register(this);
  }

  ngOnInit(): void {
  }

}
