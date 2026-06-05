// Decompiled with JetBrains decompiler
// Type: CommonModApi.Tuple
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace CommonModApi

{ 
public static class Tuple
{
  public static Tuple<T1, T2> Create<T1, T2>(T1 item1, T2 second)
  {
    return new Tuple<T1, T2>(item1, second);
  }
}
}
