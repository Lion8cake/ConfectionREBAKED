using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Items;

namespace TheConfectionRebirth.ModSupport.Thorium.Items.Armor;

[ExtendsFromMod(TheConfectionRebirth.ThoriumModName)]
[AutoloadEquip(EquipType.Legs)]
public sealed class JawbreakerLeggings : BardItem {
	public override bool IsLoadingEnabled(Mod mod) => TheConfectionRebirth.IsThoriumLoaded;

	public override void SetBardDefaults() {
		Item.width = 18;
		Item.height = 18;

		Item.defense = 12;

		Item.value = Item.sellPrice(gold: 1, silver: 80);
		Item.rare = ItemRarityID.LightRed;
	}

	public override void UpdateEquip(Player player) {
		// TODO: What effects
	}

	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient<Jawbreaker>(15)
			.AddTile(TileID.MythrilAnvil)
			.Register();
	}
}
