using BBT.Prism.Auditing;
using BBT.Prism.Data;
using BBT.Prism.Modularity;

namespace BBT.Prism.Application;

[Modules(
    typeof(PrismAuditingContractsModule),
    typeof(PrismDataModule)
)]
public class PrismDddApplicationContractsModule : PrismModule
{
}