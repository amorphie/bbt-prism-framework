using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace BBT.Prism.Modularity;

public sealed class ModuleLoader: IModuleLoader
{
   public IPrismModuleDescriptor[] LoadModules(
        IServiceCollection services,
        Type startupModuleType)
    {
        Check.NotNull(services, nameof(services));
        Check.NotNull(startupModuleType, nameof(startupModuleType));

        var modules = GetDescriptors(services, startupModuleType);

        modules = SortByDependency(modules, startupModuleType);

        return modules.ToArray();
    }

    private List<IPrismModuleDescriptor> GetDescriptors(
        IServiceCollection services,
        Type startupModuleType)
    {
        var modules = new List<PrismModuleDescriptor>();

        FillModules(modules, services, startupModuleType);
        SetDependencies(modules);

        return modules.Cast<IPrismModuleDescriptor>().ToList();
    }

    private void FillModules(
        List<PrismModuleDescriptor> modules,
        IServiceCollection services,
        Type startupModuleType)
    {
        var logger = services.GetInitLogger<PrismApplicationBase>();

        //All modules starting from the startup module
        foreach (var moduleType in PrismModuleHelper.FindAllModuleTypes(startupModuleType, logger))
        {
            modules.Add(CreateModuleDescriptor(services, moduleType));
        }
    }

    private void SetDependencies(List<PrismModuleDescriptor> modules)
    {
        foreach (var module in modules)
        {
            SetDependencies(modules, module);
        }
    }

    private List<IPrismModuleDescriptor> SortByDependency(List<IPrismModuleDescriptor> modules, Type startupModuleType)
    {
        var sortedModules = modules.SortByDependencies(m => m.Dependencies);
        sortedModules.MoveItem(m => m.Type == startupModuleType, modules.Count - 1);
        return sortedModules;
    }

    private PrismModuleDescriptor CreateModuleDescriptor(IServiceCollection services, Type moduleType, bool isLoadedAsPlugIn = false)
    {
        return new PrismModuleDescriptor(moduleType, CreateAndRegisterModule(services, moduleType), isLoadedAsPlugIn);
    }

    private IPrismModule CreateAndRegisterModule(IServiceCollection services, Type moduleType)
    {
        var module = (IPrismModule)Activator.CreateInstance(moduleType)!;
        services.AddSingleton(moduleType, module);
        return module;
    }

    private void SetDependencies(List<PrismModuleDescriptor> modules, PrismModuleDescriptor module)
    {
        foreach (var dependedModuleType in PrismModuleHelper.FindDependedModuleTypes(module.Type))
        {
            var dependedModule = modules.FirstOrDefault(m => m.Type == dependedModuleType);
            if (dependedModule == null)
            {
                throw new PrismException("Could not find a depended module " + dependedModuleType.AssemblyQualifiedName + " for " + module.Type.AssemblyQualifiedName);
            }

            module.AddDependency(dependedModule);
        }
    }
}
