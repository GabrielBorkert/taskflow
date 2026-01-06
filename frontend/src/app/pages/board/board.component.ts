import { Component, OnInit } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { TaskService } from '../../services/task.service';
import { Task } from '../../models/task.model';
import { CreateTask } from '../../models/create-task';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { UpdateTask } from '../../models/update-task';

@Component({
  selector: 'app-board',
  templateUrl: './board.component.html',
  styleUrls: ['./board.component.css'],
  standalone: true,
  imports: [CommonModule, FormsModule, DatePipe, MatToolbarModule, ReactiveFormsModule,
    MatCardModule, MatButtonModule, MatInputModule, MatIconModule,]
})
export class BoardComponent implements OnInit {
 tasks: Task[] = [];
  loading = false;
  error: string | null = null;

  constructor(private taskService: TaskService) {}

  ngOnInit(): void {
    // this.loadTasks();
  }

  // #region Variables
  public form: FormGroup = new FormGroup({
    title: new FormControl<string | null>(null, [Validators.required]),
    description: new FormControl<string | null>(null, [Validators.required])
  });
  
  // #endregion Variables

  loadTasks(): void {
    this.loading = true;
    this.error = null;
    
    this.taskService.getAllTasks().subscribe({
      next: (data) => {
        this.tasks = data;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Erro ao carregar tasks: ' + err.message;
        this.loading = false;
        console.error('Erro:', err);
      }
    });
  }

  createTask(): void {
    if(this.form.valid){
      this.taskService.createTask(this.form.getRawValue()).subscribe({
        next: (task) => {
          console.log('Task criada:', task);
          this.loadTasks(); // Recarrega a lista
        },
        error: (err) => {
          console.error('Erro ao criar task:', err);
          this.error = 'Erro ao criar task';
        }
      });
    }
  }

  deleteTask(id: number): void {
    if (confirm('Deseja realmente deletar esta task?')) {
      this.taskService.deleteTask(id).subscribe({
        next: () => {
          console.log('Task deletada');
          this.loadTasks(); // Recarrega a lista
        },
        error: (err) => {
          console.error('Erro ao deletar task:', err);
          this.error = 'Erro ao deletar task';
        }
      });
    }
  }

  toggleComplete(task: Task): void {
    const updateTask: UpdateTask = {
      title: task.title,
      description: task.description ?? '',
      isCompleted: task.status
    };

    this.taskService.updateTask(task.id, updateTask).subscribe({
      next: () => {
        console.log('Task atualizada');
        this.loadTasks(); // Recarrega a lista
      },
      error: (err) => {
        console.error('Erro ao atualizar task:', err);
        this.error = 'Erro ao atualizar task';
      }
    });
  }
}
