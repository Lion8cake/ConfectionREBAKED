using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace TheConfectionRebirth.ModSupport
{
    internal abstract class ModSupportBaseClass
    {
        public virtual string ModName => "";

        void Hook()
        {
            if (ModLoader.TryGetMod(ModName, out Mod Mod))
                OnHook(Mod);
        }

        public abstract void OnHook(Mod Mod);

        public ModSupportBaseClass()
        {
            Hook();
        }

        public static void HookAll()
        {
            new CalamityModSupport();
            //new AvalonTestingSupport();
        }
    }

    /*internal class AvalonTestingSupport : ModSupportBaseClass
    {
        public override string ModName => "AvalonTesting";
        public override void OnHook(Mod Mod)
        {
        }
    }*/

    internal class CalamityModSupport : ModSupportBaseClass
    {
        public override string ModName => "CalamityMod";
        public override void OnHook(Mod Mod)
        {
        }
    }
}
