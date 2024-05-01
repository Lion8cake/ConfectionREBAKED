/*using TheConfectionRebirth.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Weapons
{
	public class CherryGrenade : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cherry Bomb");
			Tooltip.SetDefault("After detonation cherry shards with come out and explode.");
			SacrificeTotal = 99;
		}
	
		public override void SetDefaults()
		{
			item.damage = 52;
			item.width = 14;
			item.height = 20;
			item.maxStack = 1;
			item.consumable = true;
			item.useStyle = 1;
			item.rare = ItemRarityID.Orange;
			item.maxStack = 9999;
			item.UseSound = SoundID.Item1;
			item.useAnimation = 44;
			item.useTime = 44;
			item.value = 200;
			item.noUseGraphic = true;
			item.melee = false;
			item.ranged = true;
			item.shoot = mod.ProjectileType("CherryGrenade");
			item.shootSpeed = 7.5f;
		}
		
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<CherryShells>(), 1);
			recipe.AddIngredient(ItemID.Grenade, 5);
			recipe.SetResult(this, 5);
			recipe.AddRecipe();
		}
	}
}*/