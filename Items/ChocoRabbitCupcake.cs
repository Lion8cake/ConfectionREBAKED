using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items
{
	public class ChocoRabbitCupcake : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 5;

			Main.RegisterItemAnimation(Type, new DrawAnimationVertical(int.MaxValue, 3));

			ItemID.Sets.FoodParticleColors[Item.type] = new Color[3] {
				new Color(153, 96, 62),
				new Color(242, 183, 236),
				new Color(230, 206, 148)
			};

			ItemID.Sets.IsFood[Type] = true;
		}

		public override void SetDefaults() {
			Item.DefaultToFood(24, 32, BuffID.WellFed2, 36000);
			Item.value = Item.buyPrice(0, 0, 7, 50);
			Item.rare = ItemRarityID.LightRed;
		}
	}
}