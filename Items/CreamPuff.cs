using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Items.Archived;

namespace TheConfectionRebirth.Items
{
    public class CreamPuff : ModItem, IArchived
    {
        public override void SetStaticDefaults()
        {
            SacrificeTotal = 1;
        }

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
            return ItemID.LightShard;
		}
	}
}