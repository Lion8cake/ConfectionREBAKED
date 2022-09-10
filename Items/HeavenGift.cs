using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Items.Weapons;
using Terraria.DataStructures;

namespace TheConfectionRebirth.Items
{
	public class HeavenGift : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Heaven's Gift");
			Tooltip.SetDefault("An old Legacy item ment for the 1.3 version of the confection."
				+ "\nThank you to all who have supported the confection in the past, thank you just thanks!"
				+ "\nSell this to any npc to get a platinum coin.");
		}

		public override void SetDefaults() {
			Item.width = 20;
			Item.height = 20;
			Item.sellPrice(platinum: 1);
			Item.rare = ItemRarityID.Green;
		}
	}
}