using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Buffs.NeapoliniteBuffs;

namespace TheConfectionRebirth.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class NeapoliniteHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("10% Increased Ranged Damage"
                    + "\n4% Increased Ranged Critical Strike Chance");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = 10000;
            Item.rare = ItemRarityID.LightRed;
            Item.defense = 6;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<NeapoliniteConeMail>() && legs.type == ModContent.ItemType<NeapoliniteGreaves>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Every 10mph you go damage is increased by 4% and critical strike chance is increased by 2% which will last for 5 seconds. This stacks up to 5 times";
            if (player.velocity.X >= 2.2f && !player.HasBuff(ModContent.BuffType<ChocolateChargeII>()) && !player.HasBuff(ModContent.BuffType<ChocolateChargeIII>()) && !player.HasBuff(ModContent.BuffType<ChocolateChargeIV>()) && !player.HasBuff(ModContent.BuffType<ChocolateChargeV>()) || -player.velocity.X >= 2.2f && !player.HasBuff(ModContent.BuffType<ChocolateChargeII>()) && !player.HasBuff(ModContent.BuffType<ChocolateChargeIII>()) && !player.HasBuff(ModContent.BuffType<ChocolateChargeIV>()) && !player.HasBuff(ModContent.BuffType<ChocolateChargeV>()))
            {
                player.AddBuff(ModContent.BuffType<ChocolateChargeI>(), 300);
            }
            if (player.velocity.X >= 4.4f && !player.HasBuff(ModContent.BuffType<ChocolateChargeIII>()) && !player.HasBuff(ModContent.BuffType<ChocolateChargeIV>()) && !player.HasBuff(ModContent.BuffType<ChocolateChargeV>()) || -player.velocity.X >= 4.4f && !player.HasBuff(ModContent.BuffType<ChocolateChargeIII>()) && !player.HasBuff(ModContent.BuffType<ChocolateChargeIV>()) && !player.HasBuff(ModContent.BuffType<ChocolateChargeV>()))
            {
                player.AddBuff(ModContent.BuffType<ChocolateChargeII>(), 300);
                player.ClearBuff(ModContent.BuffType<ChocolateChargeI>());
            }
            if (player.velocity.X >= 6.6f && !player.HasBuff(ModContent.BuffType<ChocolateChargeIV>()) && !player.HasBuff(ModContent.BuffType<ChocolateChargeV>()) || -player.velocity.X >= 6.6f && !player.HasBuff(ModContent.BuffType<ChocolateChargeV>()) && !player.HasBuff(ModContent.BuffType<ChocolateChargeIV>()))
            {
                player.AddBuff(ModContent.BuffType<ChocolateChargeIII>(), 300);
                player.ClearBuff(ModContent.BuffType<ChocolateChargeI>());
                player.ClearBuff(ModContent.BuffType<ChocolateChargeII>());
            }
            if (player.velocity.X >= 8.8f && !player.HasBuff(ModContent.BuffType<ChocolateChargeV>()) || -player.velocity.X >= 8.8f && !player.HasBuff(ModContent.BuffType<ChocolateChargeV > ()))
            {
                player.AddBuff(ModContent.BuffType<ChocolateChargeIV>(), 300);
                player.ClearBuff(ModContent.BuffType<ChocolateChargeI>());
                player.ClearBuff(ModContent.BuffType<ChocolateChargeII>());
                player.ClearBuff(ModContent.BuffType<ChocolateChargeIII>());
            }
            if (player.velocity.X >= 11f || -player.velocity.X >= 11f)
            {
                player.AddBuff(ModContent.BuffType<ChocolateChargeV>(), 300);
                player.ClearBuff(ModContent.BuffType<ChocolateChargeI>());
                player.ClearBuff(ModContent.BuffType<ChocolateChargeII>());
                player.ClearBuff(ModContent.BuffType<ChocolateChargeIII>());
                player.ClearBuff(ModContent.BuffType<ChocolateChargeIV>());
            }
        }
    }
}