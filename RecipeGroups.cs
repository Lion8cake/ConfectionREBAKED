using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TheConfectionRebirth.Items.Weapons;
using AltLibrary.Content.Items;

namespace TheConfectionRebirth
{
    internal class RecipeGroups : ModSystem
    {
        public static RecipeGroup ConfectionRecipeGroup;

        public override void Unload()
        {
            ConfectionRecipeGroup = null;
        }

        public override void AddRecipeGroups()
        {
            ConfectionRecipeGroup = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.TerraBlade)}",
            ItemID.TerraBlade, ItemID.TerraBlade);

            RecipeGroup.RegisterGroup("ConfectionREBAKED:TerraBlade recipes", ConfectionRecipeGroup);
        }

        public override void AddRecipes()
        {
            Recipe baseRecipe = Recipe.Create(ItemID.TerraBlade);
            baseRecipe.AddIngredient(ModContent.ItemType<TrueDeathsRaze>())
                .AddIngredient(ModContent.ItemType<TrueSucrosa>())
                .AddIngredient(ItemID.BrokenHeroSword)
                .AddTile(TileID.MythrilAnvil)
                .Register();
            Recipe recipe = Recipe.Create(ItemID.TerraBlade);
                recipe.AddIngredient(ItemID.TrueNightsEdge)
                .AddIngredient(ModContent.ItemType<TrueSucrosa>())
                .AddIngredient(ItemID.BrokenHeroSword)
                .AddTile(TileID.MythrilAnvil)
                .Register();
            Recipe recipe2 = Recipe.Create(ItemID.TerraBlade);
                recipe2.AddIngredient(ItemID.TrueExcalibur)
                .AddIngredient(ModContent.ItemType<TrueDeathsRaze>())
                .AddIngredient(ItemID.BrokenHeroSword)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}