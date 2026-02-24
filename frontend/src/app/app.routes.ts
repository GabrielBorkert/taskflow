import { Routes } from '@angular/router';
import { LoginComponent } from './pages/login/login.component';
import { BoardComponent } from './pages/board/board.component';
import { EditTaskComponent } from './pages/edit-task/edit-task.component';
import { authGuard } from './shared/guards/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'board', component: BoardComponent, canActivate: [authGuard] },
  { path: 'edit', component: EditTaskComponent, canActivate: [authGuard] },
  { path: '**', redirectTo: 'login' }
];