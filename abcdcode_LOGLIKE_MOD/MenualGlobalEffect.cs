// -----------------------------------------------------------------------------
// Library of Ruina mod script: MenualGlobalEffect
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\MenualGlobalEffect.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>MenualGlobalEffect</summary>

    public class MenualGlobalEffect : GlobalLogueEffectBase
    {
        public static MenualGlobalEffect CurEffect;

        public override void OnClick()
        {
            base.OnClick();
            MenualGlobalEffect.CurEffect = this;
            Singleton<GlobalLogueEffectManager>.Instance.UpdateSprites();
        }
    }
}
