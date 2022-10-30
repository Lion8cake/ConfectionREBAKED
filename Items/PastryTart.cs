using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items
{
	public class PastryTart : ModItem
	{
		public override void SetStaticDefaults() {
			SacrificeTotal = 5;

			Main.RegisterItemAnimation(Type, new DrawAnimationVertical(int.MaxValue, 3));

			ItemID.Sets.FoodParticleColors[Item.type] = new Color[3] {
				new Color(153, 98, 62),
				new Color(84, 28, 187),
				new Color(36, 26, 130)
			};

			ItemID.Sets.IsFood[Type] = true;
		}

		public override void SetDefaults() {
			Item.DefaultToFood(22, 22, BuffID.WellFed2, 72000);
			Item.value = Item.buyPrice(0, 3);
			Item.rare = ItemRarityID.Orange;
		}
	}
}