using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using TheConfectionRebirth.Buffs;
using TheConfectionRebirth.Buffs.NeapoliniteBuffs;
using TheConfectionRebirth.Dusts;
using TheConfectionRebirth.Projectiles;

namespace TheConfectionRebirth
{
	public class ConfectionPlayer : ModPlayer
	{
		public bool cookiestPet;

		public bool SacchariteLashed;
		public bool candleFire;
		public int candleFlameDelay = 0;

		public bool neapoliniteMelee;
		public int meleeVanilla;
		public bool neapoliniteRanger;
		public bool neapoliniteMage;
		public int mageStrawberry;
		public bool neapoliniteSummoner;
		public int summonerCone;

		public int vanillaValorDamage;
		public int vanillaTimer;

		public int strawberryManaHealed;
		public int strawberryTimer;
		public int strawberryStartingMana;
		public int strawberrySpawnStrawTimer;

		public int coneTimer;
		public int coneSummonID;
		public static bool hasSwirlBuff(Player player) => player.HasBuff(ModContent.BuffType<SwirlySwarmI>()) || player.HasBuff(ModContent.BuffType<SwirlySwarmII>()) || player.HasBuff(ModContent.BuffType<SwirlySwarmIII>()) || player.HasBuff(ModContent.BuffType<SwirlySwarmIV>()) || player.HasBuff(ModContent.BuffType<SwirlySwarmV>());

		public int neapolinitePowerLevel;

		public int bakersDozenHitCount = 0;

		public override void ResetEffects()
		{
			cookiestPet = false;

			if (!candleFire)
			{
				candleFlameDelay = 0;
			}
			SacchariteLashed = false;
			candleFire = false;
			
			if (!neapoliniteMelee)
			{
				vanillaValorDamage = 0;
				vanillaTimer = 0;
				meleeVanilla = -1;
			}
			if (vanillaTimer <= 0)
			{
				vanillaValorDamage = 0;
			}
			if (!neapoliniteMage)
			{
				strawberryManaHealed = 0;
				strawberryTimer = 0;
				strawberryStartingMana = 0;
				strawberrySpawnStrawTimer = 0;
				mageStrawberry = -1;
			}
			if (strawberryTimer <= 0)
			{
				strawberryManaHealed = 0;
			}
			if (!neapoliniteSummoner)
			{
				coneTimer = 0;
				summonerCone = -1;
			}
			if (!hasSwirlBuff(Player))
			{
				if (coneSummonID > -1)
				{
					Main.projectile[coneSummonID].Kill();
					coneSummonID = -1;
				}
			}
			neapoliniteMelee = false;
			neapoliniteRanger = false;
			neapoliniteMage = false;
			neapoliniteSummoner = false;
			neapolinitePowerLevel = 0;
		}

		public override void UpdateDead()
		{
			SacchariteLashed = false;
			candleFire = false;
		}

		public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
		{
			if (SacchariteLashed)
			{
				if (Main.rand.NextBool(4))
				{
					int dust = Dust.NewDust(Player.Center + new Vector2(Main.rand.NextFloat(-(Player.width / 2), Player.width / 2), Main.rand.NextFloat(-(Player.height / 2), Player.height / 2)), 10, 10, ModContent.DustType<SacchariteDust>());
					drawInfo.DustCache.Add(dust);
				}
			}
			if (candleFire && candleFlameDelay <= 0)
			{
				if (Main.rand.Next(4) < 3)
				{
					Dust dust = Dust.NewDustDirect(new Vector2(Player.position.X - 2f, Player.position.Y - 2f), Player.width + 4, Player.height + 4, DustID.Torch, Player.velocity.X * 0.4f, Player.velocity.Y * 0.4f, 100, default(Color), 3.5f);
					dust.noGravity = true;
					dust.velocity *= 1.8f;
					dust.velocity.Y -= 0.5f;
					if (Main.rand.NextBool(4))
					{
						dust.noGravity = false;
						dust.scale *= 0.5f;
					}
					drawInfo.DustCache.Add(dust.dustIndex);
				}
				Lighting.AddLight((int)(Player.position.X / 16f), (int)(Player.position.Y / 16f + 1f), 1f, 0.3f, 0.1f);
			}
		}

		public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
		{
			if (candleFire)
			{
				WeightedRandom<string> deathmessage = new();
				deathmessage.Add(Language.GetTextValue("Mods.TheConfectionRebirth.PlayerDeathReason.CandleFire.0", Player.name));
				deathmessage.Add(Language.GetTextValue("Mods.TheConfectionRebirth.PlayerDeathReason.CandleFire.1", Player.name));
				deathmessage.Add(Language.GetTextValue("Mods.TheConfectionRebirth.PlayerDeathReason.CandleFire.2", Player.name));
				deathmessage.Add(Language.GetTextValue("Mods.TheConfectionRebirth.PlayerDeathReason.CandleFire.3", Player.name));
				deathmessage.Add(Language.GetTextValue("Mods.TheConfectionRebirth.PlayerDeathReason.CandleFire.4", Player.name));
				deathmessage.Add(Language.GetTextValue("Mods.TheConfectionRebirth.PlayerDeathReason.CandleFire.5", Player.name));
				deathmessage.Add(Language.GetTextValue("Mods.TheConfectionRebirth.PlayerDeathReason.CandleFire.6", Player.name));
				damageSource = PlayerDeathReason.ByCustomReason(deathmessage);
				return true;
			}
			return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genGore, ref damageSource);
		}


		public override void UpdateBadLifeRegen()
		{
			if (candleFire && candleFlameDelay <= 0)
			{
				if (Player.lifeRegen > 0)
				{
					Player.lifeRegen = 0;
				}
				Player.lifeRegenTime = 0f;
				Player.lifeRegen -= 8;
			}
		}

		public override void PostUpdate()
		{
			if (neapoliniteMelee)
			{
				int rank = Math.Min(vanillaValorDamage, 1750) / 350 - 1;
				if (rank != meleeVanilla)
				{
					vanillaTimer = 300;
				}
				meleeVanilla = rank;

				IncrimentNeapoliniteBuffPower(Player, meleeVanilla);
				if (vanillaTimer > 0)
					vanillaTimer--;
			}
			if (neapoliniteRanger)
			{
				int rank;
				float vel = Player.velocity.Length();
				if (vel >= 11f)
					rank = 4;
				else
				{
					float speed = vel / 2.2f;
					rank = (int)(speed - 1);
				}
				IncrimentNeapoliniteBuffPower(Player, rank, 1);
			}
			if (neapoliniteMage)
			{
				int rank = (int)(strawberryManaHealed / 25);
				IncrimentNeapoliniteBuffPower(Player, rank - 1, 2);
				if (strawberryTimer > 0)
					strawberryTimer--;

				if (Player.statMana != Player.statManaMax2)
				{
					if (Player.manaRegenDelay == 0)
					{
						if (strawberryManaHealed <= 0 && strawberryTimer <= 0)
						{
							strawberryTimer = 180;
							strawberryStartingMana = Player.statMana;
						}
						if (strawberryTimer > 0)
						{
							strawberryManaHealed += (Player.statMana - strawberryStartingMana);
						}
						strawberryStartingMana = Player.statMana;
					}
				}

				if (strawberrySpawnStrawTimer > 60)
				{
					strawberrySpawnStrawTimer = 0;

					for (int i = 0; i <= mageStrawberry; i++)
					{
						int damage = 1;
						if (Player.HeldItem.DamageType == DamageClass.Magic)
						{
							damage = Player.HeldItem.damage / 2;
						}
						Vector2 pointPoisition = Player.RotatedRelativePoint(Player.MountedCenter);
						float mouseX = (float)Main.mouseX + Main.screenPosition.X - pointPoisition.X;
						float mouseY = (float)Main.mouseY + Main.screenPosition.Y - pointPoisition.Y;
						float f = Main.rand.NextFloat() * ((float)Math.PI * 2f);
						float width = 20f;
						float height = 60f;
						Vector2 pos2 = pointPoisition + f.ToRotationVector2() * MathHelper.Lerp(width, height, Main.rand.NextFloat());
						for (int avaliablePos = 0; avaliablePos < 50; avaliablePos++)
						{
							pos2 = pointPoisition + f.ToRotationVector2() * MathHelper.Lerp(width, height, Main.rand.NextFloat());
							if (Collision.CanHit(pointPoisition, 0, 0, pos2 + (pos2 - pointPoisition).SafeNormalize(Vector2.UnitX) * 8f, 0, 0))
							{
								break;
							}
							f = Main.rand.NextFloat() * ((float)Math.PI * 2f);
						}
						Vector2 velc = Main.MouseWorld - pos2;
						Vector2 pos = Utils.SafeNormalize(new Vector2(mouseX, mouseY), Vector2.UnitY) * 14f;
						velc = velc.SafeNormalize(pos) * 14f;
						velc = Vector2.Lerp(velc, pos, 0.25f);
						Projectile.NewProjectile(new EntitySource_Misc(""), pos2, velc, ModContent.ProjectileType<StrawberryStrike>(), damage, Player.HeldItem.knockBack, Player.whoAmI);
					}
				}
			}
			if (neapoliniteSummoner)
			{
				int rank = summonerCone;
				IncrimentNeapoliniteBuffPower(Player, rank, 3);
				coneTimer++;
				if (summonerCone < 4)
				{
					if (coneTimer >= 60 * 8)
					{
						summonerCone++;
						coneTimer = 0;
					}
				}
				if (coneTimer > 60 * 8)
				{
					coneTimer = 60 * 8;
				}
				if (summonerCone < -1)
					summonerCone = -1;
			}
			if (hasSwirlBuff(Player))
			{
				List<int> validSummonIDs = new List<int>();
				for (int i = 0; i < Main.maxProjectiles; i++)
				{
					Projectile proj = Main.projectile[i];
					if (proj.active && proj.minion && proj.owner == Player.whoAmI)
					{
						validSummonIDs.Add(i);
					}
				}
				if (coneSummonID <= -1)
				{
					if (validSummonIDs.Count != 0)
					{
						int num;
						if (validSummonIDs.Count > 1)
							num = Main.rand.Next(0, validSummonIDs.Count);
						else
							num = 0;
						Projectile ghost = Main.projectile[validSummonIDs[num]];
						int ghostProjID = Projectile.NewProjectile(new EntitySource_Misc(""), Player.position, Vector2.Zero, ghost.type, ghost.damage, ghost.knockBack, Player.whoAmI);
						Projectile ghostProj = Main.projectile[ghostProjID];
						ghostProj.minionSlots = 0;
						ghostProj.minion = false;
						ghostProj.minionPos = validSummonIDs.Count;
						coneSummonID = ghostProjID;
					}
					else
					{
						Player.GetDamage(DamageClass.Summon) += 0.1f;
					}
				}
				else
				{
					Projectile proj = Main.projectile[coneSummonID];
					if (!proj.active)
					{
						coneSummonID = -1;
					}
					else
					{
						proj.minionPos = validSummonIDs.Count;
					}
				}
			}
			if (neapolinitePowerLevel > 0)
			{
				for (int j = 0; j < neapolinitePowerLevel; j++)
				{
					bool alreadyExists = false;
					for (int i = 0; i < Main.maxProjectiles; i++)
					{
						Projectile proj = Main.projectile[i];
						if (proj.active && proj.type == ModContent.ProjectileType<NeapoliniteCookies>() && proj.ai[0] == j && proj.owner == Player.whoAmI)
						{
							alreadyExists = true;
						}
					}
					if (!alreadyExists)
					{
						Projectile.NewProjectile(new EntitySource_Misc(""), Player.Center, Vector2.Zero, ModContent.ProjectileType<NeapoliniteCookies>(), 0, 0, Player.whoAmI, j);
					}
				}
			}
			if (candleFire && Collision.WetCollision(Player.position, Player.width, Player.height))
			{
				Player.ClearBuff(ModContent.BuffType<HumanCandle>());
			}
			if (candleFire && Main.rand.NextBool(200))
			{
				candleFlameDelay = Main.rand.Next(40, 100);
			}
			if (candleFlameDelay > 0)
			{
				candleFlameDelay--;
			}
		}

		public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (neapoliniteMelee)
			{
				if (item.CountsAsClass(DamageClass.Melee))
					OnHitValor(hit.Damage);
			}
		}

		public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (neapoliniteMelee)
			{
				if (proj.CountsAsClass(DamageClass.Melee))
					OnHitValor(hit.Damage);
			}
		}

		public void OnHitValor(int damage)
		{
			if (neapoliniteMelee)
			{
				if (vanillaValorDamage <= 0)
				{
					vanillaTimer = 300;
				}
				if (vanillaTimer > 0)
				{
					vanillaValorDamage += damage;
				}
			}
		}

		public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
		{
			if (neapoliniteSummoner)
			{
				coneTimer = 0;
				summonerCone -= 2;
			}
		}

		public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
		{
			if (neapoliniteSummoner)
			{
				coneTimer = 0;
				summonerCone -= 2;
			}
		}

		public override void GetDyeTraderReward(List<int> rewardPool)
		{
			rewardPool.Add(ModContent.ItemType<Items.CandyCornDye>());
			rewardPool.Add(ModContent.ItemType<Items.FoaminWispDye>());
			rewardPool.Add(ModContent.ItemType<Items.GummyWispDye>());
			rewardPool.Add(ModContent.ItemType<Items.SwirllingChocolateDye>());
		}

		public static void IncrimentNeapoliniteBuffPower(Player player, int Power, int BuffType = 0)
		{
			if (Power < 0 || Power > 4)
			{
				return; 
			}

			int time = 300;
			int type = 0;
			int[][] validTypes = new int[4][] {
				new int[5] { ModContent.BuffType<VanillaValorI>(), ModContent.BuffType<VanillaValorII>(), ModContent.BuffType<VanillaValorIII>(), ModContent.BuffType<VanillaValorIV>(), ModContent.BuffType<VanillaValorV>() },
				new int[5] { ModContent.BuffType<ChocolateChargeI>(), ModContent.BuffType<ChocolateChargeII>(), ModContent.BuffType<ChocolateChargeIII>(), ModContent.BuffType<ChocolateChargeIV>(), ModContent.BuffType<ChocolateChargeV>() },
				new int[5] { ModContent.BuffType<StrawberryStrikeI>(), ModContent.BuffType<StrawberryStrikeII>(), ModContent.BuffType<StrawberryStrikeIII>(), ModContent.BuffType<StrawberryStrikeIV>(), ModContent.BuffType<StrawberryStrikeV>() },
				new int[5] { ModContent.BuffType<SwirlySwarmI>(), ModContent.BuffType<SwirlySwarmII>(), ModContent.BuffType<SwirlySwarmIII>(), ModContent.BuffType<SwirlySwarmIV>(), ModContent.BuffType<SwirlySwarmV>() }
			};
			type = validTypes[BuffType][Power];

			for (int j = 0; j < Player.MaxBuffs; j++)
			{
				for (int k = 0; k < 5; k++)
				{
					if (player.buffType[j] == validTypes[BuffType][k])
					{
						if (k > Power)
						{
							return;
						}
					}
				}
			}

			if (type != 0)
			{
				if (Power > 0)
				{
					for (int i = 0; i < Power; i++)
					{
						if (player.HasBuff(validTypes[BuffType][i]))
							player.ClearBuff(validTypes[BuffType][i]);
					}
				}
				player.AddBuff(type, time);
			}
		}

		public static int NeapoliniteHelmetNumber(PlayerDrawSet drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			int HelmetType = 0;
			if (drawPlayer.armor[10].type == ModContent.ItemType<Items.Armor.NeapoliniteHat>())
			{
				HelmetType = 4;
			}
			else if (drawPlayer.armor[10].type == ModContent.ItemType<Items.Armor.NeapoliniteHeadgear>())
			{
				HelmetType = 3;
			}
			else if (drawPlayer.armor[10].type == ModContent.ItemType<Items.Armor.NeapoliniteHelmet>())
			{
				HelmetType = 2;
			}
			else if (drawPlayer.armor[10].type == ModContent.ItemType<Items.Armor.NeapoliniteMask>())
			{
				HelmetType = 1;
			}
			else if (drawPlayer.armor[0].type == ModContent.ItemType<Items.Armor.NeapoliniteHat>())
			{
				HelmetType = 4;
			}
			else if (drawPlayer.armor[0].type == ModContent.ItemType<Items.Armor.NeapoliniteHeadgear>())
			{
				HelmetType = 3;
			}
			else if (drawPlayer.armor[0].type == ModContent.ItemType<Items.Armor.NeapoliniteHelmet>())
			{
				HelmetType = 2;
			}
			else if (drawPlayer.armor[0].type == ModContent.ItemType<Items.Armor.NeapoliniteMask>())
			{
				HelmetType = 1;
			}
			return HelmetType;
		}
	}

	public class NeapoliniteConeMailDrawing : PlayerDrawLayer
	{
		public override Position GetDefaultPosition()
		{
			return new AfterParent(PlayerDrawLayers.Torso);
		}

		protected override void Draw(ref PlayerDrawSet drawinfo)
		{
			int HelmetType = ConfectionPlayer.NeapoliniteHelmetNumber(drawinfo);
			if (drawinfo.usesCompositeTorso)
			{
				DrawComposite(ref drawinfo);
			}
			else if (drawinfo.drawPlayer.body > 0 && HelmetType > 0) //Old chestplate renderer, here incase some weird fucked up mod wants to use the old renderer
			{
				Texture2D male = (Texture2D)ModContent.Request<Texture2D>("TheConfectionRebirth/Items/Armor/NeapoliniteConeMail_Body_" + HelmetType);
				Texture2D female = (Texture2D)ModContent.Request<Texture2D>("TheConfectionRebirth/Items/Armor/NeapoliniteConeMail_Body_" + HelmetType);
				Rectangle bodyFrame = drawinfo.drawPlayer.bodyFrame;
				int num = drawinfo.armorAdjust;
				bodyFrame.X += num;
				bodyFrame.Width -= num;
				if (drawinfo.drawPlayer.direction == -1)
				{
					num = 0;
				}
				if (!drawinfo.drawPlayer.invis || (drawinfo.drawPlayer.body != 21 && drawinfo.drawPlayer.body != 22))
				{
					Texture2D texture = (drawinfo.drawPlayer.Male ? male : female);
					DrawData item2 = new DrawData(texture, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)) + num), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), bodyFrame, drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
					item2.shader = drawinfo.cBody;
					drawinfo.DrawDataCache.Add(item2);
				}
			}
		}

		public static void DrawComposite(ref PlayerDrawSet drawinfo)
		{
			int HelmetType = ConfectionPlayer.NeapoliniteHelmetNumber(drawinfo);
			Vector2 vector = new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2));
			Vector2 vector2 = Main.OffsetsPlayerHeadgear[drawinfo.drawPlayer.bodyFrame.Y / drawinfo.drawPlayer.bodyFrame.Height];
			vector2.Y -= 2f;
			vector += vector2 * (float)(-((Enum)drawinfo.playerEffect).HasFlag((Enum)(object)(SpriteEffects)2).ToDirectionInt());
			float bodyRotation = drawinfo.drawPlayer.bodyRotation;
			Vector2 val = vector;
			Vector2 bodyVect = drawinfo.bodyVect;
			Vector2 compositeOffset_BackArm = GetCompositeOffset_BackArm(ref drawinfo);
			_ = val + compositeOffset_BackArm;
			bodyVect += compositeOffset_BackArm;
			if (drawinfo.drawPlayer.body > 0 && HelmetType > 0)
			{
				if (!drawinfo.drawPlayer.invis)
				{
					Texture2D value = (Texture2D)ModContent.Request<Texture2D>("TheConfectionRebirth/Items/Armor/NeapoliniteConeMail_Body_" + HelmetType);
					PlayerDrawLayers.DrawCompositeArmorPiece(ref drawinfo, CompositePlayerDrawContext.Torso, new DrawData(value, vector, drawinfo.compTorsoFrame, drawinfo.colorArmorBody, bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect)
					{
						shader = drawinfo.cBody
					});
				}
			}
			if (drawinfo.drawFloatingTube)
			{
				drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Extra[105].Value, vector, (Rectangle?)new Rectangle(0, 56, 40, 56), drawinfo.floatingTubeColor, bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect, 0f)
				{
					shader = drawinfo.cFloatingTube
				});
			}
		}

		private static Vector2 GetCompositeOffset_BackArm(ref PlayerDrawSet drawinfo)
		{
			return new Vector2((float)(6 * ((!((Enum)drawinfo.playerEffect).HasFlag((Enum)(object)(SpriteEffects)1)) ? 1 : (-1))), (float)(2 * ((!((Enum)drawinfo.playerEffect).HasFlag((Enum)(object)(SpriteEffects)2)) ? 1 : (-1))));
		}
	}
	
	public class NeapoliniteConeMailArmDrawing : PlayerDrawLayer
	{
		public override Position GetDefaultPosition()
		{
			return new AfterParent(PlayerDrawLayers.ArmOverItem);
		}

		protected override void Draw(ref PlayerDrawSet drawinfo)
		{
			int HelmetType = ConfectionPlayer.NeapoliniteHelmetNumber(drawinfo);
			if (drawinfo.usesCompositeTorso)
			{
				DrawArmComposite(ref drawinfo);
			}
			else if (drawinfo.drawPlayer.body > 0 && HelmetType > 0) //Old hand rendering, again here for when other broken mods use the old renderer
			{
				Texture2D arms = (Texture2D)ModContent.Request<Texture2D>("TheConfectionRebirth/Items/Armor/NeapoliniteConeMail_Body_" + HelmetType);
				Rectangle bodyFrame = drawinfo.drawPlayer.bodyFrame;
				int num = drawinfo.armorAdjust;
				bodyFrame.X += num;
				bodyFrame.Width -= num;
				if (drawinfo.drawPlayer.direction == -1)
				{
					num = 0;
				}
				if (drawinfo.drawPlayer.invis && (drawinfo.drawPlayer.body == 21 || drawinfo.drawPlayer.body == 22))
				{
					return;
				}
				DrawData item;
				item = new DrawData(arms, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)) + num), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), bodyFrame, drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cBody;
				drawinfo.DrawDataCache.Add(item);
			}
		}

		public static void DrawArmComposite(ref PlayerDrawSet drawinfo)
		{
			int HelmetType = ConfectionPlayer.NeapoliniteHelmetNumber(drawinfo);
			Vector2 vector = new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2));
			Vector2 vector2 = Main.OffsetsPlayerHeadgear[drawinfo.drawPlayer.bodyFrame.Y / drawinfo.drawPlayer.bodyFrame.Height];
			vector2.Y -= 2f;
			vector += vector2 * (float)(-((Enum)drawinfo.playerEffect).HasFlag((Enum)(object)(SpriteEffects)2).ToDirectionInt());
			float bodyRotation = drawinfo.drawPlayer.bodyRotation;
			float rotation = drawinfo.drawPlayer.bodyRotation + drawinfo.compositeFrontArmRotation;
			Vector2 bodyVect = drawinfo.bodyVect;
			Vector2 compositeOffset_FrontArm = GetCompositeOffset_FrontArm(ref drawinfo);
			bodyVect += compositeOffset_FrontArm;
			vector += compositeOffset_FrontArm;
			Vector2 position = vector + drawinfo.frontShoulderOffset;
			if (drawinfo.compFrontArmFrame.X / drawinfo.compFrontArmFrame.Width >= 7)
			{
				vector += new Vector2((float)((!((Enum)drawinfo.playerEffect).HasFlag((Enum)(object)(SpriteEffects)1)) ? 1 : (-1)), (float)((!((Enum)drawinfo.playerEffect).HasFlag((Enum)(object)(SpriteEffects)2)) ? 1 : (-1)));
			}
			_ = drawinfo.drawPlayer.invis;
			int num2 = (drawinfo.compShoulderOverFrontArm ? 1 : 0);
			int num3 = ((!drawinfo.compShoulderOverFrontArm) ? 1 : 0);
			int num4 = ((!drawinfo.compShoulderOverFrontArm) ? 1 : 0);
			bool flag = !drawinfo.hidesTopSkin;
			if (drawinfo.drawPlayer.body > 0 && HelmetType > 0)
			{
				if (!drawinfo.drawPlayer.invis)
				{
					Texture2D value = (Texture2D)ModContent.Request<Texture2D>("TheConfectionRebirth/Items/Armor/NeapoliniteConeMail_Body_" + HelmetType);
					for (int i = 0; i < 2; i++)
					{
						if (i == num2 && !drawinfo.hideCompositeShoulders)
						{
							PlayerDrawLayers.DrawCompositeArmorPiece(ref drawinfo, CompositePlayerDrawContext.FrontShoulder, new DrawData(value, position, drawinfo.compFrontShoulderFrame, drawinfo.colorArmorBody, bodyRotation, bodyVect, 1f, drawinfo.playerEffect)
							{
								shader = drawinfo.cBody
							});
						}
						if (i == num3)
						{
							PlayerDrawLayers.DrawCompositeArmorPiece(ref drawinfo, CompositePlayerDrawContext.FrontArm, new DrawData(value, vector, drawinfo.compFrontArmFrame, drawinfo.colorArmorBody, rotation, bodyVect, 1f, drawinfo.playerEffect)
							{
								shader = drawinfo.cBody
							});
						}
					}
				}
			}
			if (drawinfo.drawPlayer.handon > 0)
			{
				Texture2D value2 = TextureAssets.AccHandsOnComposite[drawinfo.drawPlayer.handon].Value;
				PlayerDrawLayers.DrawCompositeArmorPiece(ref drawinfo, CompositePlayerDrawContext.FrontArmAccessory, new DrawData(value2, vector, drawinfo.compFrontArmFrame, drawinfo.colorArmorBody, rotation, bodyVect, 1f, drawinfo.playerEffect)
				{
					shader = drawinfo.cHandOn
				});
			}
		}

		private static Vector2 GetCompositeOffset_FrontArm(ref PlayerDrawSet drawinfo)
		{
			return new Vector2((float)(-5 * ((!((Enum)drawinfo.playerEffect).HasFlag((Enum)(object)(SpriteEffects)1)) ? 1 : (-1))), 0f);
		}
	}

	/*public class TopCakeCandlesDrawing : PlayerDrawLayer
	{
		public override Position GetDefaultPosition()
		{
			return new AfterParent(PlayerDrawLayers.Head);
		}

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			if (!drawPlayer.dead && ((drawPlayer.armor[10].type == 0 && drawPlayer.armor[0].type == ModContent.ItemType<Items.Armor.TopCake>()) || drawPlayer.armor[10].type == ModContent.ItemType<Items.Armor.TopCake>()))
			{
				Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("TheConfectionRebirth/Items/Armor/TopCake_Candles");

				float drawX = (int)drawInfo.Position.X + drawPlayer.width / 2;
				float drawY = (int)drawInfo.Position.Y + drawPlayer.height - drawPlayer.bodyFrame.Height / 2 - 5.5f;
				Vector2 origin = drawInfo.headVect;
				Vector2 position = new Vector2(drawX, drawY) + drawPlayer.headPosition - Main.screenPosition;
				Rectangle frame = drawPlayer.bodyFrame;
				float rotation = drawPlayer.headRotation;
				SpriteEffects spriteEffects = drawInfo.playerEffect;

				DrawData drawData = new(texture, position, frame, drawInfo.colorArmorHead, rotation, origin, 1f, spriteEffects, 0);
				drawData.shader = drawInfo.cHead;
				drawInfo.DrawDataCache.Add(drawData);
			}
		}
	}*/
}
