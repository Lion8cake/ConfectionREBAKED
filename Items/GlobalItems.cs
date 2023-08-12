using Terraria.ModLoader;
using Terraria.ID;

namespace TheConfectionRebirth.Items
{
    public class GlobalItems : GlobalItem
    {
        public override bool InstancePerEntity => true;

		public override void SetStaticDefaults()
		{
            ItemID.Sets.ShimmerTransformToItem[ItemID.ViciousPowder] = ModContent.ItemType<SugarPowder>();
        }
	}
}
