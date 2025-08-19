using TheConfectionRebirth.Items;
using TheConfectionRebirth.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Items.Placeable;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace TheConfectionRebirth.Items.Weapons
{
	public class CrunchBall : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 1;
		}
	
		public override void SetDefaults()
		{
			Item.damage = 38;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 4;
			Item.width = 28;
			Item.height = 30;
			Item.useTime = 16;
			Item.useAnimation = 16;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 2f;
			Item.value = 500000;
			Item.rare = ItemRarityID.LightRed;
			Item.UseSound = SoundID.Item8;
			Item.autoReuse = false;
			Item.shoot = ModContent.ProjectileType<RockCandy>();
			Item.shootSpeed = 16f;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<RockCandy>()] < 1)
			{
				return true;
			}
			else
			{
				Vector2 vel = velocity.RotatedByRandom(MathHelper.ToRadians(10));
				Projectile.NewProjectile(source, position, vel, type, damage, knockback, player.whoAmI);
				return false;
			}
		}
	}
}