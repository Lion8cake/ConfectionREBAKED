/*using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Weapons
{
	public class CherryBurstArrow : ModItem
	{
		public override void SetStaticDefaults() {
			Tooltip.SetDefault("3 cherry shards come out and explodes after impact.");
			SacrificeTotal = 99;
		}

		public override void SetDefaults() {
			item.damage = 12;
			item.ranged = true;
			item.width = 8;
			item.height = 8;
			item.maxStack = 9999;
			item.consumable = true; 
			item.knockBack = 1.5f;
			item.value = 80;
			item.rare = ItemRarityID.Orange;
			item.shoot = ModContent.ProjectileType<Projectiles.CherryBurstArrow>();
			item.shootSpeed = 7f;
			item.ammo = AmmoID.Arrow;
		}

		public override void AddRecipes() {
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(40, 25);
			recipe.AddIngredient(ModContent.ItemType<CherryShells>(), 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 25);
			recipe.AddRecipe();
		}
	}
}*/
