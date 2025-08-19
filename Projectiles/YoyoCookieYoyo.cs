using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Projectiles
{
    public class YoyoCookieYoyo : ModProjectile
    {
		public int homingTimer = 0;

		public bool IsRegenning = false;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = 16f;
            ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 230f;
            ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 10f;
			Main.projFrames[Type] = 2;
		}

        public override void SetDefaults()
        {
            Projectile.extraUpdates = 0;
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = 99;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.scale = 1f;
        }

		public override void AI() {
			homingTimer += (IsRegenning ? -1 : 1);
			if (homingTimer >= 300 && !IsRegenning) {
				Projectile.NewProjectile(new EntitySource_Misc(""), Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<YoyoCookieKiss>(), Projectile.damage * 5, 1f);
				IsRegenning = true;
			}
			if (homingTimer <= 0 && IsRegenning) {
				IsRegenning = false;
			}
			Projectile.frame = (IsRegenning ? 1 : 0);
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			if (!IsRegenning) {
				homingTimer = 0;
			}
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo info) {
			if (!IsRegenning) {
				homingTimer = 0;
			}
		}

		public override void SendExtraAI(BinaryWriter writer) {
			writer.Write(homingTimer);
			writer.Write(IsRegenning);
		}

		public override void ReceiveExtraAI(BinaryReader reader) {
			IsRegenning = reader.ReadBoolean();
			homingTimer = reader.ReadInt32();
		}
	}
}
