import { Component, OnInit } from '@angular/core';
import { WatchlistModel } from './watchlist.model';
import { AccessModel } from 'src/app/models/access.model';
import { SessionService } from 'src/app/services/session.service';
import { ToastService } from 'src/app/services/toast.service';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';
import { WatchlistService } from './watchlist.service';


	@Component({
	selector: 'app-watchlist-form',
	templateUrl: './watchlist-form.component.html',
	styles: [
	]
	})
	export class WatchlistFormComponent implements OnInit {
	public watchlist: WatchlistModel = new WatchlistModel ();
	public access: AccessModel = new AccessModel();
	public mode: string = '';
	public hasAccess: boolean = false;
	public id: number = 0;

constructor(
	private watchlistService: WatchlistService,
	private sessionService: SessionService,
	private router: Router,
	private route: ActivatedRoute,
	private toastr: ToastService
) 
	{
	this.setAccess();
	}

	ngOnInit(): void {
	}

	
	public setPageMode(): void {
		if (this.id === undefined || this.id === 0)
			this.setPageAddMode();

		if (this.hasAccess) {
		}
	}
	public setPageAddMode(): void {
		if (!this.access.canInsert) {
			this.toastr.warning('You do not have add access of this page.');
			return;
		}
		this.hasAccess = true;
		this.mode = 'Add';

	}
	public save(isFormValid: boolean | null): void {
			if (isFormValid) {
				if (!this.access.canInsert && !this.access.canUpdate) {
					this.toastr.warning('You do not have add or edit access of this page.');
					return;
				}
				this.watchlistService.save(this.watchlist).subscribe(data => {
					if (data === 0)
						this.toastr.warning('Record is already exist.');
					else if (data > 0) {
						this.toastr.success('Record submitted successfully.');
						this.cancel();
					}
				});
			} else {
				this.toastr.warning('Please provide valid input.');
			}
		}

	public cancel(): void {
			this.router.navigate(['/app/watchlist/list']);
		}

		public setAccess(): void {
			this.access = this.sessionService.getAccess("app/watchlist");
			// this.access.canInsert = true;
			// this.access.canUpdate = true;
			// this.access.canView = true;
			// this.access.canDelete= true;
			
		}
}

