using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Items;
using TheConfectionRebirth.Items.Banners;
using TheConfectionRebirth.Items.Weapons;

namespace TheConfectionRebirth.NPCs
{
    public class Iscreamer : ModNPC
	{
		public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 4;
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, new NPCID.Sets.NPCBestiaryDrawModifiers
			{
				Position = new Vector2(0, 8f)
			});
		}

        public override void SetDefaults()
        {
            NPC.width = 60;
            NPC.height = 60;
            NPC.damage = 50;
            NPC.defense = 22;
            NPC.lifeMax = 400;
			//NPC.HitSound = new SoundStyle($"{nameof(TheConfectionRebirth)}/Sounds/Custom/IceScreamerHurt");
			//NPC.DeathSound = new SoundStyle($"{nameof(TheConfectionRebirth)}/Sounds/Custom/IceScreamerDeath");
            NPC.value = 600f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.knockBackResist = 0.5f;
			NPC.aiStyle = -1;
			Banner = NPC.type;
            BannerItem = ModContent.ItemType<IscreamerBanner>();
            SpawnModBiomes = new int[] { ModContent.GetInstance<ConfectionUndergroundBiome>().Type };
		}

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

                new FlavorTextBestiaryInfoElement("Mods.TheConfectionRebirth.Bestiary.Iscreamer")
            });
        }

		public override void OnSpawn(IEntitySource source)
		{
			NPC.localAI[0] = Main.rand.Next(0, 6);
		}

		public override void AI()
		{
			bool flag20 = false;
			if (NPC.justHit)
			{
				NPC.ai[2] = 0f;
			}
			if (NPC.ai[2] >= 0f)
			{
				int num827 = 16;
				bool flag22 = false;
				bool flag23 = false;
				if (NPC.position.X > NPC.ai[0] - (float)num827 && NPC.position.X < NPC.ai[0] + (float)num827)
				{
					flag22 = true;
				}
				else if ((NPC.velocity.X < 0f && NPC.direction > 0) || (NPC.velocity.X > 0f && NPC.direction < 0))
				{
					flag22 = true;
				}
				num827 += 24;
				if (NPC.position.Y > NPC.ai[1] - (float)num827 && NPC.position.Y < NPC.ai[1] + (float)num827)
				{
					flag23 = true;
				}
				if (flag22 && flag23)
				{
					NPC.ai[2] += 1f;
					if (NPC.ai[2] >= 30f && num827 == 16)
					{
						flag20 = true;
					}
					if (NPC.ai[2] >= 60f)
					{
						NPC.ai[2] = -200f;
						NPC.direction *= -1;
						NPC.velocity.X *= -1f;
						NPC.collideX = false;
					}
				}
				else
				{
					NPC.ai[0] = NPC.position.X;
					NPC.ai[1] = NPC.position.Y;
					NPC.ai[2] = 0f;
				}
				NPC.TargetClosest();
			}
			else
			{
				NPC.ai[2] += 1f;
				if (Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) > NPC.position.X + (float)(NPC.width / 2))
				{
					NPC.direction = -1;
				}
				else
				{
					NPC.direction = 1;
				}
			}
			int num828 = (int)((NPC.position.X + (float)(NPC.width / 2)) / 16f) + NPC.direction * 2;
			int num831 = (int)((NPC.position.Y + (float)NPC.height) / 16f);
			bool flag25 = true;
			int num832 = 3;
			if (NPC.position.Y + (float)NPC.height > Main.player[NPC.target].position.Y)
			{
				for (int num865 = num831; num865 < num831 + num832; num865++)
				{
					if ((Main.tile[num828, num865].HasUnactuatedTile && Main.tileSolid[Main.tile[num828, num865].TileType]) || Main.tile[num828, num865].LiquidAmount > 0)
					{
						flag25 = false;
						break;
					}
				}
			}
			if (Main.player[NPC.target].npcTypeNoAggro[Type])
			{
				bool flag27 = false;
				for (int num866 = num831; num866 < num831 + num832 - 2; num866++)
				{
					if ((Main.tile[num828, num866].HasUnactuatedTile && Main.tileSolid[Main.tile[num828, num866].TileType]) || Main.tile[num828, num866].LiquidAmount > 0)
					{
						flag27 = true;
						break;
					}
				}
				NPC.directionY = (!flag27).ToDirectionInt();
			}
			if (flag20)
			{
				flag25 = true;
			}
			if (flag25)
			{
				NPC.velocity.Y += 0.1f;
				if (NPC.velocity.Y > 3f)
				{
					NPC.velocity.Y = 3f;
				}
			}
			else
			{
				if (NPC.directionY < 0 && NPC.velocity.Y > 0f)
				{
					NPC.velocity.Y -= 0.1f;
				}
				if (NPC.velocity.Y < -4f)
				{
					NPC.velocity.Y = -4f;
				}
			}
			
			if (NPC.collideX)
			{
				NPC.velocity.X = NPC.oldVelocity.X * -0.4f;
				if (NPC.direction == -1 && NPC.velocity.X > 0f && NPC.velocity.X < 1f)
				{
					NPC.velocity.X = 1f;
				}
				if (NPC.direction == 1 && NPC.velocity.X < 0f && NPC.velocity.X > -1f)
				{
					NPC.velocity.X = -1f;
				}
			}
			if (NPC.collideY)
			{
				NPC.velocity.Y = NPC.oldVelocity.Y * -0.25f;
				if (NPC.velocity.Y > 0f && NPC.velocity.Y < 1f)
				{
					NPC.velocity.Y = 1f;
				}
				if (NPC.velocity.Y < 0f && NPC.velocity.Y > -1f)
				{
					NPC.velocity.Y = -1f;
				}
			}
			float num868 = 2f;
			if (NPC.direction == -1 && NPC.velocity.X > 0f - num868)
			{
				NPC.velocity.X -= 0.1f;
				if (NPC.velocity.X > num868)
				{
					NPC.velocity.X -= 0.1f;
				}
				else if (NPC.velocity.X > 0f)
				{
					NPC.velocity.X += 0.05f;
				}
				if (NPC.velocity.X < 0f - num868)
				{
					NPC.velocity.X = 0f - num868;
				}
			}
			else if (NPC.direction == 1 && NPC.velocity.X < num868)
			{
				NPC.velocity.X += 0.1f;
				if (NPC.velocity.X < 0f - num868)
				{
					NPC.velocity.X += 0.1f;
				}
				else if (NPC.velocity.X < 0f)
				{
					NPC.velocity.X -= 0.05f;
				}
				if (NPC.velocity.X > num868)
				{
					NPC.velocity.X = num868;
				}
			}
			num868 = 1.5f;
			if (NPC.directionY == -1 && NPC.velocity.Y > 0f - num868)
			{
				NPC.velocity.Y -= 0.04f;
				if (NPC.velocity.Y > num868)
				{
					NPC.velocity.Y -= 0.05f;
				}
				else if (NPC.velocity.Y > 0f)
				{
					NPC.velocity.Y += 0.03f;
				}
				if (NPC.velocity.Y < 0f - num868)
				{
					NPC.velocity.Y = 0f - num868;
				}
			}
			else if (NPC.directionY == 1 && NPC.velocity.Y < num868)
			{
				NPC.velocity.Y += 0.04f;
				if (NPC.velocity.Y < 0f - num868)
				{
					NPC.velocity.Y += 0.05f;
				}
				else if (NPC.velocity.Y < 0f)
				{
					NPC.velocity.Y -= 0.03f;
				}
				if (NPC.velocity.Y > num868)
				{
					NPC.velocity.Y = num868;
				}
			}

			NPC.ai[3]++;
			if (NPC.ai[3] > 300)
			{
				NPC.ai[3] = 0;
				if (Main.netMode == NetmodeID.MultiplayerClient || Main.player[NPC.target] == null)
				{
					return;
				}
				Vector2 chosenTile = Vector2.Zero;
				Point point = Main.player[NPC.target].Center.ToTileCoordinates();
				if (NPC.AI_AttemptToFindTeleportSpot(ref chosenTile, point.X, point.Y, 25, 10, teleportInAir: true))
				{
					for (int i = 0; i < Main.maxNPCs; i++)
					{
						NPC npc = Main.npc[i];
						float distance = 700f;
						int count = 0;
						if (count > 6)
						{
							break;
						}
						Vector2 chosenTile2 = Vector2.Zero;
						if (npc.active && npc.life > 0 && npc != NPC && npc.type != Type && npc.rarity <= 0 && npc.type != NPCID.TargetDummy && npc.chaseable && !npc.despawnEncouraged && !npc.friendly && !npc.townNPC && npc.Distance(NPC.Center) <= distance)
						{
							bool AirTeleport = false;
							if (npc.noGravity)
							{
								AirTeleport = true;
							}
							if (NPC.AI_AttemptToFindTeleportSpot(ref chosenTile2, point.X, point.Y, 25, 10, teleportInAir: AirTeleport))
							{
								Vector2 newPos2 = new(chosenTile2.X * 16f - (float)(npc.width / 2), chosenTile2.Y * 16f - (float)npc.height);
								NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, npc.whoAmI);
								Teleport(npc, newPos2);
							}
							count++;
						}
					}
					Vector2 newPos = new(chosenTile.X * 16f - (float)(NPC.width / 2), chosenTile.Y * 16f - (float)NPC.height);
					NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, NPC.whoAmI);
					Teleport(NPC, newPos);
					return;
				}
				NPC.active = false;
				NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, NPC.whoAmI);
			}
		}

		public static void Teleport(NPC npc, Vector2 newPos, int extraInfo = 0)
		{
			float dustCountMult = ((npc.teleportTime > 0f) ? 0.3f : 1f);
			Vector2 otherPosition = npc.position;
			SpawnBannaWarpDust(npc.getRect(), dustCountMult, false);
			npc.position = newPos;
			SpawnBannaWarpDust(npc.getRect(), dustCountMult, true);
			npc.teleportTime = 1f;
			if (Main.netMode == NetmodeID.Server)
			{
				NetMessage.SendData(MessageID.TeleportEntity, -1, -1, null, 1, npc.whoAmI, newPos.X, newPos.Y, 1);
			}
		}

		private static void SpawnBannaWarpDust(Rectangle effectRect, float dustCountMult, bool isTeleportSpot)
		{
			SoundEngine.PlaySound(in SoundID.Item8, effectRect.Center.ToVector2());
			int dustCount = (int)(50f * dustCountMult);
			for (int i = 0; i < dustCount; i++)
			{
				Dust dust = Main.dust[Dust.NewDust(new Vector2(effectRect.X, effectRect.Y), effectRect.Width, effectRect.Height, 309, 0f, 0f, 150, Color.Yellow, 1.2f)];
				dust.velocity *= 0.5f;
				dust.noGravity = true;
			}
			for (int i = 0; i < dustCount / 2; i++)
			{
				Dust dust = Main.dust[Dust.NewDust(new Vector2(effectRect.X, effectRect.Y), effectRect.Width, effectRect.Height, 309, 0f, 0f, 150, Color.Yellow, 2.6f)];
				dust.velocity *= 0.5f;
				dust.noGravity = true;
			}
			int count = 16;
			for (int i = 0; i < count; i++)
			{
				float degree = 360 / count * i;
				float radians = MathHelper.ToRadians(degree);
				Vector2 velcoity = Vector2.One.RotatedBy(radians) * 2f;
				int num = Dust.NewDust(new Vector2(effectRect.X + (effectRect.Width / 2), effectRect.Y + (effectRect.Height / 2)), 1, 1, 309);
				Dust dust = Main.dust[num];
				dust.noGravity = true;
				dust.scale = 4f;
				dust.velocity = velcoity;
				dust.color = Color.Yellow;

				degree = 360 / count * i;
				radians = MathHelper.ToRadians(degree);
				velcoity = Vector2.One.RotatedBy(radians);
				num = Dust.NewDust(new Vector2(effectRect.X + (effectRect.Width / 2), effectRect.Y + (effectRect.Height / 2)), 1, 1, 309);
				dust = Main.dust[num];
				dust.noGravity = true;
				dust.scale = 3f;
				dust.velocity = velcoity;
				dust.color = Color.Yellow;
			}

		}

		public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
		{
			ParticleSystem.AddParticle(new Spawn_BearClaw(), target.Center, new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), 1));
		}

		public override bool? CanFallThroughPlatforms()
		{
			return true;
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter++;
			int frame = NPC.frame.Y / frameHeight;
			if (NPC.frameCounter >= 10)
			{
				frame++;
				NPC.frameCounter = 0;
			}

			if (frame > Main.npcFrameCount[Type] - 1)
				frame = 0;

			NPC.frame.Y = frame * frameHeight;
			NPC.spriteDirection = NPC.direction;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Texture2D texture;
			if (NPC.localAI[0] <= 0)
			{
				texture = TextureAssets.Npc[Type].Value;
			}
			else
			{
				texture = ModContent.Request<Texture2D>(Texture + "_" + NPC.localAI[0]).Value;
			}
			SpriteEffects spriteEffects = 0;
			if (NPC.spriteDirection == 1)
			{
				spriteEffects = (SpriteEffects)1;
			}
			Vector2 halfSize = new((float)(TextureAssets.Npc[NPC.type].Width() / 2), (float)(TextureAssets.Npc[NPC.type].Height() / Main.npcFrameCount[NPC.type] / 2));
			Rectangle frame = NPC.frame;
			float num305 = 0f;
			float num306 = Main.NPCAddHeight(NPC);
			spriteBatch.Draw(texture, new Vector2(NPC.position.X - screenPos.X + (float)(NPC.width / 2) - (float)texture.Width * NPC.scale / 2f + halfSize.X * NPC.scale, NPC.position.Y - screenPos.Y + (float)NPC.height - (float)texture.Height * NPC.scale / (float)Main.npcFrameCount[NPC.type] + 4f + halfSize.Y * NPC.scale + num306 + num305 + NPC.gfxOffY), (Rectangle?)frame, NPC.GetAlpha(drawColor), NPC.rotation, halfSize, NPC.scale, spriteEffects, 0f);
			return false;
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BearClaw>(), 100));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BananawarpPeel>(), 10));
			npcLoot.Add(ItemDropRule.Food(ModContent.ItemType<Brownie>(), 150));
		}

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            //if (spawnInfo.Player.InModBiome(ModContent.GetInstance<ConfectionBiome>()) && !spawnInfo.AnyInvasionActive() && Main.hardMode && (spawnInfo.Player.ZoneRockLayerHeight || spawnInfo.Player.ZoneUnderworldHeight)) {
            //    return 0.2f;
            //}
            return 0f;
        }
		
		public override void HitEffect(NPC.HitInfo hit)
        {
            if (Main.netMode == NetmodeID.Server)
            {
                return;
            }

			if (NPC.life > 0)
			{
				for (int i = 0; (double)i < hit.Damage / (double)NPC.lifeMax * 50.0; i++)
				{
					int dustID = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Cloud, 0f, 0f, 0, default(Color), 1.5f);
					Dust dust = Main.dust[dustID];
					dust.velocity *= 1.5f;
					dust.noGravity = true;
				}
			}
			else
			{
				for (int i = 0; i < 10; i++)
				{
					int dustID = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Cloud, 0f, 0f, 0, default(Color), 1.5f);
					Dust dust = Main.dust[dustID];
					dust.velocity *= 2f;
					dust.noGravity = true;
				}
				for (int i = 0; i < 4; i++)
				{
					int type = 11 + i;
					if (type > 13)
					{
						type = Main.rand.Next(11, 14);
					}
					int goreID = Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.position.X, NPC.position.Y + (float)(NPC.height / 2) - 10f), new Vector2((float)hit.HitDirection, 0f), type, NPC.scale);
					Gore gore = Main.gore[goreID];
					gore.velocity *= 0.3f;
				}
			}
		}
	}
}