using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Armor.GummyOutfit
{
    [AutoloadEquip(EquipType.Legs)]
    public class GummyPants : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ArmorIDs.Legs.Sets.HidesBottomSkin[Item.legSlot] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.vanity = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(silver: 40);
        }
    }
}