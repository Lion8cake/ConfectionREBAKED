using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.ModSupport.Thorium.Items.Tools;

public sealed class JawbreakerPickaxe : ModItem {
	public override bool IsLoadingEnabled(Mod mod) => TheConfectionRebirth.IsThoriumLoaded;

	public override void Load() {
		On_Player.PickTile += static (On_Player.orig_PickTile orig, Player self, int x, int y, int pickPower) => {
			if (self.TryGetModPlayer<ThoriumDLCPlayer>(out var dlcPlayer) && dlcPlayer.JawbreakerPickEffects) {
				for (int i = -1; i <= 1; i++) {
					for (int j = -1; j <= 1; j++) {
						orig(self, x + i, y + j, pickPower);
					}
				}
			}
			else {
				orig(self, x, y, pickPower);
			}
		};
	}

	public override void SetDefaults() {
		Item.width = 40;
		Item.height = 40;

		Item.pick = 174;
		Item.tileBoost = 1;

		Item.useTime = 8;
		Item.useAnimation = 25;
		Item.useStyle = ItemUseStyleID.Swing;
		Item.UseSound = SoundID.Item1;
		Item.attackSpeedOnlyAffectsWeaponAnimation = true;

		Item.DamageType = DamageClass.Melee;
		Item.damage = 14;
		Item.knockBack = 5f;
		Item.autoReuse = true;
		Item.useTurn = true;

		Item.value = Item.sellPrice(gold: 2);
		Item.rare = ItemRarityID.LightRed;
	}

	public override void HoldItem(Player player) {
		player.GetModPlayer<ThoriumDLCPlayer>().JawbreakerPickEffects = true;
	}

	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient<Jawbreaker>(8)
			.AddTile(TileID.MythrilAnvil)
			.Register();
	}
}
