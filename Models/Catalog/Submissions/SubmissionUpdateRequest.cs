using System.ComponentModel.DataAnnotations;

namespace DaisyStudy.Models.Catalog.Submissions;

public class SubmissionUpdateRequest
{
    public int SubmissionID { set; get; }

    [Display(Name = "Bài làm")]
    public string? Description { set; get; }
    public List<IFormFile>? ThumbnailImages { get; set; }
}
