import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import {
  CreateTaskRequest,
  CreateTaskResponse,
  TaskDetail,
  TaskListItem,
  TaskStatus,
  UpdateTaskRequest,
} from '../models/task.model';

@Injectable({ providedIn: 'root' })
export class TaskApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/tasks`;
  private readonly projectsUrl = `${environment.apiUrl}/projects`;

  getAll(status?: TaskStatus): Observable<TaskListItem[]> {
    const params = status ? new HttpParams().set('status', status) : undefined;
    return this.http.get<TaskListItem[]>(this.baseUrl, { params });
  }

  getByProject(projectId: number): Observable<TaskListItem[]> {
    return this.http.get<TaskListItem[]>(`${this.projectsUrl}/${projectId}/tasks`);
  }

  getById(id: number): Observable<TaskDetail> {
    return this.http.get<TaskDetail>(`${this.baseUrl}/${id}`);
  }

  create(request: CreateTaskRequest): Observable<CreateTaskResponse> {
    return this.http.post<CreateTaskResponse>(this.baseUrl, request);
  }

  update(id: number, request: UpdateTaskRequest): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, request);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
