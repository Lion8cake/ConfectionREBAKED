using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Projectiles;

namespace TheConfectionRebirth.Items.Weapons
{
	public class PopSoda : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soda Potion");
			Tooltip.SetDefault("A fizzy beberage that contains a weird looking orange substance");
			SacrificeTotal = 99;
		}
	
		public override void SetDefaults()
		{
			Item.useStyle = 1;
			Item.shootSpeed = 14f;
			Item.rare = 3;
			Item.damage = 60;
			Item.DamageType = DamageClass.Ranged;
			Item.shoot = ModContent.ProjectileType<PopSodaProjectile>();
			Item.width = 18;
			Item.height = 20;
			Item.knockBack = 3f;
			Item.maxStack = 99;
			Item.UseSound = SoundID.Item1;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.value = 2000;
			Item.consumable = true;
		}
	}
}
