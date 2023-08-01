using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Pets.MeawzerPet
{
	public class MeawzerPetProjectile : ModProjectile
	{
		public override void SetStaticDefaults() {
			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(0, 0, 1)
				.WithOffset(-6, -30f)
				.WithSpriteDirection(-1)
				.WithCode(DelegateMethods.CharacterPreview.Float);
		}

		public override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.ZephyrFish);

			AIType = ProjectileID.ZephyrFish;
		}

		public override bool PreAI() {
			Player player = Main.player[Projectile.owner];

			player.zephyrfish = false;

			return true;
		}

		public override void AI() {
			Player player = Main.player[Projectile.owner];

			if (!player.dead && player.HasBuff(ModContent.BuffType<MeawzerPet>())) {
				Projectile.timeLeft = 2;
			}
		}
	}
}
