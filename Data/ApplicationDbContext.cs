﻿using DaisyStudy.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DaisyStudy.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);

        builder.ApplyConfiguration(new ApplicationUserEntityConfiguration());
        builder.ApplyConfiguration(new AnswerConfiguration());
        builder.ApplyConfiguration(new ClassConfiguration());
        builder.ApplyConfiguration(new ClassDetailConfiguration());
        builder.ApplyConfiguration(new CommentConfiguration());
        builder.ApplyConfiguration(new ContactConfiguration());
        builder.ApplyConfiguration(new ExamScheduleConfiguration());
        builder.ApplyConfiguration(new HomeworkConfiguration());
        builder.ApplyConfiguration(new MessageConfiguration());
        builder.ApplyConfiguration(new NotificationConfiguration());
        builder.ApplyConfiguration(new NotificationImageConfiguration());
        builder.ApplyConfiguration(new QuestionConfiguration());
        builder.ApplyConfiguration(new RoomConfiguration());
        builder.ApplyConfiguration(new StudentExamConfiguration());
        builder.ApplyConfiguration(new StudentExamDetailConfiguration());
        builder.ApplyConfiguration(new SubmissionConfiguration());
        builder.Entity<IdentityRole<string>>().ToTable("AppRoles");
        builder.Entity<IdentityUserClaim<string>>().ToTable("AppUserClaims");
        builder.Entity<IdentityUserRole<string>>().ToTable("AppUserRoles").HasKey(x => new { x.UserId, x.RoleId });
        builder.Entity<IdentityUserLogin<string>>().ToTable("AppUserLogins").HasKey(x => x.UserId);
        builder.Entity<IdentityRoleClaim<string>>().ToTable("AppRoleClaims");
        builder.Entity<IdentityUserToken<string>>().ToTable("AppUserTokens").HasKey(x => x.UserId);
    }

    public DbSet<DaisyStudy.Data.Class> Class { get; set; } = default!;
}


public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.ToTable("AppUsers");

        builder.Property(x => x.FirstName).IsRequired().HasMaxLength(255);
        builder.Property(x => x.LastName).IsRequired().HasMaxLength(255);
        builder.Property(x => x.Dob).IsRequired();
        builder.Property(x => x.AccountBalance).IsRequired().HasColumnType("decimal(18,2)");
    }
}

public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
{
    public void Configure(EntityTypeBuilder<Answer> builder)
    {
        builder.ToTable("Answers");

        builder.HasKey(x => x.AnswerID);

        builder.Property(x => x.AnswerID).UseIdentityColumn();
        builder.Property(x => x.QuestionID).IsRequired();
        builder.Property(x => x.AnswerString).IsRequired();
        builder.Property(x => x.ImagePath).HasMaxLength(200).IsRequired();
        builder.Property(x => x.IsCorrect).IsRequired().HasDefaultValue(false);

        builder.HasOne(x => x.Question).WithMany(x => x.Answers).HasForeignKey(x => x.QuestionID);
    }
}

public class ClassConfiguration : IEntityTypeConfiguration<Class>
{
    public void Configure(EntityTypeBuilder<Class> builder)
    {
        builder.ToTable("Classes");

        builder.HasKey(x => x.ID);

        builder.Property(x => x.ID).UseIdentityColumn();
        builder.Property(x => x.ClassID).IsUnicode(false).IsRequired();
        builder.Property(x => x.ClassName).IsRequired();
        builder.Property(x => x.Tuition).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(x => x.ImagePath).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Status).IsRequired();
        builder.Property(x => x.isPublic).IsRequired();
        builder.Property(x => x.ViewCount).IsRequired().HasDefaultValue(0);
        builder.Property(x => x.DateCreated).IsRequired();
    }
}

public class ClassDetailConfiguration : IEntityTypeConfiguration<ClassDetail>
{
    public void Configure(EntityTypeBuilder<ClassDetail> builder)
    {
        builder.ToTable("ClassDetails");

        builder.HasKey(x => new { x.ClassID, x.UserID });

        builder.HasOne(x => x.Class).WithMany(x => x.ClassDetails).HasForeignKey(x => x.ClassID);
        builder.HasOne(x => x.User).WithMany(x => x.ClassDetails).HasForeignKey(x => x.UserID);
    }
}

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("Comments");

        builder.HasKey(x => x.CommentID);

        builder.Property(x => x.UserID).IsRequired();
        builder.Property(x => x.NotificationID).IsRequired();
        builder.Property(x => x.Content).IsRequired();
        builder.Property(x => x.DateTimeCreated).IsRequired();

        builder.HasOne(x => x.Notification).WithMany(x => x.Comments).HasForeignKey(x => x.NotificationID);
        builder.HasOne(x => x.AppUser).WithMany(x => x.Comments).HasForeignKey(x => x.UserID);
    }
}

public class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.ToTable("Contacts");

        builder.HasKey(x => x.ContactID);

        builder.Property(x => x.ContactID).UseIdentityColumn();
        builder.Property(x => x.CustomerName).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Email).HasMaxLength(200).IsRequired();
        builder.Property(x => x.PhoneNumber).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Message).IsRequired();
    }
}

public class ExamScheduleConfiguration : IEntityTypeConfiguration<ExamSchedule>
{
    public void Configure(EntityTypeBuilder<ExamSchedule> builder)
    {
        builder.ToTable("ExamSchedules");

        builder.HasKey(x => x.ExamScheduleID);

        builder.Property(x => x.ExamScheduleID).UseIdentityColumn();
        builder.Property(x => x.ClassID).IsRequired();
        builder.Property(x => x.ExamScheduleName).IsRequired();
        builder.Property(x => x.DateTimeCreated).IsRequired();
        builder.Property(x => x.ExamDateTime).IsRequired();
        builder.Property(x => x.ExamTime).IsRequired();

        builder.HasOne(x => x.Class).WithMany(x => x.ExamSchedules).HasForeignKey(x => x.ClassID);
    }
}

public class HomeworkConfiguration : IEntityTypeConfiguration<Homework>
{
    public void Configure(EntityTypeBuilder<Homework> builder)
    {
        builder.ToTable("Homeworks");

        builder.HasKey(x => x.HomeworkID);

        builder.Property(x => x.HomeworkID).UseIdentityColumn();
        builder.Property(x => x.ClassID).IsRequired();
        builder.Property(x => x.Deadline).IsRequired();
        builder.Property(x => x.DateTimeCreated).IsRequired();

        builder.HasOne(x => x.Class).WithMany(x => x.Homeworks).HasForeignKey(x => x.ClassID);
    }
}

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable("Messages");

        builder.Property(s => s.Content).IsRequired().HasMaxLength(500);

        builder.HasOne(s => s.ToRoom)
            .WithMany(m => m.Messages)
            .HasForeignKey(s => s.ToRoomId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("Notifications");

        builder.HasKey(x => x.NotificationID);

        builder.Property(x => x.NotificationID).UseIdentityColumn();
        builder.Property(x => x.ClassID).IsRequired();
        builder.Property(x => x.Title).IsRequired();
        builder.Property(x => x.Content).IsRequired();
        builder.Property(x => x.DateTimeCreated).IsRequired();

        builder.HasOne(x => x.Class).WithMany(x => x.Notifications).HasForeignKey(x => x.ClassID);
    }
}

public class NotificationImageConfiguration : IEntityTypeConfiguration<NotificationImage>
{
    public void Configure(EntityTypeBuilder<NotificationImage> builder)
    {
        builder.ToTable("NotificationImages");

        builder.HasKey(x => x.ImageID);

        builder.Property(x => x.NotificationID).IsRequired();
        builder.Property(x => x.ImageID).UseIdentityColumn();
        builder.Property(x => x.ImagePath).HasMaxLength(200).IsRequired();

        builder.HasOne(x => x.Notification).WithMany(x => x.NotificationImages).HasForeignKey(x => x.NotificationID);
    }
}

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.ToTable("Questions");

        builder.HasKey(x => x.QuestionID);

        builder.Property(x => x.QuestionID).UseIdentityColumn();
        builder.Property(x => x.QuestionString).IsRequired();
        builder.Property(x => x.Point).IsRequired().HasDefaultValue(0);
        builder.Property(x => x.ImagePath).HasMaxLength(200).IsRequired();
        builder.HasOne(x => x.ExamSchedule).WithMany(x => x.Questions).HasForeignKey(x => x.ExamScheduleID);
    }
}

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.ToTable("Rooms");

        builder.Property(s => s.Name).IsRequired().HasMaxLength(100);

        builder.HasOne(s => s.Admin)
            .WithMany(u => u.Rooms)
            .IsRequired();
    }
}

public class StudentExamConfiguration : IEntityTypeConfiguration<StudentExam>
{
    public void Configure(EntityTypeBuilder<StudentExam> builder)
    {
        builder.ToTable("StudentExams");

        builder.HasKey(x => x.StudentExamID);

        builder.Property(x => x.StudentExamID).UseIdentityColumn();
        builder.Property(x => x.ExamScheduleID).IsRequired();
        builder.Property(x => x.StudentID).IsRequired();
        builder.Property(x => x.Mark).IsRequired();
        builder.Property(x => x.StudentExamDateTime).IsRequired();

        builder.HasOne(x => x.ExamSchedule).WithMany(x => x.StudentExams).HasForeignKey(x => x.ExamScheduleID);
        builder.HasOne(x => x.Student).WithMany(x => x.StudentExams).HasForeignKey(x => x.StudentID);
    }
}

public class StudentExamDetailConfiguration : IEntityTypeConfiguration<StudentExamDetail>
{
    public void Configure(EntityTypeBuilder<StudentExamDetail> builder)
    {
        builder.ToTable("StudentExamDetails");

        builder.HasKey(x => x.StudentExamDetailID);

        builder.Property(x => x.StudentExamDetailID).UseIdentityColumn();
        builder.Property(x => x.StudentExamID).IsRequired();
        builder.Property(x => x.AnswerID).IsRequired();

        builder.HasOne(x => x.StudentExam).WithMany(x => x.StudentExamDetails).HasForeignKey(x => x.StudentExamID).OnDelete(DeleteBehavior.ClientCascade);
        builder.HasOne(x => x.Answer).WithMany(x => x.StudentExamDetails).HasForeignKey(x => x.AnswerID);
    }
}

public class SubmissionConfiguration : IEntityTypeConfiguration<Submission>
{
    public void Configure(EntityTypeBuilder<Submission> builder)
    {
        builder.ToTable("Submissions");

        builder.HasKey(x => new { x.HomeworkID, x.StudentID });

        builder.Property(x => x.Mark).IsRequired().HasDefaultValue(0);
        builder.Property(x => x.Mark).IsRequired();

        builder.HasOne(x => x.Homework).WithMany(x => x.Submissions).HasForeignKey(x => x.HomeworkID);
        builder.HasOne(x => x.Student).WithMany(x => x.Submissions).HasForeignKey(x => x.StudentID);
    }
}