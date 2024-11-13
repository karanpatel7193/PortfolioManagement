import { SpinnerComponent } from './spinner.component';
import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root'
})
export class SpinnerService {
    private spinnerComponent?: SpinnerComponent;
    public isSpinnerShow: boolean = false;
    constructor() { }

    _register(spinner: SpinnerComponent): void {
        this.spinnerComponent = spinner;
    }

    show(): void {
        if (this.spinnerComponent)
            this.isSpinnerShow = this.spinnerComponent.isShow = true;
    };

    hide(): void {
        if (this.spinnerComponent)
            this.isSpinnerShow = this.spinnerComponent.isShow = false;
    }
}