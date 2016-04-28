using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace Authorization.Mvc
{
    public static class PolicyScanner
    {
        public static ICollection<PolicyRequirement> ScanMvcEndpointsForPolicyRequirements(Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));

            return assembly.GetTypes()
                .Where(type => typeof(Controller).IsAssignableFrom(type))
                .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                .Where(m => !m.GetCustomAttributes(typeof(CompilerGeneratedAttribute), true).Any() &&
                    !m.GetCustomAttributes(typeof(NonActionAttribute), true).Any())
                .Select(x => new PolicyRequirement
                {
                    Controller = x.DeclaringType?.Name,
                    Action = x.Name,
                    Policies = x.GetCustomAttributes(true).OfType<PolicyAttribute>().Select(y => y.PolicyType.Name).ToArray()
                })
                .ToArray();
        }
    }
}
