// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.FrameDummy
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System.IO;
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{

    public class FrameDummy : MonoBehaviour
    {
        public void Update()
        {
            Vector3 localPosition;
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                this.gameObject.transform.localPosition += new Vector3(-1f, 0.0f);
                string path = Application.persistentDataPath + "/ModConfigs/curposition.txt";
                localPosition = this.gameObject.transform.localPosition;
                string contents = localPosition.ToString();
                File.WriteAllText(path, contents);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                this.gameObject.transform.localPosition += new Vector3(1f, 0.0f);
                string path = Application.persistentDataPath + "/ModConfigs/curposition.txt";
                localPosition = this.gameObject.transform.localPosition;
                string contents = localPosition.ToString();
                File.WriteAllText(path, contents);
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                this.gameObject.transform.localPosition += new Vector3(0.0f, 1f);
                string path = Application.persistentDataPath + "/ModConfigs/curposition.txt";
                localPosition = this.gameObject.transform.localPosition;
                string contents = localPosition.ToString();
                File.WriteAllText(path, contents);
            }
            if (!Input.GetKey(KeyCode.DownArrow))
                return;
            this.gameObject.transform.localPosition += new Vector3(0.0f, -1f);
            string path1 = Application.persistentDataPath + "/ModConfigs/curposition.txt";
            localPosition = this.gameObject.transform.localPosition;
            string contents1 = localPosition.ToString();
            File.WriteAllText(path1, contents1);
        }
    }
}
