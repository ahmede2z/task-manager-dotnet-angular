using TaskManager.Application.Abstractions;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.Persistence.Repositories;

public class ProjectRepository(ApplicationDbContext context)
    : Repository<Project>(context), IProjectRepository;
