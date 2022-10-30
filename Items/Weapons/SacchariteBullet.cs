using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Items.Placeable;

namespace TheConfectionRebirth.Items.Weapons
{
	public class SacchariteBullet : ModItem
	{
		public override void SetStaticDefaults() {
			SacrificeTotal = 99;
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
			CreateRecipe(25)
				.AddIngredient(ItemID.MusketBall, 25)
				.AddIngredient<Saccharite>()
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
