using TaskManager.Application.Abstractions;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.Persistence.Repositories;

public class TaskItemRepository(ApplicationDbContext context)
    : Repository<TaskItem>(context), ITaskItemRepository;
