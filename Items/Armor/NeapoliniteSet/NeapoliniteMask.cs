using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Terraria.Localization;

namespace TheConfectionRebirth.Items.Armor.NeapoliniteSet
{
	[AutoloadEquip(EquipType.Head)]
    public class NeapoliniteMask : ModItem
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
            Item.defense = 22;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<NeapoliniteConeMail>() && legs.type == ModContent.ItemType<NeapoliniteGreaves>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = Language.GetTextValue("Mods.TheConfectionRebirth.SetBonus.NeapoliniteMask");
			player.GetModPlayer<ConfectionPlayer>().neapoliniteMelee = true;
		}

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Melee) += 0.07f;
            player.GetAttackSpeed(DamageClass.Melee) += 0.051f;
        }
    }
}
