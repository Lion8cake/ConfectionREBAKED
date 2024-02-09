using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TheConfectionRebirth.Items.Placeable;
using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Items.HealerItems;

namespace TheConfectionRebirth.ModSupport.Thorium.Items.Armor;

[ExtendsFromMod(TheConfectionRebirth.ThoriumModName)]
[AutoloadEquip(EquipType.Head)]
public sealed class YumdropFairyTiara : ThoriumItem {
	public const float HealerDamageIncrease = 10f;
	public const int HealerCritChanceIncrease = 4;
	public const float ManaCostReduction = 10f;

	public static LocalizedText SetBonus { get; private set; }

	public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(HealerDamageIncrease, HealerCritChanceIncrease, ManaCostReduction);

	public override bool IsLoadingEnabled(Mod mod) => TheConfectionRebirth.IsThoriumLoaded;

	public override void Load() {
		SetBonus = this.GetLocalization("SetBonus");
	}

	public override void SetStaticDefaults() {
		ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;

		isHealer = true;
	}

	public override void SetDefaults() {
		Item.width = 34;
		Item.height = 12;

		Item.defense = 11;

		Item.value = Item.sellPrice(gold: 1);
		Item.rare = ItemRarityID.Pink;
	}

	public override bool IsArmorSet(Item head, Item body, Item legs) {
		return body.type == ModContent.ItemType<YumdropFairyBreastplate>() && legs.type == ModContent.ItemType<YumdropFairyPompomGreaves>();
	}

	public override void UpdateArmorSet(Player player) {
		player.setBonus = SetBonus.Value;
		player.GetModPlayer<ThoriumDLCPlayer>().NeapoliniteHealer = true;
	}

	public override void UpdateEquip(Player player) {
		player.GetDamage<HealerDamage>() += HealerDamageIncrease / 100f;
		player.GetCritChance<HealerDamage>() += HealerCritChanceIncrease;
		player.manaCost -= ManaCostReduction / 100f;
	}

	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient<LifePoweredEnergyCell>()
			.AddIngredient<NeapoliniteBar>(8)
			.AddTile(TileID.MythrilAnvil)
			.Register();
	}
}
