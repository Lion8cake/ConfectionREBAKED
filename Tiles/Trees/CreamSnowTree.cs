using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using Terraria.ModLoader.Assets;
using TheConfectionRebirth;
using Terraria;

namespace TheConfectionRebirth.Tiles.Trees
{
    class CreamSnowTree : ModTree
    {
        private Mod mod
        {
            get
            {
                return ModLoader.GetMod("TheConfectionRebirth");
            }
        }

        public override int DropWood()
        {
            return ModContent.ItemType<Items.Placeable.CreamWood>();
        }

        public override Texture2D GetTexture()
        {
            return ModContent.Request<Texture2D>("CreamSnowTree").Value;
        }

        public override Texture2D GetBranchTextures(int i, int j, int trunkOffset, ref int frame)
        {
            return ModContent.Request<Texture2D>("CreamSnowTree_Branches").Value;
        }

        public override Texture2D GetTopTextures(int i, int j, ref int frame, ref int frameWidth, ref int frameHeight, ref int xOffsetLeft, ref int yOffset)
        {
            frameWidth = 80;
            frameHeight = 80;
            yOffset += 2;
            //xOffsetLeft += 16;
            return ModContent.Request<Texture2D>("CreamSnowTree_Tops").Value;
        }
    }
}
