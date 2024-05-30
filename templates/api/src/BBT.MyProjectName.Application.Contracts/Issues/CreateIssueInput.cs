using System.ComponentModel.DataAnnotations;

namespace BBT.MyProjectName.Issues;

public class CreateIssueInput
{
    [Required]
    [MaxLength(IssueConsts.MaxTitleLength)]
    public string Title { get; set; }
    [MaxLength(IssueConsts.MaxTextLength)]
    public string? Text { get; set; }
}