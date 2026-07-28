using TaskManager.Domain.Common;

namespace TaskManager.Domain.Entities;

public class TaskItem : BaseEntity
{
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public TaskStatus Status { get; set; }

    public DateOnly DueDate { get; set; }

    public int ProjectId { get; set; }
}
