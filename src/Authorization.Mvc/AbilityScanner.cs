using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace Authorization.Mvc
{
    public static class AbilityScanner
    {
        public static ICollection<AbilityRequirement> ScanMvcEndpointsForRequirements(Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));

            return assembly.GetTypes()
                .Where(type => typeof(Controller).IsAssignableFrom(type))
                .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                .Where(m => !m.GetCustomAttributes(typeof(CompilerGeneratedAttribute), true).Any() &&
                    !m.GetCustomAttributes(typeof(NonActionAttribute), true).Any())
                .Select(x => new AbilityRequirement
                {
                    Controller = x.DeclaringType?.Name,
                    Action = x.Name,
                    Policies = x.GetCustomAttributes(true).OfType<CustomAttribute>().Select(y => y.AbilityType.Name).ToArray()
                })
                .ToArray();
        }
    }
}
