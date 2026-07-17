// -----------------------------------------------------------------------------
// Shared CommonModApi utility: HookGenerationError
// Namespace/file: ruina-roguelike-reborn-main\CommonModApi\HookGenerationError.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using System.Reflection;

 
namespace CommonModApi { 

/// <summary>HookGenerationError</summary>

public class HookGenerationError
{
  public MethodInfo HookMethod;
  public object TargetTypeDescriptor;
  public string TargetMethod;
  public string Error;

  public HookGenerationError(MethodInfo hookMethod, string error)
  {
    this.HookMethod = hookMethod;
    this.Error = error;
  }

  public override string ToString()
  {
    return $"Could not generate hook for {this.HookMethod} to {this.TargetTypeDescriptor}.{this.TargetMethod} because {this.Error}";
  }
}
}
