using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace BBT.Prism;

public interface IApplicationServiceProvider : IPrismApplication
{
    void SetServiceProvider([NotNull] IServiceProvider serviceProvider);
    Task InitializeAsync([NotNull] IServiceProvider serviceProvider);
    void Initialize([NotNull] IServiceProvider serviceProvider);
}