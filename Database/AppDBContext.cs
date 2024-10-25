using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PuanConnect.Models;

namespace PuanConnect.Database;

public class AppDBContext : IdentityDbContext<User>
{
    public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

    public DbSet<Event> Events { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Attendee> Attendees { get; set; }
    public DbSet<EventQuestion> EventQuestions { get; set; }
    public DbSet<EventAnswer> EventAnswers { get; set; }
}