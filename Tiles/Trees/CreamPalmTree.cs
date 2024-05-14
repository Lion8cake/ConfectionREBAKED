using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent;
using Terraria.ModLoader;
using TheConfectionRebirth.Tiles.Trees;

namespace TheConfectionRebirth.Tiles.Trees
{
    public class CreamPalmTree : ModPalmTree
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
            GrowsOnTileId = [ModContent.TileType<Creamsand>()];
        }

        public override Asset<Texture2D> GetTexture()
        {
            return ModContent.Request<Texture2D>("TheConfectionRebirth/Tiles/Trees/CreamPalmTree");
        }

        public override int SaplingGrowthType(ref int style)
        {
            style = 1;
            return ModContent.TileType<CreamSapling>();
        }

        public override Asset<Texture2D> GetOasisTopTextures()
        {
            return ModContent.Request<Texture2D>("TheConfectionRebirth/Tiles/Trees/CreamPalmOasisTree_Tops");
        }

        public override Asset<Texture2D> GetTopTextures()
        {
            return ModContent.Request<Texture2D>("TheConfectionRebirth/Tiles/Trees/CreamPalmTree_Tops");
        }

		public override int TreeLeaf() {
			return ModContent.GoreType<CreamTreeLeaf>();
		}

		public override bool Shake(int x, int y, ref bool createLeaves) {
			return base.Shake(x, y, ref createLeaves);
		}

		public override int DropWood()
        {
			return 7;// ModContent.ItemType<Items.Placeable.CreamWood>();
        }
    }
}
