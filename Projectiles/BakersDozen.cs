using Terraria;
using Terraria.ModLoader;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Terraria.Graphics.Shaders;
using Terraria.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria.DataStructures;
using System.Linq;
using System;
using TheConfectionRebirth.Dusts;
using log4net.Core;
using System.IO;

namespace TheConfectionRebirth.Projectiles
{
    public class BakersDozen : ModProjectile
    {
		public bool dozenAttack = false;

		public int[] dozenTarget = new int[2] { -1, -1 };
		
		public int dozenAttackCount = 0;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[Type] = 5;
			ProjectileID.Sets.TrailCacheLength[Type] = 15;
			ProjectileID.Sets.DrawScreenCheckFluff[Type] = 960;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.aiStyle = 3;
            Projectile.timeLeft = 1200;
            Projectile.DamageType = DamageClass.Melee;
        }

		public override void OnSpawn(IEntitySource source)
		{
			int frameCosen = 0;
			int[] projFrameCount = new int[Main.projFrames[Type]];
			for (int i = 0; i < Main.maxProjectiles; i++)
			{
				Projectile proj = Main.projectile[i];
				if (proj.active && proj.owner == Projectile.owner && proj.type == Type && proj != Projectile)
				{
					projFrameCount[proj.frame]++;
				}
			}
			int minValue = projFrameCount.Min();
			for (int j = 0; j < projFrameCount.Length; j++)
			{
				if (projFrameCount[j] <= minValue)
				{
					frameCosen = j;
					break;
				}
			}
			Projectile.frame = frameCosen;
		}

		public override bool PreAI()
		{
			if (dozenAttack)
			{
				Projectile.rotation += 0.4f * (float)Projectile.direction;
				Projectile.tileCollide = false;
				Projectile.usesIDStaticNPCImmunity = true;
				Projectile.idStaticNPCHitCooldown = 5;
				Entity target;
				if (dozenTarget[1] != -1)
					target = Main.npc[dozenTarget[1]];
				else
					target = Main.player[dozenTarget[2]];
				if (!target.active)
				{
					dozenAttack = false;
					return false;
				}
				Vector2 point2 = Projectile.position + new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f);
				Vector2 point = new Vector2(target.Center.X, target.Center.Y);
				Vector2 finalVel = Vector2.Zero;
				Vector2 FinalPos = new Vector2(point2.X, point2.Y);
				float NewPosX = point.X - FinalPos.X;
				float NewPosY = point.Y - FinalPos.Y;
				float FinPos = (float)Math.Sqrt(NewPosX * NewPosX + NewPosY * NewPosY);
				FinPos = 4f / FinPos;
				NewPosX *= FinPos;
				NewPosY *= FinPos;
				finalVel.X = (Projectile.velocity.X * 2f + NewPosX) / 2f;
				finalVel.Y = (Projectile.velocity.Y * 2f + NewPosY) / 2f;
				Projectile.velocity = finalVel;
				if (Projectile.velocity.X > 10f)
					Projectile.velocity.X = 10f;
				if (Projectile.velocity.Y > 10f)
					Projectile.velocity.Y = 10f;
				return false;
			}
			else
			{
				if (Projectile.usesIDStaticNPCImmunity)
					Projectile.usesIDStaticNPCImmunity = false;
				if (!Main.dedServ)
				{
					Vector3 projcolor = new(1.56f, 0.54f, 1.82f);
					if (Projectile.frame == 1)
					{
						projcolor = new(0.96f, 0.53f, 0.40f);
					}
					else if (Projectile.frame == 2)
					{
						projcolor = new(1.16f, 0.97f, 0.71f);
					}
					else if (Projectile.frame == 3)
					{
						projcolor = new(0.48f, 1.16f, 0.84f);
					}
					Lighting.AddLight(Projectile.Center, projcolor / 4);
				}
				return true;
			}
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(dozenAttack);
			writer.Write(dozenTarget[1]);
			writer.Write(dozenTarget[2]);
			writer.Write(dozenAttackCount);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			dozenAttack = reader.ReadBoolean();
			dozenTarget[1] = reader.ReadInt32();
			dozenTarget[2] = reader.ReadInt32();
			dozenAttackCount = reader.ReadInt32();
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			OnHit(target, 1);
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			OnHit(target, 2);
		}

		private void OnHit(Entity target, int entityType)
		{
			if (dozenAttack)
			{
				BakersDozenHitEffect(target);
				dozenAttackCount++;
				if (dozenAttackCount >= 5)
					dozenAttack = false;
			}
			else
			{
				Player player = Main.player[Projectile.owner];
				player.GetModPlayer<ConfectionPlayer>().bakersDozenHitCount++;
				if (player.GetModPlayer<ConfectionPlayer>().bakersDozenHitCount > 13)
				{
					dozenAttack = true;
					dozenTarget[entityType] = target.whoAmI;
					player.GetModPlayer<ConfectionPlayer>().bakersDozenHitCount = 0;
				}
			}
		}

		public void BakersDozenHitEffect(Entity target)
		{
			Color color = Projectile.frame switch
			{
				1 => new Color(118, 70, 50, 1),
				2 => new Color(188, 168, 120, 1),
				3 => new Color(61, 204, 144, 1),
				_ => new Color(225, 130, 227, 1)
			};
			for (int i = 0; i < 8; i++)
			{
				float degree = 360 / 8 * i;
				float radians = MathHelper.ToRadians(degree);
				Vector2 velcoity = Vector2.One.RotatedBy(radians);
				int num = Dust.NewDust(Projectile.position, 10, 10, ModContent.DustType<TintableBakersDust>());
				Dust dust = Main.dust[num];
				dust.noGravity = true;
				dust.scale = 2f;
				dust.velocity = velcoity;
				dust.color = color;
			}
			Vector2 v = Main.rand.NextVector2CircularEdge(200f, 200f);
			if (v.Y < 0f)
			{
				v.Y *= -1f;
			}
			v.Y += 100f;
			Vector2 vector = v.SafeNormalize(Vector2.UnitY) * 6f;
			Projectile.NewProjectile(Projectile.GetSource_OnHit(target), target.Center - vector * 20f, vector, ModContent.ProjectileType<BakersDozenSlash>(), (int)((double)Projectile.damage * 0.75), 0f, Projectile.owner, 0f, target.Center.Y, Projectile.frame);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			default(BakersDozenTrailEffect).Draw(Projectile);

			Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
			Texture2D glow = ModContent.Request<Texture2D>(Texture + "_Glow").Value;
			int frameHeight = texture.Height / Main.projFrames[Type];
			Rectangle frame = new Rectangle(0, frameHeight * Projectile.frame, texture.Width, frameHeight);
			Vector2 drawPos = Projectile.Center - Main.screenPosition;

			float Rot = Projectile.rotation;

			Main.EntitySpriteDraw(texture, drawPos, frame, lightColor, Rot, new Vector2(texture.Width, frameHeight) / 2, Projectile.scale, SpriteEffects.None, 0);
			Main.EntitySpriteDraw(glow, drawPos, frame, Color.White, Rot, new Vector2(texture.Width, frameHeight) / 2, Projectile.scale, SpriteEffects.None, 0);
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
			VertexStrip.StripColorFunction colorFunc = StripColorsStrawberry;
			switch (proj.frame)
			{
				case 1:
					colorFunc = StripColorsChocolate; 
					break;
				case 2:
					colorFunc = StripColorsVanilla;
					break;
				case 3:
					colorFunc = StripColorsMint;
					break;
			}
			_vertexStrip.PrepareStripWithProceduralPadding(proj.oldPos, proj.oldRot, colorFunc, StripWidth, -Main.screenPosition + proj.Size / 2f + (proj.velocity / 1.5f));
			_vertexStrip.DrawTrail();
			Main.pixelShader.CurrentTechnique.Passes[0].Apply();
		}

		private Color StripColorsStrawberry(float progressOnStrip)
		{
			return StripColors(progressOnStrip, new Color(140, 40, 160));
		}
		private Color StripColorsChocolate(float progressOnStrip)
		{
			return StripColors(progressOnStrip, new Color(90, 50, 40));
		}
		private Color StripColorsVanilla(float progressOnStrip)
		{
			return StripColors(progressOnStrip, new Color(110, 90, 70));
		}

		private Color StripColorsMint(float progressOnStrip)
		{
			return StripColors(progressOnStrip, new Color(50, 110, 80));
		}

		private static Color StripColors(float progressOnStrip, Color color) {
			float num = 1f - progressOnStrip;
			Color result = color * (num * num * num * num) * 0.5f;
			result.A = 0;
			return result;
		}

		private float StripWidth(float progressOnStrip) {
			return MathHelper.SmoothStep(0f, 13f, progressOnStrip * 14);
		}
	}
}
