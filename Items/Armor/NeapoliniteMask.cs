using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Buffs.NeapoliniteBuffs;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class NeapoliniteMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("7% Increased Melee Damage"
                    + "\nIncreased Melee Speed");
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
            player.setBonus = "Every 80 damage delt will increase critical strick chance by 2% for 5 seconds, once 10% critical strick chance has been reached defence is ignored for 5 seconds";
            if (player.velocity.X >= 2.2f && !player.HasBuff(ModContent.BuffType<VanillaValorII>()) && !player.HasBuff(ModContent.BuffType<VanillaValorIII>()) && !player.HasBuff(ModContent.BuffType<VanillaValorIV>()) && !player.HasBuff(ModContent.BuffType<VanillaValorV>()) || -player.velocity.X >= 2.2f && !player.HasBuff(ModContent.BuffType<VanillaValorII>()) && !player.HasBuff(ModContent.BuffType<VanillaValorIII>()) && !player.HasBuff(ModContent.BuffType<VanillaValorIV>()) && !player.HasBuff(ModContent.BuffType<VanillaValorV>()))
            {
                player.AddBuff(ModContent.BuffType<VanillaValorI>(), 300);
            }
            if (player.velocity.X >= 4.4f && !player.HasBuff(ModContent.BuffType<VanillaValorIII>()) && !player.HasBuff(ModContent.BuffType<VanillaValorIV>()) && !player.HasBuff(ModContent.BuffType<VanillaValorV>()) || -player.velocity.X >= 4.4f && !player.HasBuff(ModContent.BuffType<VanillaValorIII>()) && !player.HasBuff(ModContent.BuffType<VanillaValorIV>()) && !player.HasBuff(ModContent.BuffType<VanillaValorV>()))
            {
                player.AddBuff(ModContent.BuffType<VanillaValorII>(), 300);
                player.ClearBuff(ModContent.BuffType<VanillaValorI>());
            }
            if (player.velocity.X >= 6.6f && !player.HasBuff(ModContent.BuffType<VanillaValorIV>()) && !player.HasBuff(ModContent.BuffType<VanillaValorV>()) || -player.velocity.X >= 6.6f && !player.HasBuff(ModContent.BuffType<VanillaValorV>()) && !player.HasBuff(ModContent.BuffType<VanillaValorIV>()))
            {
                player.AddBuff(ModContent.BuffType<VanillaValorIII>(), 300);
                player.ClearBuff(ModContent.BuffType<VanillaValorI>());
                player.ClearBuff(ModContent.BuffType<VanillaValorII>());
            }
            if (player.velocity.X >= 8.8f && !player.HasBuff(ModContent.BuffType<VanillaValorV>()) || -player.velocity.X >= 8.8f && !player.HasBuff(ModContent.BuffType<VanillaValorV>()))
            {
                player.AddBuff(ModContent.BuffType<VanillaValorIV>(), 300);
                player.ClearBuff(ModContent.BuffType<VanillaValorI>());
                player.ClearBuff(ModContent.BuffType<VanillaValorII>());
                player.ClearBuff(ModContent.BuffType<VanillaValorIII>());
            }
            if (player.velocity.X >= 11f || -player.velocity.X >= 11f)
            {
                player.AddBuff(ModContent.BuffType<VanillaValorV>(), 300);
                player.ClearBuff(ModContent.BuffType<VanillaValorI>());
                player.ClearBuff(ModContent.BuffType<VanillaValorII>());
                player.ClearBuff(ModContent.BuffType<VanillaValorIII>());
                player.ClearBuff(ModContent.BuffType<VanillaValorIV>());
            }
        }
    }
}