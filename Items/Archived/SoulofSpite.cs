using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Archived
{
	public class SoulofSpite : ModItem {
		public override void SetStaticDefaults() {
			Terraria.ID.ItemID.Sets.Deprecated[Type] = true;
		}

		public override void SetDefaults()
		{
			Item refItem = new();
			refItem.SetDefaults(ItemID.SoulofNight);
			Item.width = refItem.width;
			Item.height = refItem.height;
			Item.value = 1000;
			Item.rare = ItemRarityID.Orange;
			Item.maxStack = 999;
		}
	}
}