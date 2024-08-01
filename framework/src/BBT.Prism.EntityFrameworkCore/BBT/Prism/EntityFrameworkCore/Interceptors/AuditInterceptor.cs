using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BBT.Prism.Auditing;
using BBT.Prism.Domain.Entites;
using BBT.Prism.Domain.Entities;
using BBT.Prism.Domain.Entities.Events;
using BBT.Prism.EventBus.Domains;
using BBT.Prism.EventBus.Integrations;
using BBT.Prism.Guids;
using BBT.Prism.Reflection;
using BBT.Prism.Threading;
using BBT.Prism.Timing;
using BBT.Prism.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BBT.Prism.EntityFrameworkCore.Interceptors;

public class AuditInterceptor(
    IClock clock,
    ICurrentUser currentUser,
    IGuidGenerator guidGenerator,
    IDomainEventBus domainEventBus,
    IIntegrationEventBus integrationEventBus)
    : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        SetAuditEntity(eventData.Context!);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        SetAuditEntity(eventData.Context!);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public async override ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var eventReport = CreateEventReport(eventData.Context!);
        var response = await base.SavedChangesAsync(eventData, result, cancellationToken);
        await PublishEntityEvents(eventReport);
        return response;
    }

    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        var eventReport = CreateEventReport(eventData.Context!);
        var response = base.SavedChanges(eventData, result);
        AsyncHelper.RunSync(() => PublishEntityEvents(eventReport));
        return response;
    }

    private EntityEventReport CreateEventReport(DbContext context)
    {
        var eventReport = new EntityEventReport();
        foreach (var entry in context.ChangeTracker.Entries().ToList())
        {
            var generatesDomainEventsEntity = entry.Entity as IGeneratesDomainEvents;
            if (generatesDomainEventsEntity == null)
            {
                continue;
            }

            var localEvents = generatesDomainEventsEntity.GetDomainEvents()?.ToArray();
            if (localEvents != null && localEvents.Any())
            {
                eventReport.DomainEvents.AddRange(
                    localEvents.Select(
                        eventRecord => new DomainEventEntry(
                            entry.Entity,
                            eventRecord.EventData,
                            eventRecord.EventOrder
                        )
                    )
                );
                generatesDomainEventsEntity.ClearDomainEvents();
            }

            var integrationEvents = generatesDomainEventsEntity.GetIntegrationEvents()?.ToArray();
            if (integrationEvents != null && integrationEvents.Any())
            {
                eventReport.IntegrationEvents.AddRange(
                    integrationEvents.Select(
                        eventRecord => new DomainEventEntry(
                            entry.Entity,
                            eventRecord.EventData,
                            eventRecord.EventOrder)
                    )
                );
                generatesDomainEventsEntity.ClearIntegrationEvents();
            }
        }

        return eventReport;
    }

    private async Task PublishEntityEvents(EntityEventReport changeReport)
    {
        foreach (var localEvent in changeReport.DomainEvents.OrderBy(o => o.EventOrder))
        {
            await domainEventBus.PublishAsync(localEvent.EventData.GetType(), localEvent.EventData);
        }

        foreach (var integrationEvent in changeReport.IntegrationEvents.OrderBy(o => o.EventOrder))
        {
            await integrationEventBus.PublishAsync(integrationEvent.EventData.GetType(), integrationEvent.EventData);
        }
    }

    protected virtual void ApplyConceptsForAddedEntity(EntityEntry entry)
    {
        CheckAndSetId(entry);
        SetConcurrencyStampIfNull(entry);
        SetCreationAuditProperties(entry);
    }

    private void CheckAndSetId(EntityEntry entry)
    {
        if (entry.Entity is IEntity<Guid> entityWithGuidId)
        {
            TrySetGuidId(entry, entityWithGuidId);
        }
    }

    private void TrySetGuidId(EntityEntry entry, IEntity<Guid> entity)
    {
        if (entity.Id != default)
        {
            return;
        }

        var idProperty = entry.Property("Id").Metadata.PropertyInfo!;

        //Check for DatabaseGeneratedAttribute
        var dbGeneratedAttr = ReflectionHelper
            .GetSingleAttributeOrDefault<DatabaseGeneratedAttribute>(
                idProperty
            );

        if (dbGeneratedAttr != null && dbGeneratedAttr.DatabaseGeneratedOption != DatabaseGeneratedOption.None)
        {
            return;
        }

        EntityHelper.TrySetId(
            entity,
            () => guidGenerator.Create(),
            true
        );
    }

    private void SetConcurrencyStampIfNull(EntityEntry entry)
    {
        var entity = entry.Entity as IHasConcurrencyStamp;
        if (entity == null)
        {
            return;
        }

        if (entity.ConcurrencyStamp != null)
        {
            return;
        }

        entity.ConcurrencyStamp = Guid.NewGuid().ToString("N");
    }

    private void UpdateConcurrencyStamp(EntityEntry entry)
    {
        var entity = entry.Entity as IHasConcurrencyStamp;
        if (entity == null)
        {
            return;
        }

        entry.Property("ConcurrencyStamp").OriginalValue = entity.ConcurrencyStamp;
        entity.ConcurrencyStamp = Guid.NewGuid().ToString("N");
    }

    private void SetCreationAuditProperties(EntityEntry entry)
    {
        if (entry.Entity is IHasCreatedAt)
        {
            entry.Property("CreatedAt").CurrentValue = clock.Now;
        }

        if (entry.Entity is ICreationAuditedObject)
        {
            if (currentUser.Id.HasValue)
            {
                entry.Property("CreatedBy").CurrentValue = currentUser.Id.Value;
            }
            
            if (currentUser.ActorUserId.HasValue)
            {
                entry.Property("CreatedByBehalfOf").CurrentValue = currentUser.ActorUserId.Value;
            }
        }
    }

    private void ApplyConceptsForModifiedEntity(EntityEntry entry)
    {
        if (entry.State == EntityState.Modified && entry.Properties.Any(x =>
                x.IsModified && (x.Metadata.ValueGenerated == ValueGenerated.Never ||
                                 x.Metadata.ValueGenerated == ValueGenerated.OnAdd)))
        {
            IncrementEntityVersionProperty(entry);
            SetModificationAuditProperties(entry);

            if (entry.Entity is ISoftDelete && entry.Entity.As<ISoftDelete>().IsDeleted)
            {
                SetDeletionAuditProperties(entry);
            }
        }
    }

    private void SetModificationAuditProperties(EntityEntry entry)
    {
        if (entry.Entity is IHasModifyTime)
        {
            entry.Property("ModifiedAt").CurrentValue = clock.Now;
        }
        
        if (entry.Entity is IModifyAuditedObject)
        {
            if (currentUser.Id.HasValue)
            {
                entry.Property("ModifiedBy").CurrentValue = currentUser.Id.Value;
            }
            
            if (currentUser.ActorUserId.HasValue)
            {
                entry.Property("ModifiedByBehalfOf").CurrentValue = currentUser.ActorUserId.Value;
            }
        }
    }

    private void SetDeletionAuditProperties(EntityEntry entry)
    {
        if (entry.Entity is IHasDeletionTime)
        {
            entry.Property("DeletedAt").CurrentValue = clock.Now;
        }
        
        if (entry.Entity is IDeletionAuditedObject)
        {
            if (currentUser.Id.HasValue)
            {
                entry.Property("DeletedBy").CurrentValue = clock.Now;
            }
        }
    }

    private void IncrementEntityVersionProperty(EntityEntry entry)
    {
        if (entry.Entity is IHasEntityVersion)
        {
            entry.Property("EntityVersion").CurrentValue =
                Convert.ToInt32(entry.Property("EntityVersion").CurrentValue ?? 0) + 1;
        }
    }

    private void ApplyConceptsForDeletedEntity(EntityEntry entry)
    {
        if (!(entry.Entity is ISoftDelete))
        {
            return;
        }

        entry.Reload();
        ObjectHelper.TrySetProperty(entry.Entity.As<ISoftDelete>(), x => x.IsDeleted, () => true);
        SetDeletionAuditProperties(entry);
    }

    private void SetAuditEntity(DbContext context)
    {
        foreach (var entry in context!.ChangeTracker.Entries())
        {
            if (entry.State.IsIn(EntityState.Modified, EntityState.Deleted))
            {
                UpdateConcurrencyStamp(entry);
            }

            switch (entry.State)
            {
                case EntityState.Added:
                    ApplyConceptsForAddedEntity(entry);
                    break;
                case EntityState.Modified:
                    ApplyConceptsForModifiedEntity(entry);
                    break;
                case EntityState.Deleted:
                    ApplyConceptsForDeletedEntity(entry);
                    break;
            }
        }
    }
}