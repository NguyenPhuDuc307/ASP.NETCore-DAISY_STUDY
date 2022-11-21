﻿using System.ComponentModel.DataAnnotations;
using DaisyStudy.Data.Enums;
using DaisyStudy.Models.Catalog.Classes;
using DaisyStudy.Models.Catalog.Notifications;

namespace DaisyStudy.ViewModels.Catalog.Classes;

public class ClassViewModel
{
    public int ID { set; get; }

    [Display(Name = "Mã lớp học")]
    public string? ClassID { set; get; }

    [Display(Name = "Giáo viên")]
    public string? Teacher { set; get; }

    public string? TeacherUserName { set; get; }

    [Display(Name = "Ảnh đại diện")]
    public string? TeacherImage { set; get; }

    [Display(Name = "Số học viên")]
    public int StudentNumber { set; get; }

    [Display(Name = "Hình ảnh")]
    public string? Image { set; get; }

    [Display(Name = "Tên lớp học")]
    public string? ClassName { set; get; }

    [Display(Name = "Chủ đề")]
    public string? Topic { set; get; }

    [Display(Name = "Phòng học")]
    public string? ClassRoom { set; get; }

    [Display(Name = "Mô tả")]
    public string? Description { set; get; }
    
    [Display(Name = "Học phí")]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0} đ")]
    public decimal Tuition { set; get; }

    [Display(Name = "Ngày tạo")]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime DateCreated { set; get; }

    [Display(Name = "Lượt xem")]
    public int ViewCount { set; get; }

    [Display(Name = "Trạng thái")]
    public Status Status { set; get; }

    [Display(Name = "Công khai")]
    public IsPublic isPublic { set; get; }

    public ICollection<ClassDetailViewModel>? ClassDetails { set; get; }
    public ICollection<NotificationViewModel>? Notifications { set; get; }
}

