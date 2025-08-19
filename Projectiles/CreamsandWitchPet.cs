using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Projectiles
{
	public class CreamsandWitchPet : ModProjectile
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
			Projectile.width = 32;
			Projectile.height = 45;
		}

		public override bool PreAI() {
			Player player = Main.player[Projectile.owner];

			player.lizard = false;

			return true;
		}

		public override void AI() {
			Player player = Main.player[Projectile.owner];

			if (!player.active)
			{
				Projectile.active = false;
				return;
			}

			if (!player.dead && player.HasBuff(ModContent.BuffType<Buffs.CreamsandWitchPet>()))
			{
				player.GetModPlayer<ConfectionPlayer>().creamsandWitchPet = true;
			}
			if (player.GetModPlayer<ConfectionPlayer>().creamsandWitchPet)
			{
				Projectile.timeLeft = 2;
			}
		}
	}
}
