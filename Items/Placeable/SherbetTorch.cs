using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Items.Placeable
{
    public class SherbetTorch : ModItem
    {
		public override void SetStaticDefaults() {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
			ItemID.Sets.Torches[Type] = true;
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.ShimmerTorch;
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 13));
		}

		public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 12;
            Item.maxStack = 9999;
            Item.holdStyle = 1;
            Item.noWet = true;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.rare = ItemRarityID.Blue;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<Tiles.SherbetTorch>();
            Item.flame = true;
            Item.value = 500;
        }

        public override void HoldItem(Player player)
        {
			LightColor(out float r, out float g, out float b);
			if (Main.rand.Next(player.itemAnimation > 0 ? 40 : 80) == 0)
            {
                Dust.NewDust(new Vector2(player.itemLocation.X + 16f * player.direction, player.itemLocation.Y - 14f * player.gravDir), 4, 4, ModContent.DustType<SherbetDust>());
            }
            Vector2 position = player.RotatedRelativePoint(new Vector2(player.itemLocation.X + 12f * player.direction + player.velocity.X, player.itemLocation.Y - 14f + player.velocity.Y), true);
            Lighting.AddLight(position, r, g, b);
		}

        public override void PostUpdate()
        {
            if (!Item.wet)
            {
				LightColor(out float r, out float g, out float b);
				Lighting.AddLight((int)((Item.position.X + Item.width / 2) / 16f), (int)((Item.position.Y + Item.height / 2) / 16f), r, g, b);
            }
		}

        public override void AddRecipes()
        {
            CreateRecipe(3).AddIngredient(ItemID.Torch, 3).AddIngredient(ModContent.ItemType<SherbetBricks>()).Register();
        }

		private void LightColor(out float red, out float green, out float blue) {
			red = 0;
			green = 0;
			blue = 0;
			Main.GetItemDrawFrame(Item.type, out Texture2D itemtexturre, out Rectangle itemFrame);
			switch (itemFrame.Y / itemFrame.Height) {
				case 0:
					red = 1.92f;
					green = 0.26f;
					blue = 0.26f;
					break;
				case 1:
					red = 1.93f;
					green = 0.68f;
					blue = 0.26f;
					break;
				case 2:
					red = 2.53f;
					green = 0.91f;
					blue = 0.03f;
					break;
				case 3:
					red = 2.52f;
					green = 1.58f;
					blue = 0.03f;
					break;
				case 4:
					red = 1.99f;
					green = 1.67f;
					blue = 0.15f;
					break;
				case 5:
					red = 1.04f;
					green = 1.57f;
					blue = 0.15f;
					break;
				case 6:
					red = 0.23f;
					green = 1.07f;
					blue = 0.29f;
					break;
				case 7:
					red = 0.23f;
					green = 1.06f;
					blue = 1.06f;
					break;
				case 8:
					red = 0.29f;
					green = 0.8f;
					blue = 1.31f;
					break;
				case 9:
					red = 0.37f;
					green = 0.57f;
					blue = 2.27f;
					break;
				case 10:
					red = 0.66f;
					green = 0.37f;
					blue = 2.26f;
					break;
				case 11:
					red = 1.01f;
					green = 0.36f;
					blue = 1.62f;
					break;
				case 12:
					red = 1.62f;
					green = 0.36f;
					blue = 1.58f;
					break;
			}
		}
    }
}