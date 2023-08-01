using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Items.Placeable
{
    public class ConfectionTorch : ModItem
    {

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
            Item.createTile = ModContent.TileType<Tiles.ConfectionTorch>();
            Item.flame = true;
            Item.value = 500;
        }

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
			ItemID.Sets.Torches[Type] = true;
		}

        public override void HoldItem(Player player)
        {
            if (Main.rand.Next(player.itemAnimation > 0 ? 40 : 80) == 0)
            {
                Dust.NewDust(new Vector2(player.itemLocation.X + 16f * player.direction, player.itemLocation.Y - 14f * player.gravDir), 4, 4, ModContent.DustType<SherbetDust>());
            }
            Vector2 position = player.RotatedRelativePoint(new Vector2(player.itemLocation.X + 12f * player.direction + player.velocity.X, player.itemLocation.Y - 14f + player.velocity.Y), true);
            Lighting.AddLight(position, 0.27f, 1.34f, 1.69f);
        }

        public override void PostUpdate()
        {
            if (!Item.wet)
            {
                Lighting.AddLight((int)((Item.position.X + Item.width / 2) / 16f), (int)((Item.position.Y + Item.height / 2) / 16f), 0.27f, 1.34f, 1.69f);
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe(3).AddIngredient(ItemID.Torch, 3).AddIngredient(ModContent.ItemType<Creamstone>()).Register();
            CreateRecipe(3).AddIngredient(ItemID.Torch, 3).AddIngredient(ModContent.ItemType<HardenedCreamsand>()).Register();
            CreateRecipe(3).AddIngredient(ItemID.Torch, 3).AddIngredient(ModContent.ItemType<OrangeIce>()).Register();
        }
    }
}