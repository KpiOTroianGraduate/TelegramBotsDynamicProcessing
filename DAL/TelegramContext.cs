using Contracts.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public sealed class TelegramContext : DbContext
{
    public TelegramContext(DbContextOptions<TelegramContext> options) : base(options)
    {
        //Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    public DbSet<User> Users { get; set; } = null!;

    public DbSet<TelegramBot> TelegramBots { get; set; } = null!;

    public DbSet<Command> Commands { get; set; } = null!;

    public DbSet<CommandAction> CommandActions { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region User setup

        modelBuilder.Entity<User>()
            .HasKey(u => u.Id);

        modelBuilder.Entity<User>()
            .HasMany(u => u.TelegramBots)
            .WithOne(b => b.User)
            .HasForeignKey(b => b.UserId)
            .HasPrincipalKey(u => u.Id)
            .OnDelete(DeleteBehavior.Cascade);

        #endregion

        #region TelegramBot setup

        modelBuilder.Entity<TelegramBot>()
            .HasKey(u => u.Id);

        modelBuilder.Entity<TelegramBot>()
            .HasMany(b => b.Commands)
            .WithOne(c => c.TelegramBot)
            .HasForeignKey(c => c.BotId)
            .HasPrincipalKey(b => b.Id)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TelegramBot>()
            .HasMany(b => b.CommandActions)
            .WithOne(a => a.TelegramBot)
            .HasForeignKey(a => a.TelegramBotId)
            .HasPrincipalKey(b => b.Id)
            .OnDelete(DeleteBehavior.Cascade);

        #endregion

        #region Command setup

        modelBuilder.Entity<Command>()
            .HasKey(u => u.Id);

        #endregion

        #region CommandAction setup

        modelBuilder.Entity<CommandAction>()
            .HasKey(u => u.Id);

        modelBuilder.Entity<CommandAction>()
            .HasMany(a => a.Commands)
            .WithOne(c => c.CommandAction)
            .HasForeignKey(c => c.CommandActionId)
            .HasPrincipalKey(a => a.Id)
            .OnDelete(DeleteBehavior.ClientSetNull);

        #endregion
    }
}