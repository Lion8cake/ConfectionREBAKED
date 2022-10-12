using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Archived
{
    public class KeyofSpite : ModItem, IArchived
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Key of Spite");
            Tooltip.SetDefault("'Charged with the essence of many souls'");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 12;
            Item.maxStack = 99;
        }

        public int ArchivatesTo() => ItemID.NightKey;
    }
}