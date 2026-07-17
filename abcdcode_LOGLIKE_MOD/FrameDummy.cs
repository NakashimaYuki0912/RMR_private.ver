// -----------------------------------------------------------------------------
// Library of Ruina mod script: FrameDummy
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\FrameDummy.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using System.IO;
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>FrameDummy</summary>

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
