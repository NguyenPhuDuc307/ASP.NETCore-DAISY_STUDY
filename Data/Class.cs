using DaisyStudy.Data.Enums;

namespace DaisyStudy.Data;

public class Class
{
    public int ID { set; get; }
    public string? ClassID { set; get; }
    public string? ClassName { set; get; }
    public string? ImagePath { set; get; }
    public string? Topic { set; get; }
    public string? ClassRoom { set; get; }
    public string? Description { set; get; }
    public decimal Tuition { set; get; }
    public DateTime DateCreated { set; get; }
    public int ViewCount { set; get; }
    public Status Status { set; get; }
    public IsPublic isPublic { set; get; }
    public List<ClassDetail>? ClassDetails { set; get; }
    public List<Homework>? Homeworks { set; get; }
    public List<Notification>? Notifications { set; get; }
    public List<ExamSchedule>? ExamSchedules { set; get; }
}