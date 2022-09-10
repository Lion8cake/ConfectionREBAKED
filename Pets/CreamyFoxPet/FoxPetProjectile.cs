using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Pets.CreamyFoxPet
{
	public class FoxPetProjectile : ModProjectile
	{
		public override void SetStaticDefaults() {
			Main.projFrames[Projectile.type] = 11;
			Main.projPet[Projectile.type] = true;
		}

		public override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.Puppy);

			AIType = ProjectileID.Puppy;
			Projectile.width = 68;
			Projectile.height = 36;
		}

		public override bool PreAI() {
			Player player = Main.player[Projectile.owner];

			player.puppy = false;

			return true;
		}

		public override void AI() {
			Player player = Main.player[Projectile.owner];

			if (!player.dead && player.HasBuff(ModContent.BuffType<FoxPet>())) {
				Projectile.timeLeft = 2;
			}
		}
	}
}
