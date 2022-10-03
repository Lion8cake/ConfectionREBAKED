using TheConfectionRebirth.Projectiles;
using TheConfectionRebirth.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Weapons
{
	public class CreamofKickin : ModItem
	{
		public override void SetStaticDefaults() {
		    DisplayName.SetDefault("Cream of Kickin'");
			Tooltip.SetDefault("Apon using <right> a cookie will shoot instantly but with less damage."
			    + "\nApon using <left> a ball of meat will shoot with increased damage and a spin attack.");
		}

		public override void SetDefaults() {
			Item.width = 22;
			Item.height = 20;
			Item.value = Item.sellPrice(silver: 288);
			Item.rare = ItemRarityID.Pink;
			Item.noMelee = true;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 40;
			Item.useTime = 40;
			Item.knockBack = 4f;
			Item.damage = 45;
			Item.noUseGraphic = true;
			Item.shoot = ModContent.ProjectileType<CreamofKickinMeatball>();
			Item.shootSpeed = 15.1f;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Melee;
			Item.crit = 15;
			Item.channel = true;
		}
		
		public override void AddRecipes() 
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Items.CreamPuff>(), 1);
			recipe.AddIngredient(ModContent.ItemType<Items.CanofMeat>(), 1);
			recipe.AddIngredient(ModContent.ItemType<Items.SoulofDelight>(), 7);
			recipe.AddIngredient(ItemID.SoulofNight, 7);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
		
		public override bool AltFunctionUse(Player player) {
			return true;
		}

		public override bool CanUseItem(Player player) {
			if (player.altFunctionUse == 2) {
			Item.noMelee = true;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 40;
			Item.useTime = 40;
			Item.knockBack = 4f;
			Item.damage = 18;
			Item.noUseGraphic = true;
			Item.shoot = ModContent.ProjectileType<CreamofKickinCookie>();
			Item.shootSpeed = 15.1f;
		    Item.UseSound = SoundID.Item1;
		    Item.DamageType = DamageClass.Melee;
		    Item.crit = 15;
		    Item.channel = true;
			}
			else {
			Item.noMelee = true;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 40;
			Item.useTime = 40;
			Item.knockBack = 4f;
			Item.damage = 45;
			Item.noUseGraphic = true;
			Item.shoot = ModContent.ProjectileType<CreamofKickinMeatball>();
			Item.shootSpeed = 15.1f;
		    Item.UseSound = SoundID.Item1;
		    Item.DamageType = DamageClass.Melee;
		    Item.crit = 15;
		    Item.channel = true;
			}
			return base.CanUseItem(player);
		}
	}
}