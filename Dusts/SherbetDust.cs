using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TheConfectionRebirth.Dusts
{
    public class SherbetDust : ModDust
    {
		private int frame = 0;

		private int frameCounter = 0;

		public override void OnSpawn(Dust dust)
        {
            dust.velocity *= 0.4f;
            dust.noLight = true;
            dust.scale *= 1f;
        }

		public override bool Update(Dust dust) {
			frameCounter++;
			if (frameCounter > 5) {
				frameCounter = 0;
				frame++;
				if (frame > 12) {
					frame = 0;
				}
			}
			dust.frame = new Rectangle(10 * frame, 0, 10, 10);
			return true;
		}
	}
}