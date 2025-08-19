using TheConfectionRebirth.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Weapons
{
	public class CherryGrenade : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 99;
		}
	
		public override void SetDefaults()
		{
			Item.damage = 52;
			Item.width = 14;
			Item.height = 20;
			Item.maxStack = 1;
			Item.consumable = true;
			Item.useStyle = 1;
			Item.rare = ItemRarityID.Orange;
			Item.maxStack = 9999;
			Item.UseSound = SoundID.Item1;
			Item.useAnimation = 44;
			Item.useTime = 44;
			Item.value = 200;
			Item.noUseGraphic = true;
			Item.DamageType = DamageClass.Ranged;
			Item.shoot = ModContent.ProjectileType<Projectiles.CherryGrenade>();
			Item.shootSpeed = 7.5f;
		}
	}
}