// -----------------------------------------------------------------------------
// Card/key page upgrade: UpgradeMetadata
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\UpgradeMetadata.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using System;


namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>UpgradeMetadata</summary>

    public class UpgradeMetadata
    {
        public bool canStack = false;
        public int index;
        public int count = 1;
        public string actualPid = LogLikeMod.ModId;
        public string unparsedPid = "";

        public static UpgradeMetadata UnpackPidUnsafe(string packageId)
        {
            UpgradeMetadata.UnpackPid(packageId, out UpgradeMetadata data);
            return data;
        }

        public static bool UnpackPid(string packageId, out UpgradeMetadata metadata)
        {
            metadata = new UpgradeMetadata();
            if (!packageId.Contains(LogCardUpgradeManager.UpgradeKeyword))
                return false;
            string[] strArray1 = packageId.Split(new string[1]
            {
                LogCardUpgradeManager.UpgradeKeyword
            }, StringSplitOptions.None);
            string[] strArray2 = strArray1[0].Split(':');
            metadata = new UpgradeMetadata()
            {
                canStack = bool.Parse(strArray2[0]),
                index = int.Parse(strArray2[1]),
                count = int.Parse(strArray2[2]),
                actualPid = strArray1[1],
                unparsedPid = packageId
            };
            return true;
        }
    }
}
