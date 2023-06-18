using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Archived
{
    public class KeyofSpite : ModItem {
		public override void SetStaticDefaults() {
			Terraria.ID.ItemID.Sets.Deprecated[Type] = true;
		}

		public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 12;
            Item.maxStack = 99;
        }
    }
}