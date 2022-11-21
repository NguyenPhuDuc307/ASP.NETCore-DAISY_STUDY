﻿using System.ComponentModel.DataAnnotations;

namespace DaisyStudy.Models.Catalog.Classes;

public class ClassUpdateRequest
{
public int ID { set; get; }

[Display(Name = "Tên lớp học")]
public string? ClassName { set; get; }

[Display(Name = "Chủ đề")]
public string? Topic { set; get; }

[Display(Name = "Phòng học")]
public string? ClassRoom { set; get; }

[Display(Name = "Mô tả")]
public string? Description { set; get; }

[Display(Name = "Hình ảnh")]
public IFormFile? ThumbnailImage { get; set; }
public decimal? Tuition { get; set; }
}
