import { Component, OnInit } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { TaskService } from '../../services/task.service';
import { Task, TaskItemStatus } from '../../models/task.model';
import { CreateTask } from '../../models/create-task';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { UpdateTask } from '../../models/update-task';
import { Priority, TaskPriority } from '../../models/priorities-task';
import {MatRadioModule} from '@angular/material/radio';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { EditTaskComponent } from '../edit-task/edit-task.component';
import { emitDistinctChangesOnlyDefaultValue } from '@angular/compiler';
import { NotificationService } from '../../shared/services/notification.service';

@Component({
  selector: 'app-board',
  templateUrl: './board.component.html',
  styleUrls: ['./board.component.css'],
  standalone: true,
  imports: [CommonModule, FormsModule, DatePipe, MatToolbarModule, ReactiveFormsModule,
    MatCardModule, MatButtonModule, MatInputModule, MatIconModule, MatRadioModule, MatDialogModule]
})
export class BoardComponent implements OnInit {
  public tasks: Task[] = [];
  public priorities: Priority[] = [];
  loading = false;
  error: string | null = null;

  constructor(
    private taskService: TaskService,
    private modalService: MatDialog,
    private notificationService: NotificationService,
  ) {}

  ngOnInit(): void {
    this.loadTasks();
  }

  getPriorityColor(priorityId: number): string {
    const priority = this.priorities.find(p => p.id === priorityId);
    return priority?.color || '#94a3b8'; // cinza como fallback
  }

  /**
   * Retorna o nome da prioridade baseado no ID
   */
  getPriorityName(priorityId: number): string {
    const priority = this.priorities.find(p => p.id === priorityId);
    return priority?.name || 'Sem prioridade';
  }

  // #region Variables
  public form: FormGroup = new FormGroup({
    title: new FormControl<string | null>(null, [Validators.required]),
    description: new FormControl<string | null>(null, [Validators.required]),
    status: new FormControl<TaskItemStatus>(TaskItemStatus.ToDo),
    priority: new FormControl<TaskPriority>(TaskPriority.Media),
  });
  
  public TaskItemStatus = TaskItemStatus;
  public TaskPriority = TaskPriority;
  // #endregion Variables

  loadTasks(): void {
    this.loading = true;
    this.error = null;
    
    this.taskService.getAllTasks().subscribe({
      next: (data) => {
        this.tasks = data;
        this.loading = false;
        
        this.getAllPriorities();
      },
      error: (err) => {
        this.error = 'Erro ao carregar tasks: ' + err.message;
        this.loading = false;
        console.error('Erro:', err);
      }
    });
  }

  getAllPriorities(): void {
    this.taskService.getAllPriorities().subscribe({
      next: (data) => {
        this.priorities = [...data];
      },
      error: (err) => {
        this.error = 'Erro ao carregar as prioridades: ' + err.message;
        this.loading = false;
        this.notificationService.error('Erro ao carregar as prioridades!');
      }
    })
  }

  createTask(): void {
    if(this.form.valid){
      this.taskService.createTask(this.form.getRawValue()).subscribe({
        next: (task) => {
          this.notificationService.success('Tarefa criada com sucesso!');
          this.loadTasks();
        },
        error: (err) => {
          this.notificationService.error('Erro ao criar tarefa!');
          this.error = 'Erro ao criar task';
        }
      });
    }
  }

  deleteTask(id: number): void {
    if (confirm('Deseja realmente deletar esta task?')) {
      this.taskService.deleteTask(id).subscribe({
        next: () => {
          this.notificationService.success('Tarefa excluída com sucesso!');
          this.loadTasks(); // Recarrega a lista
        },
        error: (err) => {
          this.notificationService.error('Erro ao excluir tarefa!');
          this.error = 'Erro ao deletar task';
        }
      });
    }
  }

  toggleComplete(task: Task): void {
    // const updateTask: UpdateTask = {
    //   title: task.title,
    //   description: task.description ?? '',
    //   isCompleted: task.status
    // };

    this.taskService.updateTask(task.id, task).subscribe({
      next: () => {
        this.notificationService.success('Tarefa atualizada com sucesso!');
        this.loadTasks(); // Recarrega a lista
      },
      error: (err) => {
        this.notificationService.error('Erro ao atualizar tarefa: ' + err);
        this.error = 'Erro ao atualizar task';
      }
    });
  }

  editTask(task: Task, newStatus: number = 0) {
    task.status = newStatus != 0 ? newStatus : task.status;

    if(newStatus == 0){
      const modal = this.modalService.open(EditTaskComponent, { width: '500px', data: task });
      
      modal.afterClosed().subscribe(result => 
      { 
          if(result?.sucess) {
            this.notificationService.success('Tarefa salva com sucesso!');
            this.loadTasks
          }
      });
    }
    else{
      this.taskService.updateTask(task.id, task).subscribe({
        next: (task) => {
          this.notificationService.success('Tarefa atualizada com sucesso!');
          this.loadTasks();
        },
        error: (err) => {
          this.notificationService.error('Erro ao tentar salvar tarefa!');
        }
      });
    }
  }
}
