import { Routes } from '@angular/router';
import { BoardComponent } from './pages/board/board.component';
import { EditTaskComponent } from './pages/edit-task/edit-task.component';

export const routes: Routes = [
    { path: '', component: BoardComponent },
    { path: 'edit', component: EditTaskComponent },
    { path: '**', redirectTo: '' },
];
