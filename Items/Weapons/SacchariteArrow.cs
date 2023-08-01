using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Items.Placeable;

namespace TheConfectionRebirth.Items.Weapons
{
	public class SacchariteArrow : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 99;
		}

		public override void SetDefaults() {
			Item.damage = 6;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 8;
			Item.height = 8;
			Item.maxStack = 9999;
			Item.consumable = true; 
			Item.knockBack = 1.5f;
			Item.value = 80;
			Item.rare = ItemRarityID.Orange;
			Item.shoot = ModContent.ProjectileType<Projectiles.SacchariteArrow>();
			Item.shootSpeed = 7f;
			Item.ammo = AmmoID.Arrow;
		}

		public override void AddRecipes() {
			CreateRecipe(25)
				.AddIngredient(ItemID.WoodenArrow, 25)
				.AddIngredient<Saccharite>()
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
