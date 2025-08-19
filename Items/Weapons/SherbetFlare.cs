using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Items.Placeable;

namespace TheConfectionRebirth.Items.Weapons
{
	public class SherbetFlare : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 99;
		}

		public override void SetDefaults() {
			Item.damage = 1;
			Item.DamageType = DamageClass.Ranged;
			Item.maxStack = 9999;
			Item.consumable = true; 
			Item.knockBack = 1.5f;
			Item.shoot = ModContent.ProjectileType<Projectiles.SherbetFlare>();
			Item.shootSpeed = 6f;
			Item.width = 12;
			Item.height = 12;
			Item.ammo = AmmoID.Flare;
			Item.value = 7;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color((byte)TheConfectionRebirth.SherbR, (byte)TheConfectionRebirth.SherbG, (byte)TheConfectionRebirth.SherbB, byte.MaxValue);
		}
	}
}
