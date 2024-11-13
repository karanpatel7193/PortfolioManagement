import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-master',
  template: `<router-outlet></router-outlet>`,
})
export class MasterComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

}
