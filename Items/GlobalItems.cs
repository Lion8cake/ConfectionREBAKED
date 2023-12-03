using Terraria.ModLoader;
using Terraria.ID;
using Terraria;

namespace TheConfectionRebirth.Items
{
    public class GlobalItems : GlobalItem
    {
        public override bool InstancePerEntity => true;

		public override void SetStaticDefaults()
		{
            ItemID.Sets.ShimmerTransformToItem[ItemID.ViciousPowder] = ModContent.ItemType<SugarPowder>();
        }

		public override bool CanUseItem(Item item, Player player) {
			if (player.GetModPlayer<ConfectionPlayer>().CandySuffocation) {
				return false;
			}
			return true;
		}
	}
}
