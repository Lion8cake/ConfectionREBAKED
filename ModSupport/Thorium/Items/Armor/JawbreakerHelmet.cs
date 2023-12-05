using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using ThoriumMod.Items;

namespace TheConfectionRebirth.ModSupport.Thorium.Items.Armor;

[ExtendsFromMod(TheConfectionRebirth.ThoriumModName)]
[AutoloadEquip(EquipType.Head)]
public sealed class JawbreakerHelmet : BardItem {
	public static LocalizedText SetBonus { get; private set; }

	public override bool IsLoadingEnabled(Mod mod) => TheConfectionRebirth.IsThoriumLoaded;

	public override void Load() {
		SetBonus = this.GetLocalization("SetBonus");

		On_Player.PickTile += static (On_Player.orig_PickTile orig, Player self, int x, int y, int pickPower) => {
			if (self.TryGetModPlayer<ThoriumDLCPlayer>(out var dlcPlayer) && dlcPlayer.JawbreakerSetEffects) {
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

	public override void SetBardDefaults() {
		Item.width = 18;
		Item.height = 18;

		Item.defense = 11;

		Item.value = Item.sellPrice(gold: 1, silver: 20);
		Item.rare = ItemRarityID.LightRed;
	}

	public override bool IsArmorSet(Item head, Item body, Item legs) {
		return body.type == ModContent.ItemType<JawbreakerChestplate>() && legs.type == ModContent.ItemType<JawbreakerLeggings>();
	}

	public override void UpdateArmorSet(Player player) {
		var dlcPlayer = player.GetModPlayer<ThoriumDLCPlayer>();

		player.setBonus = SetBonus.Value;
		dlcPlayer.JawbreakerSetEffects = true;
	}

	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient<Jawbreaker>(3)
			.AddTile(TileID.MythrilAnvil)
			.Register();
	}
}
