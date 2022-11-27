using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;


namespace DaisyStudy.Models.Catalog.ExamSchedules;

public class ExamSchedulesCreateRequest
{

    [Display(Name = "Mã lớp học")]
    public int ClassID { set; get; }

    [Display(Name = "Tên kì thi")]
    public string? ExamScheduleName { set; get; }

    [Display(Name = "Ngày thi")]
    /*[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]*/
    public DateTime ExamDateTime { set; get; }

    [Display(Name = "Thời gian thi")]
    public int ExamTime { set; get; }
    public string? Description { set; get; }

}