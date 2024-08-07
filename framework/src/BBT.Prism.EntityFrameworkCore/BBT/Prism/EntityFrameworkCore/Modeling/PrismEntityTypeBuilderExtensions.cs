using System;
using BBT.Prism.Auditing;
using BBT.Prism.Domain.Entites;
using BBT.Prism.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BBT.Prism.EntityFrameworkCore.Modeling;

public static class PrismEntityTypeBuilderExtensions
{
    public static void ConfigureByConvention(this EntityTypeBuilder b)
    {
        b.TryConfigureConcurrencyStamp();
        b.TryConfigureSoftDelete();
        b.TryConfigureDeletionTime();
        b.TryConfigureDeletionAudited();
        b.TryConfigureCreatedAt();
        b.TryConfigureModifiedAt();
        b.TryConfigureModificationAudited();
    }

    public static void ConfigureConcurrencyStamp<T>(this EntityTypeBuilder<T> b)
        where T : class, IHasConcurrencyStamp
    {
        b.As<EntityTypeBuilder>().TryConfigureConcurrencyStamp();
    }

    public static void TryConfigureConcurrencyStamp(this EntityTypeBuilder b)
    {
        if (b.Metadata.ClrType.IsAssignableTo<IHasConcurrencyStamp>())
        {
            b.Property(nameof(IHasConcurrencyStamp.ConcurrencyStamp))
                .IsConcurrencyToken()
                .HasMaxLength(ConcurrencyStampConsts.MaxLength)
                .HasColumnName(nameof(IHasConcurrencyStamp.ConcurrencyStamp));
        }
    }
    
    public static void ConfigureSoftDelete<T>(this EntityTypeBuilder<T> b)
        where T : class, ISoftDelete
    {
        b.As<EntityTypeBuilder>().TryConfigureSoftDelete();
    }

    public static void TryConfigureSoftDelete(this EntityTypeBuilder b)
    {
        if (b.Metadata.ClrType.IsAssignableTo<ISoftDelete>())
        {
            b.Property(nameof(ISoftDelete.IsDeleted))
                .IsRequired()
                .HasDefaultValue(false)
                .HasColumnName(nameof(ISoftDelete.IsDeleted));
        }
    }

    public static void ConfigureDeletionTime<T>(this EntityTypeBuilder<T> b)
        where T : class, IHasDeletionTime
    {
        b.As<EntityTypeBuilder>().TryConfigureDeletionTime();
    }

    public static void TryConfigureDeletionTime(this EntityTypeBuilder b)
    {
        if (b.Metadata.ClrType.IsAssignableTo<IHasDeletionTime>())
        {
            b.TryConfigureSoftDelete();

            b.Property(nameof(IHasDeletionTime.DeletedAt))
                .IsRequired(false)
                .HasColumnName(nameof(IHasDeletionTime.DeletedAt));
        }
    }

    public static void ConfigureDeletionAudited<T>(this EntityTypeBuilder<T> b)
        where T : class, IDeletionAuditedObject
    {
        b.As<EntityTypeBuilder>().TryConfigureDeletionAudited();
    }

    public static void TryConfigureDeletionAudited(this EntityTypeBuilder b)
    {
        if (b.Metadata.ClrType.IsAssignableTo<IDeletionAuditedObject>())
        {
            b.TryConfigureDeletionTime();

            b.Property(nameof(IDeletionAuditedObject.DeletedAt))
                .IsRequired(false)
                .HasColumnName(nameof(IDeletionAuditedObject.DeletedAt));
        }
    }

    public static void ConfigureCreatedAt<T>(this EntityTypeBuilder<T> b)
        where T : class, IHasCreatedAt
    {
        b.As<EntityTypeBuilder>().TryConfigureCreatedAt();
    }

    public static void TryConfigureCreatedAt(this EntityTypeBuilder b)
    {
        if (b.Metadata.ClrType.IsAssignableTo<IHasCreatedAt>())
        {
            b.Property(nameof(IHasCreatedAt.CreatedAt))
                .IsRequired()
                .HasColumnName(nameof(IHasCreatedAt.CreatedAt));
        }
    }

    public static void ConfigureCreationAudited<T>(this EntityTypeBuilder<T> b)
        where T : class, ICreationAuditedObject
    {
        b.As<EntityTypeBuilder>().TryConfigureCreationAudited();
    }

    public static void TryConfigureCreationAudited(this EntityTypeBuilder b)
    {
        if (b.Metadata.ClrType.IsAssignableTo<ICreationAuditedObject>())
        {
            b.As<EntityTypeBuilder>().TryConfigureCreatedAt();
        }
    }

    public static void ConfigureModifiedAt<T>(this EntityTypeBuilder<T> b)
        where T : class, IHasModifyTime
    {
        b.As<EntityTypeBuilder>().TryConfigureModifiedAt();
    }

    public static void TryConfigureModifiedAt(this EntityTypeBuilder b)
    {
        if (b.Metadata.ClrType.IsAssignableTo<IHasModifyTime>())
        {
            b.Property(nameof(IHasModifyTime.ModifiedAt))
                .IsRequired(false)
                .HasColumnName(nameof(IHasModifyTime.ModifiedAt));
        }
    }

    public static void ConfigureModificationAudited<T>(this EntityTypeBuilder<T> b)
        where T : class, IModifyAuditedObject
    {
        b.As<EntityTypeBuilder>().TryConfigureModificationAudited();
    }

    public static void TryConfigureModificationAudited(this EntityTypeBuilder b)
    {
        if (b.Metadata.ClrType.IsAssignableTo<IModifyAuditedObject>())
        {
            b.TryConfigureModifiedAt();

            b.Property(nameof(IModifyAuditedObject.ModifiedBy))
                .IsRequired(false)
                .HasColumnName(nameof(IModifyAuditedObject.ModifiedBy));
            
            b.Property(nameof(IModifyAuditedObject.ModifiedByBehalfOf))
                .IsRequired(false)
                .HasColumnName(nameof(IModifyAuditedObject.ModifiedByBehalfOf));
        }
    }

    public static void ConfigureAudited<T>(this EntityTypeBuilder<T> b)
        where T : class, IAuditedObject
    {
        b.As<EntityTypeBuilder>().TryConfigureAudited();
    }

    public static void TryConfigureAudited(this EntityTypeBuilder b)
    {
        if (b.Metadata.ClrType.IsAssignableTo<IAuditedObject>())
        {
            b.As<EntityTypeBuilder>().TryConfigureCreationAudited();
            b.As<EntityTypeBuilder>().TryConfigureModificationAudited();
        }
    }

    public static void ConfigureFullAudited<T>(this EntityTypeBuilder<T> b)
        where T : class, IFullAuditedObject
    {
        b.As<EntityTypeBuilder>().TryConfigureFullAudited();
    }

    public static void TryConfigureFullAudited(this EntityTypeBuilder b)
    {
        if (b.Metadata.ClrType.IsAssignableTo<IFullAuditedObject>())
        {
            b.As<EntityTypeBuilder>().TryConfigureAudited();
            b.As<EntityTypeBuilder>().TryConfigureDeletionAudited();
        }
    }

    public static void ConfigureCreationAuditedAggregateRoot<T>(this EntityTypeBuilder<T> b)
        where T : class
    {
        b.As<EntityTypeBuilder>().TryConfigureCreationAudited();
        b.As<EntityTypeBuilder>().TryConfigureConcurrencyStamp();
    }

    public static void ConfigureAuditedAggregateRoot<T>(this EntityTypeBuilder<T> b)
        where T : class
    {
        b.As<EntityTypeBuilder>().TryConfigureAudited();
        b.As<EntityTypeBuilder>().TryConfigureConcurrencyStamp();
    }

    public static void ConfigureFullAuditedAggregateRoot<T>(this EntityTypeBuilder<T> b)
        where T : class
    {
        b.As<EntityTypeBuilder>().TryConfigureFullAudited();
        b.As<EntityTypeBuilder>().TryConfigureConcurrencyStamp();
    }
}