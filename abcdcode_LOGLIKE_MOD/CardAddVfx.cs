// Stub implementation - the real implementation exists in the Workshop DLL.
// Added here because the source file was missing from the repository.

using UnityEngine;

namespace abcdcode_LOGLIKE_MOD
{
    public static class CardAddVfx
    {
        public static void RunCardVfx(LogLikeMod.UILogCardSlot slot)
        {
            if (slot == null)
                return;
            var go = slot.gameObject;
            if (go != null)
            {
                go.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
            }
        }
    }
}
