using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Dusts
{
    public class WildAiryTintDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
			dust.noLight = false;
        }

		public override bool MidUpdate(Dust dust)
		{
			return false;
		}

		public override bool PreDraw(Dust dust)
		{
			Color color = dust.color;
			color.A = 0;
			Main.spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>(Texture), dust.position - Main.screenPosition, (Rectangle?)dust.frame, color, dust.GetVisualRotation(), new Vector2(4f, 4f), dust.scale, (SpriteEffects)0, 0f);
			return false;
		}
	}
}