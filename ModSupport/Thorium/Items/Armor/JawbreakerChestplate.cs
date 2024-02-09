using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Items;

namespace TheConfectionRebirth.ModSupport.Thorium.Items.Armor;

[ExtendsFromMod(TheConfectionRebirth.ThoriumModName)]
[AutoloadEquip(EquipType.Body)]
public sealed class JawbreakerChestplate : BardItem {
	public override bool IsLoadingEnabled(Mod mod) => TheConfectionRebirth.IsThoriumLoaded;

	public override void SetBardDefaults() {
		Item.width = 18;
		Item.height = 18;

		Item.defense = 14;

		Item.value = Item.sellPrice(gold: 2, silver: 40);
		Item.rare = ItemRarityID.LightRed;
	}

	public override void UpdateEquip(Player player) {
		// TODO: What effects
	}

	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient<Jawbreaker>(20)
			.AddTile(TileID.MythrilAnvil)
			.Register();
	}
}
