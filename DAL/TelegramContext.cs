using Contracts.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public interface ITelegramContext
{
    DbSet<User> Users { get; set; }
    DbSet<TelegramBot> TelegramBots { get; set; }
    DbSet<Command> Commands { get; set; }
    DbSet<CommandAction> CommandActions { get; set; }

    Task<int> SaveChangesAsync();
}

public sealed class TelegramContext : DbContext, ITelegramContext
{
    public TelegramContext(DbContextOptions<TelegramContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<User> Users { get; set; } = null!;

    public DbSet<TelegramBot> TelegramBots { get; set; } = null!;

    public DbSet<Command> Commands { get; set; } = null!;

    public DbSet<CommandAction> CommandActions { get; set; } = null!;

    public Task<int> SaveChangesAsync()
    {
        return base.SaveChangesAsync();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region User setup

        modelBuilder.Entity<User>()
            .HasKey(u => u.Id);

        modelBuilder.Entity<User>()
            .HasIndex(b => b.Email)
            .IsUnique();

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
            .HasIndex(b => b.Token)
            .IsUnique();

        modelBuilder.Entity<TelegramBot>()
            .HasMany(b => b.Commands)
            .WithOne(c => c.TelegramBot)
            .HasForeignKey(c => c.TelegramBotId)
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

        //var user = new User
        //{
        //    Id = Guid.NewGuid(),
        //    Email = "tpo9h.cawa@gmail.com"
        //};

        //modelBuilder.Entity<User>()
        //    .HasData(user);

        //var telegramBot = new TelegramBot
        //{
        //    IsActive = true,
        //    Id = Guid.NewGuid(),
        //    UserId = user.Id,
        //    Token = "5656162661:AAFR-yAPsrYrGTFa6XYSmD0Ijkg0z81aPrI"
        //};

        //modelBuilder.Entity<TelegramBot>()
        //    .HasData(telegramBot);
    }
}