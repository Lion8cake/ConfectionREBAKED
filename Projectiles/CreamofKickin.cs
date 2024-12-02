using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.DataStructures;

namespace TheConfectionRebirth.Projectiles
{
	public class CreamofKickin : ModProjectile
	{
		private readonly Asset<Texture2D> chainAsset = ModContent.Request<Texture2D>("TheConfectionRebirth/Projectiles/CreamofKickin" + "_Chain");

		private readonly Asset<Texture2D> chainAsset2 = ModContent.Request<Texture2D>("TheConfectionRebirth/Projectiles/CreamofKickin" + "_Chain_2");

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
			Main.projFrames[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.netImportant = true;
			Projectile.width = 22;
			Projectile.height = 22;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
			DrawOriginOffsetY = -6;
			DrawOffsetX = -6;
		}

		public override void OnSpawn(IEntitySource source)
		{
			if (Projectile.ai[2] <= -1)
			{
				Projectile.frame = Main.rand.Next(0, 2);
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.position, Vector2.Zero, ModContent.ProjectileType<CreamofKickin>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 3, 0, Projectile.whoAmI);
			}
			else
			{
				Projectile.frame = Main.projectile[(int)Projectile.ai[2]].frame == 1 ? 0 : 1;
			}
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			if (Projectile.owner == Main.myPlayer)
			{
				float num = 1f;
				if (Projectile.ai[0] == 0f)
				{
					num *= 1.2f;
				}
				if (Projectile.ai[0] == 1f || Projectile.ai[0] == 2f)
				{
					num *= 2f;
				}
				modifiers.SourceDamage *= num;
				float num21 = Projectile.knockBack;
				int num26 = ((Main.player[Projectile.owner].Center.X < target.Center.X) ? 1 : (-1));
				if (Projectile.ai[0] == 0f)
				{
					num21 *= 0.35f;
				}
				if (Projectile.ai[0] == 6f)
				{
					num21 *= 0.5f;
				}
				modifiers.HitDirectionOverride = num26;
				modifiers.Knockback *= num21 / Projectile.knockBack;
			}
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (Projectile.ai[0] == 0f)
			{
				Vector2 mountedCenter = Main.player[Projectile.owner].MountedCenter;
				Vector2 shortestVectorFromPlayerToTarget = targetHitbox.ClosestPointInRect(mountedCenter) - mountedCenter;
				shortestVectorFromPlayerToTarget.Y /= 0.8f;
				float hitRadius = 55f; 
				return shortestVectorFromPlayerToTarget.Length() <= hitRadius;
			}
			return base.Colliding(projHitbox, targetHitbox);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			int num = 10;
			int num2 = 0;
			Vector2 vector = Projectile.velocity;
			float num3 = 0.2f;
			if (Projectile.ai[0] == 1f || Projectile.ai[0] == 5f)
			{
				num3 = 0.4f;
			}
			if (Projectile.ai[0] == 6f)
			{
				num3 = 0f;
			}
			if (Projectile.oldVelocity.X != Projectile.velocity.X)
			{
				if (Math.Abs(Projectile.oldVelocity.X) > 4f)
				{
					num2 = 1;
				}
				Projectile.velocity.X = (0f - Projectile.oldVelocity.X) * num3;
				Projectile.localAI[0] += 1f;
			}
			if (Projectile.oldVelocity.Y != Projectile.velocity.Y)
			{
				if (Math.Abs(Projectile.oldVelocity.Y) > 4f)
				{
					num2 = 1;
				}
				Projectile.velocity.Y = (0f - Projectile.oldVelocity.Y) * num3;
				Projectile.localAI[0] += 1f;
			}
			if (Projectile.ai[0] == 1f)
			{
				Projectile.ai[0] = 5f;
				Projectile.localNPCHitCooldown = num;
				Projectile.netUpdate = true;
				Point scanAreaStart = Projectile.TopLeft.ToTileCoordinates();
				Point scanAreaEnd = Projectile.BottomRight.ToTileCoordinates();
				num2 = 2;
				Projectile.CreateImpactExplosion(2, Projectile.Center, ref scanAreaStart, ref scanAreaEnd, Projectile.width, out var causedShockwaves);
				Projectile.CreateImpactExplosion2_FlailTileCollision(Projectile.Center, causedShockwaves, vector);
				Projectile.position -= vector;
			}
			if (num2 > 0)
			{
				Projectile.netUpdate = true;
				for (int i = 0; i < num2; i++)
				{
					Collision.HitTiles(Projectile.position, vector, Projectile.width, Projectile.height);
				}
				SoundEngine.PlaySound(SoundID.Dig, new Vector2((int)Projectile.position.X, (int)Projectile.position.Y));
			}
			if (Projectile.ai[0] != 3f && Projectile.ai[0] != 0f && Projectile.ai[0] != 5f && Projectile.ai[0] != 6f && Projectile.localAI[0] >= 10f)
			{
				Projectile.ai[0] = 4f;
				Projectile.netUpdate = true;
			}
			/*if (wet)
			{
				wetVelocity = velocity;
			}*/
			return false;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			if (!player.active || player.dead || player.noItems || player.CCed || Vector2.Distance(Projectile.Center, player.Center) > 900f)
			{
				Projectile.Kill();
				return;
			}
			if (Main.myPlayer == Projectile.owner && Main.mapFullscreen)
			{
				Projectile.Kill();
				return;
			}
			Vector2 mountedCenter = player.MountedCenter;
			//bool doFastThrowDust = false;
			bool flag = true;
			bool flag2 = false;
			int num = 10;
			float num11 = 24f;
			float num16 = 800f;
			float num17 = 3f;
			float num18 = 16f;
			float num19 = 6f;
			float num20 = 48f;
			float num21 = 1f;
			float num22 = 14f;
			int num2 = 60;
			int num3 = 10;
			int num4 = 15;
			int num5 = 10;
			int num6 = num + 5;

			//we use the Dao of Pow because thats what we closestly relate to
			num = 13;
			num11 = 21f;
			num18 = 20f;
			num20 = 24f;
			num4 = 12;

			float num7 = player.GetTotalAttackSpeed(DamageClass.Melee);
			num11 *= num7;
			num21 *= num7;
			num22 *= num7;
			num17 *= num7;
			num18 *= num7;
			num19 *= num7;
			num20 *= num7;
			float num8 = num11 * (float)num;
			float num9 = num8 + 160f;
			Projectile.localNPCHitCooldown = num3;
			bool isOffspring = false;
			Projectile projectileParent = null;
			if (Projectile.ai[2] > -1)
			{
				projectileParent = Main.projectile[(int)Projectile.ai[2]];
				if (!projectileParent.active)
				{
					Projectile.ai[0] = 4f;
				}
				mountedCenter = projectileParent.Center;
				isOffspring = true;
			}
			switch ((int)Projectile.ai[0])
			{
				case 0:
					{
						flag2 = true;
						if (Projectile.owner == Main.myPlayer)
						{
							Vector2 mouseWorld = Main.MouseWorld;
							Vector2 vector3 = mountedCenter.DirectionTo(mouseWorld).SafeNormalize(Vector2.UnitX * (float)player.direction);
							player.ChangeDir((vector3.X > 0f) ? 1 : (-1));
							if (!player.channel)
							{
								Projectile.ai[0] = 1f;
								Projectile.ai[1] = 0f;
								Projectile.velocity = vector3 * num11 + player.velocity;
								Projectile.Center = mountedCenter;
								Projectile.netUpdate = true;
								Projectile.ResetLocalNPCHitImmunity();
								Projectile.localNPCHitCooldown = num5;
								break;
							}
						}
						Projectile.localAI[1] += 1f;
						Vector2 vector4 = Utils.RotatedBy(new Vector2((float)player.direction), (double)((float)Math.PI * 10f * (Projectile.localAI[1] / 60f) * (float)player.direction), default(Vector2));
						vector4.Y *= 0.8f;
						if (vector4.Y * player.gravDir > 0f)
						{
							vector4.Y *= 0.5f;
						}
						Projectile.Center = mountedCenter + vector4 * 30f;
						Projectile.velocity = Vector2.Zero;
						Projectile.localNPCHitCooldown = num4;
						break;
					}
				case 1:
					{
						//doFastThrowDust = true;
						bool flag3 = Projectile.ai[1]++ >= (float)num;
						flag3 |= Projectile.Distance(mountedCenter) >= num16;
						if (player.controlUseItem)
						{
							Projectile.ai[0] = 6f;
							Projectile.ai[1] = 0f;
							Projectile.netUpdate = true;
							Projectile.velocity *= 0.2f;
							//This is where we summon flail type projectiles similar to the drippler crippler
							break;
						}
						if (flag3)
						{
							Projectile.ai[0] = 2f;
							Projectile.ai[1] = 0f;
							Projectile.netUpdate = true;
							Projectile.velocity *= 0.3f;
							//Same here, best place to spawn a flail projectile
						}
						player.ChangeDir((player.Center.X < Projectile.Center.X) ? 1 : (-1));
						Projectile.localNPCHitCooldown = num5;
						break;
					}
				case 2:
					{
						Vector2 vector = Projectile.DirectionTo(mountedCenter).SafeNormalize(Vector2.Zero);
						if (Projectile.Distance(mountedCenter) <= num18)
						{
							Projectile.Kill();
							return;
						}
						if (player.controlUseItem)
						{
							Projectile.ai[0] = 6f;
							Projectile.ai[1] = 0f;
							Projectile.netUpdate = true;
							Projectile.velocity *= 0.2f;
						}
						else
						{
							Projectile.velocity *= 0.98f;
							Projectile.velocity = Projectile.velocity.MoveTowards(vector * num18, num17);
							player.ChangeDir((player.Center.X < Projectile.Center.X) ? 1 : (-1));
						}
						break;
					}
				case 3:
					{
						if (!isOffspring && !player.controlUseItem)
						{
							Projectile.ai[0] = 4f;
							Projectile.ai[1] = 0f;
							Projectile.netUpdate = true;
							break;
						}
						float num10 = Projectile.Distance(mountedCenter);
						Projectile.tileCollide = Projectile.ai[1] == 1f;
						bool flag4 = num10 <= num8;
						if (flag4 != Projectile.tileCollide)
						{
							Projectile.tileCollide = flag4;
							Projectile.ai[1] = (Projectile.tileCollide ? 1 : 0);
							Projectile.netUpdate = true;
						}
						if (num10 > (float)num2)
						{
							if (num10 >= num8)
							{
								Projectile.velocity *= 0.5f;
								Projectile.velocity = Projectile.velocity.MoveTowards(Projectile.DirectionTo(mountedCenter).SafeNormalize(Vector2.Zero) * num22, num22);
							}
							Projectile.velocity *= 0.98f;
							Projectile.velocity = Projectile.velocity.MoveTowards(Projectile.DirectionTo(mountedCenter).SafeNormalize(Vector2.Zero) * num22, num21);
						}
						else
						{
							if (Projectile.velocity.Length() < 6f)
							{
								Projectile.velocity.X *= 0.96f;
								Projectile.velocity.Y += 0.2f;
							}
							if (!isOffspring && player.velocity.X == 0f)
							{
								Projectile.velocity.X *= 0.96f;
							}
							else if (projectileParent.velocity.X == 0f)
							{
								Projectile.velocity.X *= 0.96f;
							}
						}
						if (!isOffspring)
							player.ChangeDir((player.Center.X < Projectile.Center.X) ? 1 : (-1));
						break;
					}
				case 4:
					{
						Projectile.tileCollide = false;
						Vector2 vector2 = Projectile.DirectionTo(mountedCenter).SafeNormalize(Vector2.Zero);
						if (Projectile.Distance(mountedCenter) <= num20)
						{
							Projectile.Kill();
							return;
						}
						Projectile.velocity *= 0.98f;
						Projectile.velocity = Projectile.velocity.MoveTowards(vector2 * num20, num19);
						Vector2 target = Projectile.Center + Projectile.velocity;
						Vector2 value = mountedCenter.DirectionFrom(target).SafeNormalize(Vector2.Zero);
						if (Vector2.Dot(vector2, value) < 0f)
						{
							Projectile.Kill();
							return;
						}
						player.ChangeDir((player.Center.X < Projectile.Center.X) ? 1 : (-1));
						break;
					}
				case 5:
					if (Projectile.ai[1]++ >= (float)num6)
					{
						Projectile.ai[0] = 6f;
						Projectile.ai[1] = 0f;
						Projectile.netUpdate = true;
					}
					else
					{
						Projectile.localNPCHitCooldown = num5;
						Projectile.velocity.Y += 0.6f;
						Projectile.velocity.X *= 0.95f;
						player.ChangeDir((player.Center.X < Projectile.Center.X) ? 1 : (-1));
					}
					break;
				case 6:
					if (!player.controlUseItem || Projectile.Distance(mountedCenter) > num9)
					{
						Projectile.ai[0] = 4f;
						Projectile.ai[1] = 0f;
						Projectile.netUpdate = true;
						break;
					}
					if (!Projectile.shimmerWet)
					{
						Projectile.velocity.Y += 0.8f;
					}
					Projectile.velocity.X *= 0.95f;
					player.ChangeDir((player.Center.X < Projectile.Center.X) ? 1 : (-1));
					break;
			}
			Projectile.direction = ((Projectile.velocity.X > 0f) ? 1 : (-1));
			Projectile.spriteDirection = Projectile.direction;
			Projectile.ownerHitCheck = flag2;
			if (flag)
			{
				if (Projectile.velocity.Length() > 1f)
				{
					Projectile.rotation = Projectile.velocity.ToRotation() + Projectile.velocity.X * 0.1f;
				}
				else
				{
					Projectile.rotation += Projectile.velocity.X * 0.1f;
				}
			}
			Projectile.timeLeft = 2;
			if (!isOffspring)
			{
				player.heldProj = Projectile.whoAmI;
				player.SetDummyItemTime(2);
				player.itemRotation = Projectile.DirectionFrom(mountedCenter).ToRotation();
				if (Projectile.Center.X < mountedCenter.X)
				{
					player.itemRotation += (float)Math.PI;
				}
				player.itemRotation = MathHelper.WrapAngle(player.itemRotation);
			}
			//Projectile.AI_015_Flails_Dust(doFastThrowDust); //Spawn dusts from a flail like the Blue moon or sunfury
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Player player = Main.player[Projectile.owner];
			Vector2 playerArmPosition = Main.GetPlayerArmPosition(Projectile);
			if (Projectile.ai[2] > -1 && Main.projectile[(int)Projectile.ai[2]].active)
			{
				playerArmPosition = Main.projectile[(int)Projectile.ai[2]].Center;
			}
			playerArmPosition -= Vector2.UnitY * player.gfxOffY;
			Asset<Texture2D> asset = Projectile.frame == 0 ? chainAsset : chainAsset2;
			Rectangle? sourceRectangle = null;
			float num = 0f;
			Vector2 origin = (sourceRectangle.HasValue ? (sourceRectangle.Value.Size() / 2f) : (asset.Size() / 2f));
			Vector2 center = Projectile.Center;
			Vector2 v = playerArmPosition.MoveTowards(center, 4f) - center;
			Vector2 vector = v.SafeNormalize(Vector2.Zero);
			float num2 = (float)(sourceRectangle.HasValue ? sourceRectangle.Value.Height : asset.Height()) + num;
			float rotation = vector.ToRotation() + (float)Math.PI / 2f;
			int num3 = 0;
			float num4 = v.Length() + num2 / 2f;
			while (num4 > 0f)
			{
				Color color = Lighting.GetColor((int)center.X / 16, (int)(center.Y / 16f));
				Main.spriteBatch.Draw(asset.Value, center - Main.screenPosition, sourceRectangle, color, rotation, origin, 1f, (SpriteEffects)0, 0f);
				center += vector * num2;
				num3++;
				num4 -= num2;
			}
			return true;
		}
	}
}
