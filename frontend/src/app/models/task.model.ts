export enum TaskItemStatus {
    ToDo = 1,
    InProgress = 2,
    Done = 3,
}

export enum TaskPriority {
    Low = 1,
    Medium = 2,
    High = 3,
}

export interface Task {
    id: number;
    title: string;
    description?: string;
    status: TaskItemStatus;
    priority: TaskPriority;
    createdAt: Date;
    updatedAt: Date;
}