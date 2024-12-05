import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-registration-success',
  templateUrl: './registration-success.component.html',
})
export class RegistrationSuccessComponent implements OnInit {

    constructor(private activeModal: NgbActiveModal, private router: Router) {
    }
    
    ngOnInit() {
    }
    
    public OnClosed(): void {
        this.activeModal.close('');
        this.router.navigate(['auth/login'])
        //this.router.navigate([{ outlets: { popup: null } }]);
    }

}
