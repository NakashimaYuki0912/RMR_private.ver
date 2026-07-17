// -----------------------------------------------------------------------------
// Shared CommonModApi utility: HookGenerationResults
// Namespace/file: ruina-roguelike-reborn-main\CommonModApi\HookGenerationResults.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using MonoMod.RuntimeDetour;
using System.Collections.Generic;

 
namespace CommonModApi
{
/// <summary>HookGenerationResults</summary>
public class HookGenerationResults
{
  public List<Hook> Hooks;
  public List<HookGenerationError> Errors;

  public HookGenerationResults()
  {
    this.Hooks = new List<Hook>();
    this.Errors = new List<HookGenerationError>();
  }
}
}
