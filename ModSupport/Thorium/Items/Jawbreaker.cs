using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Items.Placeable;

namespace TheConfectionRebirth.ModSupport.Thorium.Items;

public sealed class Jawbreaker : ModItem {
	public override bool IsLoadingEnabled(Mod mod) => TheConfectionRebirth.IsThoriumLoaded;

	public override void SetStaticDefaults() {
		Item.ResearchUnlockCount = 25;
	}

	public override void SetDefaults() {
		Item.width = 20;
		Item.height = 20;

		Item.rare = ItemRarityID.LightRed;
		Item.value = Item.sellPrice(silver: 35);

		Item.maxStack = Item.CommonMaxStack;
	}

	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient<Saccharite>(2)
			.AddIngredient<Creamstone>(8)
			.AddTile(TileID.MythrilAnvil)
			.Register();
	}
}
