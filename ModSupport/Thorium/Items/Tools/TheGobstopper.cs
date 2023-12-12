using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using TheConfectionRebirth.ModSupport.Thorium.Projectiles;

namespace TheConfectionRebirth.ModSupport.Thorium.Items.Tools;

public sealed class TheGobstopper : ModItem {
	public override bool IsLoadingEnabled(Mod mod) => TheConfectionRebirth.IsThoriumLoaded;

	public override void SetDefaults() {
		Item.CloneDefaults(ItemID.ReinforcedFishingPole);
		Item.shoot = ModContent.ProjectileType<TheGobstopperPro>();
		Item.shootSpeed = 16f;

		Item.fishingPole = 40;

		Item.rare = ItemRarityID.LightRed;
		Item.value = Item.sellPrice(gold: 2);
	}

	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient<Jawbreaker>(10)
			.AddTile(TileID.MythrilAnvil)
			.Register();
	}
}
