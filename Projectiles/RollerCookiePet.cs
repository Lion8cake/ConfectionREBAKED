using Microsoft.Build.Locator;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Projectiles
{
	public class RollerCookiePet : ModProjectile
	{
		public override void SetStaticDefaults() {
			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(0, 0, 1)
				.WithOffset(0, 2f)
				.WithSpriteDirection(-1)
				.WithCode((Projectile proj, bool walking) =>
				{
					proj.rotation = walking ? (float)Math.PI * 2f * ((float)Main.timeForVisualEffects % 20f / 20f) : 0f;
				});
		}

		public override void SetDefaults() {
			Projectile.netImportant = true;
			Projectile.width = 22;
			Projectile.height = 22;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft *= 5;
		}

		public override void AI() {
			Player player = Main.player[Projectile.owner];

			if (!player.active)
			{
				Projectile.active = false;
				return;
			}

			if (!player.dead && player.HasBuff(ModContent.BuffType<Buffs.RollerCookiePetBuff>()))
			{
				player.GetModPlayer<ConfectionPlayer>().rollerCookiePet = true;
			}
			if (player.GetModPlayer<ConfectionPlayer>().rollerCookiePet)
			{
				Projectile.timeLeft = 2;
			}

			bool ownerLeft = false;
			bool ownerRight = false;
			bool ownerAbove = false;
			bool jump = false;
			int followDistance = 55;
			
			if (player.Center.X < Projectile.Center.X - followDistance)
			{
				ownerLeft = true;
			}
			else if (player.Center.X > Projectile.Center.X + followDistance)
			{
				ownerRight = true;
			}

			if (player.rocketDelay2 > 0)
			{
				Projectile.ai[0] = 1f;
			}
			Vector2 distance = new Vector2(player.Center.X - Projectile.Center.X, player.Center.Y - Projectile.Center.Y);
			float distanceSquared = (float)Math.Sqrt(distance.X * distance.X + distance.Y * distance.Y);
			if (distanceSquared > 2000f)
			{
				Projectile.position.X = player.Center.X - (float)(Projectile.width / 2);
				Projectile.position.Y = player.Center.Y - (float)(Projectile.height / 2);
			}
			else if (distanceSquared > 500 || (Math.Abs(distance.Y) > 300f && (!(Projectile.localAI[0] > 0f))))
			{
				if (distance.Y > 0f && Projectile.velocity.Y < 0f)
				{
					Projectile.velocity.Y = 0f;
				}
				if (distance.Y < 0f && Projectile.velocity.Y > 0f)
				{
					Projectile.velocity.Y = 0f;
				}
				Projectile.ai[0] = 1f;
			}
			if (Projectile.ai[0] != 0f) //Flight
			{
				if (Main.rand.NextBool(2))
				{
					int dustID = Dust.NewDust(Projectile.position + new Vector2(2, 2), Projectile.width, Projectile.height, ModContent.DustType<ChocolateFlame>());
					Dust dust = Main.dust[dustID];
					dust.noGravity = true;
					dust.scale = 1.4f;
				}

				float flightSpeed = 0.2f;
				Projectile.tileCollide = false;
				if (distanceSquared < 200 && player.velocity.Y == 0f && Projectile.position.Y + (float)Projectile.height <= player.position.Y + (float)player.height && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
				{
					Projectile.ai[0] = 0f;
					if (Projectile.velocity.Y < -6f)
					{
						Projectile.velocity.Y = -6f;
					}
				}
				if (distanceSquared < 60f)
				{
					distance.X = Projectile.velocity.X;
					distance.Y = Projectile.velocity.Y;
				}
				else
				{
					distanceSquared = 10f / distanceSquared;
					distance.X *= distanceSquared;
					distance.Y *= distanceSquared;
				}
				
				if (Projectile.velocity.X < distance.X)
				{
					Projectile.velocity.X += flightSpeed;
					if (Projectile.velocity.X < 0f)
					{
						Projectile.velocity.X += flightSpeed * 1.5f;
					}
				}
				if (Projectile.velocity.X > distance.X)
				{
					Projectile.velocity.X -= flightSpeed;
					if (Projectile.velocity.X > 0f)
					{
						Projectile.velocity.X -= flightSpeed * 1.5f;
					}
				}
				if (Projectile.velocity.Y < distance.Y)
				{
					Projectile.velocity.Y += flightSpeed;
					if (Projectile.velocity.Y < 0f)
					{
						Projectile.velocity.Y += flightSpeed * 1.5f;
					}
				}

				if (Projectile.velocity.Y > distance.Y)
				{
					Projectile.velocity.Y -= flightSpeed;
					if (Projectile.velocity.Y > 0f)
					{
						Projectile.velocity.Y -= flightSpeed * 1.5f;
					}
				}
				
				if (Projectile.velocity.X > 0.5)
				{
					Projectile.spriteDirection = -1;
				}
				else if (Projectile.velocity.X < -0.5)
				{
					Projectile.spriteDirection = 1;
				}

				Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.58f;
				
				if (Projectile.spriteDirection == -1)
				{
					Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
				}
				else
				{
					Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 3.14f;
				}
			}
			else //Walk
			{
				Projectile.rotation += Projectile.velocity.X / 16;
				Projectile.tileCollide = true;
				float speed = 0.08f;
				float maxSpeed = 6.5f;
				
				if (ownerLeft)
				{
					if (Projectile.velocity.X > -3.5)
					{
						Projectile.velocity.X -= speed;
					}
					else
					{
						Projectile.velocity.X -= speed * 0.25f;
					}
				}
				else if (ownerRight)
				{
					if (Projectile.velocity.X < 3.5)
					{
						Projectile.velocity.X += speed;
					}
					else
					{
						Projectile.velocity.X += speed * 0.25f;
					}
				}
				else
				{
					Projectile.velocity.X *= 0.9f;
					if (Projectile.velocity.X >= 0f - speed && Projectile.velocity.X <= speed)
					{
						Projectile.velocity.X = 0f;
					}
				}
				
				if (ownerLeft || ownerRight)
				{
					int tileX = (int)Projectile.Center.X / 16;
					int tileY = (int)Projectile.Center.Y / 16;
					if (ownerLeft)
					{
						tileX -= 2;
					}
					if (ownerRight)
					{
						tileX += 2;
					}
					if (WorldGen.SolidTile(tileX, tileY))
					{
						jump = true;
					}
				}
				if (player.Center.Y - 8f > Projectile.Center.Y)
				{
					ownerAbove = true;
				}
				Collision.StepUp(ref Projectile.position, ref Projectile.velocity, Projectile.width, Projectile.height, ref Projectile.stepSpeed, ref Projectile.gfxOffY);
				if (Projectile.velocity.Y == 0f)
				{
					if (!ownerAbove && (Projectile.velocity.X < 0f || Projectile.velocity.X > 0f))
					{
						int tileX = (int)Projectile.Center.X / 16;
						int tileY = (int)Projectile.Center.Y / 16 + 1;
						if (ownerLeft)
						{
							tileX--;
						}
						if (ownerRight)
						{
							tileX++;
						}
						WorldGen.SolidTile(tileX, tileY);
					}
					if (jump)
					{
						int tileX = (int)(Projectile.position.X + (float)(Projectile.width / 2)) / 16;
						int tileY = (int)(Projectile.position.Y + (float)Projectile.height) / 16;
						if (WorldGen.SolidTileAllowBottomSlope(tileX, tileY) || Main.tile[tileX, tileY].IsHalfBlock || Main.tile[tileX, tileY].Slope > 0)
						{
							try
							{
								tileX = (int)Projectile.Center.X / 16;
								tileY = (int)Projectile.Center.Y / 16;
								if (ownerLeft)
								{
									tileX--;
								}
								if (ownerRight)
								{
									tileX++;
								}
								tileX += (int)Projectile.velocity.X;
								if (!WorldGen.SolidTile(tileX, tileY - 1) && !WorldGen.SolidTile(tileX, tileY - 2))
								{
									Projectile.velocity.Y = -5.1f;
								}
								else if (!WorldGen.SolidTile(tileX, tileY - 2))
								{
									Projectile.velocity.Y = -7.1f;
								}
								else if (WorldGen.SolidTile(tileX, tileY - 5))
								{
									Projectile.velocity.Y = -11.1f;
								}
								else if (WorldGen.SolidTile(tileX, tileY - 4))
								{
									Projectile.velocity.Y = -10.1f;
								}
								else
								{
									Projectile.velocity.Y = -9.1f;
								}
							}
							catch
							{
								Projectile.velocity.Y = -9.1f;
							}
						}
					}
				}
				if (Projectile.velocity.X > maxSpeed)
				{
					Projectile.velocity.X = maxSpeed;
				}
				if (Projectile.velocity.X < 0f - maxSpeed)
				{
					Projectile.velocity.X = 0f - maxSpeed;
				}
				if (Projectile.velocity.X < 0f)
				{
					Projectile.direction = -1;
				}
				if (Projectile.velocity.X > 0f)
				{
					Projectile.direction = 1;
				}
				if (Projectile.velocity.X > speed && ownerRight)
				{
					Projectile.direction = 1;
				}
				if (Projectile.velocity.X < 0f - speed && ownerLeft)
				{
					Projectile.direction = -1;
				}

				Projectile.spriteDirection = -Projectile.direction;

				Projectile.velocity.Y += 0.4f;
				if (Projectile.velocity.Y > 10f)
				{
					Projectile.velocity.Y = 10f;
				}
			}
		}
	}
}