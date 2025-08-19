using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Items;
using TheConfectionRebirth.Items.Banners;
using TheConfectionRebirth.Items.Weapons;

namespace TheConfectionRebirth.NPCs
{
    public class Iscreamer : ModNPC
	{
		private sbyte Index;

		private float degrees;

		public override void Load()
		{
			VariationManager<Iscreamer>.AddGroup("Strawberry", ModContent.Request<Texture2D>(Texture));
			VariationManager<Iscreamer>.AddGroup("Cream", ModContent.Request<Texture2D>(Texture + "_Cream"));
			VariationManager<Iscreamer>.AddGroup("Chocolate", ModContent.Request<Texture2D>(Texture + "_Chocolate"));
			VariationManager<Iscreamer>.AddGroup("Mint", ModContent.Request<Texture2D>(Texture + "_Mint"));
			VariationManager<Iscreamer>.AddGroup("Orange", ModContent.Request<Texture2D>(Texture + "_Orange"));
			VariationManager<Iscreamer>.AddGroup("Blueberry", ModContent.Request<Texture2D>(Texture + "_Blueberry"));
			VariationManager<Iscreamer>.AddGroup("Snow", ModContent.Request<Texture2D>(Texture + "_Snow"), () => Main.SceneMetrics.EnoughTilesForSnow);
		}

		public override void Unload() => VariationManager<Iscreamer>.Clear();

		public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 4;
        }

        public override void SetDefaults()
        {
            NPC.width = 60;
            NPC.height = 60;
            NPC.damage = 50;
            NPC.defense = 22;
            NPC.lifeMax = 400;
			NPC.HitSound = new SoundStyle($"{nameof(TheConfectionRebirth)}/Sounds/Custom/IceScreamerHurt");
			NPC.DeathSound = new SoundStyle($"{nameof(TheConfectionRebirth)}/Sounds/Custom/IceScreamerDeath");
            NPC.value = 60f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.knockBackResist = 0.5f;
			NPC.aiStyle = 22;
			AIType = NPCID.FloatyGross;
			Banner = NPC.type;
            BannerItem = ModContent.ItemType<IscreamerBanner>();
            SpawnModBiomes = new int[] { ModContent.GetInstance<ConfectionUndergroundBiome>().Type, ModContent.GetInstance<IceConfectionUndergroundBiome>().Type };
			Index = -1;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

                new FlavorTextBestiaryInfoElement("Mods.TheConfectionRebirth.Bestiary.Iscreamer")
            });
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (Main.netMode == NetmodeID.Server)
            {
                return;
            }

            if (NPC.life <= 0)
            {
                var entitySource = NPC.GetSource_Death();

                for (int i = 0; i < 3; i++)
                {
                    Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), 13);
                    Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), 12);
                    Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), 11);
                }
            }
        }

        const int speed = 10;
        const int maxFrames = 3;
        int frame;
		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter++;
			if (NPC.frameCounter >= speed)
			{
				frame++;
				NPC.frameCounter = 0;
			}

			if (frame > maxFrames)
				frame = 0;

			NPC.frame.Y = frame * frameHeight;
			NPC.spriteDirection = NPC.direction;
		}

		public override bool PreAI()
		{
			if (Index == -1)
			{
				Index = (sbyte)VariationManager<Iscreamer>.GetRandomGroup().Index;
				if (Main.netMode == NetmodeID.Server)
					NetMessage.SendData(MessageID.SyncNPC, number: NPC.whoAmI);
			}

			return true;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if (NPC.IsABestiaryIconDummy)
				return true;

			DS.DrawNPC(NPC, VariationManager<Iscreamer>.GetByIndex(Index).Get().Value, spriteBatch, screenPos, drawColor);
			return false;
		}

		public override void AI()
        {
            int num184 = (int)(NPC.Center.X / 16f);
            int num185 = (int)(NPC.Center.Y / 16f);
            if (++NPC.ai[3] >= 60f * 10f)
            {
                Player player = Main.player[NPC.target];
                Point playerPos = new((int)(player.Center.X / 16), (int)(player.Center.Y / 16));
                List<Tuple<int, int>> floodFindResults = Util.FloodFindFuncs.FloodFind(playerPos, 16, 25);
				NPC.ai[3] = 0f;
				if (floodFindResults.Count == 0)
                    return;

                Teleport(NPC, floodFindResults);
                List<NPC> npcsToTeleportCandidates = new();
				const float sqrtMin = 1400f * 1400f;
				float d = ConfectionWorld.DifficultyScale;
				int maddy = (int)(3500 * d);
                for (int x = 0; x < Main.npc.Length; x++)
                {
                    if (x == NPC.whoAmI)
                        continue;
                    NPC test = Main.npc[x];
                    if (test.active && !test.friendly && !test.boss && test.lifeMax < maddy && test.type != NPCID.TargetDummy && test.Center.DistanceSQ(NPC.Center) < sqrtMin)
						npcsToTeleportCandidates.Add(test);
                }
                List<NPC> npcsToTeleport = new();
                for (int i = 0; i < 4; i++)
                {
                    if (npcsToTeleportCandidates.Count == 0) break;
                    int index = Main.rand.Next(npcsToTeleportCandidates.Count);
                    npcsToTeleport.Add(npcsToTeleportCandidates[index]);
                    npcsToTeleportCandidates.RemoveAt(index);
                }
                npcsToTeleport.ForEach(i => Teleport(i, floodFindResults));

                SoundEngine.PlaySound(SoundID.Item8, NPC.Center);
            }
        }

        public void Teleport(NPC npc, List<Tuple<int, int>> floodFindResults)
        {
            float swirlSize = 1.664f;

            List<Tuple<int, int>> floodFindResultsClone = new();
            floodFindResultsClone.AddRange(floodFindResults);

            Vector2 originalPos = npc.Center;
            while (true)
            {
                int rand = Main.rand.Next(0, floodFindResultsClone.Count);
                Tuple<int, int> location = floodFindResultsClone[rand];
                Vector2 pos = new Vector2(location.Item1 * 16, location.Item2 * 16);
                npc.Center = pos;
                if (!Collision.SolidCollision(npc.position, npc.width, npc.height))
                    break;
                floodFindResultsClone.RemoveAt(rand);
                if (floodFindResultsClone.Count == 0)
                {
                    npc.Center = originalPos;
                    return;
                }
            }

            npc.netUpdate = true;
            float Closeness = 50f;
            degrees += 2.5f;
            for (float swirlDegrees = degrees; swirlDegrees < 160f + degrees; swirlDegrees += 7f)
            {
                Closeness -= swirlSize;
                double radians = (double)swirlDegrees * (Math.PI / 180.0);
                Vector2 position = npc.Center + new Vector2(Closeness * (float)Math.Sin(radians), Closeness * (float)Math.Cos(radians));
                Vector2 westPosFar = npc.Center - new Vector2(Closeness * (float)Math.Sin(radians), Closeness * (float)Math.Cos(radians));
                Vector2 northPosFar = npc.Center + new Vector2(Closeness * (float)Math.Sin(radians + 1.57), Closeness * (float)Math.Cos(radians + 1.57));
                Vector2 southPosFar = npc.Center - new Vector2(Closeness * (float)Math.Sin(radians + 1.57), Closeness * (float)Math.Cos(radians + 1.57));
                int d5 = Dust.NewDust(position, 2, 2, 159, 0f, 0f, 0, new Color(209, 255, 0));
                int d6 = Dust.NewDust(westPosFar, 2, 2, 159, 0f, 0f, 0, new Color(209, 255, 0));
                int d7 = Dust.NewDust(northPosFar, 2, 2, 159, 0f, 0f, 0, new Color(209, 255, 0));
                int d8 = Dust.NewDust(southPosFar, 2, 2, 159, 0f, 0f, 0, new Color(209, 255, 0));
                Vector2 position2 = npc.Center + new Vector2((Closeness - 30f) * (float)Math.Sin(radians), (Closeness - 30f) * (float)Math.Cos(radians));
                Vector2 westPosClose = npc.Center - new Vector2((Closeness - 30f) * (float)Math.Sin(radians), (Closeness - 30f) * (float)Math.Cos(radians));
                Vector2 northPosClose = npc.Center + new Vector2((Closeness - 30f) * (float)Math.Sin(radians + 1.57), (Closeness - 30f) * (float)Math.Cos(radians + 1.57));
                Vector2 southPosClose = npc.Center - new Vector2((Closeness - 30f) * (float)Math.Sin(radians + 1.57), (Closeness - 30f) * (float)Math.Cos(radians + 1.57));
            }
			SoundEngine.PlaySound(new SoundStyle("TheConfectionRebirth/Sounds/Custom/IceScreamerShriek"), npc.position);
		}

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BearClaw>(), 100));

			npcLoot.Add(new LeadingConditionRule(new Conditions.TenthAnniversaryIsUp())).OnSuccess(ItemDropRule.Common(ModContent.ItemType<BananawarpPeel>(), 10));

			npcLoot.Add(ItemDropRule.Food(ModContent.ItemType<Brownie>(), 150));
		}

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.InModBiome(ModContent.GetInstance<ConfectionBiome>()) && !spawnInfo.AnyInvasionActive() && Main.hardMode && (spawnInfo.Player.ZoneRockLayerHeight || spawnInfo.Player.ZoneUnderworldHeight)) {
                return 0.2f;
            }
            return 0f;
        }

		public override void SendExtraAI(BinaryWriter writer) => writer.Write(Index);

		public override void ReceiveExtraAI(BinaryReader reader) => Index = reader.ReadSByte();
	}
}

/*else if (aiStyle == 22)
	{
		bool flag17 = false;
		bool flag18 = type == 330 && !Main.pumpkinMoon;
		if (type == 253 && !Main.eclipse)
		{
			flag18 = true;
		}
		if (type == 490 && Main.dayTime)
		{
			flag18 = true;
		}
		if (justHit)
		{
			this.ai[2] = 0f;
		}
		if (type == 316 && (Main.player[target].dead || Vector2.Distance(base.Center, Main.player[target].Center) > 3000f))
		{
			TargetClosest();
			if (Main.player[target].dead || Vector2.Distance(base.Center, Main.player[target].Center) > 3000f)
			{
				EncourageDespawn(10);
				flag17 = true;
				flag18 = true;
			}
		}
		if (flag18)
		{
			if (velocity.X == 0f)
			{
				velocity.X = (float)Main.rand.Next(-1, 2) * 1.5f;
				netUpdate = true;
			}
		}
		else if (this.ai[2] >= 0f)
		{
			int num298 = 16;
			bool flag19 = false;
			bool flag20 = false;
			if (position.X > this.ai[0] - (float)num298 && position.X < this.ai[0] + (float)num298)
			{
				flag19 = true;
			}
			else if ((velocity.X < 0f && direction > 0) || (velocity.X > 0f && direction < 0))
			{
				flag19 = true;
			}
			num298 += 24;
			if (position.Y > this.ai[1] - (float)num298 && position.Y < this.ai[1] + (float)num298)
			{
				flag20 = true;
			}
			if (flag19 && flag20)
			{
				this.ai[2] += 1f;
				if (this.ai[2] >= 30f && num298 == 16)
				{
					flag17 = true;
				}
				if (this.ai[2] >= 60f)
				{
					this.ai[2] = -200f;
					direction *= -1;
					velocity.X *= -1f;
					collideX = false;
				}
			}
			else
			{
				this.ai[0] = position.X;
				this.ai[1] = position.Y;
				this.ai[2] = 0f;
			}
			TargetClosest();
		}
		else if (type == 253)
		{
			TargetClosest();
			this.ai[2] += 2f;
		}
		else
		{
			if (type == 330)
			{
				this.ai[2] += 0.1f;
			}
			else
			{
				this.ai[2] += 1f;
			}
			if (Main.player[target].position.X + (float)(Main.player[target].width / 2) > position.X + (float)(width / 2))
			{
				direction = -1;
			}
			else
			{
				direction = 1;
			}
		}
		int num299 = (int)((position.X + (float)(width / 2)) / 16f) + direction * 2;
		int num300 = (int)((position.Y + (float)height) / 16f);
		bool flag21 = true;
		bool flag22 = false;
		int num301 = 3;
		if (type == 122)
		{
			if (justHit)
			{
				this.ai[3] = 0f;
				localAI[1] = 0f;
			}
			float num302 = 7f;
			Vector2 vector33 = new Vector2(position.X + (float)width * 0.5f, position.Y + (float)height * 0.5f);
			float num303 = Main.player[target].position.X + (float)(Main.player[target].width / 2) - vector33.X;
			float num304 = Main.player[target].position.Y + (float)(Main.player[target].height / 2) - vector33.Y;
			float num305 = (float)Math.Sqrt(num303 * num303 + num304 * num304);
			float num306 = num305;
			num305 = num302 / num305;
			num303 *= num305;
			num304 *= num305;
			if (Main.netMode != 1 && this.ai[3] == 32f && !Main.player[target].npcTypeNoAggro[type])
			{
				int num307 = 25;
				int num308 = 84;
				int num309 = Projectile.NewProjectile(GetSpawnSource_ForProjectile(), vector33.X, vector33.Y, num303, num304, num308, num307, 0f, Main.myPlayer);
			}
			num301 = 8;
			if (this.ai[3] > 0f)
			{
				this.ai[3] += 1f;
				if (this.ai[3] >= 64f)
				{
					this.ai[3] = 0f;
				}
			}
			if (Main.netMode != 1 && this.ai[3] == 0f)
			{
				localAI[1] += 1f;
				if (localAI[1] > 120f && Collision.CanHit(position, width, height, Main.player[target].position, Main.player[target].width, Main.player[target].height) && !Main.player[target].npcTypeNoAggro[type])
				{
					localAI[1] = 0f;
					this.ai[3] = 1f;
					netUpdate = true;
				}
			}
		}
		else if (type == 75)
		{
			num301 = 4;
			position += netOffset;
			if (Main.rand.Next(6) == 0)
			{
				int num310 = Dust.NewDust(position, width, height, 55, 0f, 0f, 200, this.color);
				Dust dust = Main.dust[num310];
				dust.velocity *= 0.3f;
			}
			if (Main.rand.Next(40) == 0)
			{
				SoundEngine.PlaySound(27, (int)position.X, (int)position.Y);
			}
			position -= netOffset;
		}
		else if (type == 169)
		{
			position += netOffset;
			Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0f, 0.6f, 0.75f);
			alpha = 30;
			if (Main.rand.Next(3) == 0)
			{
				int num311 = Dust.NewDust(position, width, height, 92, 0f, 0f, 200);
				Dust dust = Main.dust[num311];
				dust.velocity *= 0.3f;
				Main.dust[num311].noGravity = true;
			}
			position -= netOffset;
			if (justHit)
			{
				this.ai[3] = 0f;
				localAI[1] = 0f;
			}
			float num312 = 5f;
			Vector2 vector34 = new Vector2(position.X + (float)width * 0.5f, position.Y + (float)height * 0.5f);
			float num313 = Main.player[target].position.X + (float)(Main.player[target].width / 2) - vector34.X;
			float num314 = Main.player[target].position.Y + (float)(Main.player[target].height / 2) - vector34.Y;
			float num315 = (float)Math.Sqrt(num313 * num313 + num314 * num314);
			float num316 = num315;
			num315 = num312 / num315;
			num313 *= num315;
			num314 *= num315;
			if (num313 > 0f)
			{
				direction = 1;
			}
			else
			{
				direction = -1;
			}
			spriteDirection = direction;
			if (direction < 0)
			{
				rotation = (float)Math.Atan2(0f - num314, 0f - num313);
			}
			else
			{
				rotation = (float)Math.Atan2(num314, num313);
			}
			if (Main.netMode != 1 && this.ai[3] == 16f)
			{
				int num317 = 45;
				int num318 = 128;
				int num319 = Projectile.NewProjectile(GetSpawnSource_ForProjectile(), vector34.X, vector34.Y, num313, num314, num318, num317, 0f, Main.myPlayer);
			}
			num301 = 10;
			if (this.ai[3] > 0f)
			{
				this.ai[3] += 1f;
				if (this.ai[3] >= 64f)
				{
					this.ai[3] = 0f;
				}
			}
			if (Main.netMode != 1 && this.ai[3] == 0f)
			{
				localAI[1] += 1f;
				if (localAI[1] > 120f && Collision.CanHit(position, width, height, Main.player[target].position, Main.player[target].width, Main.player[target].height))
				{
					localAI[1] = 0f;
					this.ai[3] = 1f;
					netUpdate = true;
				}
			}
		}
		else if (type == 268)
		{
			rotation = velocity.X * 0.1f;
			num301 = ((!(Main.player[target].Center.Y < base.Center.Y)) ? 6 : 12);
			if (Main.netMode != 1 && !confused)
			{
				this.ai[3] += 1f;
				if (justHit)
				{
					this.ai[3] = -45f;
					localAI[1] = 0f;
				}
				if (Main.netMode != 1 && this.ai[3] >= (float)(60 + Main.rand.Next(60)))
				{
					this.ai[3] = 0f;
					if (Collision.CanHit(position, width, height, Main.player[target].position, Main.player[target].width, Main.player[target].height))
					{
						float num320 = 10f;
						Vector2 vector35 = new Vector2(position.X + (float)width * 0.5f - 4f, position.Y + (float)height * 0.7f);
						float num321 = Main.player[target].position.X + (float)(Main.player[target].width / 2) - vector35.X;
						float num322 = Math.Abs(num321) * 0.1f;
						float num323 = Main.player[target].position.Y + (float)(Main.player[target].height / 2) - vector35.Y - num322;
						num321 += (float)Main.rand.Next(-10, 11);
						num323 += (float)Main.rand.Next(-30, 21);
						float num324 = (float)Math.Sqrt(num321 * num321 + num323 * num323);
						float num325 = num324;
						num324 = num320 / num324;
						num321 *= num324;
						num323 *= num324;
						int num326 = 40;
						int num327 = 288;
						int num328 = Projectile.NewProjectile(GetSpawnSource_ForProjectile(), vector35.X, vector35.Y, num321, num323, num327, num326, 0f, Main.myPlayer);
					}
				}
			}
		}
		if (type == 490)
		{
			num301 = 4;
			if (target >= 0)
			{
				float num329 = (Main.player[target].Center - base.Center).Length();
				num329 /= 70f;
				if (num329 > 8f)
				{
					num329 = 8f;
				}
				num301 += (int)num329;
			}
		}
		if (position.Y + (float)height > Main.player[target].position.Y)
		{
			for (int num330 = num300; num330 < num300 + num301; num330++)
			{
				if (Main.tile[num299, num330] == null)
				{
					Main.tile[num299, num330] = new Tile();
				}
				if ((Main.tile[num299, num330].nactive() && Main.tileSolid[Main.tile[num299, num330].type]) || Main.tile[num299, num330].liquid > 0)
				{
					if (num330 <= num300 + 1)
					{
						flag22 = true;
					}
					flag21 = false;
					break;
				}
			}
		}
		if (Main.player[target].npcTypeNoAggro[type])
		{
			bool flag23 = false;
			for (int num331 = num300; num331 < num300 + num301 - 2; num331++)
			{
				if (Main.tile[num299, num331] == null)
				{
					Main.tile[num299, num331] = new Tile();
				}
				if ((Main.tile[num299, num331].nactive() && Main.tileSolid[Main.tile[num299, num331].type]) || Main.tile[num299, num331].liquid > 0)
				{
					flag23 = true;
					break;
				}
			}
			directionY = (!flag23).ToDirectionInt();
		}
		if (type == 169 || type == 268)
		{
			for (int num332 = num300 - 3; num332 < num300; num332++)
			{
				if (Main.tile[num299, num332] == null)
				{
					Main.tile[num299, num332] = new Tile();
				}
				if ((Main.tile[num299, num332].nactive() && Main.tileSolid[Main.tile[num299, num332].type] && !TileID.Sets.Platforms[Main.tile[num299, num332].type]) || Main.tile[num299, num332].liquid > 0)
				{
					flag22 = false;
					flag17 = true;
					break;
				}
			}
		}
		if (flag17)
		{
			flag22 = false;
			flag21 = true;
			if (type == 268)
			{
				velocity.Y += 2f;
			}
		}
		if (flag21)
		{
			if (type == 75 || type == 169)
			{
				velocity.Y += 0.2f;
				if (velocity.Y > 2f)
				{
					velocity.Y = 2f;
				}
			}
			else if (type == 490)
			{
				velocity.Y += 0.03f;
				if ((double)velocity.Y > 0.75)
				{
					velocity.Y = 0.75f;
				}
			}
			else
			{
				velocity.Y += 0.1f;
				if (type == 316 && flag18)
				{
					velocity.Y -= 0.05f;
					if (velocity.Y > 6f)
					{
						velocity.Y = 6f;
					}
				}
				else if (velocity.Y > 3f)
				{
					velocity.Y = 3f;
				}
			}
		}
		else
		{
			if (type == 75 || type == 169)
			{
				if ((directionY < 0 && velocity.Y > 0f) || flag22)
				{
					velocity.Y -= 0.2f;
				}
			}
			else if (type == 490)
			{
				if ((directionY < 0 && velocity.Y > 0f) || flag22)
				{
					velocity.Y -= 0.075f;
				}
				if (velocity.Y < -0.75f)
				{
					velocity.Y = -0.75f;
				}
			}
			else if (directionY < 0 && velocity.Y > 0f)
			{
				velocity.Y -= 0.1f;
			}
			if (velocity.Y < -4f)
			{
				velocity.Y = -4f;
			}
		}
		if (type == 75 && wet)
		{
			velocity.Y -= 0.2f;
			if (velocity.Y < -2f)
			{
				velocity.Y = -2f;
			}
		}
		if (collideX)
		{
			velocity.X = oldVelocity.X * -0.4f;
			if (direction == -1 && velocity.X > 0f && velocity.X < 1f)
			{
				velocity.X = 1f;
			}
			if (direction == 1 && velocity.X < 0f && velocity.X > -1f)
			{
				velocity.X = -1f;
			}
		}
		if (collideY)
		{
			velocity.Y = oldVelocity.Y * -0.25f;
			if (velocity.Y > 0f && velocity.Y < 1f)
			{
				velocity.Y = 1f;
			}
			if (velocity.Y < 0f && velocity.Y > -1f)
			{
				velocity.Y = -1f;
			}
		}
		float num333 = 2f;
		if (type == 75)
		{
			num333 = 3f;
		}
		if (type == 253)
		{
			num333 = 4f;
		}
		if (type == 490)
		{
			num333 = 1.5f;
		}
		if (type == 330)
		{
			alpha = 0;
			num333 = 4f;
			if (!flag18)
			{
				TargetClosest();
			}
			else
			{
				EncourageDespawn(10);
			}
			if (direction < 0 && velocity.X > 0f)
			{
				velocity.X *= 0.9f;
			}
			if (direction > 0 && velocity.X < 0f)
			{
				velocity.X *= 0.9f;
			}
		}
		if (direction == -1 && velocity.X > 0f - num333)
		{
			velocity.X -= 0.1f;
			if (velocity.X > num333)
			{
				velocity.X -= 0.1f;
			}
			else if (velocity.X > 0f)
			{
				velocity.X += 0.05f;
			}
			if (velocity.X < 0f - num333)
			{
				velocity.X = 0f - num333;
			}
		}
		else if (direction == 1 && velocity.X < num333)
		{
			velocity.X += 0.1f;
			if (velocity.X < 0f - num333)
			{
				velocity.X += 0.1f;
			}
			else if (velocity.X < 0f)
			{
				velocity.X -= 0.05f;
			}
			if (velocity.X > num333)
			{
				velocity.X = num333;
			}
		}
		num333 = ((type != 490) ? 1.5f : 1f);
		if (directionY == -1 && velocity.Y > 0f - num333)
		{
			velocity.Y -= 0.04f;
			if (velocity.Y > num333)
			{
				velocity.Y -= 0.05f;
			}
			else if (velocity.Y > 0f)
			{
				velocity.Y += 0.03f;
			}
			if (velocity.Y < 0f - num333)
			{
				velocity.Y = 0f - num333;
			}
		}
		else if (directionY == 1 && velocity.Y < num333)
		{
			velocity.Y += 0.04f;
			if (velocity.Y < 0f - num333)
			{
				velocity.Y += 0.05f;
			}
			else if (velocity.Y < 0f)
			{
				velocity.Y -= 0.03f;
			}
			if (velocity.Y > num333)
			{
				velocity.Y = num333;
			}
		}
		if (type == 122)
		{
			Lighting.AddLight((int)position.X / 16, (int)position.Y / 16, 0.4f, 0f, 0.25f);
		}
	}*/
