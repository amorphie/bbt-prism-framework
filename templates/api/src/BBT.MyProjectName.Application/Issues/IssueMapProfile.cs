using AutoMapper;

namespace BBT.MyProjectName.Issues;

internal class IssueMapProfile : Profile
{
    public IssueMapProfile()
    {
        CreateMap<Issue, IssueDto>();
    }
}