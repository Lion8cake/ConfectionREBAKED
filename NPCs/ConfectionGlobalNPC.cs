using System;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using static Terraria.GameContent.ItemDropRules.Chains;
using static Terraria.GameContent.ItemDropRules.Conditions;
using TheConfectionRebirth.ModSupport;
using TheConfectionRebirth.Items.Placeable;
using TheConfectionRebirth.Items;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Buffs.NeapoliniteBuffs;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Personalities;
using TheConfectionRebirth.Dusts;
using TheConfectionRebirth.Buffs;
using TheConfectionRebirth.Projectiles;
using Terraria.DataStructures;
using System.Collections.Generic;
using Terraria.GameContent.Events;
using Steamworks;
using Terraria.Localization;

namespace TheConfectionRebirth.NPCs
{
	public class ConfectionGlobalNPC : GlobalNPC
	{
		public override bool InstancePerEntity => true;

		public bool SacchariteLashed;
		public bool candleFire;
		public int candleFlameDelay = 0;
		public bool sacchariteAmmoDebuff;

		public override void ResetEffects(NPC npc)
		{
			if (!candleFire)
			{
				candleFlameDelay = 0;
			}
			SacchariteLashed = false;
			candleFire = false;
			sacchariteAmmoDebuff = false;

			if (npc.noTileCollide)
			{
				if (candleFire && npc.boss && Main.netMode != NetmodeID.MultiplayerClient && Collision.WetCollision(npc.position, npc.width, npc.height))
				{
					for (int k = 0; k < NPC.maxBuffs; k++)
					{
						if (npc.buffType[k] == ModContent.BuffType<HumanCandle>())
						{
							npc.DelBuff(k);
						}
					}
				}
			}
			else
			{
				if (candleFire && Main.netMode != NetmodeID.MultiplayerClient && Collision.WetCollision(npc.position, npc.width, npc.height))
				{
					for (int k = 0; k < NPC.maxBuffs; k++)
					{
						if (npc.buffType[k] == ModContent.BuffType<HumanCandle>())
						{
							npc.DelBuff(k);
						}
					}
				}
			}
		}

		public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
		{
			if (SacchariteLashed)
			{
				modifiers.Defense -= 4;
			}
		}

		public override void DrawEffects(NPC npc, ref Color drawColor)
		{
			if (SacchariteLashed)
			{
				if (Main.rand.NextBool(4))
				{
					Dust.NewDust(npc.Center + new Vector2(Main.rand.NextFloat(-(npc.width / 2), npc.width / 2), Main.rand.NextFloat(-(npc.height / 2), npc.height / 2)), 10, 10, ModContent.DustType<SacchariteDust>());
				}
			}
			if (candleFire && candleFlameDelay <= 0)
			{
				if (Main.rand.Next(4) < 3)
				{
					Dust dust = Dust.NewDustDirect(new Vector2(npc.position.X - 2f, npc.position.Y - 2f), npc.width + 4, npc.height + 4, DustID.Torch, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, default(Color), 3.5f);
					dust.noGravity = true;
					dust.velocity *= 1.8f;
					dust.velocity.Y -= 0.5f;
					if (Main.rand.NextBool(4))
					{
						dust.noGravity = false;
						dust.scale *= 0.5f;
					}
				}
				Lighting.AddLight((int)(npc.position.X / 16f), (int)(npc.position.Y / 16f + 1f), 1f, 0.3f, 0.1f);
			}
		}

		public override void UpdateLifeRegen(NPC npc, ref int damage)
		{
			if (candleFire && candleFlameDelay <= 0)
			{
				if (npc.lifeRegen > 0)
				{
					npc.lifeRegen = 0;
				}
				npc.lifeRegen -= 8;
			}
			if (npc.oiled && candleFire && candleFlameDelay <= 0)
			{
				if (npc.lifeRegen > 0)
				{
					npc.lifeRegen = 0;
				}
				npc.lifeRegen -= 50;
				if (damage < 10)
				{
					damage = 10;
				}
			}
			if (sacchariteAmmoDebuff)
			{
				if (npc.lifeRegen > 0)
				{
					npc.lifeRegen = 0;
				}

				int count = 0;
				for (int i = 0; i < 1000; i++)
				{
					Projectile projectile = Main.projectile[i];
					if (projectile.active && (projectile.type == ModContent.ProjectileType<SacchariteArrow>() || projectile.type == ModContent.ProjectileType<SacchariteBullet>()) && projectile.ai[1] == npc.whoAmI)
					{
						count++;
					}
				}

				float count2 = count * 0.2f;
				npc.lifeRegen -= (int)(count2 * 4 * 20);
				if (damage < count2 * 20)
				{
					damage = (int)(count2 * 20);
				}
			}
		}

		public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
		{
			if (player.InModBiome(ModContent.GetInstance<ConfectionBiome>()) && (double)player.position.Y > Main.rockLayer * 16.0 + (double)NPC.sHeight)
			{
				spawnRate = (int)((double)spawnRate * 0.65);
				maxSpawns = (int)((float)maxSpawns * 1.3f);
			}
		}

		public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
		{
			SpawnNPC_ConfectionNPC(spawnInfo, 0, out bool blockVanillaSpawn);
			if (blockVanillaSpawn)
			{
				if (pool.ContainsKey(0))
				{
					pool[0] = 0f;
				}
			}
		}

		private static int SpawnNPC_TryFindingProperGroundTileType(int spawnTileType, int x, int y)
		{
			if (!NPC.IsValidSpawningGroundTile(x, y))
			{
				for (int i = y + 1; i < y + 30; i++)
				{
					if (NPC.IsValidSpawningGroundTile(x, i))
					{
						return Main.tile[x, i].TileType;
					}
				}
			}
			return spawnTileType;
		}

		/// <summary>
		/// Used to repeat less code and to have all the spawn conditions in 1 place, this is to get as close as possible to vanilla spawning
		/// </summary>
		/// <param name="spawnInfo"></param>
		/// <param name="npcType"></param>
		/// <returns></returns>
		public static float SpawnNPC_ConfectionNPC(NPCSpawnInfo spawnInfo, int npcType)
		{
			return SpawnNPC_ConfectionNPC(spawnInfo, npcType, out _);
		}

		/// <summary>
		/// Used to repeat less code and to have all the spawn conditions in 1 place, this is to get as close as possible to vanilla spawning
		/// </summary>
		/// <param name="spawnInfo"></param>
		/// <param name="npcType"></param>
		/// <param name="blockVanillaSpawn"></param>
		/// <returns></returns>
		public static float SpawnNPC_ConfectionNPC(NPCSpawnInfo spawnInfo, int npcType, out bool blockVanillaSpawn)
		{
			blockVanillaSpawn = false;
			int x = spawnInfo.SpawnTileX;
			int y = spawnInfo.SpawnTileY;
			Player player = spawnInfo.Player;
			int spawnTile = spawnInfo.SpawnTileType;
			int tileType = Main.tile[x, y].TileType; //num56
			tileType = SpawnNPC_TryFindingProperGroundTileType(tileType, x, y);
			int wall = Main.tile[x, y - 1].WallType; //num58
			if (Main.tile[x, y - 2].WallType == 244 || Main.tile[x, y].WallType == 244)
			{
				wall = 244;
			}
			//called surface yet gets above the ROCK layer not surface, this includes the dirt layer too
			bool surface = (double)y <= Main.rockLayer; //num9
														//true surface and its additional conditions
			bool surface2 = (double)y <= Main.worldSurface; //num13
			bool dirtLayer = (double)y >= Main.rockLayer; //num14
			bool raining = Main.cloudAlpha > 0f; //num17
			bool beach = (double)y <= Main.worldSurface && (x < WorldGen.beachDistance || x > Main.maxTilesX - WorldGen.beachDistance); //num16
			if (Main.remixWorld)
			{
				raining = Main.raining;
				dirtLayer = ((double)y > Main.worldSurface && (double)y < Main.rockLayer);
				surface = (double)y > Main.rockLayer && y <= Main.maxTilesY - 190;
				if (player.ZoneCorrupt || player.ZoneCrimson)
				{
					beach = false;
				}
				if ((double)x < (double)Main.maxTilesX * 0.43 || (double)x > (double)Main.maxTilesX * 0.57)
				{
					if ((double)y > Main.rockLayer - 200.0 && y < Main.maxTilesY - 200 && Main.rand.Next(2) == 0)
					{
						beach = true;
					}
				}
				if ((double)y > Main.rockLayer - 20.0)
				{
					if (y <= Main.maxTilesY - 190 && !Main.rand.NextBool(3))
					{
						surface2 = true;
						Main.dayTime = false;
						if (Main.rand.NextBool(2))
						{
							Main.dayTime = true;
						}
					}
					else if ((Main.bloodMoon || (Main.eclipse && Main.dayTime)) && (double)x > (double)Main.maxTilesX * 0.38 + 50.0 && (double)x < (double)Main.maxTilesX * 0.62)
					{
						surface2 = true;
					}
				}
			}

			//spawning, adapted from vanilla SpawnNPC method in NPC.cs
			if (!((player.ZoneTowerNebula) || (player.ZoneTowerVortex) || (player.ZoneTowerStardust) || (player.ZoneTowerSolar) || (spawnInfo.Sky) || (spawnInfo.Invasion) || (wall == 244 && !Main.remixWorld) || (Main.tile[spawnInfo.SpawnTileX, spawnInfo.SpawnTileY].WallType == 62 || spawnInfo.SpiderCave)))
			{
				if ((NPC.SpawnTileOrAboveHasAnyWallInSet(x, y, WallID.Sets.AllowsUndergroundDesertEnemiesToSpawn) || spawnInfo.DesertCave) && WorldGen.checkUnderground(x, y))
				{
					//spawn Sugar ghoul here
					float num68 = 1.15f;
					if ((double)y > (Main.rockLayer * 2.0 + (double)Main.maxTilesY) / 3.0)
					{
						num68 *= 0.5f;
					}
					else if ((double)y > Main.rockLayer)
					{
						num68 *= 0.85f;
					}
					else if (Main.hardMode && !Main.rand.NextBool(5))
					{
						//blockVanillaSpawn = true; //dont need to block
						switch (Main.rand.Next(3))
						{
							case 0:
								if (ModContent.NPCType<Dudley>() == npcType)
								{
									return 1f;
								}
								break;
							default:
								if (ModContent.NPCType<SugarGhoul>() == npcType)
								{
									return 1f;
								}
								break;
						}
					}
				}
				else if (spawnInfo.PlayerInTown)
				{
					if (!((player.ZoneGraveyard) || (!spawnInfo.SafeRangeX && beach) || (spawnInfo.Water) || (tileType == TileID.SnowBlock || tileType == TileID.IceBlock) || (tileType == TileID.JungleGrass) || (tileType == TileID.Sand)))
					{
						if (tileType != ModContent.TileType<Tiles.CreamGrass>() && tileType != ModContent.TileType<Tiles.CreamGrassMowed>() && !((double)y > Main.worldSurface))
						{
							return 0f;
						}
						blockVanillaSpawn = true;
						bool flag27 = surface2;
						if (Main.raining && y <= Main.UnderworldLayer)
						{
							if (!Main.rand.NextBool(3))
							{
								if (npcType == ModContent.NPCType<GummyWorm>())
								{
									return 1f;
								}
							}
							else if (!Main.rand.NextBool(3))
							{
								if (npcType == ModContent.NPCType<ChocolateFrog>())
								{
									return 1f;
								}
							}
						}
						else if (!NPC.TooWindyForButterflies && !Main.dayTime && Main.rand.NextBool(NPC.fireFlyFriendly) && flag27)
						{
							if (npcType == ModContent.NPCType<CherryBug>())
							{
								if (Main.rand.NextBool(NPC.fireFlyMultiple))
								{
									NPC.NewNPC(NPC.GetSource_NaturalSpawn(), x * 16 + 8 - 16, y * 16, npcType);
								}
								if (Main.rand.NextBool(NPC.fireFlyMultiple))
								{
									NPC.NewNPC(NPC.GetSource_NaturalSpawn(), x * 16 + 8 + 16, y * 16, npcType);
								}
								if (Main.rand.NextBool(NPC.fireFlyMultiple))
								{
									NPC.NewNPC(NPC.GetSource_NaturalSpawn(), x * 16 + 8, y * 16 - 16, npcType);
								}
								if (Main.rand.NextBool(NPC.fireFlyMultiple))
								{
									NPC.NewNPC(NPC.GetSource_NaturalSpawn(), x * 16 + 8, y * 16 + 16, npcType);
								}
								return 1f;
							}
						}
						else if (Main.dayTime && Main.time < 18000.0 && !Main.rand.NextBool(3) && flag27)
						{
							int num101 = Main.rand.Next(3);
							switch (num101)
							{
								case 0:
									if (npcType == ModContent.NPCType<Pip>())
									{
										return 1f;
									}
									break;
								default:
									if (npcType == ModContent.NPCType<Birdnana>())
									{
										return 1f;
									}
									break;
							}
						}
						else if (!NPC.TooWindyForButterflies && !Main.raining && Main.dayTime && Main.rand.NextBool(NPC.butterflyChance) && flag27)
						{
							if (npcType == ModContent.NPCType<GrumbleBee>())
							{
								return 1f;
							}
						}
						else if (Main.rand.NextBool(2) && flag27)
						{
							int num102 = Main.rand.Next(3);
							switch (num102)
							{
								case 0:
									if (npcType == ModContent.NPCType<Pip>())
									{
										return 1f;
									}
									break;
								default:
									if (npcType == ModContent.NPCType<Birdnana>())
									{
										return 1f;
									}
									break;
							}
						}
						else if (!(y > Main.UnderworldLayer))
						{
							if (Main.remixWorld)
							{
								if ((double)y < Main.rockLayer && (double)y > Main.worldSurface)
								{
									if (npcType == ModContent.NPCType<ChocolateBunny>())
									{
										return 1f;
									}
								}
							}
							else if (!((double)y >= Main.rockLayer && y <= Main.UnderworldLayer))
							{
								if (npcType == ModContent.NPCType<ChocolateBunny>())
								{
									return 1f;
								}
							}
						}
					}
				}
				else if (!((player.ZoneDungeon) || (player.ZoneMeteor) || (DD2Event.Ongoing && player.ZoneOldOneArmy) || ((Main.remixWorld || (double)y <= Main.worldSurface) && !Main.dayTime && Main.snowMoon) || ((Main.remixWorld || (double)y <= Main.worldSurface) && !Main.dayTime && Main.pumpkinMoon) || (((double)y <= Main.worldSurface || (Main.remixWorld && (double)y > Main.rockLayer)) && Main.dayTime && Main.eclipse) || (Main.hardMode && spawnTile == TileID.MushroomGrass && spawnInfo.Water)))
				{
					if (Main.hardMode && (double)y > Main.worldSurface && player.RollLuck(Main.tenthAnniversaryWorld ? 25 : 75) == 0) //biome mimic
					{
						if (Main.rand.NextBool(2) && player.InModBiome(ModContent.GetInstance<ConfectionBiome>()) && !NPC.AnyNPCs(ModContent.NPCType<BigMimicConfection>()))
						{
							blockVanillaSpawn = true;
							if (npcType == ModContent.NPCType<BigMimicConfection>())
							{
								return 1f;
							}
						}
					}
					else if (!((((tileType == TileID.LihzahrdBrick || tileType == TileID.WoodenSpikes) && spawnInfo.Lihzahrd) || (Main.remixWorld && spawnInfo.Lihzahrd)) || (tileType == TileID.JungleGrass && ((!Main.remixWorld && (double)y > (Main.worldSurface + Main.rockLayer) / 2.0) || (Main.remixWorld && ((double)y < Main.rockLayer || Main.rand.NextBool(2)))))))
					{
						if (Sandstorm.Happening && player.ZoneSandstorm && TileID.Sets.Conversion.Sand[tileType] && NPC.Spawning_SandstoneCheck(x, y)) //confection sandstorm
						{
							if (!(!NPC.downedBoss1 && !Main.hardMode))
							{
								if (Main.hardMode && tileType == ModContent.TileType<Tiles.Creamsand>() && Main.rand.NextBool(3))
								{
									blockVanillaSpawn = true;
									if (npcType == ModContent.NPCType<SweetGummy>())
									{
										return 1f;
									}
								}
							}
						}
						else if (Main.hardMode && tileType == ModContent.TileType<Tiles.Creamsand>() && Main.rand.NextBool(2))
						{
							blockVanillaSpawn = true;
							if (npcType == ModContent.NPCType<SweetGummy>())
							{
								return 1f;
							}
						}
						else if (!(Main.hardMode && !spawnInfo.Water && surface && (tileType == TileID.Pearlsand || tileType == TileID.Pearlstone || tileType == TileID.HallowedGrass || tileType == TileID.HallowedIce)))
						{
							if (Main.hardMode && !spawnInfo.Water && surface && (tileType == ModContent.TileType<Tiles.Creamsand>() || tileType == ModContent.TileType<Tiles.Creamstone>() || tileType == ModContent.TileType<Tiles.CreamGrass>() || tileType == ModContent.TileType<Tiles.BlueIce>()))
							{
								blockVanillaSpawn = true;
								if (NPC.downedPlantBoss && (Main.remixWorld || (!Main.dayTime && Main.time < 16200.0)) && surface2 && player.RollLuck(10) == 0 && !NPC.AnyNPCs(ModContent.NPCType<RoyalCherryBug>()))
								{
									if (npcType == ModContent.NPCType<RoyalCherryBug>())
									{
										return 1f;
									}
								}
								else if (!raining || NPC.AnyNPCs(ModContent.NPCType<SherbetSlime>()) || !Main.rand.NextBool(12))
								{
									if (!Main.dayTime && Main.rand.NextBool(2))
									{
										if (player.RollLuck(500) == 0)
										{
											if (npcType == ModContent.NPCType<WildWilly>())
											{
												return 1f;
											}
										}
										else
										{
											if (npcType == ModContent.NPCType<Meowzer>())
											{
												return 1f;
											}
										}
									}
									else
									{
										if (!Main.rand.NextBool(10) && (!player.ZoneWaterCandle || !Main.rand.NextBool(10)))
										{
											if (npcType == ModContent.NPCType<Sprinkler>() || npcType == ModContent.NPCType<Sprinkling>())
											{
												return 1f;
											}
										}
										else
										{
											if (npcType == ModContent.NPCType<Rollercookie>())
											{
												return 1f;
											}
										}
									}
								}
								else
								{
									if (npcType == ModContent.NPCType<SherbetSlime>())
									{
										return 1f;
									}
								}
							}
							else if (!spawnInfo.PlayerSafe && Main.hardMode && Main.rand.NextBool(50) && !spawnInfo.Water && dirtLayer && (tileType == ModContent.TileType<Tiles.Creamsand>() || tileType == ModContent.TileType<Tiles.Creamstone>() || tileType == ModContent.TileType<Tiles.CreamGrass>() || tileType == ModContent.TileType<Tiles.BlueIce>()))
							{
								blockVanillaSpawn = true;
								if (npcType == ModContent.NPCType<CrazyCone>())
								{
									return 1f;
								}
							}
							else if (!(((tileType == TileID.Crimtane && player.ZoneCrimson) || tileType == TileID.CrimsonGrass || tileType == TileID.FleshIce || tileType == TileID.Crimstone || tileType == TileID.Crimsand || tileType == TileID.CrimsonJungleGrass) || ((tileType == TileID.Demonite && player.ZoneCorrupt) || tileType == TileID.CorruptGrass || tileType == TileID.Ebonstone || tileType == TileID.Ebonsand || tileType == TileID.CorruptIce || tileType == TileID.CorruptJungleGrass)))
							{
								if (surface2)
								{
									bool flag32 = (float)Math.Abs(x - Main.maxTilesX / 2) / (float)(Main.maxTilesX / 2) > 0.33f;
									if (flag32 && NPC.AnyDanger())
									{
										flag32 = false;
									}
									if (!player.ZoneGraveyard && Main.dayTime)
									{
										int num3 = Math.Abs(x - Main.spawnTileX);
										if (!spawnInfo.Water && num3 < Main.maxTilesX / 2 && Main.rand.NextBool(15) && (tileType == ModContent.TileType<Tiles.CreamGrass>() || tileType == ModContent.TileType<Tiles.CreamGrassMowed>()))
										{
											blockVanillaSpawn = true;
											if (!(tileType == TileID.SnowBlock || tileType == TileID.IceBlock))
											{
												if (!NPC.TooWindyForButterflies && !Main.raining && Main.dayTime && Main.rand.NextBool(NPC.butterflyChance) && surface2)
												{
													if (npcType == ModContent.NPCType<GrumbleBee>())
													{
														return 1f;
													}
												}
												else
												{
													if (npcType == ModContent.NPCType<ChocolateBunny>())
													{
														return 1f;
													}
												}
											}
										}
										else if (!spawnInfo.Water && num3 < Main.maxTilesX / 3 && Main.dayTime && Main.time < 18000.0 && (tileType == ModContent.TileType<Tiles.CreamGrass>() || tileType == ModContent.TileType<Tiles.CreamGrassMowed>()) && Main.rand.NextBool(4) && (double)y <= Main.worldSurface && NPC.CountNPCS(ModContent.NPCType<Birdnana>()) + NPC.CountNPCS(ModContent.NPCType<Pip>()) < 6)
										{
											blockVanillaSpawn = true;
											int num4 = Main.rand.Next(3);
											switch (num4)
											{
												case 0:
													if (npcType == ModContent.NPCType<Pip>())
													{
														return 1f;
													}
													break;
												default:
													if (npcType == ModContent.NPCType<Birdnana>())
													{
														return 1f;
													}
													break;
											}
										}
										else if (!spawnInfo.Water && num3 < Main.maxTilesX / 3 && Main.rand.NextBool(15) && (tileType == ModContent.TileType<Tiles.CreamGrass>() || tileType == ModContent.TileType<Tiles.CreamGrassMowed>()))
										{
											blockVanillaSpawn = true;
											int num5 = Main.rand.Next(3);
											switch (num5)
											{
												case 0:
													if (npcType == ModContent.NPCType<Pip>())
													{
														return 1f;
													}
													break;
												default:
													if (npcType == ModContent.NPCType<Birdnana>())
													{
														return 1f;
													}
													break;
											}
										}
									}
									else
									{
										if (!player.ZoneGraveyard && !NPC.TooWindyForButterflies && (tileType == ModContent.TileType<Tiles.CreamGrass>() || tileType == ModContent.TileType<Tiles.CreamGrassMowed>()) && !Main.raining && Main.rand.NextBool(NPC.fireFlyChance) && (double)y <= Main.worldSurface)
										{
											blockVanillaSpawn = true;
											if (npcType == ModContent.NPCType<CherryBug>())
											{
												if (Main.rand.NextBool(NPC.fireFlyMultiple))
												{
													NPC.NewNPC(NPC.GetSource_NaturalSpawn(), x * 16 + 8 - 16, y * 16, npcType);
												}
												if (Main.rand.NextBool(NPC.fireFlyMultiple))
												{
													NPC.NewNPC(NPC.GetSource_NaturalSpawn(), x * 16 + 8 + 16, y * 16, npcType);
												}
												if (Main.rand.NextBool(NPC.fireFlyMultiple))
												{
													NPC.NewNPC(NPC.GetSource_NaturalSpawn(), x * 16 + 8, y * 16 - 16, npcType);
												}
												if (Main.rand.NextBool(NPC.fireFlyMultiple))
												{
													NPC.NewNPC(NPC.GetSource_NaturalSpawn(), x * 16 + 8, y * 16 + 16, npcType);
												}
												return 1f;
											}
										}
									}
								}
								else if (!((surface) || (y > Main.maxTilesY - 190)))
								{
									if ((tileType == ModContent.TileType<Tiles.Creamsand>() || tileType == ModContent.TileType<Tiles.Creamstone>() || tileType == ModContent.TileType<Tiles.BlueIce>()) && Main.hardMode && !spawnInfo.PlayerSafe && Main.rand.NextBool(8) && NPC.CountNPCS(ModContent.NPCType<Iscreamer>()) < 3)
									{
										blockVanillaSpawn = true;
										if (npcType == ModContent.NPCType<Iscreamer>())
										{
											return 1f;
										}
									}
									else if ((spawnTile == TileID.SnowBlock || spawnTile == TileID.IceBlock || spawnTile == TileID.BreakableIce || spawnTile == TileID.CorruptIce || spawnTile == TileID.HallowedIce || spawnTile == TileID.FleshIce || spawnTile == ModContent.TileType<Tiles.BlueIce>()) && !spawnInfo.PlayerSafe && Main.hardMode && player.InModBiome(ModContent.GetInstance<ConfectionBiome>()) && Main.rand.NextBool(30))
									{
										//blockVanillaSpawn = true; //dont block
										if (npcType == ModContent.NPCType<StripedPigron>())
										{
											return 1f;
										}
									}
									else if (!Main.rand.NextBool(2))
									{
										if (Main.hardMode && (player.InModBiome(ModContent.GetInstance<ConfectionBiome>()) && Main.rand.NextBool(2)))
										{
											if (npcType == ModContent.NPCType<ParfaitSlime>())
											{
												return 1f;
											}
										}
										else
										{
											if (!(player.ZoneJungle))
											{
												if (!(player.ZoneGlowshroom && (spawnTile == TileID.MushroomGrass || spawnTile == TileID.MushroomBlock)))
												{
													if (Main.hardMode && player.InModBiome(ModContent.GetInstance<ConfectionBiome>()))
													{
														blockVanillaSpawn = true;
														if (Main.rand.NextBool(5))
														{
															if (npcType == ModContent.NPCType<FoaminFloat>())
															{
																return 1f;
															}
														}
														else if (Main.rand.NextBool(50) && !player.ZoneSnow)
														{
															if (npcType == ModContent.NPCType<GummyWyrmHead>())
															{
																return 1f;
															}
														}
														else if (Main.rand.NextBool(80))
														{
															if (npcType == ModContent.NPCType<IcecreamGal>())
															{
																return 1f;
															}
														}
														else
														{
															if (npcType == ModContent.NPCType<Prickster>())
															{
																return 1f;
															}
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			return 0f;
		}

		public override void OnSpawn(NPC npc, IEntitySource source)
		{
			if (TheConfectionRebirth.easter)
			{
				if (npc.type == NPCID.Bunny)
				{
					npc.Transform(ModContent.NPCType<ChocolateBunny>());
				}
				else if (npc.type == NPCID.Frog)
				{
					npc.Transform(ModContent.NPCType<ChocolateFrog>());
				}
			}
			if (npc.type == NPCID.SandShark)
			{
				if (ConfectionIDs.Sets.Confection[Main.tile[(int)((npc.position.X - 8) / 16), (int)(npc.position.Y / 16)].TileType])
				{
					npc.Transform(ModContent.NPCType<SacchariteSharpnose>());
				}
			}
		}

		public override bool PreAI(NPC npc)
		{
			if (candleFire && Main.rand.NextBool(200))
			{
				candleFlameDelay = Main.rand.Next(40, 100);
			}
			if (candleFlameDelay > 0)
			{
				candleFlameDelay--;
			}
			return true;
		}

		public override void SetStaticDefaults()
		{
			var nurseHappiness = NPCHappiness.Get(NPCID.Nurse);
			var wizardHappiness = NPCHappiness.Get(NPCID.Wizard);
			var partygirlHappiness = NPCHappiness.Get(NPCID.PartyGirl);
			var tavernkeepHappiness = NPCHappiness.Get(NPCID.DD2Bartender);

			var clothierHappiness = NPCHappiness.Get(NPCID.Clothier);
			var witchdoctorHappiness = NPCHappiness.Get(NPCID.WitchDoctor);
			var taxcollectorHappiness = NPCHappiness.Get(NPCID.TaxCollector);

			nurseHappiness.SetBiomeAffection<ConfectionBiome>(AffectionLevel.Like);
			wizardHappiness.SetBiomeAffection<ConfectionBiome>(AffectionLevel.Like);
			partygirlHappiness.SetBiomeAffection<ConfectionBiome>(AffectionLevel.Like);
			tavernkeepHappiness.SetBiomeAffection<ConfectionBiome>(AffectionLevel.Like);

			clothierHappiness.SetBiomeAffection<ConfectionBiome>(AffectionLevel.Dislike);
			witchdoctorHappiness.SetBiomeAffection<ConfectionBiome>(AffectionLevel.Dislike);
			taxcollectorHappiness.SetBiomeAffection<ConfectionBiome>(AffectionLevel.Dislike);
		}

		#region Global Drop Conditions
		public class SoulOfDelight : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				if (SoulOfWhateverConditionCanDrop(info))
				{
					return info.player.InModBiome<ConfectionBiome>();
				}
				return false;
			}

			public bool CanShowItemDropInUI()
			{
				return true;
			}

			public string GetConditionDescription()
			{
				return Language.GetTextValue("Mods.TheConfectionRebirth.Bestiary_ItemDropConditions.SoulOfDelight");
			}
		}

		public class SoulOfNightCorrupt : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				if (SoulOfWhateverConditionCanDrop(info))
				{
					return info.player.ZoneCorrupt;
				}
				return false;
			}

			public bool CanShowItemDropInUI()
			{
				return true;
			}

			public string GetConditionDescription()
			{
				return Language.GetTextValue("Mods.TheConfectionRebirth.Bestiary_ItemDropConditions.SoulOfSpite");
			}
		}

		public class SoulOfSpite : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				if (SoulOfWhateverConditionCanDrop(info))
				{
					return info.player.ZoneCrimson;
				}
				return false;
			}

			public bool CanShowItemDropInUI()
			{
				return true;
			}

			public string GetConditionDescription()
			{
				return Language.GetTextValue("Mods.TheConfectionRebirth.Bestiary_ItemDropConditions.SoulOfNight");
			}
		}

		public class ConfectionKeyCondition : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				if (info.npc.value > 0f && Main.hardMode && !info.IsInSimulation)
				{
					return info.player.InModBiome<ConfectionBiome>();
				}
				return false;
			}

			public bool CanShowItemDropInUI()
			{
				return true;
			}

			public string GetConditionDescription()
			{
				return Language.GetTextValue("Mods.TheConfectionRebirth.Bestiary_ItemDropConditions.ConfectionKeyCondition");
			}
		}
		#endregion

		#region ConfectionDropRule
		public class ConfectionDropRule : IItemDropRuleCondition
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				return ConfectionWorldGeneration.confectionorHallow;
			}

			public bool CanShowItemDropInUI()
			{
				return false;
			}

			public string GetConditionDescription()
			{
				return null;
			}
		}
		#endregion

		#region HallowDropRule
		public class HallowDropRule : IItemDropRuleCondition
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				return !ConfectionWorldGeneration.confectionorHallow;
			}

			public bool CanShowItemDropInUI()
			{
				return false;
			}

			public string GetConditionDescription()
			{
				return null;
			}
		}

		#endregion

		#region 50/50
		public class oneInTwo : IItemDropRuleCondition
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				return Main.rand.NextBool(2);
			}

			public bool CanShowItemDropInUI()
			{
				return true;
			}

			public string GetConditionDescription()
			{
				return null;
			}
		}
		#endregion

		#region notexpertordrunk
		public class NotDrunkandExpert : IItemDropRuleCondition
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				return (!Main.drunkWorld || !ConfectionModCalling.FargoBoBW) && !Main.expertMode;
			}

			public bool CanShowItemDropInUI()
			{
				return true;
			}

			public string GetConditionDescription()
			{
				return null;
			}
		}
		#endregion

		#region drunkactive
		public class DrunkWorldIsActive : IItemDropRuleCondition
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				ConfectionModCalling.UpdateFargoBoBW();
				return (Main.drunkWorld || ConfectionModCalling.FargoBoBW);
			}

			public bool CanShowItemDropInUI()
			{
				return true;
			}

			public string GetConditionDescription()
			{
				return null;
			}
		}
		#endregion

		#region NotDrunkActive
		public class DrunkWorldIsNotActive : IItemDropRuleCondition
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				ConfectionModCalling.UpdateFargoBoBW();
				return (!Main.drunkWorld || !ConfectionModCalling.FargoBoBW);
			}

			public bool CanShowItemDropInUI()
			{
				return true;
			}

			public string GetConditionDescription()
			{
				return null;
			}
		}
		#endregion

		public override void ModifyGlobalLoot(GlobalLoot globalLoot)
		{
			globalLoot.Add(new ItemDropWithConditionRule(ModContent.ItemType<SoulofDelight>(), 5, 1, 1, new SoulOfDelight()));
			globalLoot.Add(new ItemDropWithConditionRule(ModContent.ItemType<SoulofSpite>(), 5, 1, 1, new SoulOfSpite()));
			globalLoot.Add(new ItemDropWithConditionRule(ItemID.SoulofNight, 5, 1, 1, new SoulOfNightCorrupt()));
			globalLoot.Add(new ItemDropWithConditionRule(ModContent.ItemType<ConfectionBiomeKey>(), 2500, 1, 1, new ConfectionKeyCondition()));

			globalLoot.RemoveWhere(
				rule => rule is ItemDropWithConditionRule drop
					&& drop.itemId == ItemID.SoulofNight
					&& drop.condition is Conditions.SoulOfNight
			);
		}

		public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
		{
			static bool TwinsDrops(DropAttemptInfo info)
			{
				NPC npc = info.npc;
				if (npc is null)
				{
					return false;
				}
				if (npc.type == NPCID.Retinazer)
				{
					return !NPC.AnyNPCs(NPCID.Spazmatism);
				}
				else if (npc.type == NPCID.Spazmatism)
				{
					return !NPC.AnyNPCs(NPCID.Retinazer);
				}
				return false;
			}
			if (npc.type == NPCID.Retinazer || npc.type == NPCID.Spazmatism)
			{
				try
				{
					IItemDropRule obj9 = npcLoot.Get(false).Find(delegate (IItemDropRule rule) {
						LeadingConditionRule val19 = (LeadingConditionRule)(object)((rule is LeadingConditionRule) ? rule : null);
						return val19 != null && val19.condition is MissingTwin;
					});
					LeadingConditionRule LCR_LTS = (LeadingConditionRule)(object)((obj9 is LeadingConditionRule) ? obj9 : null);
					if (LCR_LTS != null)
					{
						IItemDropRule ruleToChain2 = LCR_LTS.ChainedRules.Find(delegate (IItemDropRuleChainAttempt chainAttempt) {
							TryIfSucceeded val17 = (TryIfSucceeded)(object)((chainAttempt is TryIfSucceeded) ? chainAttempt : null);
							if (val17 != null)
							{
								IItemDropRule ruleToChain7 = val17.RuleToChain;
								LeadingConditionRule val18 = (LeadingConditionRule)(object)((ruleToChain7 is LeadingConditionRule) ? ruleToChain7 : null);
								if (val18 != null)
								{
									return val18.condition is NotExpert;
								}
							}
							return false;
						}).RuleToChain;
						LeadingConditionRule LCR_NotExpert10 = (LeadingConditionRule)(object)((ruleToChain2 is LeadingConditionRule) ? ruleToChain2 : null);
						if (LCR_NotExpert10 != null)
						{
							LCR_NotExpert10.ChainedRules.RemoveAll(delegate (IItemDropRuleChainAttempt chainAttempt) {
								TryIfSucceeded val15 = (TryIfSucceeded)(object)((chainAttempt is TryIfSucceeded) ? chainAttempt : null);
								if (val15 != null)
								{
									IItemDropRule ruleToChain6 = val15.RuleToChain;
									CommonDrop val16 = (CommonDrop)(object)((ruleToChain6 is CommonDrop) ? ruleToChain6 : null);
									if (val16 != null)
									{
										return val16.itemId == ItemID.HallowedBar;
									}
								}
								return false;
							});
						}
					}
				}
				catch (ArgumentNullException)
				{
				}
			}

			if (npc.type == NPCID.Gastropod)
			{
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ShellBlock>(), 2, 15, 25));
			}

			if (npc.type == NPCID.BloodMummy)
			{
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CanofMeat>(), 10));
			}
			if (npc.type == NPCID.DesertGhoulCrimson)
			{
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CanofMeat>(), 15));
			}
			if (npc.type == NPCID.SandsharkCrimson)
			{
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CanofMeat>(), 25));
			}

			var entries = npcLoot.Get(false);
			if (npc.type == NPCID.WallofFlesh)
			{
				foreach (var entry in entries)
				{
					if (entry is ItemDropWithConditionRule rule && rule.itemId == ItemID.Pwnhammer)
					{
						npcLoot.Remove(rule);
						break;
					}
				}

				DrunkWorldIsNotActive NotDrunkWorld = new DrunkWorldIsNotActive();
				DrunkWorldIsActive DrunkWorld = new DrunkWorldIsActive();

				LeadingConditionRule ConfectionHammer = new LeadingConditionRule(new ConfectionDropRule());
				ConfectionHammer.OnSuccess(ItemDropRule.ByCondition(NotDrunkWorld, ModContent.ItemType<Items.Weapons.GrandSlammer>()));
				npcLoot.Add(ConfectionHammer);

				LeadingConditionRule HallowHammer = new LeadingConditionRule(new HallowDropRule());
				HallowHammer.OnSuccess(ItemDropRule.ByCondition(NotDrunkWorld, ItemID.Pwnhammer));
				npcLoot.Add(HallowHammer);

				LeadingConditionRule fiftyfifty = new LeadingConditionRule(new oneInTwo());
				fiftyfifty.OnSuccess(ItemDropRule.ByCondition(DrunkWorld, ModContent.ItemType<Items.Weapons.GrandSlammer>()));
				fiftyfifty.OnFailedConditions(ItemDropRule.ByCondition(DrunkWorld, ItemID.Pwnhammer));
				npcLoot.Add(fiftyfifty);

			}

			if (npc.type == NPCID.TheDestroyer || npc.type == NPCID.SkeletronPrime)
			{
				npcLoot.RemoveWhere(
				rule => rule is ItemDropWithConditionRule drop
					&& drop.itemId == ItemID.HallowedBar
				);
			}

			if (npc.type == NPCID.TheDestroyer || npc.type == NPCID.SkeletronPrime)
			{
				NotDrunkandExpert ExpertDrunkmode = new NotDrunkandExpert();
				NotExpert Expertmode = new NotExpert();

				LeadingConditionRule ConfectionCondition = new LeadingConditionRule(new ConfectionDropRule());
				ConfectionCondition.OnSuccess(ItemDropRule.ByCondition(ExpertDrunkmode, ModContent.ItemType<Items.Placeable.NeapoliniteOre>(), 1, 15 * 5, 30 * 5));
				npcLoot.Add(ConfectionCondition);

				LeadingConditionRule HallowCondition = new LeadingConditionRule(new HallowDropRule());
				HallowCondition.OnSuccess(ItemDropRule.ByCondition(ExpertDrunkmode, ModContent.ItemType<Items.Placeable.HallowedOre>(), 1, 15 * 5, 30 * 5));
				npcLoot.Add(HallowCondition);

				LeadingConditionRule DrunkCondition = new LeadingConditionRule(new DrunkWorldIsActive());
				DrunkCondition.OnSuccess(ItemDropRule.ByCondition(Expertmode, ModContent.ItemType<Items.Placeable.HallowedOre>(), 1, 8 * 5, 15 * 5));
				ConfectionCondition.OnSuccess(ItemDropRule.ByCondition(Expertmode, ModContent.ItemType<Items.Placeable.NeapoliniteOre>(), 1, 8 * 5, 15 * 5));
				npcLoot.Add(DrunkCondition);
			}
			if (npc.type == NPCID.BloodMummy || npc.type == NPCID.DesertGhoulCrimson || npc.type == NPCID.SandsharkCrimson)
			{
				npcLoot.Remove(FindDarkShard(npcLoot));
			}
		}

		private static IItemDropRule FindDarkShard(NPCLoot loot)
		{
			foreach (IItemDropRule item in loot.Get(false))
			{
				CommonDrop c = (CommonDrop)(object)((item is CommonDrop) ? item : null);
				if (c != null && c.itemId == ItemID.DarkShard)
				{
					return (IItemDropRule)(object)c;
				}
			}
			return null;
		}

		public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers)
		{
			if (player.HasBuff(ModContent.BuffType<VanillaValorV>()) && !ConfectionIDs.Sets.IsEnemyVanillaCritImmune[npc.type])
			{
				modifiers.HideCombatText();
				modifiers.Defense *= 0;
			}
		}
		public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
		{
			if (Main.player[projectile.owner].HasBuff(ModContent.BuffType<VanillaValorV>()) && !ConfectionIDs.Sets.IsEnemyVanillaCritImmune[npc.type])
			{
				modifiers.HideCombatText();
				modifiers.Defense *= 0;
			}

			if (!projectile.npcProj && !projectile.trap && projectile.IsMinionOrSentryRelated)
			{
				float projTagMultiplier = ProjectileID.Sets.SummonTagDamageMultiplier[projectile.type];
				if (npc.HasBuff<SacchariteLashTagDamage>())
				{
					modifiers.FlatBonusDamage += 10 * projTagMultiplier;
				}
				if (npc.HasBuff<GummyWormWhipTagDamage>())
				{
					modifiers.FlatBonusDamage += 8 * projTagMultiplier;
				}
			}
		}

		public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone)
		{
			if (!ConfectionIDs.Sets.IsEnemyVanillaCritImmune[npc.type] && player.HasBuff(ModContent.BuffType<VanillaValorV>()))
			{
				if (hit.Crit)
				{
					Color color3 = new(230, 196, 125);
					CombatText.NewText(new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height), color3, hit.Damage, true);
				}
				else
				{
					HitText(npc, hit);
				}
			}
		}

		public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
		{
			if (!ConfectionIDs.Sets.IsEnemyVanillaCritImmune[npc.type] && Main.player[projectile.owner].HasBuff(ModContent.BuffType<VanillaValorV>()))
			{
				if (hit.Crit)
				{
					Color color2 = new(230, 196, 125);
					CombatText.NewText(new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height), color2, hit.Damage, true);
				}
				else
				{
					HitText(npc, hit);
				}
			}
		}

		private static void HitText(NPC npc, NPC.HitInfo hit)
		{
			double num = hit.Damage;
			bool crit = hit.Crit;
			if (hit.InstantKill)
			{
				num = ((npc.realLife > 0) ? Main.npc[npc.realLife].life : npc.life);
			}
			if (!hit.InstantKill && npc.lifeMax > 1 && !npc.HideStrikeDamage)
			{
				if (npc.friendly)
				{
					Color color = (crit ? CombatText.DamagedFriendlyCrit : CombatText.DamagedFriendly);
					CombatText.NewText(new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height), color, (int)num, crit);
				}
				else
				{
					Color color2 = (crit ? CombatText.DamagedHostileCrit : CombatText.DamagedHostile);
					CombatText.NewText(new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height), color2, (int)num, crit);
				}
			}
		}

		public static Condition InConfection = new Condition("Mods.TheConfectionRebirth.InConfection", () => Main.LocalPlayer.InModBiome<ConfectionBiome>());
		public static Condition NotInConfection = new Condition("Mods.TheConfectionRebirth.NotInConfection", () => !Main.LocalPlayer.InModBiome<ConfectionBiome>());

		public static Condition confectionworld = new Condition("Mods.TheConfectionRebirth.TheConfection", () => ConfectionWorldGeneration.confectionorHallow);

		public static Condition hallowworld = new Condition("Mods.TheConfectionRebirth.TheHallow", () => !ConfectionWorldGeneration.confectionorHallow);

		public static Condition paintingNotCondition = new Condition("Mods.TheConfectionRebirth.NotInSeveralBiomes", () => (!Main.LocalPlayer.ZoneJungle && !Main.LocalPlayer.ZoneDesert && !Main.LocalPlayer.ZoneSnow && !Main.bloodMoon));

		public override void ModifyShop(NPCShop shop)
		{
			if (shop.NpcType == NPCID.Dryad)
			{
				shop.InsertAfter(ItemID.HallowedGrassEcho, ModContent.ItemType<CreamgrassWall>(), Condition.Hardmode, confectionworld);
				shop.InsertAfter(ItemID.HallowedSeeds, ModContent.ItemType<CreamBeans>(), Condition.Hardmode, confectionworld, Condition.NotInGraveyard);
				shop.InsertAfter(ItemID.HallowedSeeds, ModContent.ItemType<CreamBeans>(), Condition.Hardmode, hallowworld, Condition.InGraveyard);
				shop.InsertAfter(ItemID.HallowedSeeds, ItemID.HallowedSeeds, Condition.Hardmode, confectionworld, Condition.InGraveyard);

				shop.InsertAfter(ItemID.PottedHallowCedar, ModContent.ItemType<PottedConfectionCedar>(), Condition.Hardmode, confectionworld, Condition.MoonPhaseFull);
				shop.InsertAfter(ItemID.PottedHallowCedar, ModContent.ItemType<PottedConfectionCedar>(), Condition.Hardmode, confectionworld, Condition.MoonPhaseWaningGibbous);
				shop.InsertAfter(ItemID.PottedHallowTree, ModContent.ItemType<PottedConfectionTree>(), Condition.Hardmode, confectionworld, Condition.MoonPhaseThirdQuarter);
				shop.InsertAfter(ItemID.PottedHallowTree, ModContent.ItemType<PottedConfectionTree>(), Condition.Hardmode, confectionworld, Condition.MoonPhaseWaningCrescent);
				shop.InsertAfter(ItemID.PottedHallowPalm, ModContent.ItemType<PottedConfectionPalm>(), Condition.Hardmode, confectionworld, Condition.MoonPhaseNew);
				shop.InsertAfter(ItemID.PottedHallowPalm, ModContent.ItemType<PottedConfectionPalm>(), Condition.Hardmode, confectionworld, Condition.MoonPhaseWaxingCrescent);
				shop.InsertAfter(ItemID.PottedHallowBamboo, ModContent.ItemType<PottedConfectionBamboo>(), Condition.Hardmode, confectionworld, Condition.MoonPhaseFirstQuarter);
				shop.InsertAfter(ItemID.PottedHallowBamboo, ModContent.ItemType<PottedConfectionBamboo>(), Condition.Hardmode, confectionworld, Condition.MoonPhaseWaxingGibbous);

				if (shop.TryGetEntry(ItemID.HallowedGrassEcho, out NPCShop.Entry entry))
				{
					entry.AddCondition(hallowworld);
				}
				if (shop.TryGetEntry(ItemID.HallowedSeeds, out NPCShop.Entry entry2))
				{
					entry2.AddCondition(hallowworld);
				}

				if (shop.TryGetEntry(ItemID.PottedHallowCedar, out NPCShop.Entry entry4))
				{
					entry4.AddCondition(hallowworld);
				}
				if (shop.TryGetEntry(ItemID.PottedHallowTree, out NPCShop.Entry entry5))
				{
					entry5.AddCondition(hallowworld);
				}
				if (shop.TryGetEntry(ItemID.PottedHallowPalm, out NPCShop.Entry entry6))
				{
					entry6.AddCondition(hallowworld);
				}
				if (shop.TryGetEntry(ItemID.PottedHallowBamboo, out NPCShop.Entry entry7))
				{
					entry7.AddCondition(hallowworld);
				}
			}
			if (shop.NpcType == NPCID.Steampunker)
			{
				shop.InsertAfter(ItemID.BlueSolution, ModContent.ItemType<Items.CreamSolution>(), Condition.Hardmode, InConfection, Condition.NotInGraveyard);
				shop.InsertAfter(ItemID.BlueSolution, ModContent.ItemType<Items.CreamSolution>(), Condition.Hardmode, Condition.InHallow, Condition.InGraveyard);
				shop.InsertAfter(ItemID.BlueSolution, ItemID.BlueSolution, Condition.Hardmode, InConfection, Condition.InGraveyard);
				if (shop.TryGetEntry(ItemID.GreenSolution, out NPCShop.Entry entry8))
				{
					entry8.AddCondition(NotInConfection);
				}
			}
			if (shop.NpcType == NPCID.Wizard)
			{
				shop.InsertAfter(ItemID.Bell, ModContent.ItemType<Items.Kazoo>(), Condition.Hardmode);
			}
			if (shop.NpcType == NPCID.BestiaryGirl)
			{
				shop.InsertAfter(ItemID.WorldGlobe, new Item(ModContent.ItemType<Items.HallowedGlobe>())
				{
					shopCustomPrice = Item.buyPrice(0, 3, 0, 0)
				}, Condition.DownedPlantera);
			}
			if (shop.NpcType == NPCID.Painter && shop.Name == "Decor")
			{
				shop.InsertAfter(ItemID.Purity, new Item(ModContent.ItemType<ConfectionPainting>())
				{
					shopCustomPrice = Item.buyPrice(0, 1)
				}, InConfection, paintingNotCondition);

				if (shop.TryGetEntry(ItemID.TheLandofDeceivingLooks, out NPCShop.Entry entry))
				{
					entry.AddCondition(NotInConfection);
				}
			}
		}
	}
}
