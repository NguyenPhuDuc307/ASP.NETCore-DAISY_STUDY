﻿namespace DaisyStudy.Data;

public class Question
{
    public int QuestionID { set; get; }
    public int ExamScheduleID { set; get; }
    public ExamSchedule? ExamSchedule { set; get; }
    public string? QuestionString { set; get; }
    public float Point { set; get; }
    public string? ImagePath { set; get; }
    public long ImageFileSize { set; get; }
    public List<Answer>? Answers { set; get; }
}

