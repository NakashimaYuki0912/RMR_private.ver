// -----------------------------------------------------------------------------
// Library of Ruina mod script: CameraRender
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\CameraRender.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using System;
using UI;
using UnityEngine;

namespace abcdcode_LOGLIKE_MOD
{
    /// <summary>CameraRender</summary>
    public class CameraRender : MonoBehaviour
    {
        public bool Actived;
        public int index;
        public int skip = 0;

        private Camera _cam;
        private string _lastNameKey;
        private bool _lastNeedRender;
        private int _nameRecheckCooldown;

        public bool CheckingRender()
        {
            try
            {
                // Re-check name only every N frames — GetCharacterName allocates and walks book data.
                if (_nameRecheckCooldown-- > 0)
                    return _lastNeedRender;

                _nameRecheckCooldown = 20;
                var renderer = SingletonBehavior<UICharacterRenderer>.Instance;
                if (renderer == null || renderer.characterList == null
                    || this.index < 0 || this.index >= renderer.characterList.Count)
                {
                    _lastNeedRender = false;
                    return false;
                }

                var slot = renderer.characterList[this.index];
                var unit = slot != null ? slot.unitModel : null;
                var book = unit != null ? unit.CustomBookItem : null;
                if (book == null)
                {
                    _lastNeedRender = false;
                    return false;
                }

                string name = book.GetCharacterName();
                _lastNameKey = name;
                _lastNeedRender = !string.IsNullOrEmpty(name)
                    && LogLikeMod.spinedatas != null
                    && LogLikeMod.spinedatas.ContainsKey(name);
                return _lastNeedRender;
            }
            catch
            {
                _lastNeedRender = false;
                return false;
            }
        }

        public void Update()
        {
            try
            {
                --this.skip;
                if (this.skip >= 0)
                    return;

                // Not in RMR reception — almost never need forced spine portrait render.
                if (!LogLikeMod.CheckStage())
                {
                    this.skip = 45;
                    return;
                }

                if (!this.CheckingRender())
                {
                    this.skip = 20;
                    return;
                }

                if (_cam == null)
                    _cam = this.gameObject.GetComponent<Camera>();
                if (_cam == null || !_cam.isActiveAndEnabled)
                {
                    this.skip = 20;
                    return;
                }

                _cam.Render();
                // Was every ~3 frames; every ~7 frames is enough for portrait UI and much cheaper.
                this.skip = 6;
            }
            catch
            {
                this.skip = 30;
            }
        }
    }
}
