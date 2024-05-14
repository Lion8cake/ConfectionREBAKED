using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;
using TheConfectionRebirth.Tiles;

namespace ExampleMod.Content.Tiles
{
    public class SprinklerCactusCactus : ModCactus
    {
        public override void SetStaticDefaults()
        {
            GrowsOnTileId = [ModContent.TileType<Creamsand>()];
        }

        public override Asset<Texture2D> GetTexture()
        {
            return ModContent.Request<Texture2D>("TheConfectionRebirth/Tiles/Trees/SprinkleCactus");
        }

        public override Asset<Texture2D> GetFruitTexture()
        {
            return ModContent.Request<Texture2D>("TheConfectionRebirth/Tiles/Trees/SprinkleCactusPear");
        }
    }
}
