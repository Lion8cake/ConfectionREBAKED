using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Placeable
{
    public class CreamstoneBrick : ModItem
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
            Item.createTile = ModContent.TileType<Tiles.CreamstoneBrick>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Creamstone>(2)
                .AddIngredient<Creamsand>()
                .AddTile(TileID.Furnaces)
                .Register();
            CreateRecipe()
                .AddIngredient<CreamstoneBrickWall>(4)
                .Register();
        }
    }
}
