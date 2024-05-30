using System.ComponentModel.DataAnnotations;

namespace BBT.MyProjectName.Issues;

public class CloseIssueInput
{
    [Required]
    [Range(1, 20)]
    public IssueCloseReason CloseReason { get; set; }
}