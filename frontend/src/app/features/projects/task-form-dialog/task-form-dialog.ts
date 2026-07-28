import { Component, inject } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';

import { CreateTaskRequest, TASK_STATUSES, TaskListItem, TaskStatus } from '../../../core/models/task.model';

export interface TaskFormDialogData {
  projectId: number;
  task?: TaskListItem;
}

interface TaskForm {
  title: FormControl<string>;
  description: FormControl<string>;
  status: FormControl<TaskStatus>;
  dueDate: FormControl<Date | null>;
}

function parseDateOnly(value: string): Date {
  const [year, month, day] = value.split('-').map(Number);
  return new Date(year, month - 1, day);
}

function formatDateOnly(value: Date): string {
  const year = value.getFullYear();
  const month = String(value.getMonth() + 1).padStart(2, '0');
  const day = String(value.getDate()).padStart(2, '0');
  return `${year}-${month}-${day}`;
}

@Component({
  selector: 'app-task-form-dialog',
  imports: [
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatDatepickerModule,
    MatButtonModule,
  ],
  templateUrl: './task-form-dialog.html',
  styleUrl: './task-form-dialog.scss',
})
export class TaskFormDialog {
  protected readonly data = inject<TaskFormDialogData>(MAT_DIALOG_DATA);
  private readonly dialogRef = inject(MatDialogRef<TaskFormDialog>);

  protected readonly isEdit = !!this.data.task;
  protected readonly statusOptions = TASK_STATUSES;

  protected readonly form = new FormGroup<TaskForm>({
    title: new FormControl(this.data.task?.title ?? '', {
      nonNullable: true,
      validators: [Validators.required, Validators.maxLength(200)],
    }),
    description: new FormControl(this.data.task?.description ?? '', {
      nonNullable: true,
      validators: [Validators.maxLength(1000)],
    }),
    status: new FormControl(this.data.task?.status ?? 'ToDo', {
      nonNullable: true,
      validators: [Validators.required],
    }),
    dueDate: new FormControl(this.data.task ? parseDateOnly(this.data.task.dueDate) : null, {
      validators: [Validators.required],
    }),
  });

  save(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const { title, description, status, dueDate } = this.form.getRawValue();
    const request: CreateTaskRequest = {
      title,
      description: description || undefined,
      status,
      dueDate: formatDateOnly(dueDate!),
      projectId: this.data.projectId,
    };
    this.dialogRef.close(request);
  }

  cancel(): void {
    this.dialogRef.close();
  }
}
