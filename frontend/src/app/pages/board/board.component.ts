import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { Task, TaskPriority, TaskStatus } from '../../models/task.model';
import { TaskService } from '../../services/task.service';
import { FormsModule } from '@angular/forms';
import { CommonModule, DatePipe } from '@angular/common';

import { MatToolbarModule } from '@angular/material/toolbar';
import { MatCardModule }    from '@angular/material/card';
import { MatButtonModule }  from '@angular/material/button';
import { MatInputModule }   from '@angular/material/input';
import { MatIconModule }    from '@angular/material/icon';
import { EditTaskComponent } from '../edit-task/edit-task.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-board',
  templateUrl: './board.component.html',
  styleUrls: ['./board.component.css'],
  standalone: true,
  imports: [CommonModule, FormsModule, DatePipe, MatToolbarModule,
    MatCardModule, MatButtonModule, MatInputModule, MatIconModule,]
})
export class BoardComponent implements OnInit {
  tasks$: Observable<Task[]>;
  TaskStatus = TaskStatus; // pra usar no HTML

  // para formulário simples de criação
  newTitle = '';
  newDescription = '';

  constructor(
    private taskService: TaskService,
    private dialog: MatDialog
  ) {
    this.tasks$ = this.taskService.tasks$;
  }

  ngOnInit(): void {}

  getTasksByStatus(tasks: Task[], status: TaskStatus): Task[] {
    return tasks.filter((t) => t.status === status);
  }

  createTask(): void {
    const title = this.newTitle.trim();
    if (!title) return;

    this.taskService.add({
      title,
      description: this.newDescription,
    });

    this.newTitle = '';
    this.newDescription = '';
  }

  moveTo(task: Task, status: TaskStatus): void {
    this.taskService.updateStatus(task.id, status);
  }

  deleteTask(task: Task): void {
    this.taskService.delete(task.id);
  }

  openModal(task: Task): void {
  const dialogRef = this.dialog.open(EditTaskComponent, {
    width: '400px',
    data: task,
  });

  dialogRef.afterClosed().subscribe(result => {
    if (result) {
      console.log('Usuário confirmou');
    }
  });
}
}
