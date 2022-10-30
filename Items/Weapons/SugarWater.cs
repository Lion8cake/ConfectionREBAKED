using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Items.Placeable;
using TheConfectionRebirth.Projectiles;

namespace TheConfectionRebirth.Items.Weapons
{
	public class SugarWater : ModItem
	{
		public override void SetStaticDefaults()
		{
			SacrificeTotal = 99;
		}
	
		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.shootSpeed = 14f;
			Item.rare = ItemRarityID.Orange;
			Item.damage = 20;
			Item.shoot = ModContent.ProjectileType<SugarWaterBottle>();
			Item.width = 18;
			Item.height = 20;
			Item.maxStack = 999;
			Item.consumable = true;
			Item.knockBack = 3f;
			Item.UseSound = SoundID.Item1;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.value = 200;
		}
		
		public override void AddRecipes() 
		{
			CreateRecipe(10)
				.AddIngredient<Sprinkles>(3)
				.AddIngredient<CreamBeans>()
				.AddIngredient(ItemID.BottledWater, 10)
				.Register();
		}
	}
}
