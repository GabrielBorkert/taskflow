import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Task, TaskStatus, TaskPriority } from '../models/task.model';

const STORAGE_KEY = 'taskflow_tasks';

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  private tasks: Task[] = [];
  private tasksSubject = new BehaviorSubject<Task[]>([]);
  tasks$ = this.tasksSubject.asObservable();

  constructor() {
    this.loadFromStorage();
  }

  private loadFromStorage(): void {
    const json = localStorage.getItem(STORAGE_KEY);
    if(json){
      const data = JSON.parse(json) as any[];
      this.tasks = data.map((t) => ({
        ...t,
        createdAt: new Date(t.createdAt),
        updatedAt: t.updatedAt ? new Date(t.updatedAt) : undefined,
      }));
    } else {
      this.tasks = [];
    }
    this.emitChanges();
  }

  private saveToStorage(): void{
    localStorage.setItem(STORAGE_KEY, JSON.stringify(this.tasks));
  }

  private emitChanges(): void{
    this.tasksSubject.next([...this.tasks]);
    this.saveToStorage();
  }

  public getAll(): Task[] {
    return [...this.tasks];
  }

  public add(taskPartial: {title: string; description?: string; priority?: TaskPriority}): void{
    const now = new Date();
    const newTask: Task = {
      id: this.generateId(),
      title: taskPartial.title,
      description: taskPartial.description,
      priority: taskPartial.priority ?? TaskPriority.Medium,
      status: TaskStatus.ToDo,
      createdAt: now,
      updatedAt: now,
    };

    this.tasks.push(newTask);
    this.emitChanges();
  }

  public update(task: Task): void{
    const index = this.tasks.findIndex((t) => t.id === task.id);
    if (index === -1) return;

    this.tasks[index] = {
      ...task,
      updatedAt: new Date(),
    };

    this.emitChanges();
  }

  public delete(id: number): void{
    this.tasks = this.tasks.filter((t) => t.id !== id);
    this.emitChanges();
  }

  public updateStatus(id: number, status: TaskStatus): void{
    const task = this.tasks.find((t) => t.id === id);
    if(!task) return;

    task.status = status;
    task.updatedAt = new Date();
    this.emitChanges();
  }

  private generateId(): number {
    return this.tasks.length > 0
      ? Math.max(...this.tasks.map((t) => t.id)) + 1
      : 1;
  }

}
