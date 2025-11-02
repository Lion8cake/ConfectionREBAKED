using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.ModSupport.BiomeLava.Items;

namespace TheConfectionRebirth.ModSupport.BiomeLava.NPCs
{
	public class BiomeLavaShopEditor : GlobalNPC
	{
		public override bool IsLoadingEnabled(Mod mod)
		{
			return ConfectionModCalling.BiomeLava != null;
		}

		public override void ModifyShop(NPCShop shop)
		{
			if (shop.NpcType == NPCID.WitchDoctor)
			{
				shop.InsertAfter(ItemID.OasisFountain, ModContent.ItemType<ConfectionLavaFountain>());
			}
		}
	}
}
