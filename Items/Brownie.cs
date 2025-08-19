using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items
{
	public class Brownie : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 5;

			Main.RegisterItemAnimation(Type, new DrawAnimationVertical(int.MaxValue, 3));

			ItemID.Sets.FoodParticleColors[Item.type] = new Color[3] {
				new Color(127, 69, 31),
				new Color(115, 64, 37),
				new Color(60, 29, 11)
			};

			ItemID.Sets.IsFood[Type] = true;
		}

		public override void SetDefaults()
		{
			Item.DefaultToFood(22, 22, BuffID.WellFed3, 57600);
			Item.SetShopValues(ItemRarityColor.LightRed4, Item.buyPrice(0, 3));
		}
	}
}