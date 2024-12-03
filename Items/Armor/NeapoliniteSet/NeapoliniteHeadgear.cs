using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Armor.NeapoliniteSet
{
    [AutoloadEquip(EquipType.Head)]
    public class NeapoliniteHeadgear : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = 10000;
            Item.rare = ItemRarityID.LightRed;
            Item.defense = 4;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<NeapoliniteConeMail>() && legs.type == ModContent.ItemType<NeapoliniteGreaves>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = Language.GetTextValue("Mods.TheConfectionRebirth.SetBonus.NeapoliniteHeadgear");
			player.GetModPlayer<ConfectionPlayer>().neapoliniteMage = true;
		}

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Magic) += 0.06f;
            player.statManaMax2 += 80;
        }
    }
}
