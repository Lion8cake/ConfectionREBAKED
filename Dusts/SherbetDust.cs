using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TheConfectionRebirth.Dusts
{
    public class SherbetDust : ModDust
    {
		public override void OnSpawn(Dust dust)
        {
            dust.velocity *= 0.4f;
            dust.noLight = true;
        }

		public override Color? GetAlpha(Dust dust, Color lightColor)
		{
			return TheConfectionRebirth.SherbertColor;
		}
	}
}