using TheConfectionRebirth.Mounts;
using TheConfectionRebirth.Tiles;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace TheConfectionRebirth.Items
{
	public class DriversPermint : ModItem
	{
		public override void SetStaticDefaults() {
		    DisplayName.SetDefault("Driver's Permint");
			Tooltip.SetDefault("Summons a Rollercycle to ride on");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.width = 20;
			Item.height = 30;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = 250000;
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.Item79;
			Item.noMelee = true;
			Item.mountType = ModContent.MountType<Rollercycle>();
		}
		
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(null, "KeyofDelight", 1).AddIngredient(null, "NeapoliniteBar", 30).AddIngredient(null, "Sprinkles", 80).AddIngredient(null, "CookieDough", 20).AddIngredient(null, "Saccharite", 100).AddIngredient(null, "SoulofDelight", 30).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}