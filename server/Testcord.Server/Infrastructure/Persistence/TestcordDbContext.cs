using Microsoft.EntityFrameworkCore;
using Testcord.Server.Domain.Entities;

namespace Testcord.Server.Infrastructure.Persistence;

public sealed class TestcordDbContext : DbContext
{
    public TestcordDbContext(DbContextOptions<TestcordDbContext> options)
        : base(options)
    {
    }

    public DbSet<Channel> Channels => Set<Channel>();
    public DbSet<DirectChat> DirectChats => Set<DirectChat>();
    public DbSet<DirectChatParticipant> DirectChatParticipants => Set<DirectChatParticipant>();
    public DbSet<FriendRequest> FriendRequests => Set<FriendRequest>();
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<ServerEntity> Servers => Set<ServerEntity>();
    public DbSet<ServerMember> ServerMembers => Set<ServerMember>();
    public DbSet<User> Users => Set<User>();
    public DbSet<UserSettings> UserSettings => Set<UserSettings>();
    public DbSet<CallSession> CallSessions => Set<CallSession>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Email).HasMaxLength(256);
            entity.Property(x => x.Nickname).HasMaxLength(32);
            entity.Property(x => x.DisplayName).HasMaxLength(64);
            entity.HasIndex(x => x.Email).IsUnique();
            entity.HasIndex(x => x.Nickname).IsUnique();
        });

        modelBuilder.Entity<FriendRequest>(entity =>
        {
            entity.ToTable("FriendRequests");
            entity.HasKey(x => x.Id);
        });

        modelBuilder.Entity<DirectChat>(entity =>
        {
            entity.ToTable("DirectChats");
            entity.HasKey(x => x.Id);
        });

        modelBuilder.Entity<DirectChatParticipant>(entity =>
        {
            entity.ToTable("DirectChatParticipants");
            entity.HasKey(x => x.Id);
        });

        modelBuilder.Entity<ServerEntity>(entity =>
        {
            entity.ToTable("Servers");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<ServerMember>(entity =>
        {
            entity.ToTable("ServerMembers");
            entity.HasKey(x => x.Id);
        });

        modelBuilder.Entity<Channel>(entity =>
        {
            entity.ToTable("Channels");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.ToTable("Messages");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Content).HasMaxLength(4000);
        });

        modelBuilder.Entity<CallSession>(entity =>
        {
            entity.ToTable("CallSessions");
            entity.HasKey(x => x.Id);
        });

        modelBuilder.Entity<UserSettings>(entity =>
        {
            entity.ToTable("UserSettings");
            entity.HasKey(x => x.Id);
        });
    }
}
