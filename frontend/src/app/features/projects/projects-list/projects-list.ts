import { Component, inject } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { MatListModule } from '@angular/material/list';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

import { ProjectApiService } from '../../../core/services/project-api.service';

@Component({
  selector: 'app-projects-list',
  imports: [MatListModule, MatProgressSpinnerModule],
  templateUrl: './projects-list.html',
  styleUrl: './projects-list.scss',
})
export class ProjectsList {
  private readonly projectApi = inject(ProjectApiService);

  protected readonly projectsResource = rxResource({
    stream: () => this.projectApi.getAll(),
  });
}
