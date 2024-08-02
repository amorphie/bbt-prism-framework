using System.Collections.Generic;

namespace BBT.Prism.MultiLingualEntities;

public interface IMultiLingualEntity<TTranslation>
    where TTranslation : class, IEntityTranslation
{
    ICollection<TTranslation> Translations { get; set; }
}