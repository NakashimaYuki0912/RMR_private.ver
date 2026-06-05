// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.LogSoundEffectManager
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using NAudio.Wave;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{

    public class LogSoundEffectManager : Singleton<LogSoundEffectManager>
    {
        public List<string> PreList;
        public Dictionary<string, AudioClip> clips;

        public LogSoundEffectManager()
        {
            this.clips = new Dictionary<string, AudioClip>();
            this.PreList = new List<string>();
        }

        public IEnumerator PreloadAudioClip()
        {
            DirectoryInfo info = new DirectoryInfo(LogLikeMod.path + "/AudioClip");
            FileInfo[] fileInfoArray = info.GetFiles();
            for (int index = 0; index < fileInfoArray.Length; ++index)
            {
                FileInfo file = fileInfoArray[index];
                this.PreList.Add(file.FullName);
                file = (FileInfo)null;
            }
            fileInfoArray = (FileInfo[])null;
            yield return new WaitForEndOfFrame();
            List<string> list = new List<string>((IEnumerable<string>)this.PreList);
            foreach (string pa in list)
            {
                string name = Path.GetFileNameWithoutExtension(pa);
                this.LoadClipInPreList(name);
                yield return new WaitForEndOfFrame();
                name = (string)null;
            }
        }

        public static AudioClip mp3toAudioClip(string path)
        {
            Mp3FileReader sourceProvider = new Mp3FileReader(path);
            WaveFileWriter.CreateWaveFile(path + ".wav", (IWaveProvider)sourceProvider);
            AudioClip audioClip = LogSoundEffectManager.WavtoAudioClip(path + ".wav");
            File.Delete(path + ".wav");
            return audioClip;
        }

        public static AudioClip WavtoAudioClip(string path)
        {
            WAV wav = new WAV(File.ReadAllBytes(path));
            AudioClip audioClip = AudioClip.Create("Default", wav.SampleCount, 1, wav.Frequency, false);
            audioClip.SetData(wav.LeftChannel, 0);
            return audioClip;
        }

        public AudioClip LoadClipInPreList(string name)
        {
            string path = this.PreList.Find((Predicate<string>)(x => Path.GetFileNameWithoutExtension(x) == name));
            if (path == null)
                return (AudioClip)null;
            switch (Path.GetExtension(path))
            {
                case ".wav":
                    AudioClip audioClip1 = LogSoundEffectManager.WavtoAudioClip(path);
                    this.PreList.Remove(path);
                    this.clips[name] = audioClip1;
                    return audioClip1;
                case ".mp3":
                    AudioClip audioClip2 = LogSoundEffectManager.mp3toAudioClip(path);
                    this.PreList.Remove(path);
                    this.clips[name] = audioClip2;
                    return audioClip2;
                default:
                    return (AudioClip)null;
            }
        }

        public void AddClip(string name, AudioClip clip) => this.clips[name] = clip;

        public AudioClip GetAudioClip(string name)
        {
            return this.clips.ContainsKey(name) ? this.clips[name] : (AudioClip)null;
        }
    }
}
