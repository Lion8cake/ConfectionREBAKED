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
			float r = (float)TheConfectionRebirth.SherbR / 255f;
			float g = (float)TheConfectionRebirth.SherbG / 255f;
			float b = (float)TheConfectionRebirth.SherbB / 255f;
			if (Main.rand.NextBool(player.itemAnimation > 0 ? 40 : 80))
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
				float r = (float)TheConfectionRebirth.SherbR / 255f;
				float g = (float)TheConfectionRebirth.SherbG / 255f;
				float b = (float)TheConfectionRebirth.SherbB / 255f;
				Lighting.AddLight((int)((Item.position.X + Item.width / 2) / 16f), (int)((Item.position.Y + Item.height / 2) / 16f), r, g, b);
            }
		}
    }
}