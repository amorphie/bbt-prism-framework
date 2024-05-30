using BBT.Prism.Modularity;
using BBT.Prism.Uow;

namespace BBT.Prism.Data;

[Modules(
    typeof(PrismUnitOfWorkModule))]
public class PrismDataModule: PrismModule
{
    
}