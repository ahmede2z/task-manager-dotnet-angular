import { DatePipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTableModule } from '@angular/material/table';
import { MatTooltipModule } from '@angular/material/tooltip';
import { Router } from '@angular/router';

import { CreateProjectRequest, Project } from '../../../core/models/project.model';
import { ProjectApiService } from '../../../core/services/project-api.service';
import { NotificationService } from '../../../core/services/notification.service';
import { ConfirmDialog, ConfirmDialogData } from '../../../shared/confirm-dialog/confirm-dialog';
import { ProjectFormDialog, ProjectFormDialogData } from '../project-form-dialog/project-form-dialog';

@Component({
  selector: 'app-projects-list',
  imports: [
    DatePipe,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatTooltipModule,
    MatProgressSpinnerModule,
  ],
  templateUrl: './projects-list.html',
  styleUrl: './projects-list.scss',
})
export class ProjectsList {
  private readonly projectApi = inject(ProjectApiService);
  private readonly notifications = inject(NotificationService);
  private readonly dialog = inject(MatDialog);
  private readonly router = inject(Router);

  protected readonly displayedColumns = ['name', 'description', 'createdAt', 'actions'];

  protected readonly projectsResource = rxResource({
    stream: () => this.projectApi.getAll(),
  });

  openDetails(project: Project): void {
    this.router.navigate(['/projects', project.id]);
  }

  openCreateDialog(): void {
    const ref = this.dialog.open<ProjectFormDialog, ProjectFormDialogData, CreateProjectRequest>(
      ProjectFormDialog,
      { data: {}, width: '480px' },
    );

    ref.afterClosed().subscribe((request) => {
      if (!request) {
        return;
      }

      this.projectApi.create(request).subscribe(() => {
        this.notifications.success('Project created.');
        this.projectsResource.reload();
      });
    });
  }

  openEditDialog(project: Project): void {
    const ref = this.dialog.open<ProjectFormDialog, ProjectFormDialogData, CreateProjectRequest>(
      ProjectFormDialog,
      { data: { project }, width: '480px' },
    );

    ref.afterClosed().subscribe((request) => {
      if (!request) {
        return;
      }

      this.projectApi.update(project.id, request).subscribe(() => {
        this.notifications.success('Project updated.');
        this.projectsResource.reload();
      });
    });
  }

  confirmDelete(project: Project): void {
    const data: ConfirmDialogData = {
      title: 'Delete project',
      message: `Delete "${project.name}"? This also deletes all of its tasks. This cannot be undone.`,
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

      this.projectApi.delete(project.id).subscribe(() => {
        this.notifications.success('Project deleted.');
        this.projectsResource.reload();
      });
    });
  }
}
