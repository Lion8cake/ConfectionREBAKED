using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Tiles.Trees
{
	public class SprinkleCactus : ModCactus
	{
		public override Texture2D GetTexture()
        {
            return ModContent.Request<Texture2D>("SprinkleCactus").Value;
        }
	}
}