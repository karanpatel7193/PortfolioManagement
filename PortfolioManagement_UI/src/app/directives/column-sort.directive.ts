import { Directive, Input, Renderer2, ElementRef, HostListener, Output, EventEmitter } from '@angular/core';
import { Sort } from './sort';
import { SpinnerService } from '../components/spinner/spinner.service';

@Directive({
    selector: '[appColumnSort]'
})
export class ColumnSortDirective {

    @Input() appColumnSort: Array<any> = [];
    @Output() onSorting = new EventEmitter<string>();

    constructor(private renderer: Renderer2, private targetElement: ElementRef, private spinnerService: SpinnerService) { }

    @HostListener("click")
    sortData() {
        const sort = new Sort();
        const elem = this.targetElement.nativeElement;
        const order = elem.getAttribute("data-order");
        const type = elem.getAttribute("data-type");
        const property = elem.getAttribute("data-name");

        this.onSorting.emit(property);
        this.spinnerService.show();
        if (order === "desc") {
            this.appColumnSort.sort(sort.startSort(property, order, type));
            elem.setAttribute("data-order", "asc");
            elem.classList.remove('asc-order');
            elem.classList.add('desc-order');
        } else {
            this.appColumnSort.sort(sort.startSort(property, order, type));
            elem.setAttribute("data-order", "desc");
            elem.classList.remove('desc-order');
            elem.classList.add('asc-order');
        }
        setTimeout(() => this.spinnerService.hide(), 1000);
        
    }
}
