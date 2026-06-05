// Decompiled with JetBrains decompiler
// Type: CommonModApi.HookAttribute
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

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
