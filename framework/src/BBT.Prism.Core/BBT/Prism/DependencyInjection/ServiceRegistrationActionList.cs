using System;
using System.Collections.Generic;

namespace BBT.Prism.DependencyInjection;

public class ServiceRegistrationActionList: List<Action<IOnServiceRegisteredContext>>
{
}
