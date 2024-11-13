import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';

import { AuthLayoutComponent } from './auth-layout/auth-layout.component';
import { AppLayoutComponent } from './app-layout/app-layout.component';
import { HeaderComponent } from '../components/header/header.component';
import { SidebarComponent } from '../components/sidebar/sidebar.component';
import { FormsModule } from '@angular/forms';
import { NgbModule, NgbTypeaheadModule } from '@ng-bootstrap/ng-bootstrap';
import { NgiSelectModule } from "../components/ngi-select/multiselect-dropdown-lib/src/lib/ngi-select.component";

const routes: Routes = [];

@NgModule({
  declarations: [
    AuthLayoutComponent,
    AppLayoutComponent,
    HeaderComponent,
    SidebarComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    FormsModule,
    NgbModule,
    NgiSelectModule,
    NgbTypeaheadModule
],
  exports: []
})
export class LayoutModule { }
