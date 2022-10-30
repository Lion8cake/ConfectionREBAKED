using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Archived
{
	public class CanofMeat : ModItem, IArchived
	{
		public override void SetDefaults()
		{
			Item.width = 10;
			Item.height = 12;
			Item.value = 4500;
			Item.rare = ItemRarityID.Green;
			Item.maxStack = 9999;
		}

		public int ArchivatesTo()
		{
			return ItemID.DarkShard;
		}
	}
}