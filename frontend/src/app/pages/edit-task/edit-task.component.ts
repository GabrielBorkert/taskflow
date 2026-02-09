import { CommonModule, DatePipe } from '@angular/common';
import { Component, Inject, Input } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatToolbarModule } from '@angular/material/toolbar';
import { Task } from '../../models/task.model';
import { TaskService } from '../../services/task.service';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { NotificationService } from '../../shared/services/notification.service';

@Component({
  selector: 'app-edit-task',
  imports: [CommonModule, FormsModule, MatToolbarModule, MatCardModule, MatButtonModule, MatInputModule, MatIconModule, MatSnackBarModule],
  templateUrl: './edit-task.component.html',
  styleUrl: './edit-task.component.css'
})
export class EditTaskComponent {
    task: Task;

    constructor(
      private taskService: TaskService,
      private notificationService: NotificationService,
      public modalService: MatDialogRef<EditTaskComponent>,
      @Inject(MAT_DIALOG_DATA) public data: any
    ) {
      this.task = data;
    }

    closeModal(): void {
      this.modalService.close();
    }

    onSave(){
      this.taskService.updateTask(this.task.id, this.task).subscribe({
        next: (task) => {
          this.modalService.close({sucess: true});
        },
        error: (err) => {
          this.notificationService.error('Erro ao tentar salvar tarefa!');
        }
      });
    }
}
