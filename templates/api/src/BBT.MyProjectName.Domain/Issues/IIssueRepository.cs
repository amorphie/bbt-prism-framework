using System;
using BBT.Prism.Domain.Repositories;

namespace BBT.MyProjectName.Issues;

public interface IIssueRepository: IRepository<Issue, Guid>
{
    
}