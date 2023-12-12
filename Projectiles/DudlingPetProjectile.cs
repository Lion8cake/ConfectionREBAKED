using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Projectiles
{
	public class DudlingPetProjectile : ModProjectile
	{
		public override void SetStaticDefaults() {
			Main.projFrames[Projectile.type] = 10;
			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(0, 0, 1)
				.WithOffset(-10, 2f)
				.WithSpriteDirection(-1)
				.WhenSelected(1, 5, 6);
		}

		public override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.PetLizard);

			AIType = ProjectileID.PetLizard;
			Projectile.width = 46;
			Projectile.height = 46;
		}

		public override bool PreAI() {
			Main.player[Projectile.owner].lizard = false;
			return true;
		}

		public override void AI() {
			Player player = Main.player[Projectile.owner];

			if (!player.dead && player.HasBuff(ModContent.BuffType<Buffs.DudlingPet>())) {
				Projectile.timeLeft = 2;
			}
		}
	}
}
