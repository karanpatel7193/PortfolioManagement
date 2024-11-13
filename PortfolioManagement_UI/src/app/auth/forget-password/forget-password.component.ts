import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-forget-password',
  templateUrl: './forget-password.component.html',
  styleUrls: ['./forget-password.component.scss']
})
export class ForgetPasswordComponent implements OnInit {

    constructor() { }

    ngOnInit(): void {
    }

    public save(isFormValid: boolean): void {
        throw new Error('Method not implemented.');
    }

}
