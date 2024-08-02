using System.Collections.Generic;
using System.Threading.Tasks;
using BBT.Prism.MultiLingualEntities;

namespace BBT.Prism.Domain.Services;

public interface IMultiLingualEntityManager
{
    Task<TTranslation?> GetTranslationAsync<TMultiLingual, TTranslation>(
        TMultiLingual multiLingual,
        string? culture = null,
        bool fallbackToParentCultures = true)
        where TMultiLingual : IMultiLingualEntity<TTranslation>
        where TTranslation : class, IEntityTranslation;

    Task<TTranslation?> GetTranslationAsync<TTranslation>(
        IEnumerable<TTranslation> translations,
        string? culture = null,
        bool fallbackToParentCultures = true)
        where TTranslation : class, IEntityTranslation;


    Task<List<TTranslation?>> GetBulkTranslationsAsync<TTranslation>(
        IEnumerable<IEnumerable<TTranslation>> translationsCombined,
        string? culture = null,
        bool fallbackToParentCultures = true)
        where TTranslation : class, IEntityTranslation;

    Task<List<(TMultiLingual entity, TTranslation? translation)>> GetBulkTranslationsAsync<TMultiLingual, TTranslation>(
        IEnumerable<TMultiLingual> multiLinguals,
        string? culture = null,
        bool fallbackToParentCultures = true)
        where TMultiLingual : IMultiLingualEntity<TTranslation>
        where TTranslation : class, IEntityTranslation;
}