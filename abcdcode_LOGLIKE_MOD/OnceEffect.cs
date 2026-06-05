// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.OnceEffect
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using GameSave;


namespace abcdcode_LOGLIKE_MOD
{
    /// <summary>
    /// Base class for <b>consumable</b> inventory items- comes with a few shorthands for quick stack handling and usage.<br></br>
    /// Do mind the class name is saved to disk- please use unique class names to avoid conflicts.
    /// </summary>
    public class OnceEffect : GlobalLogueEffectBase
    {
        public int stack = 1;

        /// <summary>
        /// Runs when the item is added.<br></br>
        /// For consumables, this defaults to increasing the stack and updating the sprites.
        /// </summary>
        public override void AddedNew()
        {
            ++this.stack;
            Singleton<GlobalLogueEffectManager>.Instance.UpdateSprites();
        }

        /// <summary>
        /// Determines if the item can stack.<br></br>
        /// For consumables, this defaults to true.
        /// </summary>
        public override bool CanDupliacte() => true;

        /// <summary>
        /// Used for storing persistent information to save file.<br></br>
        /// It is recommended to start the method like so:
        /// <code>SaveData data = base.GetSaveData();</code><br></br>
        /// As base.GetSaveData contains the TypeName of the effect,<br></br>
        /// which is necessary for loading the effect from a save file.<br></br>
        /// <b>Consumables also store their respective number of stacks to disk.</b>
        /// </summary>
        /// <returns>The <see cref="SaveData"/> to be stored in disk.</returns>
        public override SaveData GetSaveData()
        {
            SaveData saveData = base.GetSaveData();
            saveData.AddData("stack", this.stack);
            return saveData;
        }

        /// <summary>
        /// Used for loading persistent information from save file.<br></br>
        /// For consumables, the base version also loads the stack from save data.
        /// </summary>
        /// <param name="save">The SaveData for this effect that is being loaded.</param>
        public override void LoadFromSaveData(SaveData save)
        {
            base.LoadFromSaveData(save);
            this.stack = save.GetInt("stack");
        }

        /// <summary>
        /// A small shorthand that automatically deducts 1 from the stack,<br></br>
        /// destroys the item if it has 0 or less stacks, and updates the sprites on the item list.
        /// </summary>
        public virtual void Use()
        {
            --this.stack;
            if (this.stack <= 0)
                this.Destroy();
            Singleton<GlobalLogueEffectManager>.Instance.UpdateSprites();
        }

        /// <summary>
        /// Returns the number to be shown on the item stack counter.<br></br>
        /// Defaults to <see cref="stack"/>.
        /// </summary>
        public override int GetStack() => this.stack;
    }
}
