using TheConfectionRebirth.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items
{
	public class BananawarpPeel : ModItem
	{
		public override void SetStaticDefaults()
		{
			SacrificeTotal = 99;
		}
	
		public override void SetDefaults()
		{
			Item.width = 14;
			Item.height = 20;
			Item.consumable = true;
			Item.useStyle = 1;
			Item.rare = ItemRarityID.Orange;
			Item.maxStack = 9999;
			Item.UseSound = SoundID.Item1;
			Item.useAnimation = 10;
			Item.useTime = 10;
			Item.value = 200;
			Item.noUseGraphic = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.BananawarpPeel>();
			Item.shootSpeed = 12f;
		}
	}
}