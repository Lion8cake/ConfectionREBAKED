using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Projectiles
{
    public class StrawberryStrike : ModProjectile
    {
		public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
			Projectile.penetrate = -1;
            Projectile.timeLeft = 90;
            Projectile.aiStyle = -1;
        }

		public override void OnSpawn(IEntitySource source)
		{
			Projectile.localAI[0] = Projectile.position.X;
			Projectile.localAI[1] = Projectile.position.Y;
			Projectile.localAI[2] = 20;
			SoundStyle style = SoundID.Item29;
			style.Volume = 0.25f;
			SoundEngine.PlaySound(style, Projectile.position);
		}

		public override void AI()
        {
			Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + MathHelper.ToRadians(45);

			if (Projectile.timeLeft < 60)
			{
				Projectile.velocity.X *= 0.95f;
				Projectile.velocity.Y *= 0.95f;

				Projectile.alpha += 5;
			}
			else
			{
				if (Projectile.ai[0] < 1f)
				{
					Projectile.direction = Projectile.spriteDirection = Projectile.velocity.X > 0f ? 1 : 1;
					float CenterX = Projectile.Center.X;
					float CenterY = Projectile.Center.Y;
					float Distanse = 400f;
					bool CheckDistanse = false;
					for (int MobCounts = 0; MobCounts < 200; MobCounts++)
					{
						if (Main.npc[MobCounts].CanBeChasedBy(Projectile, false) && Collision.CanHit(Projectile.Center, 1, 1, Main.npc[MobCounts].Center, 1, 1))
						{
							float Position1 = Main.npc[MobCounts].position.X + Main.npc[MobCounts].width / 2;
							float Position2 = Main.npc[MobCounts].position.Y + Main.npc[MobCounts].height / 2;
							float Position3 = Math.Abs(Projectile.position.X + Projectile.width / 2 - Position1) + Math.Abs(Projectile.position.Y + Projectile.height / 2 - Position2);
							if (Position3 < Distanse)
							{
								Distanse = Position3;
								CenterX = Position1;
								CenterY = Position2;
								CheckDistanse = true;
							}
						}
					}
					if (CheckDistanse)
					{
						float Speed = 4f;
						Vector2 FinalPos = new Vector2(Projectile.position.X + Projectile.width * 0.5f, Projectile.position.Y + Projectile.height * 0.5f);
						float NewPosX = CenterX - FinalPos.X;
						float NewPosY = CenterY - FinalPos.Y;
						float FinPos = (float)Math.Sqrt(NewPosX * NewPosX + NewPosY * NewPosY);
						FinPos = Speed / FinPos;
						NewPosX *= FinPos;
						NewPosY *= FinPos;
						Projectile.velocity.X = (Projectile.velocity.X * 11f + NewPosX) / 11f;
						Projectile.velocity.Y = (Projectile.velocity.Y * 11f + NewPosY) / 11f;
					}
				}
			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Projectile.ai[0] = 1f;
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			Projectile.ai[0] = 1f;
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			return false;
		}

		public override void PostDraw(Color lightColor)
		{
			if (Projectile.timeLeft > 60)
			{
				if (Projectile.timeLeft > 85)
					Projectile.localAI[2]++;
				else
					Projectile.localAI[2]--;
				Texture2D texture = (Texture2D)TextureAssets.Extra[98];
				Vector2 pos = new Vector2(Projectile.localAI[0], Projectile.localAI[1]) - Main.screenPosition;
				Vector2 orig = texture.Size() / 2;
				Rectangle? frame = new Rectangle?();
				Color color = new Color(255, 255, 255, 127);
				Color color2 = new Color(224, 92, 165, 127);
				color *= Projectile.localAI[2] / 25;
				color2 *= Projectile.localAI[2] / 25;

				float scaler = (float)((double)Utils.GetLerpValue(15f, 30f, Projectile.localAI[2], true) * (double)Utils.GetLerpValue(240f, 200f, Projectile.localAI[2], true) * (1.0 + 0.200000002980232 * Math.Cos((double)Main.GlobalTimeWrappedHourly % 30.0 / 0.5 * 6.28318548202515 * 3.0)) * 0.800000011920929);
				Vector2 scale1 = new Vector2(1f, 2.5f) * 2 * scaler;
				Vector2 scale2 = new Vector2(0.6f, 1.4f) * 2 * scaler;

				Main.EntitySpriteDraw(texture, pos, frame, color, 0f, orig, scale1, SpriteEffects.None);
				Main.EntitySpriteDraw(texture, pos, frame, color, MathHelper.ToRadians(90f), orig, scale2, SpriteEffects.None);
				Main.EntitySpriteDraw(texture, pos, frame, color2, 0f, orig, scale1, SpriteEffects.None);
				Main.EntitySpriteDraw(texture, pos, frame, color2, MathHelper.ToRadians(90f), orig, scale2, SpriteEffects.None);
			}
		}
	}
}