using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;
using TheConfectionRebirth.Projectiles;

namespace TheConfectionRebirth.Items.Weapons
{
	public class TrueDeathsRaze : ModItem {

		public override void SetDefaults()
		{
			Item.damage = 74;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 15;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 6;
			Item.value = 500000;
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.Item1;
			Item.shoot = ModContent.ProjectileType<DeathsSlash>();
			Item.shootSpeed = 5f;
			//Item.noMelee = true;
			Item.shootsEveryUse = true;
		}

		public override void MeleeEffects(Player player, Rectangle hitbox) {
			if (Main.rand.Next(5) == 0) {
				Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 115, player.direction * 2, 0f, 150, default(Color), 1.4f);
			}
			int num4 = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 231, player.velocity.X * 0.2f + (float)(player.direction * 3), player.velocity.Y * 0.2f, 100, default(Color), 1.2f);
			Main.dust[num4].noGravity = true;
			Main.dust[num4].velocity.X /= 2f;
			Main.dust[num4].velocity.Y /= 2f;
		}

		public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo) {
			Vector2 positionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox);
			ParticleSystem.AddParticle(new TrueBloodyNeedle(), positionInWorld, new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), 1), default, 24);
		}

		public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone) {
			Vector2 positionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox);
			ParticleSystem.AddParticle(new TrueBloodyNeedle(), positionInWorld, new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), 1), default, 24);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			Projectile.NewProjectile(source, player.MountedCenter, velocity, ModContent.ProjectileType<TrueDeathsSlash>(), damage, knockback, player.whoAmI, (float)player.direction * player.gravDir, player.itemAnimationMax);
			Projectile.NewProjectile(source, player.MountedCenter, velocity, ModContent.ProjectileType<TrueDeathsSlash>(), damage, knockback, player.whoAmI, (float)player.direction * player.gravDir * 0.1f, 30f);
			Projectile.NewProjectile(source, player.MountedCenter, velocity, type, damage, knockback, player.whoAmI, (float)player.direction * player.gravDir, player.itemAnimationMax);
			Projectile.NewProjectile(source, player.MountedCenter, velocity, type, damage, knockback, player.whoAmI, (float)player.direction * player.gravDir * 0.1f, 30f);
			return false;
		}

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<DeathsRaze>())
				.AddIngredient(ItemID.SoulofFright, 20)
				.AddIngredient(ItemID.SoulofMight, 20)
				.AddIngredient(ItemID.SoulofSight, 20)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}