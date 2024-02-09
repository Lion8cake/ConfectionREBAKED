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
[AutoloadEquip(EquipType.Body)]
public sealed class RockCandyConemail : BardItem {
	public const int BardCritChanceIncrease = 5;

	public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(BardCritChanceIncrease);

	public override bool IsLoadingEnabled(Mod mod) => TheConfectionRebirth.IsThoriumLoaded;

	public override void SetBardDefaults() {
		Item.width = 18;
		Item.height = 18;

		Item.defense = 16;

		Item.value = Item.sellPrice(gold: 1);
		Item.rare = ItemRarityID.LightPurple;
	}

	public override void UpdateEquip(Player player) {
		player.GetCritChance<BardDamage>() += BardCritChanceIncrease;
	}

	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient<StrangePlating>(5)
			.AddIngredient<NeapoliniteBar>(10)
			.AddTile(TileID.MythrilAnvil)
			.Register();
	}
}
