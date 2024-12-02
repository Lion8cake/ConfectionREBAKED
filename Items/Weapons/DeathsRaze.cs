using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Weapons
{
	public class DeathsRaze : ModItem 
	{

		public override void SetStaticDefaults() {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 45;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 21;
			Item.useAnimation = 21;
			Item.autoReuse = true;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 4.5f;
			Item.value = 40000;
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.Item1;
			Item.shoot = ModContent.ProjectileType<Projectiles.DeathsSlash>();
			Item.shootSpeed = 5f;
			Item.shootsEveryUse = true;
		}

		public override void MeleeEffects(Player player, Rectangle hitbox) {
			if (Main.rand.NextBool(5)) {
				Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.CrimtaneWeapons, player.direction * 2, 0f, 150, default(Color), 1.4f);
			}
			int num4 = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.ViciousPowder, player.velocity.X * 0.2f + (float)(player.direction * 3), player.velocity.Y * 0.2f, 100, default(Color), 1.2f);
			Main.dust[num4].noGravity = true; //                                                   ^ maybe try 296 (crimson torch dust) if this doesn't look good
			Main.dust[num4].velocity.X /= 2f;
			Main.dust[num4].velocity.Y /= 2f;
		}

		public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo) {
			Vector2 positionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox);
			ParticleSystem.AddParticle(new Spawn_DeathsRaze(), positionInWorld, new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), 1));
		}

		public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone) {
			Vector2 positionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox);
			ParticleSystem.AddParticle(new Spawn_DeathsRaze(), positionInWorld, new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), 1));
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			Projectile.NewProjectile(source, player.MountedCenter, velocity, type, damage, knockback, player.whoAmI, (float)player.direction * player.gravDir, player.itemAnimationMax);
			Projectile.NewProjectile(source, player.MountedCenter, velocity, type, damage, knockback, player.whoAmI, (float)player.direction * player.gravDir * 0.1f, 30f);
			return false;
		}
	}
}