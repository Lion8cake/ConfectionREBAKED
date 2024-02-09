using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using TheConfectionRebirth.Items;

namespace TheConfectionRebirth.ModSupport.Thorium.Items;

public sealed class ConfectedCharm : ModItem {
	public override bool IsLoadingEnabled(Mod mod) => TheConfectionRebirth.IsThoriumLoaded;

	public override void SetStaticDefaults() {
		Item.ResearchUnlockCount = 25;
	}

	public override void SetDefaults() {
		Item.width = 20;
		Item.height = 20;

		Item.rare = ItemRarityID.LightRed;
		Item.value = Item.sellPrice(silver: 25);

		Item.maxStack = Item.CommonMaxStack;
	}

	public override void AddRecipes() {
		CreateRecipe(3)
			.AddIngredient<Sprinkles>(3)
			.AddIngredient<SoulofDelight>()
			.AddTile(TileID.Anvils)
			.Register();
	}
}
