// Decompiled with JetBrains decompiler
// Type: CommonModApi.HookHelper
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using HarmonyLib;
using MonoMod.RuntimeDetour;
using System;
using System.Reflection;

 
namespace CommonModApi
{ 
public static class HookHelper
{
  public static Hook CreateHook(
    System.Type sourceType,
    string sourceMethodName,
    object newReceiver,
    string hookMethodName)
  {
    return new Hook((MethodBase) sourceType.GetMethod(sourceMethodName, AccessTools.all), ReflectionHelper.CreateInstanceDelegate(newReceiver, hookMethodName));
  }

  public static Hook CreateHook(
    System.Type sourceType,
    MethodBase sourceMethodName,
    object newReceiver,
    string hookMethodName)
  {
    Delegate instanceDelegate = ReflectionHelper.CreateInstanceDelegate(newReceiver, hookMethodName);
    return new Hook(sourceMethodName, instanceDelegate);
  }

  public static HookGenerationResults GenerateHooks(object source)
  {
    HookGenerationResults hooks = new HookGenerationResults();
    foreach (MethodInfo method in source.GetType().GetMethods(AccessTools.all))
    {
      HookAttribute[] customAttributes = (HookAttribute[]) method.GetCustomAttributes(typeof (HookAttribute), false);
      if (customAttributes.Length != 0)
      {
        foreach (HookAttribute hookAtt in customAttributes)
        {
          Tuple<Hook, HookGenerationError> hook = HookHelper.TryGenerateHook(source, method, hookAtt);
          if (hook.First != null)
            hooks.Hooks.Add(hook.First);
          else
            hooks.Errors.Add(hook.Second);
        }
      }
    }
    return hooks;
  }

  public static Tuple<Hook, HookGenerationError> TryGenerateHook(
    object source,
    MethodInfo hookMethod,
    HookAttribute hookAtt)
  {
    System.Type type = (System.Type) null;
    if (hookAtt.TargetType != (System.Type) null)
      type = hookAtt.TargetType;
    if (type == (System.Type) null)
    {
      HookGenerationError second = new HookGenerationError(hookMethod, "Could not find type");
      System.Type targetType = hookAtt.TargetType;
      if ( targetType == null)
        targetType = hookAtt.TargetType;
      second.TargetTypeDescriptor =  targetType;
      second.TargetMethod = hookAtt.TargetMethod;
      return Tuple.Create<Hook, HookGenerationError>((Hook) null, second);
    }
    if (string.IsNullOrEmpty(hookAtt.TargetMethod))
    {
      HookGenerationError second = new HookGenerationError(hookMethod, "Target method was not specified");
      System.Type targetType = hookAtt.TargetType;
      if ( targetType == null)
        targetType = hookAtt.TargetType;
      second.TargetTypeDescriptor =  targetType;
      second.TargetMethod = hookAtt.TargetMethod;
      return Tuple.Create<Hook, HookGenerationError>((Hook) null, second);
    }
    MethodInfo method = type.GetMethod(hookAtt.TargetMethod, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
    if (method == (MethodInfo) null)
      return Tuple.Create<Hook, HookGenerationError>((Hook) null, new HookGenerationError(hookMethod, "Target method could not be found")
      {
        TargetTypeDescriptor =  type,
        TargetMethod = hookAtt.TargetMethod
      });
    Tuple<Hook, HookGenerationError> hook;
    try
    {
      hook = Tuple.Create<Hook, HookGenerationError>(new Hook((MethodBase) hookMethod, method, source), (HookGenerationError) null);
    }
    catch (Exception ex)
    {
      hook = Tuple.Create<Hook, HookGenerationError>((Hook) null, new HookGenerationError(hookMethod, $"Exception, {ex.GetType()}:{ex.Message}")
      {
        TargetMethod = method.ToString(),
        TargetTypeDescriptor =  type
      });
    }
    return hook;
  }
}
}
