import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ScriptViewAboutCompanyComponent } from './script-view-about-company.component';

describe('ScriptViewAboutCompanyComponent', () => {
  let component: ScriptViewAboutCompanyComponent;
  let fixture: ComponentFixture<ScriptViewAboutCompanyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ScriptViewAboutCompanyComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ScriptViewAboutCompanyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
