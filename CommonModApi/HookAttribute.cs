// -----------------------------------------------------------------------------
// Shared CommonModApi utility: HookAttribute
// Namespace/file: ruina-roguelike-reborn-main\CommonModApi\HookAttribute.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using System;

 
namespace CommonModApi
{ 
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed class HookAttribute : Attribute
{
  public HookAttribute(Type targetType, string methodName)
  {
    this.TargetType = targetType;
    this.TargetMethod = methodName;
  }

  public HookAttribute(string targetType, string methodName)
  {
    this.TargetTypeName = targetType;
    this.TargetMethod = methodName;
  }

  public Type TargetType { get; }

  public string TargetTypeName { get; }

  public string TargetMethod { get; }
}
}
