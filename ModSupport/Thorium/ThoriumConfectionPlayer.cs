using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.ModSupport.Thorium {
	public class ThoriumConfectionPlayer : ModPlayer
    {
        public bool NeapoliniteBardSet;
		public bool NeapoliniteHealerSet;

        public override void ResetEffects()
        {
            NeapoliniteBardSet = false;
			NeapoliniteHealerSet = false;
        }
    }
}