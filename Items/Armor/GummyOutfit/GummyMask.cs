using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Armor.GummyOutfit
{
    [AutoloadEquip(EquipType.Head)]
    public class GummyMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 20;
            Item.rare = ItemRarityID.Blue;
            Item.vanity = true;
            Item.value = Item.sellPrice(silver: 40);
        }
    }
}