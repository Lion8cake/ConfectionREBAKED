using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Placeable
{
    public class SherbetCampfire : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 18;
            Item.maxStack = 999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.rare = ItemRarityID.White;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<Tiles.SherbetCampfire>();
        }

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddRecipeGroup(RecipeGroupID.Wood, 10).AddIngredient(null, "SherbetTorch", 5).Register();
        }
    }
}