// Decompiled with JetBrains decompiler
// Type: CommonModApi.Tuple`2
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace CommonModApi {

public sealed class Tuple<T1, T2>
{
  public T1 First;
  public T2 Second;

  public Tuple(T1 first, T2 second)
  {
    this.First = first;
    this.Second = second;
  }

  public override string ToString() => $"[{this.First}, {this.Second}]";

}
}