using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class CreamwoodHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = 0;
            Item.rare = ItemRarityID.White;
            Item.defense = 2;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<CreamwoodBreastplate>() && legs.type == ModContent.ItemType<CreamwoodGreaves>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = Language.GetTextValue("Mods.TheConfectionRebirth.SetBonus.CreamwoodHelmet");
            player.statDefense += 1;
        }
    }
}