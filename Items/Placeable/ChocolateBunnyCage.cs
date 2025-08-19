using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace TheConfectionRebirth.Items.Placeable
{
	[LegacyName("ChocolateBunnyCageItem")]
	internal class ChocolateBunnyCage : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.BunnyCage);
            Item.createTile = ModContent.TileType<Tiles.ChocolateBunnyCage>();
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<Items.ChocolateBunny>(), 1).AddIngredient(ItemID.Terrarium, 1).Register();
        }
    }
}
