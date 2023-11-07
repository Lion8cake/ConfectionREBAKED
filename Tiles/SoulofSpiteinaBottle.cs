using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace TheConfectionRebirth.Tiles
{
    internal class SoulofSpiteinaBottle : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.Platform, TileObjectData.newTile.Width, 0);
			TileObjectData.addTile(Type);
            TileID.Sets.SwaysInWindBasic[Type] = true;

            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(152, 21, 37), name);
        }

        private readonly int animationFrameWidth = 18;

		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch) {
			bool intoRenderTargets = true;
			bool flag = intoRenderTargets || Main.LightingEveryFrame;

			if (Main.tile[i, j].TileFrameX % 18 == 0 && Main.tile[i, j].TileFrameY % 36 == 0 && flag) {
				Main.instance.TilesRenderer.AddSpecialPoint(i, j, 5);
			}

			return false;
		}

		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY) {
			if ((Framing.GetTileSafely(i, j - 1).HasTile && TileID.Sets.Platforms[Framing.GetTileSafely(i, j - 1).TileType]) ||
				(Framing.GetTileSafely(i, j - 2).HasTile && TileID.Sets.Platforms[Framing.GetTileSafely(i, j - 2).TileType]) ||
				(Framing.GetTileSafely(i, j - 3).HasTile && TileID.Sets.Platforms[Framing.GetTileSafely(i, j - 3).TileType])) {
				offsetY += 0;
			}
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 1.52f;
            g = 0.21f;
            b = 0.37f;
        }

        public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
        {
            if (i % 2 == 1)
                spriteEffects = SpriteEffects.FlipHorizontally;
        }

        public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
        {
            int uniqueAnimationFrame = Main.tileFrame[Type] + i;
            if (i % 2 == 0)
                uniqueAnimationFrame += 2;
            if (i % 3 == 0)
                uniqueAnimationFrame += 2;
            if (i % 4 == 0)
                uniqueAnimationFrame += 2;
            uniqueAnimationFrame %= 4;
            frameXOffset = uniqueAnimationFrame * animationFrameWidth;
        }

        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            frame = Main.tileFrame[TileID.SoulBottles];
        }
    }

    internal class SoulofSpiteinaBottleItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.SoulBottleNight);
			Item.placeStyle = 0;
            Item.createTile = ModContent.TileType<SoulofSpiteinaBottle>();
        }

		public override void AddRecipes() {
			CreateRecipe(1)
				.AddIngredient(ModContent.ItemType<Items.SoulofSpite>())
				.AddIngredient(ItemID.Bottle)
				.Register();
		}
	}
}
