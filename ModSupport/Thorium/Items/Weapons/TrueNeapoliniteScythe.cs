using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TheConfectionRebirth.ModSupport.Thorium.Projectiles;
using ThoriumMod.Items.HealerItems;
using ThoriumMod.Items.Misc;

namespace TheConfectionRebirth.ModSupport.Thorium.Items.Weapons;

[ExtendsFromMod(TheConfectionRebirth.ThoriumModName)]
public sealed class TrueNeapoliniteScythe : ScytheItem {
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

		Item.damage = 49;
		Item.shoot = ModContent.ProjectileType<TrueNeapoliniteScythePro>();
		scytheSoulCharge = SoulEssenceOnHit;

		Item.value = Item.sellPrice(gold: 11);
		Item.rare = ItemRarityID.Yellow;
	}

	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient<NeapoliniteScythe>()
			.AddIngredient<BrokenHeroFragment>(2)
			.AddTile(TileID.MythrilAnvil)
			.Register();
	}
}
