using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.Persistence;

public static class ApplicationDbContextSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context, CancellationToken cancellationToken)
    {
        if (await context.Projects.AnyAsync(cancellationToken))
        {
            return;
        }

        var websiteRedesign = new Project
        {
            Name = "Website Redesign",
            Description = "Rebuild the public marketing site on the new design system."
        };

        var mobileLaunch = new Project
        {
            Name = "Mobile App Launch",
            Description = "Ship the first public release of the iOS and Android clients."
        };

        var internalTooling = new Project
        {
            Name = "Internal Tooling"
        };

        context.Projects.AddRange(websiteRedesign, mobileLaunch, internalTooling);
        await context.SaveChangesAsync(cancellationToken);

        context.TaskItems.AddRange(
            new TaskItem
            {
                Title = "Audit current page templates",
                Description = "Catalogue every template still in use and flag the ones to retire.",
                Status = TaskStatus.Done,
                DueDate = new DateOnly(2026, 7, 10),
                ProjectId = websiteRedesign.Id
            },
            new TaskItem
            {
                Title = "Build the shared component library",
                Status = TaskStatus.InProgress,
                DueDate = new DateOnly(2026, 8, 14),
                ProjectId = websiteRedesign.Id
            },
            new TaskItem
            {
                Title = "Migrate the landing page",
                Status = TaskStatus.ToDo,
                DueDate = new DateOnly(2026, 8, 28),
                ProjectId = websiteRedesign.Id
            },
            new TaskItem
            {
                Title = "Finalise the onboarding flow",
                Description = "Cut the signup steps from five screens to three.",
                Status = TaskStatus.InProgress,
                DueDate = new DateOnly(2026, 8, 7),
                ProjectId = mobileLaunch.Id
            },
            new TaskItem
            {
                Title = "Set up crash reporting",
                Status = TaskStatus.ToDo,
                DueDate = new DateOnly(2026, 8, 21),
                ProjectId = mobileLaunch.Id
            },
            new TaskItem
            {
                Title = "Submit builds to the app stores",
                Status = TaskStatus.ToDo,
                DueDate = new DateOnly(2026, 9, 11),
                ProjectId = mobileLaunch.Id
            },
            new TaskItem
            {
                Title = "Replace the nightly export script",
                Status = TaskStatus.ToDo,
                DueDate = new DateOnly(2026, 9, 4),
                ProjectId = internalTooling.Id
            });

        await context.SaveChangesAsync(cancellationToken);
    }
}
