using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items
{
	public class CanofMeat : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 1;

			ItemTrader.ChlorophyteExtractinator.AddOption_OneWay(Type, 1, ItemID.DarkShard, 1);
		}

		public override void SetDefaults()
		{
			Item.width = 10;
			Item.height = 12;
			Item.value = 4500;
			Item.rare = ItemRarityID.Green;
			Item.maxStack = 9999;
		}
	}
}