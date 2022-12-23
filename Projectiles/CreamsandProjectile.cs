using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;
using Terraria.ID;

namespace TheConfectionRebirth.Projectiles {
    public class CreamsandProjectile : ModProjectile {
        public override void SetStaticDefaults() {
            Projectile.knockBack = 6f;
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
        }

        public override void AI() {
			if (Main.rand.NextBool(2)) {
				int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<ChipDust>(), 0f, Projectile.velocity.Y / 2f);
                ref Dust dust = ref Main.dust[dustIndex];
                dust.velocity.X *= 0.4f;
			}

			Projectile.tileCollide = true;
			Projectile.localAI[1] = 0f;

			Projectile.velocity.Y += 0.41f;

			Projectile.rotation -= 0.1f;

			if (Projectile.velocity.Y < -10f) {
				Projectile.velocity.Y = -10f;
			}
		}

		public override void Kill(int timeLeft) {
            int i = (int)(Projectile.position.X + Projectile.width / 2) / 16;
            int j = (int)(Projectile.position.Y + Projectile.height / 2) / 16;
            if (!WorldGen.InWorld(i, j) || Main.tile[i, j].HasTile) {
                return;
            }

            int tileType = ModContent.TileType<Tiles.Creamsand>();
            WorldGen.PlaceTile(i, j, tileType, forced: true);
            if (Main.netMode == NetmodeID.MultiplayerClient) {
                NetMessage.SendData(MessageID.TileManipulation, number: 1, number2: i, number3: j, number4: tileType);
            }
        }
    }
}