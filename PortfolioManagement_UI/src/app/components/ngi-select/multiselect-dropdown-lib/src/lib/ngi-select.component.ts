import { DropdownSettings } from './ngiselect.interface';
import { Component, OnInit, HostListener, OnDestroy, NgModule, SimpleChanges, OnChanges, ChangeDetectorRef, AfterViewChecked, ViewEncapsulation, ContentChild, ViewChild, forwardRef, Input, Output, EventEmitter, ElementRef, SimpleChange, HostBinding } from '@angular/core';
import { FormsModule, NG_VALUE_ACCESSOR, ControlValueAccessor, NG_VALIDATORS, Validator, FormControl } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MyException, ServiceResponse } from './ngiselect.model';
//import { DropdownSettings } from './ngiselect.interface';
import { ClickOutsideDirective, ScrollDirective, styleDirective, setPosition } from './clickOutside';
import { ListFilterPipe } from './list-filter';
import { Item, Badge, Search, TemplateRenderer, CIcon } from './menu-item';
import { DataService } from './ngiselect.service';
import { Subscription, Subject } from 'rxjs';
// import { VirtualScrollerModule, VirtualScrollerComponent } from './virtual-scroll/virtual-scroll';
import { debounceTime, distinctUntilChanged, tap } from 'rxjs/operators';
import { LazyLoadingService } from './httpData.service';

export const DROPDOWN_CONTROL_VALUE_ACCESSOR: any = {
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => NgiSelect),
    multi: true
};
export const DROPDOWN_CONTROL_VALIDATION: any = {
    provide: NG_VALIDATORS,
    useExisting: forwardRef(() => NgiSelect),
    multi: true,
};
export enum KEY_CODE {
    ESCAPE_KEYCODE = 27,
    DOWN_KEYCODE = 40,
    UP_KEYCODE = 38,
    ENTER_KEYCODE = 13,
    SPACE_KEYCODE = 32,
    TAB_KEYCODE = 9,
    BACKSPACE_KEYCODE = 8,
};
export enum HTTPTYPE {
    GET = 0,
    POST = 1,
};
const noop = () => {
};

@Component({
    selector: 'ngi-select',
    templateUrl: './ngiselect.component.html',
    host: { '[class]': 'defaultSettings.classes' },
    styleUrls: ['./ngiselect.component.scss'],
    providers: [DROPDOWN_CONTROL_VALUE_ACCESSOR, DROPDOWN_CONTROL_VALIDATION],
    encapsulation: ViewEncapsulation.None,
})

export class NgiSelect implements OnInit, ControlValueAccessor, OnChanges, Validator, AfterViewChecked, OnDestroy {

    @Input()
    data!: Array<any>;

    @Input()
    settings!: DropdownSettings;

    @Input()
    loading!: boolean;

    @Output('onSelect')
    onSelect: EventEmitter<any> = new EventEmitter<any>();

    @Output('onDeSelect')
    onDeSelect: EventEmitter<any> = new EventEmitter<any>();

    @Output('onSelectAll')
    onSelectAll: EventEmitter<Array<any>> = new EventEmitter<Array<any>>();

    @Output('onDeSelectAll')
    onDeSelectAll: EventEmitter<Array<any>> = new EventEmitter<Array<any>>();

    @Output('onOpen')
    onOpen: EventEmitter<any> = new EventEmitter<any>();

    @Output('onClose')
    onClose: EventEmitter<any> = new EventEmitter<any>();

    @Output('onBackSpace')
    onBackSpace: EventEmitter<any> = new EventEmitter<any>();

    @Output('onOk')
    onOk: EventEmitter<Array<any>> = new EventEmitter<Array<any>>();

    @Output('onFreeText')
    onFreeText: EventEmitter<any> = new EventEmitter<any>();

    @Output('onScrollToEnd')
    onScrollToEnd: EventEmitter<any> = new EventEmitter<any>();

    @Output('onFilterSelectAll')
    onFilterSelectAll: EventEmitter<Array<any>> = new EventEmitter<Array<any>>();

    @Output('onFilterDeSelectAll')
    onFilterDeSelectAll: EventEmitter<Array<any>> = new EventEmitter<Array<any>>();

    @Output('onAddFilterNewItem')
    onAddFilterNewItem: EventEmitter<any> = new EventEmitter<any>();

    @Output('onGroupSelect')
    onGroupSelect: EventEmitter<any> = new EventEmitter<any>();

    @Output('onGroupDeSelect')
    onGroupDeSelect: EventEmitter<any> = new EventEmitter<any>();

    @Output('onChange')
    onChange: EventEmitter<any> = new EventEmitter<any>();

    @Output('onScrollDemand')
    onUserScrollDemand!: Array<any>;

    @ContentChild(Item, { static: false }) itemTempl!: Item;
    @ContentChild(Badge, { static: false }) badgeTempl!: Badge;
    @ContentChild(Search, { static: false }) searchTempl!: Search;

    @ViewChild('searchInput', { static: false }) searchInput!: ElementRef;
    @ViewChild('selectedList', { static: false }) selectedListElem!: ElementRef;
    @ViewChild('dropdownList', { static: false }) dropdownListElem!: ElementRef;
    @ViewChild('selectedspan', { static: false }) selectedspan!: ElementRef;
    // @ViewChild(VirtualScrollerComponent, { static: false }) private virtualScroller!: VirtualScrollerComponent;

    @HostBinding('style.width') widthstyle!: string;
    @HostListener('keyup', ['$event'])
    onKeyDown(event: KeyboardEvent) {
        if (event.keyCode === KEY_CODE.ESCAPE_KEYCODE) {
            if (this.settings.escapeToClose) {
                this.closeDropdown();
            }
        }
        //Arraow Down
        if (event.keyCode === KEY_CODE.DOWN_KEYCODE && this.data.length > 0) {

            this.onArrowKeyDown();

            if (this.focusedItemIndex >= this.data.length) {
                this.focusedItemIndex = 0;
            }


            if (this.filter && this.filter.length > 0) {
                this.data = this.ds.getFilteredData();
            }

            this.data[this.focusedItemIndex].nativeElement;
            this.dropdownListElem.nativeElement.focus();
            // if (this.virtualScroller)
            //     this.virtualScroller.scrollToIndex(this.focusedItemIndex);
            //this.openDropdown();
            event.preventDefault();
        }
        //Arraw UP
        if (event.keyCode === KEY_CODE.UP_KEYCODE && this.data.length > 0) {

            this.onArrowKeyUp();

            if (this.focusedItemIndex >= this.data.length) {
                this.focusedItemIndex = this.data.length - 1;
            }
            if (this.filter && this.filter.length > 0) {
                this.data = this.ds.getFilteredData();
            }

            this.data[this.focusedItemIndex].nativeElement;

            this.dropdownListElem.nativeElement.focus();
            // if (this.virtualScroller)
            //     this.virtualScroller.scrollToIndex(this.focusedItemIndex);
            // this.openDropdown();
            event.preventDefault();
        }
        //Enter
        if (event.keyCode === KEY_CODE.ENTER_KEYCODE && this.focusedItemIndex !== null) {
            // let focusedItem = this.data[this.focusedItemIndex];
            // console.log(focusedItem);
            //this.onItemClick(focusedItem, this.focusedItemIndex, event);
        }

    }

    public virtualdata: any = [];
    public searchTerm$ = new Subject<string>();
    public filterPipe!: ListFilterPipe;
    public isBackspace: boolean = false;
    public backspaceCount: any = 0;
    public timer: any;
    public selectedItems!: Array<any>;
    public toolTipItems!: Array<any>;
    public isExistData: boolean = true;
    public isPresentData: boolean = false;
    public isLazyLoadingPresentData: boolean = true;
    public isActive: boolean = false;
    public focusedItemIndex: number = 0;
    public isFreeTextActive: boolean = false;
    public isBlanckActive = false;
    public isSelectAll: boolean = false;
    public isFilterSelectAll: boolean = false;
    public isInfiniteFilterSelectAll: boolean = false;
    public groupedData!: Array<any>;
    public filter: any;
    public chunkArray!: any[];
    public scrollTop: any;
    public chunkIndex: any[] = [];
    public cachedItems: any[] = [];
    public groupCachedItems: any[] = [];
    public totalRows: any;
    public itemHeight: any = 41.6;
    public screenItemsLen: any;
    public cachedItemsLen: any;
    public totalHeight: any;
    public scroller: any;
    public maxBuffer: any;
    public lastScrolled: any;
    public lastRepaintY: any;
    public selectedListHeight: any;
    public filterLength: any = 0;
    public infiniteFilterLength: any = 0;
    public viewPortItems: any;
    public item: any;
    public dropdownListYOffset: number = 0;
    public isScroller!: boolean;
    public widthspan: number = 100;
    public listfilterspan: number = 0;
    public subscription!: Subscription;
    public randomSize: boolean = true;
    public parseError!: boolean;
    public filteredList: any = [];
    public virtualScroollInit: boolean = false;
    public isDisabledItemPresent = false;
    public onDemandScrollFlag = false;
    public onDemandSearchScrollFlag = false;
    public constantLimitForAddition: number = 0;
    public newScrolledLimit: number = 0;
    public copyOriginalDataForIndex: any = [];
    public copyFilteredOriginalDataForIndex: any = []
    public copyFilterDataChunk: any = []
    public defaultSettings: DropdownSettings = {
        singleSelection: false,
        text: 'Select',
        enableCheckAll: true,
        selectAllText: 'Select All',
        unSelectAllText: 'UnSelect All',
        filterSelectAllText: 'Select all filtered results',
        filterUnSelectAllText: 'UnSelect all filtered results',
        enableSearchFilter: true,
        searchBy: [],
        loading: true,
        maxHeight: 150,
        badgeShowLimit: 999999999999,
        classes: '',
        limitSelection: 0,
        disabled: false,
        searchPlaceholderText: '',
        groupBy: '',
        showCheckbox: true,
        noDataLabel: 'No record found.',
        searchAutofocus: true,
        lazyLoading: false,
        labelKey: 'name',
        primaryKey: 'id',
        position: 'bottom',
        autoPosition: true,
        enableFilterSelectAll: true,
        selectGroup: false,
        addNewItemOnFilter: false,
        addNewButtonText: "Add",
        escapeToClose: true,
        clearAll: true,
        isAllowBackspaceClearSelected: false,
        allowFreeText: false,
        isFirstBlank: false,
        SelectedKey: '',
        limit: 0,
        apiURL: '',
        tooltipColumn: '',
        isOnScrollDemand: false,
        isDefaultAllSelected: false,
        isShowColorProperty: false,
        colorPropertyName: ''
    }

    constructor(
        public _elementRef: ElementRef,
        private cdr: ChangeDetectorRef,
        private ds: DataService,
        private lazzyservice: LazyLoadingService) {
        this.searchTerm$.asObservable().pipe(
            debounceTime(1000),
            distinctUntilChanged(),
            tap(term => term)
        ).subscribe(val => {
            this.filterInfiniteList(val);
        });
    }

    //handleKeyCode($event: KeyboardEvent) {
    //  switch ($event.which) {
    //    case KEY_CODE.DOWN_KEYCODE:
    //      this._handleArrowDown($event);
    //      break;
    //    case KEY_CODE.UP_KEYCODE:
    //      this._handleArrowUp($event);
    //      break;
    //    case KEY_CODE.SPACE_KEYCODE:
    //      this._handleSpace($event);
    //      break;
    //    case KEY_CODE.ENTER_KEYCODE:
    //      this._handleEnter($event);
    //      break;
    //    case KEY_CODE.TAB_KEYCODE:
    //      this._handleTab($event);
    //      break;
    //    case KEY_CODE.ESCAPE_KEYCODE:
    //      this._handleEscap();
    //      $event.preventDefault();
    //      break;
    //    case KEY_CODE.BACKSPACE_KEYCODE:
    //      this._handleBackspace();
    //      break
    //  }
    //}

    //private _handleArrowDown($event: KeyboardEvent) {

    //}

    //private _handleArrowUp($event: KeyboardEvent) {

    //}
    //private _handleSpace($event: KeyboardEvent) {

    //}

    //private _handleEnter($event: KeyboardEvent) {

    //}

    //private _handleTab($event: KeyboardEvent) {

    //}

    //private _handleBackspace() {

    //}

    //private _handleEscap() {
    //  if (this.settings.escapeToClose) {
    //    this.closeDropdown();
    //  }
    //}

    ngOnInit() {
        this.settings = Object.assign(this.defaultSettings, this.settings);
        this.cachedItems = this.cloneArray(this.data);
        if (this.cachedItems) {
            if (this.cachedItems.length > 0) {
                this.isPresentData = true;
                if (this.settings.isDefaultAllSelected) {
                    this.toggleSelectAll()
                }
            } else {
                this.isPresentData = false;
            }
        } else {
            this.isPresentData = false;
        }

        if (this.settings.position == 'top') {
            setTimeout(() => {
                this.selectedListHeight = { val: 0 };
                this.selectedListHeight.val = this.selectedListElem.nativeElement.clientHeight;
            });
        }

        this.subscription = this.ds.getData().subscribe(data => { });
        setTimeout(() => {
            this.calculateDropdownDirection();
        });

        this.virtualScroollInit = false;
        // this.onChangeCallback(this.selectedItems);
    }

    ngOnChanges(changes: { [P in keyof this]?: SimpleChange } & SimpleChanges) {
        // debugger;
        if (changes.data && !changes.data.firstChange) {


            if (this.settings.groupBy) {
                this.groupedData = this.transformData(this.data, this.settings.groupBy);
                if (this.data.length == 0) {
                    this.selectedItems = [];
                    this.toolTipItems = [];
                }
                this.groupCachedItems = this.cloneArray(this.groupedData);
            }
            this.cachedItems = this.cloneArray(this.data);

        }

        // if (!this.settings || Object.keys(this.settings).length > 0) {
        //     return;
        // }

        if (changes.settings && !changes.settings.firstChange) {
            this.settings = Object.assign(this.defaultSettings, this.settings);
        }

        if (changes.loading) { }

        if (this.settings && this.settings.lazyLoading && this.virtualScroollInit && changes.data) {
            this.virtualdata = changes.data.currentValue;
        }

        if (this.data)
            this.filterLength = this.data.length;
        else
            this.filterLength = 0;

        if (this.data) {
            let len = 0;
            this.data.forEach((obj: any, i: any) => {
                if (obj.disabled) {
                    this.isDisabledItemPresent = true;
                }
                if (!obj.hasOwnProperty('grpTitle')) {
                    len++;
                }
            });
            this.filterLength = len;
            if (this.filterLength > 0) {
                this.isPresentData = true;
            } else {
                this.isPresentData = false;
            }
            this.onFilterChange(this.data);
        }

        if (this.settings && this.settings.isFirstBlank && this.settings.singleSelection) {

            if (this.data[0] !== 0)
                this.data.splice(0, 0, "");
        }

        this.cachedItems = this.data;

        if (this.settings && this.settings.isDefaultAllSelected) {
            this.toggleSelectAll();
        }
    }

    ngDoCheck() {
        if (this.selectedItems) {
            if (this.settings.singleSelection) {
                this.isSelectAll = false;
            } else {
                if (this.data && this.selectedItems && (this.selectedItems.length == 0 || this.data.length == 0 || this.selectedItems.length < this.data.length)) {
                    this.isSelectAll = false;
                }
            }
        }
    }

    ngAfterViewInit() {
        if (this.settings.lazyLoading) {
            // this._elementRef.nativeElement.getElementsByClassName("lazyContainer")[0].addEventListener('scroll', this.onScroll.bind(this));
        }
    }

    ngAfterViewChecked() {
        if (this.selectedListElem.nativeElement.clientHeight && this.settings.position == 'top' && this.selectedListHeight) {
            this.selectedListHeight.val = this.selectedListElem.nativeElement.clientHeight;
            this.cdr.detectChanges();
        }
    }

    onSearchText(evt: any) {
        if (evt.keyCode == 13) {
            if (this.data.length > 0 && !this.isFreeTextActive) {
                let focusedItem = this.data[this.focusedItemIndex];
                this.onItemClick(focusedItem, this.focusedItemIndex, event);
            }
        }
        if (evt.key == " ") {
            if (this.settings.isAllowBackspaceClearSelected) {
                if (this.isBackspace) {
                    this.clearSelection(evt);
                    evt.stopPropagation();
                    this.isBackspace = false;
                    this.backspaceCount = 0
                }
                if (this.filter != undefined) {
                    if (this.filter.length == 0) {
                        this.backspaceCount = this.backspaceCount + 1;
                        //if (this.backspaceCount >= 2) {
                        this.isBackspace = true;
                        //}
                    }
                } else {
                    this.filter = "";
                }

                this.onBackSpace.emit();
            }
            // if (!this.isBackspace) {
            if (this.settings.allowFreeText && this.settings.singleSelection) {
                if (this.filter.length == 0) {
                    this.isFreeTextActive = false
                }
            }
            if (this.settings.lazyLoading && this.settings.apiURL != null) {
                this.clearlazyData();
                this.onlazyExecute();
            }
            this.resetItemsDataSet();
            this.resetArrowKeyActiveElement();
            // }

        } else {

            if (evt.keyCode >= 65 && evt.keyCode <= 90 || evt.keyCode >= 48 && evt.keyCode <= 57 || ((evt.ctrlKey || evt.metaKey) && evt.keyCode == 86)) {
                this.isBackspace = false;
                if (this.searchInput) {
                    this.openDropdown();
                }
                if (this.settings.allowFreeText && this.settings.singleSelection) {
                    let freetext = '';
                    if (((evt.ctrlKey || evt.metaKey) && evt.keyCode == 86)) {
                        this.isFreeTextActive = true;
                    }
                    if (this.filter.length >= 2) {
                        if (this.selectedItems) {
                            if (!this.isFreeTextActive) {
                                if (this.toolTipItems.length > 0) {
                                    freetext = this.toolTipItems[0][this.settings.labelKey] + this.filter;

                                    this.filter = freetext;
                                } else {
                                    this.selectedItems = JSON.parse(JSON.stringify({ id: 0, name: this.filter }));
                                }
                            }
                            if (this.isFreeTextActive) {
                                this.selectedItems = JSON.parse(JSON.stringify({ id: 0, name: this.filter }));
                                this.toolTipItems = [];
                                this.onChangeCallback(this.selectedItems);
                                this.onTouchedCallback(this.selectedItems);
                            }
                        }
                        this.isFreeTextActive = true;
                    } else {
                        this.isFreeTextActive = false;
                    }
                }

                if (this.settings.lazyLoading && this.filter.length > 0 && this.settings.apiURL != null) {

                    this.clearlazyData();
                    this.onlazyExecute();
                }

                this.resetArrowKeyActiveElement();
            }
        }
    }

    onItemClick(item: any, index: number, evt: any):any {
        // debugger;
        if (!index && evt.key == "Enter")
            return false;
        if (item.disabled)
            return false;
        if (this.settings.disabled)
            return false;

        let found = this.isSelected(item);
        let limit = this.selectedItems.length < this.settings.limitSelection ? true : false;
        if (this.settings.limitSelection == 0) {
            this.settings.limitSelection = this.selectedItems.length;
            limit = this.selectedItems.length < this.settings.limitSelection ? true : false;
        }

        if (this.settings.isFirstBlank && this.settings.singleSelection) {
            if (item == "") {
                this.isBlanckActive = true
                this.clearSelection(this.item);
            } else
                this.isBlanckActive = false
        }

        if (!found) {
            if (this.settings.limitSelection) {
                if (limit) {
                    this.addSelected(item);
                    this.onSelect.emit(item);
                    this.onChange.emit(item);
                }
            } else {
                if (this.settings.SelectedKey == '') {
                    this.addSelected(item);
                    this.onSelect.emit(item);
                    this.onChange.emit(item);
                } else {
                    this.addSelected(item);
                    this.onSelect.emit(item);
                    this.onChange.emit(item[this.settings.SelectedKey]);
                }
            }
            this.resetArrowKeyActiveElement();
        } else {
            if (!this.settings.singleSelection) {
                this.removeSelected(item);
                this.onDeSelect.emit(item);
                this.onChange.emit(item);
                this.resetArrowKeyActiveElement();
            } else
                this.closeDropdown();
        }

        if (this.isSelectAll || this.data.length > this.selectedItems.length)
            this.isSelectAll = false;
        if (this.data.length == this.selectedItems.length)
            this.isSelectAll = true;
        if (this.settings.groupBy)
            this.updateGroupInfo(item);

        this.resetItemsDataSet();
    }

    public validate(c: FormControl): any {
        return null;
    }

    private onTouchedCallback: (_: any) => void = noop;
    private onChangeCallback: (_: any) => void = noop;
    writeValue(value: any) {
        // debugger;
        if (value !== undefined && value !== null && value !== '') {
            if (this.settings.singleSelection) {

                this.toolTipItems = [];
                this.selectedItems = [];

                if (this.settings.groupBy) {
                    this.groupedData = this.transformData(this.data, this.settings.groupBy);
                    this.groupCachedItems = this.cloneArray(this.groupedData);
                    this.selectedItems = this.setSelectedItem([value[0]]);
                    //this.toolTipItems = [value[0]];
                } else {
                    try {
                        if (this.settings.SelectedKey == '') {
                            if (value.length > 1) {
                                this.selectedItems = [value[0]];

                                throw new MyException(404, { "msg": "Single Selection Mode, Selected Items cannot have more than one item." });
                            } else {
                                this.selectedItems = value;
                                this.toolTipItems.push(value);
                            }
                        } else {
                            this.addPropertyBaseitems(value);
                        }
                        //this.onSelect.emit(value);
                        this.onChange.emit(value);
                    }
                    catch (e) {
                        // console.error(e.body.msg);
                    }
                }
            } else {

                if (this.settings.limitSelection) {
                    this.selectedItems = this.setSelectedItem(value.slice(0, this.settings.limitSelection));
                    //this.toolTipItems = value.slice(0, this.settings.limitSelection);
                }
                else {
                    if (this.settings.SelectedKey == '') {
                        this.selectedItems = value;
                        this.toolTipItems = JSON.parse(JSON.stringify(this.selectedItems));
                    }
                    else {
                        if (value) {
                            if (value.length > 0) {
                                this.addPropertyBaseitems(value);
                            }
                            else {
                                this.selectedItems = JSON.parse(JSON.stringify(value));
                                this.toolTipItems = JSON.parse(JSON.stringify(value));
                            }
                        }
                        else {
                            this.selectedItems = value;
                            this.toolTipItems = value;
                        }

                    }
                }
                if (this.selectedItems && this.data) {
                    if (this.selectedItems.length === this.data.length && this.data.length > 0) {
                        this.isSelectAll = true;
                    }
                    if (this.settings.groupBy) {
                        this.groupedData = this.transformData(this.data, this.settings.groupBy);
                        this.groupCachedItems = this.cloneArray(this.groupedData);
                    }
                }
                //this.onSelect.emit(value);
                this.onChange.emit(value);
            }
        } else {
            this.selectedItems = [];
            this.toolTipItems = [];
        }

        this.onChangeCallback(this.selectedItems);
        this.onTouchedCallback(this.selectedItems);
    }

    addPropertyBaseitems(value: any[]) {
        try {
           let temp: any[] = [];
            if (!this.settings.singleSelection) {
                this.selectedItems = [];
                this.toolTipItems = [];
            }
            this.data.filter((el: any) => {
                if (el !== "") {
                    if (this.settings.singleSelection) {
                        if (el[this.settings.SelectedKey].toString().toLowerCase() === value.toString().toLowerCase()) {
                            if (this.settings.singleSelection) {
                                if (this.settings.SelectedKey != '') {
                                    this.selectedItems = el[this.settings.SelectedKey];
                                }
                                else {
                                    this.selectedItems = el;
                                }
                            }
                            this.toolTipItems.push(el);
                            return;
                        }
                    }
                    else {

                        value.forEach(item => {
                            let found = this.isSelected(item);
                            if (el[this.settings.SelectedKey].toString().toLowerCase() === item.toString().toLowerCase()) {
                                if (this.settings.singleSelection) {
                                    if (this.settings.SelectedKey != '') {
                                        this.selectedItems = el[this.settings.SelectedKey];
                                    }
                                    else {
                                        this.selectedItems = el;
                                    }
                                } else {
                                    if (this.settings.SelectedKey != '') {

                                        if (!found) {
                                            temp.push(el[this.settings.SelectedKey]);
                                        }
                                    }
                                    else {

                                        if (!found) {
                                            temp.push(el);
                                        }

                                    }
                                }
                                if (!found) {
                                    this.toolTipItems.push(el);
                                }
                                return;
                            }
                        });
                    }
                }
            });
            if (!this.settings.singleSelection) {
                this.selectedItems = temp;
            }
        } catch (e) {
            //console.log(e);
        }

    }

    setSelectedItem(data : any[]) {
        let itemList:any[] = [];
        this.toolTipItems = [];
        if (this.settings.SelectedKey) {
            data.forEach(item => {
                if (this.settings.limitSelection && itemList.length < this.settings.limitSelection) {
                    this.toolTipItems.push(item);
                    itemList.push(item[this.settings.SelectedKey]);
                }
                else if (!this.settings.limitSelection) {
                    this.toolTipItems.push(item);
                    itemList.push(item[this.settings.SelectedKey]);
                }
            })
            return itemList;
        }

        this.toolTipItems = data;
        return data;
    }

    //From ControlValueAccessor interface
    registerOnChange(fn: any) {
        this.onChangeCallback = fn;
    }

    //From ControlValueAccessor interface
    registerOnTouched(fn: any) {
        this.onTouchedCallback = fn;
    }

    trackByFn(index: number, item: any) {
        return item[this.settings.primaryKey];
    }

    isSelected(clickedItem: any) {
        // debugger;
        if (clickedItem.disabled) {
            return false;
        }
        let found = false;
        try {
            this.toolTipItems && this.toolTipItems.forEach(item => {
                if (clickedItem[this.settings.primaryKey] === item[this.settings.primaryKey]) {
                    found = true;
                }

                //if (this.settings.SelectedKey) {
                //  if (clickedItem[this.settings.primaryKey] == item) {
                //    found = true;
                //  }
                //} else {
                //  if (clickedItem[this.settings.primaryKey] === item[this.settings.primaryKey]) {
                //    found = true;
                //  }
                //}
            });

            if (this.selectedItems instanceof Array) {
                this.selectedItems && this.selectedItems.forEach(item => {
                    if (clickedItem[this.settings.primaryKey] === item[this.settings.primaryKey]) {
                        found = true;
                    }

                    //if (this.settings.SelectedKey) {
                    //  if (clickedItem[this.settings.primaryKey] == item) {
                    //    found = true;
                    //  }
                    //}else {
                    //  if (clickedItem[this.settings.primaryKey] === item[this.settings.primaryKey]) {
                    //    found = true;
                    //  }
                    //}
                });
            }
        } catch (e) {

        }

        return found;
    }

    addSelected(item: any) {
        if (item.disabled) {
            return;
        }

        if (this.settings.singleSelection) {
            this.selectedItems = [];
            this.toolTipItems = [];
            if (this.settings.SelectedKey) {
                if (this.settings.singleSelection) {
                    this.selectedItems = item[this.settings.SelectedKey];
                    this.toolTipItems.push(item);
                } else {
                    this.selectedItems.push(item[this.settings.SelectedKey]);
                    this.toolTipItems.push(item);
                }
            }
            else {
                if (this.settings.singleSelection) {
                    this.selectedItems = item;
                    this.toolTipItems.push(item);
                } else {
                    this.selectedItems.push(item);
                    this.toolTipItems = JSON.parse(JSON.stringify(this.selectedItems))
                }
            }
            this.closeDropdown();
        } else {
            if (this.settings.SelectedKey) {
                // if (this.settings.singleSelection) {
                //   this.selectedItems = item[this.settings.SelectedKey];
                // } else {
                this.selectedItems.push(item[this.settings.SelectedKey]);
                // }
                this.toolTipItems.push(item);
            }
            else {
                //if (this.settings.singleSelection) {
                //this.selectedItems = item[this.settings.primaryKey];
                // this.toolTipItems.push(item);
                //  }
                // else {
                this.selectedItems.push(item);
                this.toolTipItems = JSON.parse(JSON.stringify(this.selectedItems))
                //  }
                //Object.assign({}, this.selectedItems);
            }
        }

        this.onChangeCallback(this.selectedItems);
        this.onTouchedCallback(this.selectedItems);
    }

    removeSelected(clickedItem: any) {
        this.selectedItems && this.selectedItems.forEach(item => {
            if (this.settings.SelectedKey) {
                if (clickedItem[this.settings.primaryKey] == item) {
                    this.selectedItems.splice(this.selectedItems.indexOf(item), 1);
                    this.toolTipItems = this.toolTipItems.filter(obj => obj !== clickedItem);
                    //this.toolTipItems = JSON.parse(JSON.stringify(this.selectedItems));
                }

            } else {
                if (clickedItem[this.settings.primaryKey] === item[this.settings.primaryKey]) {
                    this.selectedItems.splice(this.selectedItems.indexOf(item), 1);
                    this.toolTipItems = JSON.parse(JSON.stringify(this.selectedItems));
                }
            }
        });

        this.onChangeCallback(this.selectedItems);
        this.onTouchedCallback(this.selectedItems);
    }

    setLOVSearchWidth(value: number) {
        this.widthspan = value;
    }

    setLOVFilterWidth(value: number) {
        this.listfilterspan = value;
    }

    toggleDropdown(evt: any):any {
        //debugger;
        // this.filter = '';
        this.copyOriginalDataForIndex = this.data;
        this.constantLimitForAddition = this.settings.limit?? 0;
        this.newScrolledLimit = this.settings.limit??0;
        if (this.filter && this.isActive)
            return false;

        if (this.settings.disabled) {
            return false;
        }

        if (this.settings.isOnScrollDemand) {
            this.onDemandScrollFlag = !this.onDemandScrollFlag;
            this.copyOriginalDataForIndex = this.copyOriginalDataForIndex.slice(0, this.settings.limit);
        }

        this.isActive = !this.isActive;

        //if (evt.toElement.className == "pixie-tc-check-all") {
        //  this.isActive = true;
        //} else {
        //  this.isActive = !this.isActive;
        //}

        if (this.isActive) {
            if (this.settings.searchAutofocus && this.searchInput && this.settings.enableSearchFilter && !this.searchTempl) {
                setTimeout(() => {
                    this.searchInput.nativeElement.focus();
                }, 0);
            }
            this.setLOVSearchWidth(50);
            this.setLOVFilterWidth(50);
            // this.renderer.setStyle(this.selectedspan.nativeElement, 'width', `50%`);
            this.onOpen.emit(true);
        } else {
            this.setLOVSearchWidth(100);
            this.setLOVFilterWidth(0);
            //this.renderer.setStyle(this.selectedspan.nativeElement, 'width', `100%`);
            this.onClose.emit(false);
        }

        setTimeout(() => {
            this.calculateDropdownDirection();
        }, 0);

        this.virtualdata = this.copyOriginalDataForIndex;
        this.virtualScroollInit = true;
        this.resetArrowKeyActiveElement();
        evt.preventDefault();
    }

    public clearlazyData() {
        //this.data = [];
        this.cachedItems = [];
        this.virtualdata = [];
    }

    public resetItemsDataSet() {
        if (this.filter != undefined) {
            if (this.filter == 0 && this.data.length != this.cachedItems.length) {
                this.data = this.cachedItems;
            }
        }
        else {
            if (this.data.length != this.cachedItems.length) {
                this.data = this.cachedItems;
            }
        }
        if (this.data.length > 0) {
            this.isPresentData = true;
        }
        else {
            this.isPresentData = false;
        }

    }

    public openDropdown():any {
        // console.log("open");
        if (this.isActive) {
            return;
        }
        if (this.settings.disabled) {
            return false;
        }
        this.isActive = true;
        if (this.settings.searchAutofocus && this.searchInput && this.settings.enableSearchFilter && !this.searchTempl) {
            setTimeout(() => {
                this.searchInput.nativeElement.focus();
            }, 0);
        }
        this.setLOVSearchWidth(50);
        this.setLOVFilterWidth(50);
        this.onOpen.emit(true);
    }

    public closeDropdown() {
        // console.log("close");
        if (this.searchInput && this.settings.lazyLoading) {
            if (!this.isFreeTextActive && !this.settings.allowFreeText)
                this.searchInput.nativeElement.value = "";
        }
        if (this.searchInput) {
            if (!this.isFreeTextActive && !this.settings.allowFreeText)
                this.searchInput.nativeElement.value = "";
        }
        if (!this.isFreeTextActive && !this.settings.allowFreeText)
            this.filter = "";

        if (this.isFreeTextActive && this.settings.allowFreeText) {
            this.onFreeText.emit({ value: this.filter });
        }
        if (this.onDemandScrollFlag)
            this.onDemandScrollFlag = !this.onDemandScrollFlag;

        if (this.onDemandSearchScrollFlag)
            this.onDemandSearchScrollFlag = !this.onDemandSearchScrollFlag;

        this.isActive = false;
        this.setLOVSearchWidth(100);
        this.setLOVFilterWidth(0);
        this.copyOriginalDataForIndex = [];
        this.constantLimitForAddition = 0;
        this.newScrolledLimit = 0;
        this.onClose.emit(false);
    }

    public onOkClick() {

        this.closeDropdown();
        this.onOk.emit(this.selectedItems);
    }

    public closeDropdownOnClickOut() {
        // debugger;
        if (this.isActive) {
            if (this.searchInput && this.settings.lazyLoading) {
                if (!this.isFreeTextActive && !this.settings.allowFreeText)
                    this.searchInput.nativeElement.value = "";
            }
            if (this.searchInput) {
                if (!this.isFreeTextActive && !this.settings.allowFreeText)
                    this.searchInput.nativeElement.value = "";
            }
            if (!this.isFreeTextActive && !this.settings.allowFreeText)
                this.filter = "";

            if (this.isFreeTextActive && this.settings.allowFreeText) {
                this.onFreeText.emit({ value: this.filter });
            }
            if (this.onDemandScrollFlag)
                this.onDemandScrollFlag = !this.onDemandScrollFlag;

            this.isActive = false;
            this.clearSearch();
            this.setLOVSearchWidth(100);
            this.setLOVFilterWidth(0);
            this.onClose.emit(false);
        }
    }

    toggleSelectAll() {
        // debugger;
        // if (!this.isSelectAll) {
        //!settings.lazyLoading && settings.enableFilterSelectAll && !isDisabledItemPresent
        //!settings.groupBy && filter?.length > 0 && filterLength > 0

        if (!this.settings.lazyLoading && this.settings.enableFilterSelectAll && !this.isDisabledItemPresent && this.filterLength > 0 && this.filter) {
            this.toggleFilterSelectAll();
        } else if (this.settings.lazyLoading && this.settings.enableFilterSelectAll && !this.isDisabledItemPresent && this.filterLength > 0 && this.filter) {
            this.toggleInfiniteFilterSelectAll();
        } else {

            this.selectedItems = [];
            this.toolTipItems = [];
            if (this.settings.groupBy) {
                this.groupedData.forEach((obj) => {
                    obj.selected = !obj.disabled;
                })
                this.groupCachedItems.forEach((obj) => {
                    obj.selected = !obj.disabled;
                })
            }

            // this.selectedItems = this.data.slice();
            if (this.data && this.data.length > 0) {
                this.selectedItems = this.setSelectedItem(this.data.filter((individualData) => !individualData.disabled));
                this.isSelectAll = true;

                this.onChangeCallback(this.selectedItems);
                this.onTouchedCallback(this.selectedItems);

                this.onSelectAll.emit(this.toolTipItems);
                this.onChange.emit(this.selectedItems);

            }
        }

        // } else {
        //     if (this.settings.groupBy) {
        //         this.groupedData.forEach((obj) => {
        //             obj.selected = false;
        //         });
        //         this.groupCachedItems.forEach((obj) => {
        //             obj.selected = false;
        //         })
        //     }
        //     this.selectedItems = [];
        //     this.tooltipitems = [];
        //     this.isSelectAll = false;
        //     this.onChangeCallback(this.selectedItems);
        //     this.onTouchedCallback(this.selectedItems);
        //     this.onDeSelectAll.emit(this.selectedItems);
        // }
    }

    filterGroupedList() {
        if (this.filter == "" || this.filter == null) {
            this.clearSearch();
            return;
        }
        this.groupedData = this.cloneArray(this.groupCachedItems);
        this.groupedData = this.groupedData.filter(obj => {
            let arr: any[]  = [];
            if (obj[this.settings.labelKey].toLowerCase().indexOf(this.filter.toLowerCase()) > -1) {
                arr = obj.list;
            }
            else {
                arr = obj.list.filter( (t:any) => {
                    return t[this.settings.labelKey].toLowerCase().indexOf(this.filter.toLowerCase()) > -1;
                });
            }

            obj.list = arr;
            if (obj[this.settings.labelKey].toLowerCase().indexOf(this.filter.toLowerCase()) > -1) {
                return arr;
            }
            else {
                return arr.some(cat => {
                    return cat[this.settings.labelKey].toLowerCase().indexOf(this.filter.toLowerCase()) > -1;
                }
                )
            }

        });
    }

    toggleFilterSelectAll() {
        if (!this.isFilterSelectAll) {
            let added:any[] = [];
            if (this.settings.groupBy) {
                /*                 this.groupedData.forEach((item: any) => {
                                    if (item.list) {
                                        item.list.forEach((el: any) => {
                                            if (!this.isSelected(el)) {
                                                this.addSelected(el);
                                                added.push(el);
                                            }
                                        });
                                    }
                                    this.updateGroupInfo(item);
                
                                }); */

                this.ds.getFilteredData().forEach((el: any) => {
                    if (!this.isSelected(el) && !el.hasOwnProperty('grpTitle')) {
                        this.addSelected(el);
                        added.push(el);
                    }
                });

            }
            else {
                this.ds.getFilteredData().forEach((item: any) => {
                    if (!this.isSelected(item)) {
                        this.addSelected(item);
                        added.push(item);
                    }

                });
            }

            this.isFilterSelectAll = true;
            this.onFilterSelectAll.emit(added);
        }
        else {
            let removed:any[] = [];
            if (this.settings.groupBy) {
                /*                 this.groupedData.forEach((item: any) => {
                                    if (item.list) {
                                        item.list.forEach((el: any) => {
                                            if (this.isSelected(el)) {
                                                this.removeSelected(el);
                                                removed.push(el);
                                            }
                                        });
                                    }
                                }); */
                this.ds.getFilteredData().forEach((el: any) => {
                    if (this.isSelected(el)) {
                        this.removeSelected(el);
                        removed.push(el);
                    }
                });
            }
            else {
                this.ds.getFilteredData().forEach((item: any) => {
                    if (this.isSelected(item)) {
                        this.removeSelected(item);
                        removed.push(item);
                    }

                });
            }
            this.isFilterSelectAll = false;
            this.onFilterDeSelectAll.emit(removed);
        }
    }

    toggleInfiniteFilterSelectAll() {
        if (!this.isInfiniteFilterSelectAll) {
            this.virtualdata.forEach((item: any) => {
                if (!this.isSelected(item))
                    this.addSelected(item);
            });
            this.isInfiniteFilterSelectAll = true;
        } else {
            this.virtualdata.forEach((item: any) => {
                if (this.isSelected(item))
                    this.removeSelected(item);
            });
            this.isInfiniteFilterSelectAll = false;
        }
    }

    clearSearch() {
        if (this.settings.groupBy) {
            this.groupedData = [];
            this.groupedData = this.cloneArray(this.groupCachedItems);
        }

        if (!this.isFreeTextActive && !this.settings.allowFreeText)
            this.filter = "";

        this.isFilterSelectAll = false;

    }

    onFilterChange(data: any) {
        if (this.filter && this.filter == "" || data.length == 0) {
            this.isFilterSelectAll = false;
        }
        let cnt = 0;
        data.forEach((item: any) => {

            if (!item.hasOwnProperty('grpTitle') && this.isSelected(item)) {
                cnt++;
            }
        });

        if (cnt > 0 && this.filterLength == cnt) {
            this.isFilterSelectAll = true;
        }
        else if (cnt > 0 && this.filterLength != cnt) {
            this.isFilterSelectAll = false;
        }
        this.cdr.detectChanges();
    }

    cloneArray(arr: any) {
        let i, copy;

        if (Array.isArray(arr)) {
            return JSON.parse(JSON.stringify(arr));
        } else if (typeof arr === 'object') {
            throw 'Cannot clone array containing an object!';
        } else {
            return arr;
        }
    }

    updateGroupInfo(item: any) :any {
        if (item.disabled) {
            return false;
        }
        let key = this.settings.groupBy;
        this.groupedData.forEach((obj: any) => {
            let cnt = 0;
            if (obj.grpTitle && (item[key] == obj[key])) {
                if (obj.list) {
                    obj.list.forEach((el: any) => {
                        if (this.isSelected(el)) {
                            cnt++;
                        }
                    });
                }
            }
            if (obj.list && (cnt === obj.list.length) && (item[key] == obj[key])) {
                obj.selected = true;
            }
            else if (obj.list && (cnt != obj.list.length) && (item[key] == obj[key])) {
                obj.selected = false;
            }
        });
        this.groupCachedItems.forEach((obj: any) => {
            let cnt = 0;
            if (obj.grpTitle && (item[key] == obj[key])) {
                if (obj.list) {
                    obj.list.forEach((el: any) => {
                        if (this.isSelected(el)) {
                            cnt++;
                        }
                    });
                }
            }
            if (obj.list && (cnt === obj.list.length) && (item[key] == obj[key])) {
                obj.selected = true;
            }
            else if (obj.list && (cnt != obj.list.length) && (item[key] == obj[key])) {
                obj.selected = false;
            }
        });
    }

    transformData(arr: Array<any>, field: any): Array<any> {
        const groupedObj: any = arr.reduce((prev: any, cur: any) => {
            if (!prev[cur[field]]) {
                prev[cur[field]] = [cur];
            } else {
                prev[cur[field]].push(cur);
            }
            return prev;
        }, {});
        const tempArr: any = [];
        Object.keys(groupedObj).map((x: any) => {
            let obj: any = {};
            let disabledChildrens = [];
            obj["grpTitle"] = true;
            obj[this.settings.labelKey] = x;
            obj[this.settings.groupBy] = x;
            obj['selected'] = false;
            obj['list'] = [];
            let cnt = 0;
            groupedObj[x].forEach((item: any) => {
                item['list'] = [];
                if (item.disabled) {
                    this.isDisabledItemPresent = true;
                    disabledChildrens.push(item);
                }
                obj.list.push(item);
                if (this.isSelected(item)) {
                    cnt++;
                }
            });
            if (cnt == obj.list.length) {
                obj.selected = true;
            }
            else {
                obj.selected = false;
            }

            // Check if current group item's all childrens are disabled or not
            obj['disabled'] = disabledChildrens.length === groupedObj[x].length;
            tempArr.push(obj);
            // obj.list.forEach((item: any) => {
            //     tempArr.push(item);
            // });
        });
        return tempArr;
    }

    public filterInfiniteList(evt: any) {
        // debugger;
        // console.log('filterInfiniteList:', this.filter);
        let filteredElems: Array<any> = [];
        if (this.settings.groupBy) {
            this.groupedData = this.groupCachedItems.slice();
        }
        else {
            this.data = this.cachedItems.slice();
            this.virtualdata = this.cachedItems.slice();
        }

        if ((evt != null || evt != '') && !this.settings.groupBy) {
            if (this.settings.searchBy.length > 0) {
                for (let t = 0; t < this.settings.searchBy.length; t++) {
                    this.virtualdata.filter((el: any) => {
                        if (el[this.settings.searchBy[t].toString()].toString().toLowerCase().indexOf(evt.toString().toLowerCase()) >= 0) {
                            filteredElems.push(el);
                        }
                    });
                }

            }
            else {
                this.virtualdata.filter(function (el: any) {
                    for (let prop in el) {
                        if (el[prop] != null) {
                            if (el[prop].toString().toLowerCase().indexOf(evt.toString().toLowerCase()) >= 0) {
                                filteredElems.push(el);
                                break;
                            }
                        }
                    }
                });
            }
            this.virtualdata = [];
            if (this.settings.isOnScrollDemand) {
                this.copyFilteredOriginalDataForIndex = filteredElems;
                this.onDemandSearchScrollFlag = true;
                filteredElems = filteredElems.slice(0, this.settings.limit);
                this.copyFilterDataChunk = filteredElems;
                this.newScrolledLimit = this.settings.limit??0;
            }
            this.virtualdata = filteredElems;
            this.infiniteFilterLength = this.virtualdata.length;
        }
        if (evt.toString() != '' && this.settings.groupBy) {
            this.groupedData.filter(function (el: any) {
                if (el.hasOwnProperty('grpTitle')) {
                    filteredElems.push(el);
                }
                else {
                    for (let prop in el) {
                        if (el[prop].toString().toLowerCase().indexOf(evt.toString().toLowerCase()) >= 0) {
                            filteredElems.push(el);
                            break;
                        }
                    }
                }
            });
            this.groupedData = [];
            this.groupedData = filteredElems;
            this.infiniteFilterLength = this.groupedData.length;
        }
        else if (evt.toString() == '' && this.cachedItems.length > 0) {
            this.virtualdata = [];

            this.virtualdata = this.cachedItems;
            if (this.settings.isOnScrollDemand) {
                this.onDemandSearchScrollFlag = !this.onDemandSearchScrollFlag;
                this.newScrolledLimit = this.settings.limit??0;
                this.copyOriginalDataForIndex = this.copyOriginalDataForIndex.slice(0, this.settings.limit);
            }
            this.infiniteFilterLength = 0;
        }
        // if (this.virtualScroller)
        //     this.virtualScroller.refresh();
    }

    resetInfiniteSearch() {
        this.filter = "";
        this.isInfiniteFilterSelectAll = false;
        this.virtualdata = [];
        this.virtualdata = this.cachedItems;
        this.groupedData = this.groupCachedItems;
        this.infiniteFilterLength = 0;
    }

    public get lazyURL(): string {
        let apiURI: string;
        let makeURLQuery = "first=" + this.data.length.toString() + "&last=" + this.settings.limit.toString() + "&searchvalue=" + this.filter;
        if (this.settings.apiURL.includes("?")) {
            apiURI = this.settings.apiURL + "&" + makeURLQuery;
        }
        else {
            apiURI = this.settings.apiURL + "?" + makeURLQuery;
        }
        return apiURI;
    }

    getChunkData(): Promise<any> {
        // debugger;
        return new Promise((resolve, reject) => {
            clearTimeout(this.timer);
            this.timer = setTimeout(() => {
                if (this.settings.lazyLoading && this.settings.apiURL) {
                    let apiURI: string;
                    if (!this.filter) {
                        this.filter = "";
                    }
                    let makeURLQuery = "first=" + this.data.length.toString() + "&last=" + this.settings.limit.toString() + "&searchvalue=" + this.filter;

                    if (this.settings.apiURL.includes("?")) {
                        apiURI = this.settings.apiURL + "&" + makeURLQuery;
                    }
                    else {
                        apiURI = this.settings.apiURL + "?" + makeURLQuery;
                    }

                    this.lazzyservice.getData(apiURI).subscribe((data: ServiceResponse) => {
                        this.data = this.data.concat(data.data);
                        this.cachedItems = this.data;
                        this.virtualdata = this.data;
                        // if (this.virtualScroller)
                        //     this.virtualScroller.refresh();
                        this.filterLength = this.data.length;
                        if (this.data.length == 0) {
                            this.isExistData = false;
                            this.isLazyLoadingPresentData = false;
                        }
                        else {
                            this.isExistData = true;
                            this.isLazyLoadingPresentData = true;
                        }
                        return resolve(this.virtualdata);
                    });
                }
                reject();
            }, 1000 + Math.random() * 1000);
        });
    }

    onScrollEnd(e: any) {
        // debugger;
        if (this.data) {
            if (this.settings.lazyLoading) {
                if (e.endIndex === this.data.length - 1 || e.startIndex === 0) { }
                if (e.endIndex === this.data.length - 1) {
                    if (this.isExistData) {
                        //this.onlazyExecute();
                        //this.onScrollDemand(e);
                    }
                }
            }
        }
        this.onScrollToEnd.emit(e);
    }

    private getChunkOfData(e: any) {
        this.loading = true;
        return new Promise((resolve, reject) => {
            if (this.onDemandScrollFlag && e.endIndex === this.copyOriginalDataForIndex.length - 1 && this.settings.lazyLoading && !this.onDemandSearchScrollFlag) {
                while (this.copyOriginalDataForIndex.length < this.data.length) {
                    this.newScrolledLimit = this.newScrolledLimit + this.constantLimitForAddition;
                    this.data.slice(this.copyOriginalDataForIndex.length, this.newScrolledLimit).map(items => {
                        this.copyOriginalDataForIndex.push(items);
                    });
                    this.virtualdata = this.copyOriginalDataForIndex;
                    return resolve(this.virtualdata);
                }
            }
            else if (this.onDemandSearchScrollFlag && e.endIndex === this.copyFilterDataChunk.length - 1 && this.settings.lazyLoading) {
                while (this.copyFilterDataChunk.length < this.copyFilteredOriginalDataForIndex.length) {
                    this.newScrolledLimit = this.newScrolledLimit + this.constantLimitForAddition;
                    this.copyFilteredOriginalDataForIndex.slice(this.copyFilterDataChunk.length, this.newScrolledLimit).map((items:any) => {
                        this.copyFilterDataChunk.push(items);
                    });
                    this.virtualdata = this.copyFilterDataChunk;
                    return resolve(this.virtualdata);
                }
            }
            reject();
        })
    }

    public onScrollDemand(e: any) {
        //this.loading = true;
        if (e.startIndex !== -1 && e.endIndex !== -1)
            this.getChunkOfData(e)
                .then(chunk => { this.loading = false })
                .catch(error => { this.loading = false });
    }

    onlazyExecute() {
        this.loading = true;
        this.getChunkData().then(chunk => {
            this.loading = false;
        }, () => this.loading = false);
    }

    ngOnDestroy() {
        if (this.subscription) {
            this.subscription.unsubscribe();
        }
    }

    selectGroup(item: any):any {
        if (item.disabled) {
            return false;
        }
        if (item.selected) {
            item.selected = false;
            item.list.forEach((obj: any) => {
                this.removeSelected(obj);
            });
            this.updateGroupInfo(item);
            this.onGroupSelect.emit(item);
        }
        else {
            item.selected = true;
            item.list.forEach((obj: any) => {
                if (!this.isSelected(obj)) {
                    this.addSelected(obj);
                }

            });
            this.updateGroupInfo(item);
            this.onGroupDeSelect.emit(item);
        }
    }

    addFilterNewItem() {
        this.onAddFilterNewItem.emit(this.filter);
        this.filterPipe = new ListFilterPipe(this.ds);
        this.filterPipe.transform(this.data, this.filter, this.settings.searchBy);
    }

    calculateDropdownDirection() {
        let shouldOpenTowardsTop = this.settings.position == 'top';
        if (this.settings.autoPosition) {
            const dropdownHeight = this.dropdownListElem.nativeElement.clientHeight;
            const viewportHeight = document.documentElement.clientHeight;
            const selectedListBounds = this.selectedListElem.nativeElement.getBoundingClientRect();

            const spaceOnTop: number = selectedListBounds.top;
            const spaceOnBottom: number = viewportHeight - selectedListBounds.top;
            if (spaceOnBottom < spaceOnTop && dropdownHeight < spaceOnTop) {
                this.openTowardsTop(true);
            }
            else {
                this.openTowardsTop(false);
            }
            // Keep preference if there is not enough space on either the top or bottom
            /* 			if (spaceOnTop || spaceOnBottom) {
                            if (shouldOpenTowardsTop) {
                                shouldOpenTowardsTop = spaceOnTop;
                            } else {
                                shouldOpenTowardsTop = !spaceOnBottom;
                            }
                        } */
        }

        if (this.settings.position == 'top') {
            this.openTowardsTop(true);
        }
    }

    openTowardsTop(value: boolean) {
        if (value && this.selectedListElem.nativeElement.clientHeight) {
            this.dropdownListYOffset = 15 + this.selectedListElem.nativeElement.clientHeight;

        } else {
            this.dropdownListYOffset = 0;
        }
    }

    public clearSelection(e: any) {
        // this.filter = '';
        if (!this.settings.lazyLoading && this.settings.enableFilterSelectAll && !this.isDisabledItemPresent && this.filterLength > 0 && this.filter && !this.isBackspace) {
            this.toggleFilterSelectAll();
        } else if (this.settings.lazyLoading && this.settings.enableFilterSelectAll && !this.isDisabledItemPresent && this.filterLength > 0 && this.filter && !this.isBackspace) {
            // this.toggleInfiniteFilterSelectAll();
            for (let vD of this.virtualdata) {
                if (this.isSelected(vD))
                    this.removeSelected(vD);

            }
            this.isInfiniteFilterSelectAll = false;
        } else {
            if (this.settings.groupBy) {
                this.groupCachedItems.forEach((obj) => {
                    obj.selected = false;
                })
            }

            this.clearSearch();
            this.toolTipItems.splice(0);

            if (this.settings.singleSelection) {
                this.selectedItems = [];
                this.selectedItems.splice(0);
            } else {
                this.selectedItems.splice(0);
            }

            this.resetArrowKeyActiveElement();
            this.onChangeCallback(this.selectedItems);
            this.onTouchedCallback(this.selectedItems);
            this.onDeSelectAll.emit(this.selectedItems);
            this.onChange.emit(this.selectedItems);
        }
    }

    private onArrowKeyUp() {
        if (this.focusedItemIndex === 0) {
            this.focusedItemIndex = this.data.length - 1
            return;
        }
        if (this.onArrowKey()) {
            this.focusedItemIndex--;
        }
    }

    private onArrowKeyDown() {
        if (this.focusedItemIndex === this.data.length - 1) {
            this.focusedItemIndex = 0;
            return;
        }
        if (this.onArrowKey()) {
            this.focusedItemIndex++;
        }
    }

    private onArrowKey() {
        if (this.focusedItemIndex === null) {
            this.focusedItemIndex = 0;
            return false
        }
        return true;
    }

    private resetArrowKeyActiveElement() {
        this.focusedItemIndex = null||0;
    }

    setTooltip() {
        if (this.settings.tooltipColumn) {
            if (this.toolTipItems) {
                if (this.toolTipItems.length > 0) {
                    if (this.settings.singleSelection) {
                        return this.toolTipItems[0][this.settings.tooltipColumn];
                    }
                    else {
                        let objtool:any[] = [];
                        this.toolTipItems.forEach((item: any) => {
                            objtool.push(item[this.settings.tooltipColumn]);
                        })
                        if (objtool.length > 0) {
                            return objtool.join("\n");
                        }
                    }
                }
            }
        }
    }
}

@NgModule({
    imports: [CommonModule, FormsModule],
    declarations: [NgiSelect, ClickOutsideDirective, ScrollDirective, styleDirective, ListFilterPipe, Item, TemplateRenderer, Badge, Search, setPosition, CIcon],
    exports: [NgiSelect, ClickOutsideDirective, ScrollDirective, styleDirective, ListFilterPipe, Item, TemplateRenderer, Badge, Search, setPosition, CIcon],
    providers: [DataService]
})
export class NgiSelectModule { }
