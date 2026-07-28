import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'projects',
  },
  {
    path: 'projects',
    loadComponent: () =>
      import('./features/projects/projects-list/projects-list').then((m) => m.ProjectsList),
  },
  {
    path: 'projects/:id',
    loadComponent: () =>
      import('./features/projects/project-details/project-details').then((m) => m.ProjectDetails),
  },
];
