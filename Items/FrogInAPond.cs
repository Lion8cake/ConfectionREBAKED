using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items
{
	public class FrogInAPond : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 5;

			Main.RegisterItemAnimation(Type, new DrawAnimationVertical(int.MaxValue, 3));

			ItemID.Sets.FoodParticleColors[Item.type] = new Color[3] {
				new Color(191, 212, 52),
				new Color(144, 160, 38),
				new Color(123, 78, 55)
			};

			ItemID.Sets.IsFood[Type] = true;
		}

		public override void SetDefaults() {
			Item.DefaultToFood(18, 34, BuffID.WellFed2, 36000, true);
			Item.value = Item.sellPrice(0, 0, 12, 50);
			Item.rare = ItemRarityID.LightRed;
		}

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<ChocolateFrog>()
				.AddTile(TileID.CookingPots)
				.Register();
		}
	}
}