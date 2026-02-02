import { TaskItemStatus } from "./task.model";

export interface CreateTask {
    title: string;
    description: string;
    isCompleted: boolean;
    status: TaskItemStatus;
}
