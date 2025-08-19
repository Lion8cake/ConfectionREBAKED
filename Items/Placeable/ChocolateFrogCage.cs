using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace TheConfectionRebirth.Items.Placeable
{
    [LegacyName("ChocolateFrogCageItem")]
    internal class ChocolateFrogCage : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.FrogCage);
            Item.createTile = ModContent.TileType<Tiles.ChocolateFrogCage>();
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<Items.ChocolateFrog>(), 1).AddIngredient(ItemID.Terrarium, 1).Register();
        }
    }
}
