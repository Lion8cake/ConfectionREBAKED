using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Armor.SnickerDevOutfit
{
    [AutoloadEquip(EquipType.Legs)]
    public class Unicookie : ModItem
    {
        public override void SetStaticDefaults()
        {
            ArmorIDs.Legs.Sets.OverridesLegs[Item.legSlot] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.vanity = true;
            Item.rare = ItemRarityID.Cyan;
        }
    }
}