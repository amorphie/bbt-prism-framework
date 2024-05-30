using System;
using BBT.Prism.Domain.Entities;

namespace BBT.MyProjectName.Issues;

public class Label: AggregateRoot<Guid>
{
    public string Name { get; set; }
    public string Color { get; set; }
}