using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Tiles.Trees
{
    public class SprinkleCactus : ModCactus
    {
		string Texture = "TheConfectionRebirth/Tiles/Trees/SprinkleCactus";

		public override void SetStaticDefaults()
        {
            GrowsOnTileId = [ModContent.TileType<Creamsand>()];
        }

        public override Asset<Texture2D> GetTexture()
        {
            return ModContent.Request<Texture2D>(Texture);
        }

        public override Asset<Texture2D> GetFruitTexture()
        {
            return ModContent.Request<Texture2D>(Texture + "_Pear");
        }
    }
}
