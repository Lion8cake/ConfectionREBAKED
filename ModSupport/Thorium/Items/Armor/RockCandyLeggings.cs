using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TheConfectionRebirth.Items.Placeable;
using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Items.Mechs;

namespace TheConfectionRebirth.ModSupport.Thorium.Items.Armor;

[ExtendsFromMod(TheConfectionRebirth.ThoriumModName)]
[AutoloadEquip(EquipType.Legs)]
public sealed class RockCandyLeggings : BardItem {
	public const float BardDamageIncrease = 5f;
	public const float MovementSpeedIncrease = 7f;

	public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(BardDamageIncrease, MovementSpeedIncrease);

	public override bool IsLoadingEnabled(Mod mod) => TheConfectionRebirth.IsThoriumLoaded;

	public override void SetBardDefaults() {
		Item.width = 18;
		Item.height = 18;

		Item.defense = 14;

		Item.value = Item.sellPrice(gold: 1);
		Item.rare = ItemRarityID.LightPurple;
	}

	public override void UpdateEquip(Player player) {
		player.GetDamage<BardDamage>() += BardDamageIncrease / 100f;
		player.moveSpeed = MovementSpeedIncrease / 100f;
	}

	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient<StrangePlating>(4)
			.AddIngredient<NeapoliniteBar>(8)
			.AddTile(TileID.MythrilAnvil)
			.Register();
	}
}
