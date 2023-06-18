using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.ModSupport.Thorium.Dusts
{
    public class NeapoliniteChocolateDust : ModDust
    {
		public Mod thorium;
		public override bool IsLoadingEnabled(Mod mod) {
			return ModLoader.TryGetMod("ThoriumMod", out thorium);
		}

		public override void OnSpawn(Dust dust)
        {
            dust.velocity *= 0.4f;
            dust.noLight = true;
            dust.scale *= 0.9f;
        }
    }
}