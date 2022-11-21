using System.ComponentModel.DataAnnotations;

namespace DaisyStudy.Models.Catalog.Answers;
public class AnswerViewModel
{
    [Display(Name = "Mã câu trả lời")]
    public int AnswerID { set; get; }

    [Display(Name = "Mã câu hỏi")]
    public int QuestionID { set; get; }

    [Display(Name = "Nội dung câu trả lời")]
    public string? AnswerString { set; get; }

    public bool IsCorrect { set; get; }

    public string? ImagePath { set; get; }

    public long FileSize { set; get; }
}