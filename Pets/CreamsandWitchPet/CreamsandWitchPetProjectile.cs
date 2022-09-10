using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Pets.CreamsandWitchPet
{
	public class CreamsandWitchPetProjectile : ModProjectile
	{
		public override void SetStaticDefaults() {
			Main.projFrames[Projectile.type] = 10;
			Main.projPet[Projectile.type] = true;
		}

		public override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.PetLizard);

			AIType = ProjectileID.PetLizard;
			Projectile.width = 46;
			Projectile.height = 45;
		}

		public override bool PreAI() {
			Player player = Main.player[Projectile.owner];

			player.lizard = false;

			return true;
		}

		public override void AI() {
			Player player = Main.player[Projectile.owner];

			if (!player.dead && player.HasBuff(ModContent.BuffType<CreamsandWitchPet>())) {
				Projectile.timeLeft = 2;
			}
		}
	}
}
