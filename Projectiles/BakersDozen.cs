using Terraria;
using Terraria.ModLoader;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Terraria.Graphics.Shaders;
using Terraria.Graphics;
using System.Collections.Generic;

namespace TheConfectionRebirth.Projectiles
{
    public class BakersDozen : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.aiStyle = 3;
            Projectile.timeLeft = 600;
            AIType = 52;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.frame = 0;
        }

		public override void AI() {
			Vector3 projcolor = new(1.56f, 0.54f, 1.82f);
			if (Projectile.frame == 0) {
				projcolor = new(1.56f, 0.54f, 1.82f);
			}
			if (Projectile.frame == 1) {
				projcolor = new(0.96f, 0.53f, 0.40f);
			}
			if (Projectile.frame == 2) {
				projcolor = new(1.16f, 0.97f, 0.71f);
			}
			if (Projectile.frame == 3) {
				projcolor = new(0.48f, 1.16f, 0.84f);
			}
			if (!Main.dedServ) {
				Lighting.AddLight(Projectile.Center, projcolor);
			}
		}

		public override bool PreDrawExtras() {
			BakersDozenTrailEffect trail = default(BakersDozenTrailEffect);
			trail.Draw(Projectile);
			return false;
		}
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct BakersDozenTrailEffect {
		private static VertexStrip _vertexStrip = new VertexStrip();

		public void Draw(Projectile proj) {
			MiscShaderData miscShaderData = GameShaders.Misc["LightDisc"];
			miscShaderData.UseSaturation(-2.8f);
			miscShaderData.UseOpacity(2f);
			miscShaderData.Apply();
			_vertexStrip.PrepareStripWithProceduralPadding(proj.oldPos, proj.oldRot, StripColors, StripWidth, -Main.screenPosition + proj.Size / 2f);
			_vertexStrip.DrawTrail();
			Main.pixelShader.CurrentTechnique.Passes[0].Apply();
		}

		private Color StripColors(float progressOnStrip) {
			float num = 1f - progressOnStrip;
			Color result = new Color(140, 44, 163) * (num * num * num * num) * 0.5f;
			result.A = ((byte)0);
			return result;
		}

		private float StripWidth(float progressOnStrip) {
			return 16f;
		}
	}
}
