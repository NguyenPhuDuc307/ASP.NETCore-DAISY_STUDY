using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaisyStudy.Models.Catalog.ExamSchedules;

public class ExamSchedulesViewModel
{
    public int ExamScheduleID { set; get; }

    [Display(Name = "Mã lớp học")]       
    public int? ClassID { set; get; }

    [Display(Name = "Tên lớp học")]
    public string? ClassName { get; set; }

    [Display(Name = "Tên kì thi")]
    public string? ExamScheduleName { set; get; }

    [Display(Name = "Ngày tạo")]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime DateTimeCreated { set; get; }

    [Display(Name = "Ngày thi")]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime ExamDateTime { set; get; }

    [Display(Name = "Thời gian thi")]
    public int ExamTime { set; get; }
    public string? Description { set; get; }

}