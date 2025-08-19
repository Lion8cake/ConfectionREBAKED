using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Accessories
{
    [AutoloadEquip(EquipType.Wings)]
    public class CookieWings : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(130, 6.75f);
		}

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 20;
            Item.value = 400000;
            Item.rare = ItemRarityID.Pink;
            Item.accessory = true;
        }
    }
}