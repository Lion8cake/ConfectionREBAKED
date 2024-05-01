using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items
{
    public class KeyofSpite : ModItem {
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults() {
			Item.width = 10;
			Item.height = 12;
			Item.maxStack = 9999;
		}

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<SoulofSpite>(15)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
}