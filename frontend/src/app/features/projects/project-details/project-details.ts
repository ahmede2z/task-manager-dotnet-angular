import { DatePipe } from '@angular/common';
import { Component, computed, inject, signal } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { rxResource } from '@angular/core/rxjs-interop';
import { ActivatedRoute, Router } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { MatTableModule } from '@angular/material/table';
import { MatTooltipModule } from '@angular/material/tooltip';

import {
  CreateTaskRequest,
  TASK_STATUSES,
  TaskListItem,
  TaskStatus,
  UpdateTaskRequest,
} from '../../../core/models/task.model';
import { NotificationService } from '../../../core/services/notification.service';
import { ProjectApiService } from '../../../core/services/project-api.service';
import { TaskApiService } from '../../../core/services/task-api.service';
import { ConfirmDialog, ConfirmDialogData } from '../../../shared/confirm-dialog/confirm-dialog';
import { TaskFormDialog, TaskFormDialogData } from '../task-form-dialog/task-form-dialog';

@Component({
  selector: 'app-project-details',
  imports: [
    DatePipe,
    MatButtonModule,
    MatIconModule,
    MatTableModule,
    MatTooltipModule,
    MatProgressSpinnerModule,
    MatFormFieldModule,
    MatSelectModule,
  ],
  templateUrl: './project-details.html',
  styleUrl: './project-details.scss',
})
export class ProjectDetails {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly projectApi = inject(ProjectApiService);
  private readonly taskApi = inject(TaskApiService);
  private readonly notifications = inject(NotificationService);
  private readonly dialog = inject(MatDialog);

  protected readonly displayedColumns = ['title', 'description', 'status', 'dueDate', 'actions'];
  protected readonly statusOptions = TASK_STATUSES;
  protected readonly statusFilter = signal<TaskStatus | ''>('');

  private readonly paramMap = toSignal(this.route.paramMap);
  protected readonly projectId = computed(() => Number(this.paramMap()?.get('id')));

  protected readonly projectResource = rxResource({
    params: () => ({ id: this.projectId() }),
    stream: ({ params }) => this.projectApi.getById(params.id),
  });

  protected readonly tasksResource = rxResource({
    params: () => ({ id: this.projectId(), status: this.statusFilter() || undefined }),
    stream: ({ params }) => this.taskApi.getByProject(params.id, params.status),
  });

  backToProjects(): void {
    this.router.navigate(['/projects']);
  }

  openCreateDialog(): void {
    const ref = this.dialog.open<TaskFormDialog, TaskFormDialogData, CreateTaskRequest>(TaskFormDialog, {
      data: { projectId: this.projectId() },
      width: '480px',
    });

    ref.afterClosed().subscribe((request) => {
      if (!request) {
        return;
      }

      this.taskApi.create(request).subscribe(() => {
        this.notifications.success('Task created.');
        this.tasksResource.reload();
      });
    });
  }

  openEditDialog(task: TaskListItem): void {
    const ref = this.dialog.open<TaskFormDialog, TaskFormDialogData, CreateTaskRequest>(TaskFormDialog, {
      data: { projectId: this.projectId(), task },
      width: '480px',
    });

    ref.afterClosed().subscribe((request) => {
      if (!request) {
        return;
      }

      this.taskApi.update(task.id, request).subscribe(() => {
        this.notifications.success('Task updated.');
        this.tasksResource.reload();
      });
    });
  }

  confirmDelete(task: TaskListItem): void {
    const data: ConfirmDialogData = {
      title: 'Delete task',
      message: `Delete "${task.title}"? This cannot be undone.`,
      confirmLabel: 'Delete',
    };
    const ref = this.dialog.open<ConfirmDialog, ConfirmDialogData, boolean>(ConfirmDialog, {
      data,
      width: '420px',
    });

    ref.afterClosed().subscribe((confirmed) => {
      if (!confirmed) {
        return;
      }

      this.taskApi.delete(task.id).subscribe(() => {
        this.notifications.success('Task deleted.');
        this.tasksResource.reload();
      });
    });
  }

  changeStatus(task: TaskListItem, status: TaskStatus): void {
    if (status === task.status) {
      return;
    }

    const request: UpdateTaskRequest = {
      title: task.title,
      description: task.description ?? undefined,
      status,
      dueDate: task.dueDate,
      projectId: this.projectId(),
    };

    this.taskApi.update(task.id, request).subscribe(() => {
      this.notifications.success('Status updated.');
      this.tasksResource.reload();
    });
  }
}
