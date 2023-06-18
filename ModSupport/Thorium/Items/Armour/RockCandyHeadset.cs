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
    public class RockCandyHeadset : ThoriumMod.Items.BardItem
    {
		public Mod thorium;
		public override bool IsLoadingEnabled(Mod mod) {
			return ModLoader.TryGetMod("ThoriumMod", out thorium);
		}
		public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

			object result = thorium.Call("IsBardItem", Item);
			if (result is ValueTuple<bool> tuple) {
				tuple.Item1.ToInt();
			}
		}

        public override void SetBardDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = 10000;
            Item.rare = ItemRarityID.LightPurple;
            Item.defense = 11;
		}

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<RockCandyConemail>() && legs.type == ModContent.ItemType<RockCandyLeggings>();
        }

		public override void UpdateArmorSet(Player player)
        {
			player.setBonus = Language.GetTextValue("Mods.TheConfectionRebirth.SetBonus.RockCandyHeadset");
			ThoriumConfectionPlayer playerFuncs = player.GetModPlayer<ThoriumConfectionPlayer>();
			playerFuncs.NeapoliniteBardSet = true;
		}

		public override void AddRecipes() {
			if (ModLoader.TryGetMod("ThoriumMod", out Mod mod)) {
				CreateRecipe()
				.AddIngredient(mod.Find<ModItem>("StrangePlating"), 3)
				.AddIngredient(ModContent.ItemType<NeapoliniteBar>(), 6)
				.AddTile(TileID.MythrilAnvil)
				.Register();
			}
		}

		public override void UpdateEquip(Player player)
        {
			if (ModLoader.TryGetMod("ThoriumMod", out Mod source) && source.TryFind("BardDamage", out DamageClass damageClass)) {
				player.GetDamage(damageClass) += 0.1f;
				player.GetCritChance(damageClass) += 0.04f;
			}
        }
    }
}
