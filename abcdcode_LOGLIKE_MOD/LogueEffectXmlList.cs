// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.LogueEffectXmlList
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using HarmonyLib;
using Mod;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{

    public class LogueEffectXmlList : Singleton<LogueEffectXmlList>
    {
        public Dictionary<string, Dictionary<string, LogueEffectXmlInfo>> effectDict = new Dictionary<string, Dictionary<string, LogueEffectXmlInfo>>();

        public void Init(string language)
        {
            string path2 = Path.Combine("Localize", language);
            string path1 = Path.Combine(Singleton<ModContentManager>.Instance.GetModPath(LogLikeMod.ModId), "Assemblies", "dlls");
            if (Directory.Exists(Path.Combine(path1, path2, "LogueEffectText")))
            {
                foreach (FileInfo file in new DirectoryInfo(Path.Combine(path1, path2, "LogueEffectText")).GetFiles())
                {
                    try
                    {
                        foreach (LogueEffectXmlInfo logueEffect in (new XmlSerializer(typeof(LogueEffectXmlRoot)).Deserialize((Stream)file.OpenRead()) as LogueEffectXmlRoot).LogueEffectList)
                        {
                            if (!this.effectDict.ContainsKey(LogLikeMod.ModId))
                                this.effectDict.Add(LogLikeMod.ModId, new Dictionary<string, LogueEffectXmlInfo>());
                            if (!this.effectDict[LogLikeMod.ModId].ContainsKey(logueEffect.Id) && logueEffect != null)
                                this.effectDict[LogLikeMod.ModId].Add(logueEffect.Id, logueEffect);
                            else if (logueEffect != null)
                                this.effectDict[LogLikeMod.ModId][logueEffect.Id] = logueEffect;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.Log($"Error while trying to load XML file {file.Name}: {ex}");
                    }
                }
            }
            foreach (ModContentInfo logMod in LogLikeMod.GetLogMods())
            {
                string uniqueId = logMod.invInfo.workshopInfo.uniqueId;
                if (Directory.Exists(Path.Combine(logMod.GetLogDllPath(), path2, "LogueEffectText")))
                {
                    foreach (FileInfo file in new DirectoryInfo(Path.Combine(logMod.GetLogDllPath(), path2, "LogueEffectText")).GetFiles())
                    {
                        try
                        {
                            foreach (LogueEffectXmlInfo logueEffect in (new XmlSerializer(typeof(LogueEffectXmlRoot)).Deserialize((Stream)file.OpenRead()) as LogueEffectXmlRoot).LogueEffectList)
                            {
                                if (!this.effectDict.ContainsKey(uniqueId))
                                    this.effectDict.Add(uniqueId, new Dictionary<string, LogueEffectXmlInfo>());
                                if (!this.effectDict[uniqueId].ContainsKey(logueEffect.Id))
                                    this.effectDict[uniqueId].Add(logueEffect.Id, logueEffect);
                                else
                                    this.effectDict[uniqueId][logueEffect.Id] = logueEffect;
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.Log($"Error while trying to load XML file {file.Name}: {ex}");
                        }
                    }
                }
            }
        }

        public LogueEffectXmlInfo GetEffectInfo(string id, string packageId, params object[] args)
        {
            if (!this.effectDict.ContainsKey(packageId) || !this.effectDict[packageId].ContainsKey(id))
                return null;
            LogueEffectXmlInfo effectInfo = this.effectDict[packageId][id];
            if (args != null && args.Length != 0)
                effectInfo.Desc = string.Format(effectInfo.Desc, args);
            else
                effectInfo.Desc = effectInfo.Desc.Replace("{0}", "X");
            return effectInfo;
        }
    }
}
