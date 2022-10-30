using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Pets.CookiestPet
{
	public class CookiestBlockPro : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.penetrate = -1;
			Projectile.netImportant = true;
			Projectile.timeLeft *= 5;
			Projectile.friendly = true;
			Projectile.ignoreWater = true;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			if (!player.active)
			{
				Projectile.active = false;
				return;
			}

			int num = 450;
			float num2 = 200f;
			float num3 = 300f;
			int num4 = 15;
			if (player.dead)
			{
				player.GetModPlayer<ConfectionPlayer>().cookiePet = false;
			}
			if (player.GetModPlayer<ConfectionPlayer>().cookiePet)
			{
				Projectile.timeLeft = 2;
			}

			Vector2 vector = player.Center;
			vector.X = player.Center.X;
			Projectile.rotation += Projectile.velocity.X / 20f;

			Projectile.shouldFallThrough = (player.position.Y + (float)player.height - 12f > Projectile.position.Y + (float)Projectile.height);
			Projectile.friendly = false;
			int num9 = 0;
			int num10 = 15;
			int num11 = -1;
			bool flag9 = true;
			bool flag10 = Projectile.ai[0] == 5f;
			if (!flag10)
			{
				if (Projectile.ai[0] == 1f)
				{
					Projectile.tileCollide = false;
					float num12 = 0.2f;
					float num13 = 10f;
					int num14 = 200;
					if (num13 < Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y))
					{
						num13 = Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y);
					}
					Vector2 vector5 = player.Center - Projectile.Center;
					float num15 = vector5.Length();
					if (num15 > 2000f)
					{
						Projectile.position = player.Center - new Vector2((float)Projectile.width, (float)Projectile.height) / 2f;
					}
					if (num15 < (float)num14 && player.velocity.Y == 0f && Projectile.position.Y + (float)Projectile.height <= player.position.Y + (float)player.height && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
					{
						Projectile.ai[0] = 0f;
						Projectile.netUpdate = true;
						if (Projectile.velocity.Y < -6f)
						{
							Projectile.velocity.Y = -6f;
						}
					}
					if (num15 >= 60f)
					{
						vector5.Normalize();
						vector5 *= num13;
						if (Projectile.velocity.X < vector5.X)
						{
							Projectile.velocity.X = Projectile.velocity.X + num12;
							if (Projectile.velocity.X < 0f)
							{
								Projectile.velocity.X = Projectile.velocity.X + num12 * 1.5f;
							}
						}
						if (Projectile.velocity.X > vector5.X)
						{
							Projectile.velocity.X = Projectile.velocity.X - num12;
							if (Projectile.velocity.X > 0f)
							{
								Projectile.velocity.X = Projectile.velocity.X - num12 * 1.5f;
							}
						}
						if (Projectile.velocity.Y < vector5.Y)
						{
							Projectile.velocity.Y = Projectile.velocity.Y + num12;
							if (Projectile.velocity.Y < 0f)
							{
								Projectile.velocity.Y = Projectile.velocity.Y + num12 * 1.5f;
							}
						}
						if (Projectile.velocity.Y > vector5.Y)
						{
							Projectile.velocity.Y = Projectile.velocity.Y - num12;
							if (Projectile.velocity.Y > 0f)
							{
								Projectile.velocity.Y = Projectile.velocity.Y - num12 * 1.5f;
							}
						}
					}
					if (Projectile.velocity.X != 0f)
					{
						Projectile.spriteDirection = Math.Sign(Projectile.velocity.X);
					}
					if (Main.LocalPlayer.miscCounter % 3 == 0)
					{
						int num17 = 2;
						Dust dust2 = Main.dust[Dust.NewDust(Projectile.position + new Vector2((float)(-(float)num17), (float)(-(float)num17)), 16 + num17 * 2, 16 + num17 * 2, 0, 0f, 0f, 0, default(Color), 0.8f)];
						dust2.velocity = -Projectile.velocity * 0.25f;
						dust2.velocity = dust2.velocity.RotatedByRandom(0.2617993950843811);
					}
				}

				if (Projectile.ai[0] == 2f && Projectile.ai[1] < 0f)
				{
					Projectile.friendly = false;
					Projectile.ai[1] += 1f;
					if (num10 >= 0)
					{
						Projectile.ai[1] = 0f;
						Projectile.ai[0] = 0f;
						Projectile.netUpdate = true;
						return;
					}
				}
				else if (Projectile.ai[0] == 2f)
				{
					Projectile.spriteDirection = Projectile.direction;
					Projectile.rotation = 0f;

					Projectile.velocity.Y = Projectile.velocity.Y + 0.4f;
					if (Projectile.velocity.Y > 10f)
					{
						Projectile.velocity.Y = 10f;
					}
					Projectile.ai[1] -= 1f;
					if (Projectile.ai[1] <= 0f)
					{
						if (num9 <= 0)
						{
							Projectile.ai[1] = 0f;
							Projectile.ai[0] = 0f;
							Projectile.netUpdate = true;
							return;
						}
						Projectile.ai[1] = -num9;
					}
				}
				if (num11 >= 0)
				{
					float maxDistance = num;
					float num20 = 20f;

					NPC npc = Main.npc[num11];
					Vector2 center = npc.Center;
					vector = center;
					if (Projectile.IsInRangeOfMeOrMyOwner(npc, maxDistance, out float num21, out float num22, out bool flag12))
					{
						Projectile.shouldFallThrough = (npc.Center.Y > Projectile.Bottom.Y);
						bool flag13 = Projectile.velocity.Y == 0f;
						if (Projectile.wet && Projectile.velocity.Y > 0f && !Projectile.shouldFallThrough)
						{
							flag13 = true;
						}
						if (center.Y < Projectile.Center.Y - 30f && flag13)
						{
							double num23 = (double)((center.Y - Projectile.Center.Y) * -1f);
							float num24 = 0.4f;
							float num25 = (float)Math.Sqrt(num23 * (double)2f * (double)num24);
							if (num25 > 26f)
							{
								num25 = 26f;
							}
							Projectile.velocity.Y = -num25;
						}
						if (flag9 && Vector2.Distance(Projectile.Center, vector) < num20)
						{
							if (Projectile.velocity.Length() > 10f)
							{
								Projectile.velocity /= Projectile.velocity.Length() / 10f;
							}
							Projectile.ai[0] = 2f;
							Projectile.ai[1] = (float)num10;
							Projectile.netUpdate = true;
							Projectile.direction = ((center.X - Projectile.Center.X > 0f) ? 1 : -1);
						}
					}
				}
				if (Projectile.ai[0] == 0f && num11 < 0)
				{
					if (Main.player[Projectile.owner].rocketDelay2 > 0)
					{
						Projectile.ai[0] = 1f;
						Projectile.netUpdate = true;
					}
					Vector2 vector6 = player.Center - Projectile.Center;
					if (vector6.Length() > 2000f)
					{
						Projectile.position = player.Center - new Vector2(Projectile.width, Projectile.height) / 2f;
					}
					else if (vector6.Length() > num2 || Math.Abs(vector6.Y) > num3)
					{
						Projectile.ai[0] = 1f;
						Projectile.netUpdate = true;
						if (Projectile.velocity.Y > 0f && vector6.Y < 0f)
						{
							Projectile.velocity.Y = 0f;
						}
						if (Projectile.velocity.Y < 0f && vector6.Y > 0f)
						{
							Projectile.velocity.Y = 0f;
						}
					}
				}
				if (Projectile.ai[0] == 0f)
				{
					if (num11 < 0)
					{
						if (Projectile.Distance(player.Center) > 60f && Projectile.Distance(vector) > 60f && Math.Sign(vector.X - player.Center.X) != Math.Sign(Projectile.Center.X - player.Center.X))
						{
							vector = player.Center;
						}
						Rectangle rectangle = Utils.CenteredRectangle(vector, Projectile.Size);
						int num28 = 0;
						while (num28 < 20 && !Collision.SolidCollision(rectangle.TopLeft(), rectangle.Width, rectangle.Height))
						{
							rectangle.Y += 16;
							vector.Y += 16f;
							num28++;
						}
						Vector2 value = Collision.TileCollision(player.Center - Projectile.Size / 2f, vector - player.Center, Projectile.width, Projectile.height, false, false, 1);
						vector = player.Center - Projectile.Size / 2f + value;
						if (Projectile.Distance(vector) < 32f)
						{
							float num29 = player.Center.Distance(vector);
							if (player.Center.Distance(Projectile.Center) < num29)
							{
								vector = Projectile.Center;
							}
						}
						Vector2 vector7 = player.Center - vector;
						if (vector7.Length() > num2 || Math.Abs(vector7.Y) > num3)
						{
							Rectangle r = Utils.CenteredRectangle(player.Center, Projectile.Size);
							Vector2 value2 = vector - player.Center;
							Vector2 value3 = r.TopLeft();
							for (float num30 = 0f; num30 < 1f; num30 += 0.05f)
							{
								Vector2 vector8 = r.TopLeft() + value2 * num30;
								if (Collision.SolidCollision(r.TopLeft() + value2 * num30, rectangle.Width, rectangle.Height))
								{
									break;
								}
								value3 = vector8;
							}
							vector = value3 + Projectile.Size / 2f;
						}
					}
					Projectile.tileCollide = true;
					float num31 = 0.5f;
					float num32 = 4f;
					float num33 = 4f;
					float num34 = 0.1f;
					if (num33 < Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y))
					{
						num33 = Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y);
						num31 = 0.7f;
					}
					
					float num35 = player.velocity.Length();
					if (num35 < 0.1f)
					{
						num35 = 0f;
					}
					if (num35 != 0f && num35 < num33)
					{
						num33 = num35;
					}

					int num36 = 0;
					bool flag14 = false;
					float num37 = vector.X - Projectile.Center.X;
					Vector2 vector9 = vector - Projectile.Center;
					if (Math.Abs(num37) < 50f)
					{
						Projectile.rotation = Projectile.rotation.AngleTowards(0f, 0.2f);
						Projectile.velocity.X = Projectile.velocity.X * 0.9f;
						if ((double)Math.Abs(Projectile.velocity.X) < 0.1)
						{
							Projectile.velocity.X = 0f;
						}
					}
					else if (Math.Abs(num37) > 5f)
					{
						if (num37 < 0f)
						{
							num36 = -1;
							if (Projectile.velocity.X > -num32)
							{
								Projectile.velocity.X = Projectile.velocity.X - num31;
							}
							else
							{
								Projectile.velocity.X = Projectile.velocity.X - num34;
							}
						}
						else
						{
							num36 = 1;
							if (Projectile.velocity.X < num32)
							{
								Projectile.velocity.X = Projectile.velocity.X + num31;
							}
							else
							{
								Projectile.velocity.X = Projectile.velocity.X + num34;
							}
						}
						flag14 = false;
					}
					else
					{
						Projectile.velocity.X = Projectile.velocity.X * 0.9f;
						if (Math.Abs(Projectile.velocity.X) < num31 * 2f)
						{
							Projectile.velocity.X = 0f;
						}
					}

					bool flag16 = Math.Abs(vector9.X) >= 64f || (vector9.Y <= -48f && Math.Abs(vector9.X) >= 8f);
					if (num36 != 0 && flag16)
					{
						int num38 = (int)(Projectile.position.X + Projectile.width / 2) / 16;
						int num39 = (int)Projectile.position.Y / 16;
						num38 += num36;
						num38 += (int)Projectile.velocity.X;
						for (int j = num39; j < num39 + Projectile.height / 16 + 1; j++)
						{
							if (WorldGen.SolidTile(num38, j, false))
							{
								flag14 = true;
							}
						}
					}
					if (Math.Abs(Projectile.velocity.X) > 3f)
					{
						flag14 = true;
					}

					Collision.StepUp(ref Projectile.position, ref Projectile.velocity, Projectile.width, Projectile.height, ref Projectile.stepSpeed, ref Projectile.gfxOffY, 1, false, 0);
					float num40 = Utils.GetLerpValue(0f, 100f, vector9.Y, true) * Utils.GetLerpValue(-2f, -6f, Projectile.velocity.Y, true);
					if (Projectile.velocity.Y == 0f)
					{
						if (flag14)
						{
							for (int k = 0; k < 3; k++)
							{
								int num41 = (int)(Projectile.position.X + (float)(Projectile.width / 2)) / 16;
								if (k == 0)
								{
									num41 = (int)Projectile.position.X / 16;
								}
								if (k == 2)
								{
									num41 = (int)(Projectile.position.X + (float)Projectile.width) / 16;
								}
								int num42 = (int)(Projectile.position.Y + (float)Projectile.height) / 16;
								if (WorldGen.SolidTile(num41, num42, false) || Main.tile[num41, num42].IsHalfBlock || Main.tile[num41, num42].Slope > 0 || (TileID.Sets.Platforms[(int)Main.tile[num41, num42].TileType] && Main.tile[num41, num42].HasTile && !Main.tile[num41, num42].IsActuated))
								{
									try
									{
										num41 = (int)(Projectile.position.X + (float)(Projectile.width / 2)) / 16;
										num42 = (int)(Projectile.position.Y + (float)(Projectile.height / 2)) / 16;
										num41 += num36;
										num41 += (int)Projectile.velocity.X;
										if (!WorldGen.SolidTile(num41, num42 - 1, false) && !WorldGen.SolidTile(num41, num42 - 2, false))
										{
											Projectile.velocity.Y = -5.1f;
										}
										else if (!WorldGen.SolidTile(num41, num42 - 2, false))
										{
											Projectile.velocity.Y = -7.1f;
										}
										else if (WorldGen.SolidTile(num41, num42 - 5, false))
										{
											Projectile.velocity.Y = -11.1f;
										}
										else if (WorldGen.SolidTile(num41, num42 - 4, false))
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
							if (vector.Y - Projectile.Center.Y < -48f)
							{
								float num43 = vector.Y - Projectile.Center.Y;
								num43 *= -1f;
								if (num43 < 60f)
								{
									Projectile.velocity.Y = -6f;
								}
								else if (num43 < 80f)
								{
									Projectile.velocity.Y = -7f;
								}
								else if (num43 < 100f)
								{
									Projectile.velocity.Y = -8f;
								}
								else if (num43 < 120f)
								{
									Projectile.velocity.Y = -9f;
								}
								else if (num43 < 140f)
								{
									Projectile.velocity.Y = -10f;
								}
								else if (num43 < 160f)
								{
									Projectile.velocity.Y = -11f;
								}
								else if (num43 < 190f)
								{
									Projectile.velocity.Y = -12f;
								}
								else if (num43 < 210f)
								{
									Projectile.velocity.Y = -13f;
								}
								else if (num43 < 270f)
								{
									Projectile.velocity.Y = -14f;
								}
								else if (num43 < 310f)
								{
									Projectile.velocity.Y = -15f;
								}
								else
								{
									Projectile.velocity.Y = -16f;
								}
							}
							if (Projectile.wet && num40 == 0f)
							{
								Projectile.velocity.Y = Projectile.velocity.Y * 2f;
							}
						}
						if (Projectile.localAI[2] == 0f)
						{
							Projectile.localAI[2] = 1f;
							for (int l = 0; l < 6; l++)
							{
								Dust dust3 = Main.dust[Dust.NewDust(Projectile.position + Projectile.velocity, 16, 16, ModContent.DustType<Dusts.CreamwoodDust>(), 0f, 0f, 0, default(Color), 0.8f)];
								dust3.velocity.X = Projectile.velocity.X * 0.25f;
								dust3.velocity.Y = -2f + Math.Abs(Projectile.velocity.Y) * 0.25f;
								dust3.velocity = dust3.velocity.RotatedByRandom(0.2617993950843811);
							}
						}
					}
					else if (true)
					{
						Projectile.localAI[2] = 0f;
					}
					if (Projectile.velocity.X > num33)
					{
						Projectile.velocity.X = num33;
					}
					if (Projectile.velocity.X < -num33)
					{
						Projectile.velocity.X = -num33;
					}
					if (Projectile.velocity.X < 0f)
					{
						Projectile.direction = -1;
					}
					if (Projectile.velocity.X > 0f)
					{
						Projectile.direction = 1;
					}
					if (Projectile.velocity.X == 0f)
					{
						Projectile.direction = (player.Center.X > Projectile.Center.X) ? 1 : -1;
					}
					if (Projectile.velocity.X > num31 && num36 == 1)
					{
						Projectile.direction = 1;
					}
					if (Projectile.velocity.X < -num31 && num36 == -1)
					{
						Projectile.direction = -1;
					}
					Projectile.spriteDirection = Projectile.direction;
					Projectile.velocity.Y = Projectile.velocity.Y + (0.4f + num40 * 1f);
					if (Projectile.velocity.Y > 10f)
					{
						Projectile.velocity.Y = 10f;
					}
				}
				return;
			}
			if (num11 < 0)
			{
				Projectile.ai[0] = 0f;
				Projectile.ai[1] = 0f;
				return;
			}
			float maxDistance2 = (float)num;
			NPC npc2 = Main.npc[num11];
			vector = npc2.Center;
			if (!Projectile.IsInRangeOfMeOrMyOwner(npc2, maxDistance2, out _, out _, out _))
			{
				Projectile.ai[0] = 0f;
				Projectile.ai[1] = 0f;
				return;
			}
			Point point2 = npc2.Top.ToTileCoordinates();
			int n = 0;
			int num54 = point2.Y;
			while (n < num4)
			{
				Tile tile2 = Main.tile[point2.X, num54];
				if (tile2 == null || tile2.HasTile)
				{
					break;
				}
				n++;
				num54++;
			}
			int num55 = num4 / 2;
			if (n < num55)
			{
				Projectile.ai[0] = 0f;
				Projectile.ai[1] = 0f;
				return;
			}
			if (Projectile.Hitbox.Intersects(npc2.Hitbox) && Projectile.velocity.Y >= 0f)
			{
				Projectile.velocity.Y = -8f;
				Projectile.velocity.X = (float)(Projectile.direction * 10);
			}
			float num56 = 20f;
			float maxAmountAllowedToMove = 4f;
			float num57 = 40f;
			float num58 = 40f;
			Vector2 top = npc2.Top;
			float num59 = (float)Math.Cos(Main.timeForVisualEffects / (double)num57 * 6.2831854820251465);
			if (num59 > 0f)
			{
				num59 *= -1f;
			}
			num59 *= num58;
			top.Y += num59;
			Vector2 vector10 = top - Projectile.Center;
			if (vector10.Length() > num56)
			{
				vector10 = vector10.SafeNormalize(Vector2.Zero) * num56;
			}
			Projectile.velocity = Projectile.velocity.MoveTowards(vector10, maxAmountAllowedToMove);
			Projectile.frame = 8;
			Projectile.rotation += 0.6f * (float)Projectile.spriteDirection;
		}
	}
}
