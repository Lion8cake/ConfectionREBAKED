using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Placeable
{
    public class ChocolateFudge : ModItem
    {
        public override void SetStaticDefaults()
        {
            SacrificeTotal = 100;
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
            Item.createTile = ModContent.TileType<Tiles.ChocolateFudge>();
        }

        public override void AddRecipes()
        {
            CreateRecipe(30).AddIngredient(ModContent.ItemType<Items.Placeable.Creamsand>(), 30).AddIngredient(1134, 1).AddTile(TileID.WorkBenches).Register();
        }
    }
}
