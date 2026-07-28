export type TaskStatus = 'ToDo' | 'InProgress' | 'Done';

export interface TaskListItem {
  id: number;
  title: string;
  description: string | null;
  status: TaskStatus;
  dueDate: string;
  projectName: string;
}

export interface TaskDetail {
  id: number;
  title: string;
  description: string | null;
  status: TaskStatus;
  dueDate: string;
  projectName: string;
}

export interface CreateTaskRequest {
  title: string;
  description?: string;
  status: TaskStatus;
  dueDate: string;
  projectId: number;
}

export interface UpdateTaskRequest {
  title: string;
  description?: string;
  status: TaskStatus;
  dueDate: string;
  projectId: number;
}

export interface CreateTaskResponse {
  id: number;
  title: string;
  description: string | null;
  status: TaskStatus;
  dueDate: string;
  projectId: number;
}
