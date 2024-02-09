using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.ModSupport.Thorium.Items.Tools;

public sealed class JawbreakerHamaxe : ModItem {
	public override bool IsLoadingEnabled(Mod mod) => TheConfectionRebirth.IsThoriumLoaded;

	public override void SetDefaults() {
		Item.width = 40;
		Item.height = 40;

		Item.axe = 20;
		Item.hammer = 85;
		Item.tileBoost = 1;

		Item.useTime = 7;
		Item.useAnimation = 35;
		Item.useStyle = ItemUseStyleID.Swing;
		Item.UseSound = SoundID.Item1;
		Item.attackSpeedOnlyAffectsWeaponAnimation = true;

		Item.DamageType = DamageClass.Melee;
		Item.damage = 38;
		Item.knockBack = 6.5f;
		Item.autoReuse = true;
		Item.useTurn = true;

		Item.value = Item.sellPrice(gold: 2);
		Item.rare = ItemRarityID.LightRed;
	}

	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient<Jawbreaker>(7)
			.AddTile(TileID.MythrilAnvil)
			.Register();
	}
}
