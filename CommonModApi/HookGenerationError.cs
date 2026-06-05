// Decompiled with JetBrains decompiler
// Type: CommonModApi.HookGenerationError
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System.Reflection;

 
namespace CommonModApi { 

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
