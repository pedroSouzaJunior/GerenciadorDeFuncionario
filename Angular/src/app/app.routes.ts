import { Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { AuthGuard } from './guards/auth.guard';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { 
    path: 'funcionarios', 
    loadComponent: () => import('./pages/funcionarios-list/funcionarios-list.component').then(m => m.FuncionariosListComponent), 
    canActivate: [AuthGuard] 
  },
  { path: '', redirectTo: 'login', pathMatch: 'full' },
];
