using System.ComponentModel.DataAnnotations;

namespace DaisyStudy.Models.Catalog.Submissions;

public class SubmissionCreateRequest
{
    public int HomeworkID { set; get; }

    public Guid StudentID { set; get; }

    [Display(Name = "Bài làm")]
    public string? Description { set; get; }
}

