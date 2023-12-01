using ConfectionDLC.Thorium.Items.Armor;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TheConfectionRebirth.Items.Placeable;
using TheConfectionRebirth.ModSupport.Thorium.Buffs;
using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Items.Mechs;

namespace TheConfectionRebirth.ModSupport.Thorium.Items.Armor;

[ExtendsFromMod(TheConfectionRebirth.ThoriumModName)]
[AutoloadEquip(EquipType.Head)]
public sealed class RockCandyHeadset : BardItem {
	public const float BardDamageIncrease = 10f;
	public const int BardCritChanceIncrease = 4;

	public static LocalizedText SetBonus { get; private set; }

	public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(BardDamageIncrease, BardCritChanceIncrease);

	public override bool IsLoadingEnabled(Mod mod) => TheConfectionRebirth.IsThoriumLoaded;

	public override void Load() {
		SetBonus = this.GetLocalization("SetBonus");
	}

	public override void SetStaticDefaults() {
		ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
	}

	public override void SetBardDefaults() {
		Item.width = 18;
		Item.height = 18;

		Item.defense = 11;

		Item.value = Item.sellPrice(gold: 1);
		Item.rare = ItemRarityID.Pink;
	}

	public override bool IsArmorSet(Item head, Item body, Item legs) {
		return body.type == ModContent.ItemType<RockCandyConemail>() && legs.type == ModContent.ItemType<RockCandyLeggings>();
	}

	public override void UpdateArmorSet(Player player) {
		var dlcPlayer = player.GetModPlayer<ThoriumDLCPlayer>();

		player.setBonus = SetBonus.Value;
		dlcPlayer.NeapoliniteBard = true;

		int rank = Math.Min(player.GetModPlayer<RockCandyPlayer>().InspirationConsumed / 4, ModContent.GetInstance<RockinStar>().StageCount);
		if (rank > 0) {
			dlcPlayer.RockinStarStage = rank;
			player.AddBuff(ModContent.BuffType<RockinStar>(), 180);
		}
	}

	public override void UpdateEquip(Player player) {
		player.GetDamage<BardDamage>() += BardDamageIncrease / 100f;
		player.GetCritChance<BardDamage>() += BardCritChanceIncrease;
	}

	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient<StrangePlating>(3)
			.AddIngredient<NeapoliniteBar>(6)
			.AddTile(TileID.MythrilAnvil)
			.Register();
	}
}
