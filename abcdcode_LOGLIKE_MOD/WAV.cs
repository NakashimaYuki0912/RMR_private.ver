// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.WAV
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System.IO;

 
namespace abcdcode_LOGLIKE_MOD {

public class WAV
{
  public static float bytesToFloat(byte firstByte, byte secondByte)
  {
    return (float) (short) ((int) secondByte << 8 | (int) firstByte) / 32768f;
  }

  public static int bytesToInt(byte[] bytes, int offset = 0)
  {
    int num = 0;
    for (int index = 0; index < 4; ++index)
      num |= (int) bytes[offset + index] << index * 8;
    return num;
  }

  public static byte[] GetBytes(string filename) => File.ReadAllBytes(filename);

  public float[] LeftChannel { get; internal set; }

  public float[] RightChannel { get; internal set; }

  public int ChannelCount { get; internal set; }

  public int SampleCount { get; internal set; }

  public int Frequency { get; internal set; }

  public WAV(string filename)
    : this(WAV.GetBytes(filename))
  {
  }

  public WAV(byte[] wav)
  {
    this.ChannelCount = (int) wav[22];
    this.Frequency = WAV.bytesToInt(wav, 24);
    int index1;
    int index2;
    int num;
    for (index1 = 12; wav[index1] != (byte) 100 || wav[index1 + 1] != (byte) 97 || wav[index1 + 2] != (byte) 116 || wav[index1 + 3] != (byte) 97; index1 = index2 + (4 + num))
    {
      index2 = index1 + 4;
      num = (int) wav[index2] + (int) wav[index2 + 1] * 256 /*0x0100*/ + (int) wav[index2 + 2] * 65536 /*0x010000*/ + (int) wav[index2 + 3] * 16777216 /*0x01000000*/;
    }
    int index3 = index1 + 8;
    this.SampleCount = (wav.Length - index3) / 2;
    if (this.ChannelCount == 2)
      this.SampleCount /= 2;
    this.LeftChannel = new float[this.SampleCount];
    this.RightChannel = this.ChannelCount != 2 ? (float[]) null : new float[this.SampleCount];
    int index4 = 0;
    while (index3 < wav.Length)
    {
      this.LeftChannel[index4] = WAV.bytesToFloat(wav[index3], wav[index3 + 1]);
      index3 += 2;
      if (this.ChannelCount == 2)
      {
        this.RightChannel[index4] = WAV.bytesToFloat(wav[index3], wav[index3 + 1]);
        index3 += 2;
      }
      ++index4;
    }
  }

  public override string ToString()
  {
    return $"[WAV: LeftChannel={this.LeftChannel}, RightChannel={this.RightChannel}, ChannelCount={this.ChannelCount}, SampleCount={this.SampleCount}, Frequency={this.Frequency}]";
  }
}
}
