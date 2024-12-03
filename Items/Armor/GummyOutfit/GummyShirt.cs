using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Armor.GummyOutfit
{
    [AutoloadEquip(EquipType.Body)]
    public class GummyShirt : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 20;
            Item.vanity = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(silver: 40);
        }
    }
}