using System.Linq;
using System.Reflection;
using UnityEngine;

internal static class MonoCacheExceptionsDetector
{
    private const string OnEnableMethodName = "OnEnable";
    private const string OnDisableMethodName = "OnDisable";
    private const string UpdateMethodName = "Update";
    private const string FixedUpdateMethodName = "FixedUpdate";
    private const string LateUpdateMethodName = "LateUpdate";

    private const BindingFlags MethodFlags = BindingFlags.Public |
                                             BindingFlags.NonPublic |
                                             BindingFlags.Instance |
                                             BindingFlags.DeclaredOnly;

    public static void CheckForExceptions()
    {
        var targetMethodNames = new string[]
        {
            OnEnableMethodName, OnDisableMethodName, UpdateMethodName, FixedUpdateMethodName, LateUpdateMethodName
        };
        var monoCacheType = typeof(MonoCache);
        var subclassTypes = Assembly
            .GetAssembly(monoCacheType)
            .GetTypes()
            .Where(type => type.IsAssignableFrom(monoCacheType));

        foreach (var type in subclassTypes)
        {
            if (type == monoCacheType)
                continue;

            var methods = type.GetMethods(MethodFlags);

            foreach (var targetMethodName in targetMethodNames)
            {
                foreach (var method in methods)
                {
                    if (method.Name == targetMethodName)
                    {
                        Debug.LogError($"You are using the basic Unity method <{targetMethodName}> " +
                                       $"in <{type.Name}>! Use the analogue from <{nameof(MonoCache)}>");
                    }
                }
            }
        }
    }
}