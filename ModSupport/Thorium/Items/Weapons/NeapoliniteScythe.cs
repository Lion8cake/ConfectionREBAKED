using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TheConfectionRebirth.Items.Placeable;
using TheConfectionRebirth.ModSupport.Thorium.Projectiles;
using ThoriumMod.Items.HealerItems;

namespace TheConfectionRebirth.ModSupport.Thorium.Items.Weapons;

[ExtendsFromMod(TheConfectionRebirth.ThoriumModName)]
public sealed class NeapoliniteScythe : ScytheItem {
	public const int SoulEssenceOnHit = 2;

	public override bool IsLoadingEnabled(Mod mod) => TheConfectionRebirth.IsThoriumLoaded;

	public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(SoulEssenceOnHit);

	public override void SetStaticDefaults() {
		SetStaticDefaultsToScythe();
	}

	public override void SetDefaults() {
		SetDefaultsToScythe();

		Item.width = 58;
		Item.height = 56;

		Item.damage = 40;
		Item.shoot = ModContent.ProjectileType<NeapoliniteScythePro>();
		scytheSoulCharge = SoulEssenceOnHit;

		Item.value = Item.sellPrice(gold: 5);
		Item.rare = ItemRarityID.Pink;
	}

	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient<NeapoliniteBar>(12)
			.AddTile(TileID.MythrilAnvil)
			.Register();
	}
}
