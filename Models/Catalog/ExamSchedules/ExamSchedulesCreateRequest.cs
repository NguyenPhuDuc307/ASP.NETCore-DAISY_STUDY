using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;


namespace DaisyStudy.Models.Catalog.ExamSchedules;

public class ExamSchedulesCreateRequest
{

    [Display(Name = "Mã lớp học")]
    public int ClassID { set; get; }

    [Display(Name = "Tên kỳ thi")]
    public string? ExamScheduleName { set; get; }

    [Display(Name = "Ngày thi")]
    [Required(ErrorMessage = "Ngày thi không được bỏ trống")]
    public DateTime ExamDateTime { set; get; }

    [Display(Name = "Thời gian thi")]
    public int ExamTime { set; get; }
    [Display(Name = "Mô tả")]
    public string? Description { set; get; }
    public string? ReturnUrl { set; get; }

}