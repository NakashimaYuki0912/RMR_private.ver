// Decompiled with JetBrains decompiler
// Type: CommonModApi.HookGenerationResults
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using MonoMod.RuntimeDetour;
using System.Collections.Generic;

 
namespace CommonModApi
{
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
