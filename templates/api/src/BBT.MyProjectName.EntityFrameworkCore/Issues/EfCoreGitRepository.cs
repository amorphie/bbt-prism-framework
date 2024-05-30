using System;
using BBT.MyProjectName.EntityFrameworkCore;
using BBT.Prism.Domain.EntityFrameworkCore;

namespace BBT.MyProjectName.Issues;

public class EfCoreGitRepository(MyProjectNameDbContext dbContext, IServiceProvider serviceProvider)
    : EfCoreRepository<MyProjectNameDbContext, GitRepository, Guid>(dbContext, serviceProvider), IGitRepository;