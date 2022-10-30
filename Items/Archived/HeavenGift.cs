using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Items.Weapons;
using Terraria.DataStructures;

namespace TheConfectionRebirth.Items.Archived
{
	public class HeavenGift : ModItem
	{
		public override void SetDefaults() {
			Item.width = 20;
			Item.height = 20;
			Item.value = Item.sellPrice(platinum: 1);
			Item.rare = ItemRarityID.Green;
		}
	}
}