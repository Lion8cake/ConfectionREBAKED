using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;
using System;
using Terraria;
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
    public class BigMimicConfection : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 14;
			NPCID.Sets.TrailCacheLength[NPC.type] = NPCID.Sets.TrailCacheLength[NPCID.BigMimicHallow];
			NPCID.Sets.TrailingMode[NPC.type] = NPCID.Sets.TrailingMode[NPCID.BigMimicHallow];
		}

        public override void SetDefaults()
        {
			NPC.width = 28;
			NPC.height = 44;
			NPC.aiStyle = 87;
			NPC.damage = 90;
			NPC.defense = 34;
			NPC.lifeMax = 3500;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.NPCDeath6;
			NPC.value = 30000f;
			NPC.knockBackResist = 0.1f;
			NPC.rarity = 5;
			Banner = Type;
			BannerItem = ModContent.ItemType<ConfectionMimicBanner>();
			SpawnModBiomes = new int[1] { ModContent.GetInstance<ConfectionUndergroundBiome>().Type };
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

                new FlavorTextBestiaryInfoElement("Mods.TheConfectionRebirth.Bestiary.BigMimicConfection")
            });
        }

		public override void FindFrame(int frameHeight)
		{
			if (NPC.velocity.Y == 0f)
			{
				NPC.spriteDirection = NPC.direction;
			}
			if (NPC.ai[0] == 0f || NPC.ai[0] == 7f)
			{
				NPC.rotation = 0f;
				NPC.frameCounter = 0.0;
				NPC.frame.Y = 0;
			}
			else if (NPC.ai[0] == 1f)
			{
				NPC.rotation = 0f;
				NPC.frameCounter = 0.0;
				int num284 = 6;
				if (NPC.ai[1] < (float)num284)
				{
					NPC.frame.Y = frameHeight;
				}
				else if (NPC.ai[1] < (float)(num284 * 2))
				{
					NPC.frame.Y = frameHeight * 2;
				}
				else if (NPC.ai[1] < (float)(num284 * 3))
				{
					NPC.frame.Y = frameHeight * 3;
				}
				else if (NPC.ai[1] < (float)(num284 * 4))
				{
					NPC.frame.Y = frameHeight * 4;
				}
				else if (NPC.ai[1] < (float)(num284 * 5))
				{
					NPC.frame.Y = frameHeight * 5;
				}
				else
				{
					NPC.frame.Y = frameHeight * 6;
				}
			}
			else if (NPC.ai[0] == 8f)
			{
				NPC.rotation = 0f;
				NPC.frameCounter += 1.0;
				if (NPC.frameCounter >= 24.0)
				{
					NPC.frameCounter = 0.0;
				}
				NPC.frame.Y = frameHeight * Math.Min(6, Math.Max(3, 3 + (int)NPC.frameCounter / 6));
			}
			else if (NPC.ai[0] == 2f || NPC.ai[0] == 6f)
			{
				NPC.rotation = 0f;
				if (NPC.velocity.Y == 0f)
				{
					int num285 = 6;
					NPC.frameCounter += 1.0;
					if (NPC.frame.Y < frameHeight * 7)
					{
						NPC.frame.Y = frameHeight * 12;
					}
					if (NPC.frame.Y < frameHeight * 10)
					{
						if (NPC.frameCounter > 8.0)
						{
							NPC.frame.Y += frameHeight;
							NPC.frameCounter = 0.0;
							if (NPC.frame.Y == frameHeight * 10)
							{
								NPC.frameCounter = num285 * 2;
							}
						}
					}
					else if (NPC.frameCounter < (double)num285)
					{
						NPC.frame.Y = frameHeight * 12;
					}
					else if (NPC.frameCounter < (double)(num285 * 2))
					{
						NPC.frame.Y = frameHeight * 11;
					}
					else if (NPC.frameCounter < (double)(num285 * 3))
					{
						NPC.frame.Y = frameHeight * 10;
					}
					else
					{
						NPC.frame.Y = frameHeight * 11;
						if (NPC.frameCounter >= (double)(num285 * 4 - 1))
						{
							NPC.frameCounter = 0.0;
						}
					}
				}
				else
				{
					NPC.frame.Y = frameHeight * 13;
					NPC.frameCounter = 0.0;
				}
			}
			else if (NPC.ai[0] == 3f)
			{
				NPC.rotation = 0f;
				NPC.frameCounter += 1.0;
				if (NPC.frameCounter > 6.0)
				{
					NPC.frameCounter = 0.0;
					if (NPC.frame.Y > frameHeight * 7)
					{
						NPC.frame.Y -= frameHeight;
					}
				}
			}
			else if (NPC.ai[0] == 4f || NPC.ai[0] == 5f)
			{
				if (NPC.ai[0] == 4f && NPC.ai[2] == 1f)
				{
					NPC.rotation = 0f;
				}
				NPC.frame.Y = frameHeight * 13;
				NPC.frameCounter = 0.0;
			}
			else
			{
				if (NPC.ai[0] != 4.1f)
				{
					return;
				}
				NPC.rotation = 0f;
				if (NPC.frame.Y > frameHeight * 6)
				{
					NPC.frameCounter = 0.0;
				}
				NPC.frameCounter += 1.0;
				int num286 = 4;
				if (NPC.frameCounter < (double)num286)
				{
					NPC.frame.Y = frameHeight * 6;
					return;
				}
				if (NPC.frameCounter < (double)(num286 * 2))
				{
					NPC.frame.Y = frameHeight * 5;
					return;
				}
				if (NPC.frameCounter < (double)(num286 * 3))
				{
					NPC.frame.Y = frameHeight * 4;
					return;
				}
				if (NPC.frameCounter < (double)(num286 * 4))
				{
					NPC.frame.Y = frameHeight * 3;
					return;
				}
				if (NPC.frameCounter < (double)(num286 * 5))
				{
					NPC.frame.Y = frameHeight * 4;
					return;
				}
				NPC.frame.Y = frameHeight * 5;
				if (NPC.frameCounter >= (double)(num286 * 6 - 1))
				{
					NPC.frameCounter = 0.0;
				}
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
			SpriteEffects spriteEffects = (SpriteEffects)0;
			if (NPC.spriteDirection == 1) {
				spriteEffects = (SpriteEffects)1;
			}
			Vector2 halfSize = new((float)(TextureAssets.Npc[NPC.type].Width() / 2), (float)(TextureAssets.Npc[NPC.type].Height() / Main.npcFrameCount[NPC.type] / 2));
			float num306 = Main.NPCAddHeight(NPC);
			if ((int)NPC.ai[0] == 4 || NPC.ai[0] == 5f || NPC.ai[0] == 6f) {
				for (int num177 = 1; num177 < NPC.oldPos.Length; num177++) {
					_ = ref NPC.oldPos[num177];
					Color newColor5 = drawColor;
					newColor5.R = (byte)(0.5 * (int)(newColor5.R * (double)(10 - num177) / 20.0));
					newColor5.G = (byte)(0.5 * (int)(newColor5.G * (double)(10 - num177) / 20.0));
					newColor5.B = (byte)(0.5 * (int)(newColor5.B * (double)(10 - num177) / 20.0));
					newColor5.A = (byte)(0.5 * (int)(newColor5.A * (double)(10 - num177) / 20.0));
					newColor5 = NPC.GetShimmerColor(newColor5);
					spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, new Vector2(NPC.oldPos[num177].X - screenPos.X + (float)(NPC.width / 2) - (float)TextureAssets.Npc[NPC.type].Width() * NPC.scale / 2f + halfSize.X * NPC.scale, NPC.oldPos[num177].Y - screenPos.Y + (float)NPC.height - (float)TextureAssets.Npc[NPC.type].Height() * NPC.scale / (float)Main.npcFrameCount[NPC.type] + 4f + halfSize.Y * NPC.scale + num306), (Rectangle?)NPC.frame, newColor5, NPC.rotation, halfSize, NPC.scale, spriteEffects, 0f);
				}
			}
			return true;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            //if (spawnInfo.Player.InModBiome(ModContent.GetInstance<ConfectionBiome>()) && !spawnInfo.AnyInvasionActive() && Main.hardMode && (spawnInfo.Player.ZoneRockLayerHeight || spawnInfo.Player.ZoneUnderworldHeight))
            //{
            //    return 0.01f;
            //}
            return 0f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.OneFromOptionsNotScalingWithLuck(1, ModContent.ItemType<CookieCrumbler>(), ModContent.ItemType<SweetTooth>(), ModContent.ItemType<SweetHook>(), ModContent.ItemType<CreamSpray>()));
            npcLoot.Add(ItemDropRule.Common(ItemID.GreaterHealingPotion, 1, 5, 10));
            npcLoot.Add(ItemDropRule.Common(ItemID.GreaterManaPotion, 1, 5, 15));
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (Main.netMode == NetmodeID.Server)
            {
                return;
            }

			int dustType = 31;
			if (NPC.life > 0)
			{
				for (int i = 0; (double)i < hit.Damage / (double)NPC.lifeMax * 50.0; i++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, dustType);
				}
			}
            else
            {
				for (int i = 0; i < 20; i++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, dustType);
				}
				int goreID = Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2((float)hit.HitDirection, 0f), 61, NPC.scale);
				Gore gore = Main.gore[goreID];
				gore.velocity *= 0.3f;
				goreID = Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2((float)hit.HitDirection, 0f), 62, NPC.scale);
				gore = Main.gore[goreID];
				gore.velocity *= 0.3f;
				goreID = Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2((float)hit.HitDirection, 0f), 63, NPC.scale);
				gore = Main.gore[goreID];
				gore.velocity *= 0.3f;
			}
		}
	}
}