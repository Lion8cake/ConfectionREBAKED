using TheConfectionRebirth.Items;
using TheConfectionRebirth.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Weapons
{
	public class CrunchBall : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crunch Ball");
			Tooltip.SetDefault("Launches a large Rock Candy ball.");
		}
	
		public override void SetDefaults()
		{
			Item.damage = 38;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 4;
			Item.width = 28;
			Item.height = 30;
			Item.useTime = 26;
			Item.useAnimation = 26;
			Item.useStyle = 5;
			Item.noMelee = true;
			Item.knockBack = 2f;
			Item.value = 500000;
			Item.rare = ItemRarityID.LightRed;
			Item.UseSound = SoundID.Item17;
			Item.autoReuse = false;
			Item.shoot = ModContent.ProjectileType<RockCandy>();
			Item.shootSpeed = 16f;
		}
	
		public override void AddRecipes() {
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Items.Placeable.Saccharite>(), 30);
			recipe.AddIngredient(ModContent.ItemType<Items.SoulofDelight>(), 15);
			recipe.AddIngredient(ItemID.SpellTome, 1);
			recipe.AddTile(TileID.Bookcases);
			recipe.Register();
		}
	}
}