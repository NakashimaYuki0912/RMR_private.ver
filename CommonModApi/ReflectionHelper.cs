// Decompiled with JetBrains decompiler
// Type: CommonModApi.ReflectionHelper
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

 
namespace CommonModApi {

public static class ReflectionHelper
{
  public static Type[] ActionDelegateTypes = new Type[8]
  {
    typeof (Action),
    typeof (Action<>),
    typeof (Action<,>),
    typeof (Action<,,>),
    typeof (Action<,,,>),
    typeof (Action<,,,,>),
    typeof (Action<,,,,,>),
    typeof (Action<,,,,,,>)
  };
  public static Type[] FuncDelegateTypes = new Type[5]
  {
    typeof (Func<>),
    typeof (Func<,>),
    typeof (Func<,,>),
    typeof (Func<,,,>),
    typeof (Func<,,,,>)
  };

  public static Func<TTarget, TField> CreatePrivateFieldGetter<TTarget, TField>(string fieldName)
  {
    FieldInfo field = typeof (TTarget).GetField(fieldName, AccessTools.all);
    return !(field == (FieldInfo) null) ? (Func<TTarget, TField>) (t => (TField) field.GetValue( t)) : throw new ArgumentException($"Private instance field {fieldName} was not found on {typeof (TTarget)}");
  }

  public static Delegate CreateInstanceDelegate(object source, string name)
  {
    MethodInfo method = source.GetType().GetMethod(name, AccessTools.all);
    if (method == (MethodInfo) null)
      throw new ArgumentException($"Method '{name}' not found on {source.GetType()}");
    Type[] typeArray = !(method.ReturnType == typeof (void)) ? ReflectionHelper.FuncDelegateTypes : ReflectionHelper.ActionDelegateTypes;
    ParameterInfo[] parameters = method.GetParameters();
    if (parameters.Length >= typeArray.Length)
      throw new ArgumentException($"Too many parameters for delegate types:{parameters.Length}");
    Type type1 = typeArray[parameters.Length];
    Type type2;
    if (type1.IsGenericType)
    {
      List<Type> typeList = new List<Type>(((IEnumerable<ParameterInfo>) parameters).Select<ParameterInfo, Type>((Func<ParameterInfo, Type>) (p => p.ParameterType)));
      if (method.ReturnType == typeof (void))
      {
        type2 = type1.MakeGenericType(typeList.ToArray());
      }
      else
      {
        typeList.Add(method.ReturnType);
        type2 = type1.MakeGenericType(typeList.ToArray());
      }
    }
    else
      type2 = type1;
    return Delegate.CreateDelegate(type2, source, method);
  }
}
}
