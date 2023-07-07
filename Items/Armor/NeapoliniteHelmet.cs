using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TheConfectionRebirth.Buffs.NeapoliniteBuffs;

namespace TheConfectionRebirth.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class NeapoliniteHelmet : ModItem
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
            Item.defense = 6;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<NeapoliniteConeMail>() && legs.type == ModContent.ItemType<NeapoliniteGreaves>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = Language.GetTextValue("Mods.TheConfectionRebirth.SetBonus.NeapoliniteHelmet");
            int rank;
            float len = player.velocity.X;
            if (len >= 11f)
                rank = 4;
            else
            {
                float speed = len / 2.2f;
                rank = (int)(speed - 1);
            }
            StackableBuffData.ChocolateCharge.AscendBuff(player, rank, 300);
			player.GetModPlayer<ConfectionPlayer>().neapoliniteArmorSetType = 2;
		}

		public override void UpdateVanitySet(Player player) {
			player.GetModPlayer<ConfectionPlayer>().neapoliniteArmorSetType = 2;
		}

		public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<Items.Placeable.NeapoliniteBar>(), 12).AddTile(TileID.MythrilAnvil).Register();
        }

        public override void UpdateEquip(Player player)
        {
            player.GetCritChance(DamageClass.Ranged) += 0.04f;
            player.GetDamage(DamageClass.Ranged) += 0.1f;
        }
    }
}
