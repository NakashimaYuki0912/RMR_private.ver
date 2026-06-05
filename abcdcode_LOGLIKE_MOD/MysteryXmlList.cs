// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.MysteryXmlList
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System;
using System.Collections.Generic;
using System.Linq;

 
namespace abcdcode_LOGLIKE_MOD {

public class MysteryXmlList : Singleton<MysteryXmlList>
{
  public List<MysteryXmlInfo> info;
  private MysteryXmlInfo[] _allInfos;

  public void Init(List<MysteryXmlInfo> info)
  {
    this.info = info;
    this._allInfos = new MysteryXmlInfo[info.Count];
    info.CopyTo(this._allInfos);
  }

  public MysteryXmlInfo GetData(LorId stageid)
  {
    return this.info.Find((Predicate<MysteryXmlInfo>) (x => x.StageId == stageid));
  }

  public void RestoreToDefault()
  {
    this.info.Clear();
    this.info.AddRange((IEnumerable<MysteryXmlInfo>) ((IEnumerable<MysteryXmlInfo>) this._allInfos).ToList<MysteryXmlInfo>());
  }
}
}
