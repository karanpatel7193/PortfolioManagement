import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from 'src/app/interceptors/auth.guard';
import { VolumeComponent } from './volume.component';

const routes: Routes = [
	{
		path: '',
		component: VolumeComponent,
		children: [
			{
				path: 'list',
				component: VolumeComponent,
				canActivate: [AuthGuard]
			},
		]
	}
];

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule]
})
export class VolumeRoutingModule { }
