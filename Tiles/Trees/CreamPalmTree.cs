using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Tiles.Trees
{
    public class CreamPalmTree : ModPalmTree
    {
        public override int DropWood()
        {
            return ModContent.ItemType<Items.Placeable.CreamWood>();
        }

        public override Texture2D GetTexture()
        {
            return ModContent.Request<Texture2D>("CreamPalmTree").Value;
        }

        public override Texture2D GetTopTextures()
        {
            return ModContent.Request<Texture2D>("CreamPalmTree_Tops").Value;
        }
    }
}