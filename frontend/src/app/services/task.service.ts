import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { Task, TaskItemStatus, TaskPriority } from '../models/task.model';
import { HttpClient } from '@angular/common/http';
import { CreateTask } from '../models/create-task';
import { UpdateTask } from '../models/update-task';
import { Priority } from '../models/priorities-task';

const STORAGE_KEY = 'taskflow_tasks';

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  // private apiUrl = 'https://localhost:7217/api/Tasks';
  private apiUrl = 'https://localhost:44348/api/Tasks';

  private tasks: Task[] = [];
  private tasksSubject = new BehaviorSubject<Task[]>([]);
  tasks$ = this.tasksSubject.asObservable();

  constructor(
    private http: HttpClient
  ) {}

   // GET - Listar todas as tasks
  getAllTasks(): Observable<Task[]> {
    return this.http.get<Task[]>(`${this.apiUrl}/GetAllTasks`);
  }

  // GET - Buscar todas prioridades criadas
  getAllPriorities(): Observable<Priority[]> {
    return this.http.get<Priority[]>(`${this.apiUrl}/GetAllPriorities`)
  }

  // GET - Buscar task por ID
  getTaskById(id: number): Observable<Task> {
    return this.http.get<Task>(`${this.apiUrl}/GetTaskById/${id}`);
  }

  // POST - Criar nova task
  createTask(task: CreateTask): Observable<Task> {
    return this.http.post<Task>(`${this.apiUrl}/CreateTask`, task);
  }

  // PUT - Atualizar task
  updateTask(id: number, task: UpdateTask): Observable<Task> {
    return this.http.put<Task>(`${this.apiUrl}/UpdateTask/${id}`, task);
  }

  // DELETE - Deletar task
  deleteTask(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/DeleteTask/${id}`);
  }

}
