// -----------------------------------------------------------------------------
// Library of Ruina mod script: AnimUpdater
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\AnimUpdater.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>AnimUpdater</summary>

    public class AnimUpdater : MonoBehaviour
    {
        public MysteryAnimatorDefault animator;

        public void SetAnim(MysteryAnimatorDefault anim, MysteryBase mysterybase)
        {
            this.animator = anim;
            this.animator.Init(mysterybase);
        }

        public void Update()
        {
            if (this.animator == null)
                return;
            this.animator.Update();
        }
    }
}
