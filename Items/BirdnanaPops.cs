using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items
{
	public class BirdnanaPops : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 5;

			Main.RegisterItemAnimation(Type, new DrawAnimationVertical(int.MaxValue, 3));

			ItemID.Sets.FoodParticleColors[Item.type] = new Color[3] {
				new Color(234, 224, 206),
				new Color(123, 78, 55),
				new Color(201, 149, 90)
			};

			ItemID.Sets.IsFood[Type] = true;
		}

		public override void SetDefaults() {
			Item.DefaultToFood(20, 30, BuffID.WellFed2, 36000);
			Item.value = Item.sellPrice(0, 0, 7, 50);
			Item.rare = ItemRarityID.LightRed;
		}

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<Birdnana>()
				.AddTile(TileID.CookingPots)
				.Register();
		}
	}
}