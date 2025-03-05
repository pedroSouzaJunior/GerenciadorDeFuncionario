import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FuncionarioExclusaoComponent } from './funcionario-exclusao.component';

describe('FuncionarioExclusaoComponent', () => {
  let component: FuncionarioExclusaoComponent;
  let fixture: ComponentFixture<FuncionarioExclusaoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FuncionarioExclusaoComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FuncionarioExclusaoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
