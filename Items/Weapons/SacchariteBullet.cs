using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Weapons
{
	public class SacchariteBullet : ModItem
	{
		public override void SetStaticDefaults() {
			Tooltip.SetDefault("Shatters into pecies apon impact.");
		}

		public override void SetDefaults() {
			Item.damage = 4;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 8;
			Item.height = 8;
			Item.maxStack = 999;
			Item.consumable = true; 
			Item.knockBack = 1.5f;
			Item.value = 30;
			Item.rare = ItemRarityID.Orange;
			Item.shoot = ModContent.ProjectileType<Projectiles.SacchariteBullet>();
			Item.shootSpeed = 16f;
			Item.ammo = AmmoID.Bullet;
		}

		public override void AddRecipes() {
			Recipe recipe = CreateRecipe(25);
			recipe.AddIngredient(ItemID.MusketBall, 25);
			recipe.AddIngredient(ModContent.ItemType<Placeable.Saccharite>(), 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
