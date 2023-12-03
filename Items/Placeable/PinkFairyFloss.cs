using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Placeable
{
    public class PinkFairyFloss : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;

			ItemTrader.ChlorophyteExtractinator.AddOption_OneWay(Type, 1, ItemID.Cloud, 1);
		}

        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 12;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<Tiles.PinkFairyFloss>();
        }

		public override void AddRecipes() {
			CreateRecipe(1).AddIngredient(ModContent.ItemType<PinkFairyFlossWall>(), 4).Register();
		}
	}
}
