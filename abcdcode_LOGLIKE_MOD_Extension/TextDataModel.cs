// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD_Extension.TextDataModel
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System;
using System.Collections.Generic;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD_Extension
{
    public static class TextDataModel
    {
      public const string ErrorText = "<color=#FF5544>ERROR</color>";
      public static Dictionary<string, string> _dic = new Dictionary<string, string>();
      public static string _currentLanguage = "kr";
      public static bool _isLoaded = false;
      public static string[] _supported = new string[5]
      {
        "kr",
        "en",
        "jp",
        "cn",
        "trcn"
      };
      public static bool _yame = false;

      public static string CurrentLanguage => TextDataModel._currentLanguage;

      public static Dictionary<string, string> textDic => TextDataModel._dic;

      public static void InitTextData(string currentLanguage)
      {
        TextDataModel._dic.Clear();
        TextDataModel._isLoaded = false;
        if (!TextDataModel._supported.Contains<string>(currentLanguage))
        {
          Debug.LogError( "not supported Language");
          currentLanguage = "en";
        }
        if (!TextDataModel._isLoaded)
        {
          Singleton<LocalizedTextLoader>.Instance.Load(currentLanguage, ref TextDataModel._dic);
          TextDataModel._isLoaded = true;
        }
        TextDataModel._currentLanguage = currentLanguage;
      }

      public static string GetText(string id, params object[] args)
      {
        if (!TextDataModel._isLoaded && !TextDataModel._yame)
        {
          TextDataModel._yame = true;
          Singleton<LocalizedTextLoader>.Instance.Load(TextDataModel._currentLanguage, ref TextDataModel._dic);
        }
        string format;
        if (!TextDataModel._dic.TryGetValue(id, out format))
          return string.Empty;
        format = format.Replace("\\n", "\n");
        if (format.Contains("[[") && format.Contains("]]"))
        {
          format = format.Replace("[[", "<sprite=");
          format = format.Replace("]]", ">");
        }
        string text;
        try
        {
          text = string.Format(format, args);
        }
        catch
        {
          text = format;
        }
        return text;
      }

      public static List<string> GetSupportedLangs()
      {
        List<string> supportedLangs = new List<string>();
        string[] supported = TextDataModel._supported;
        if (supported != null && supported.Length != 0)
        {
          for (int index = 0; index < TextDataModel._supported.Length; ++index)
            supportedLangs.Add(TextDataModel._supported[index]);
        }
        return supportedLangs;
      }
    }
}
