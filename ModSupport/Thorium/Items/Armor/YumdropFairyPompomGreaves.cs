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
[AutoloadEquip(EquipType.Legs)]
public sealed class YumdropFairyPompomGreaves : ThoriumItem {
	public const float HealerDamageIncrease = 10f;
	public const float ManaCostReduction = 10f;
	public const float MovementSpeedIncrease = 6f;

	public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(HealerDamageIncrease, ManaCostReduction, MovementSpeedIncrease);

	public override bool IsLoadingEnabled(Mod mod) => TheConfectionRebirth.IsThoriumLoaded;

	public override void SetStaticDefaults() {
		isHealer = true;
	}

	public override void SetDefaults() {
		Item.width = 26;
		Item.height = 18;

		Item.defense = 12;

		Item.value = Item.sellPrice(gold: 1);
		Item.rare = ItemRarityID.Pink;
	}

	public override void UpdateEquip(Player player) {
		player.GetDamage<HealerDamage>() += HealerDamageIncrease / 100f;
		player.manaCost -= ManaCostReduction / 100f;
		player.moveSpeed += MovementSpeedIncrease / 100f;
	}

	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient<LifePoweredEnergyCell>(2)
			.AddIngredient<NeapoliniteBar>(12)
			.AddTile(TileID.MythrilAnvil)
			.Register();
	}
}