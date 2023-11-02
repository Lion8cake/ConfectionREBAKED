using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Placeable
{
    public class Creamstone : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
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
            Item.createTile = ModContent.TileType<Tiles.Creamstone>();
        }

		public override void AddRecipes()
		{
            CreateRecipe()
                .AddIngredient<ConfectionaryCrystalsWall>(4)
                .AddTile(TileID.WorkBenches)
                .Register();

            CreateRecipe()
                .AddIngredient<MeltingConfectionWall>(4)
                .AddTile(TileID.WorkBenches)
                .Register();

            CreateRecipe()
                .AddIngredient<CrackedConfectionWall>(4)
                .AddTile(TileID.WorkBenches)
                .Register();

            CreateRecipe()
                .AddIngredient<LinedConfectionGemWall>(4)
                .AddTile(TileID.WorkBenches)
                .Register();

			CreateRecipe()
				.AddIngredient<CreamstoneWall>(4)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
}
