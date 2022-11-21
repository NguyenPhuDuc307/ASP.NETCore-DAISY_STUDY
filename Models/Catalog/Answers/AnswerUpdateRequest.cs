using System.ComponentModel.DataAnnotations;

namespace DaisyStudy.Models.Catalog.Answers;

public class AnswerUpdateRequest
{
    [Display(Name = "Mã câu hỏi")]
    public int AnswerID { set; get; }

    [Display(Name = "Nôi dung câu trả lời")]
    public string? AnswerString { set; get; }

    public bool IsCorrect { set; get; }

    public IFormFile? ImagePath { get; set; }

    public long FileSize { set; get; }
}