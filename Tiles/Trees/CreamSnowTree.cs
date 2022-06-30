using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Tiles
{
    public class CreamSnowTree : ModTree
    {
        public override TreePaintingSettings TreeShaderSettings => new TreePaintingSettings
        {
            UseSpecialGroups = true,
            SpecialGroupMinimalHueValue = 11f / 72f,
            SpecialGroupMaximumHueValue = 0.25f,
            SpecialGroupMinimumSaturationValue = 0.88f,
            SpecialGroupMaximumSaturationValue = 1f
        };

        public override void SetStaticDefaults()
        {
            GrowsOnTileId = new int[1] { ModContent.TileType<CreamBlock>() };
        }

        public override Asset<Texture2D> GetTexture()
        {
            return ModContent.Request<Texture2D>("TheConfectionRebirth/Tiles/Trees/CreamSnowTree");
        }

        /*public override int SaplingGrowthType(ref int style) {
			style = 0;
			return ModContent.TileType<Tree.CreamSnowSapling>();
		}*/

        public override Asset<Texture2D> GetBranchTextures()
        {
            return ModContent.Request<Texture2D>("TheConfectionRebirth/Tiles/Trees/CreamSnowTree_Branches");
        }

        public override Asset<Texture2D> GetTopTextures()
        {
            return ModContent.Request<Texture2D>("TheConfectionRebirth/Tiles/Trees/CreamSnowTree_Tops");
        }

        public override int DropWood()
        {
            return ModContent.ItemType<Items.Placeable.CreamWood>();
        }

        public override void SetTreeFoliageSettings(Tile tile, ref int xoffset, ref int treeFrame, ref int floorY, ref int topTextureFrameWidth, ref int topTextureFrameHeight)
        {
        }
    }
}