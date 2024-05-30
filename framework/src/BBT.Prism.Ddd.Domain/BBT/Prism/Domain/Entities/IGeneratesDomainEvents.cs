using System.Collections.Generic;

namespace BBT.Prism.Domain.Entities;

public interface IGeneratesDomainEvents
{
    IEnumerable<DomainEventRecord> GetDomainEvents();
    IEnumerable<DomainEventRecord> GetIntegrationEvents();
    
    void ClearDomainEvents();
    void ClearIntegrationEvents();
}