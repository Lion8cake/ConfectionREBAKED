using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Tiles.Trees
{
	public class CreamTreeBase : ModTree {
		public override TreePaintingSettings TreeShaderSettings => new TreePaintingSettings
        {
            UseSpecialGroups = true,
            SpecialGroupMinimalHueValue = 11f / 72f,
            SpecialGroupMaximumHueValue = 0.25f,
            SpecialGroupMinimumSaturationValue = 0.88f,
            SpecialGroupMaximumSaturationValue = 1f
        };

		public override int CreateDust() {
			return ModContent.DustType<Dusts.CreamwoodDust>();
		}

		public override void SetStaticDefaults()
        {
            GrowsOnTileId = [ModContent.TileType<CreamGrass>(), ModContent.TileType<CreamGrassMowed>(), ModContent.TileType<CreamTree>()]; //Creamtree is here so trees don't combust when converting into their custom tile type
		}

        public override bool Shake(int x, int y, ref bool createLeaves)
        {
            return false;
        }

        public override Asset<Texture2D> GetTexture()
        {
			return ModContent.Request<Texture2D>("TheConfectionRebirth/Tiles/Trees/CreamTree");
		}

        public override int SaplingGrowthType(ref int style)
        {
            style = 0;
            return ModContent.TileType<Trees.CreamSapling>();
        }

        public override void SetTreeFoliageSettings(Tile tile, ref int xoffset, ref int treeFrame, ref int floorY, ref int topTextureFrameWidth, ref int topTextureFrameHeight)
        {
        }

		public override int TreeLeaf() {
			return ModContent.GoreType<CreamTreeLeaf>();
		}

		public override Asset<Texture2D> GetBranchTextures()
        {
			return ModContent.Request<Texture2D>("TheConfectionRebirth/Tiles/Trees/CreamTree_Branches");
        }

        public override Asset<Texture2D> GetTopTextures()
        {
			return ModContent.Request<Texture2D>("TheConfectionRebirth/Tiles/Trees/CreamTree_Tops");
        }

        public override int DropWood()
        {
			return ModContent.ItemType<Items.Placeable.CreamWood>();
        }
	}
}
