using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TheConfectionRebirth.Items.Archived;
using TheConfectionRebirth.Items.Weapons;

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

            Recipe recipe3 = Recipe.Create(ItemID.SoulofNight);
            recipe3.AddIngredient(ModContent.ItemType<SoulofSpite>())
            .Register();
            Recipe recipe4 = Recipe.Create(ItemID.NightsEdge);
            recipe4.AddIngredient(ModContent.ItemType<DeathsRaze>())
            .Register();
            Recipe recipe5 = Recipe.Create(ItemID.TrueNightsEdge);
            recipe5.AddIngredient(ModContent.ItemType<TrueDeathsRaze>())
            .Register();
            Recipe recipe6 = Recipe.Create(ItemID.NightKey);
            recipe6.AddIngredient(ModContent.ItemType<KeyofSpite>())
            .Register();
        }
    }
}