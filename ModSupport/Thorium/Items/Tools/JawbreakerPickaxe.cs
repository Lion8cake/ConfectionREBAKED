using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace TheConfectionRebirth.ModSupport.Thorium.Items.Tools;

public sealed class JawbreakerPickaxe : ModItem {
	private sealed class JawbreakerPickaxeBuff : ModBuff {
		public override bool IsLoadingEnabled(Mod mod) => TheConfectionRebirth.IsThoriumLoaded;

		public const int DefenseBonus = 6;

		public override LocalizedText Description => base.Description.WithFormatArgs(DefenseBonus);

		public override void Update(Player player, ref int buffIndex) {
			player.statDefense += DefenseBonus;
		}
	}

	public override bool IsLoadingEnabled(Mod mod) => TheConfectionRebirth.IsThoriumLoaded;

	public override void Load() {
		On_Player.PickTile += static (On_Player.orig_PickTile orig, Player self, int x, int y, int pickPower) => {
			Handle(self, x, y);
			orig(self, x, y, pickPower);

			static void Handle(Player player, int x, int y) {
				if (Main.myPlayer != player.whoAmI || player.HeldItem.pick <= 0 || !WorldGen.InWorld(x, y)) {
					return;
				}

				var tile = Framing.GetTileSafely(x, y);
				if (tile.HasTile && TileID.Sets.Ore[tile.TileType] && player.GetModPlayer<ThoriumDLCPlayer>().JawbreakerSetEffects) {
					player.AddBuff(ModContent.BuffType<JawbreakerPickaxeBuff>(), 15 * 60, quiet: false);
				}
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
