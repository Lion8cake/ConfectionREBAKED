using System;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TheConfectionRebirth.Buffs.NeapoliniteBuffs;
using TheConfectionRebirth.Items.Placeable;
using TheConfectionRebirth.ModSupport.Thorium;

namespace TheConfectionRebirth.ModSupport.Thorium.Items.Armour
{
	[ExtendsFromMod("ThoriumMod")]
	[AutoloadEquip(EquipType.Head)]
    public class YumdropFairyTiara : ThoriumMod.Items.ThoriumItem
    {
		public Mod thorium;
		public override bool IsLoadingEnabled(Mod mod) {
			return ModLoader.TryGetMod("ThoriumMod", out thorium);
		}
		public override void SetStaticDefaults()
        {
			ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;

			isHealer = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 12;
            Item.value = 10000;
            Item.rare = ItemRarityID.Pink;
            Item.defense = 11;
		}

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<YumdropFairyBreastplate>() && legs.type == ModContent.ItemType<YumdropFairyPompomGreaves>();
        }

		public override void UpdateArmorSet(Player player)
        {
			player.setBonus = Language.GetTextValue("Mods.TheConfectionRebirth.SetBonus.YumdropFairyTiara");
			ThoriumConfectionPlayer playerFuncs = player.GetModPlayer<ThoriumConfectionPlayer>();
			playerFuncs.NeapoliniteHealerSet = true;
		}

		public override void AddRecipes() {
			if (ModLoader.TryGetMod("ThoriumMod", out Mod mod)) {
				CreateRecipe()
				.AddIngredient(mod.Find<ModItem>("LifeCell"), 1)
				.AddIngredient(ModContent.ItemType<NeapoliniteBar>(), 8)
				.AddTile(TileID.MythrilAnvil)
				.Register();
			}
		}

		public override void UpdateEquip(Player player)
        {
			if (ModLoader.TryGetMod("ThoriumMod", out Mod source) && source.TryFind("HealerDamage", out DamageClass damageClass)) {
				player.GetDamage(damageClass) += 0.1f;
				player.GetCritChance(damageClass) += 0.04f;
				player.manaCost -= 0.1f;
			}
        }
    }
}
