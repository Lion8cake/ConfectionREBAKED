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
using TheConfectionRebirth.Items.Banners;

namespace TheConfectionRebirth.NPCs
{

    public class CrazyCone : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 6;
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, new NPCID.Sets.NPCBestiaryDrawModifiers
            {
                Position = new Vector2(-2f, 6f),
                Velocity = 1f,
                PortraitPositionXOverride = -8f,
                PortraitPositionYOverride = 2f
            });
        }

        public override void SetDefaults()
        {
			NPC.width = 40;
			NPC.height = 40;
			NPC.aiStyle = -1;
			NPC.damage = 80;
			NPC.defense = 18;
			NPC.lifeMax = 200;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.NPCDeath6;
			NPC.value = 1000f;
			NPC.knockBackResist = 0.4f;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<CrazyConeBanner>();
            SpawnModBiomes = new int[1] { ModContent.GetInstance<ConfectionUndergroundBiome>().Type };
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

                new FlavorTextBestiaryInfoElement("Mods.TheConfectionRebirth.Bestiary.CrazyCone")
            });
        }

		public override void AI() {
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			Lighting.AddLight((int)((NPC.position.X + (float)(NPC.width / 2)) / 16f), (int)((NPC.position.Y + (float)(NPC.height / 2)) / 16f), 0.81f, 0.25f, 0.33f);
			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead) {
				NPC.TargetClosest();
			}
			if (NPC.ai[0] == 0f) {
				float num869 = 9f;
				Vector2 vector249 = new(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
				float num870 = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) - vector249.X;
				float num871 = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2) - vector249.Y;
				float num872 = (float)Math.Sqrt(num870 * num870 + num871 * num871);
				float num873 = num872;
				num872 = num869 / num872;
				num870 *= num872;
				num871 *= num872;
				NPC.velocity.X = num870;
				NPC.velocity.Y = num871;
				NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X) + 0.785f;
				NPC.ai[0] = 1f;
				NPC.ai[1] = 0f;
				NPC.netUpdate = true;
			}
			else if (NPC.ai[0] == 1f) {
				if (NPC.justHit) {
					NPC.ai[0] = 2f;
					NPC.ai[1] = 0f;
				}
				NPC.velocity *= 0.99f;
				NPC.ai[1] += 1f;
				if (NPC.ai[1] >= 100f) {
					NPC.netUpdate = true;
					NPC.ai[0] = 2f;
					NPC.ai[1] = 0f;
					NPC.velocity.X = 0f;
					NPC.velocity.Y = 0f;
				}
				else {
					NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X) + 0.785f;
				}
			}
			else {
				if (NPC.justHit) {
					NPC.ai[0] = 2f;
					NPC.ai[1] = 0f;
				}
				NPC.velocity *= 0.96f;
				NPC.ai[1] += 1f;
				float num875 = NPC.ai[1] / 120f;
				num875 = 0.1f + num875 * 0.4f;
				NPC.rotation += num875 * (float)NPC.direction;
				if (NPC.ai[1] >= 120f) {
					NPC.netUpdate = true;
					NPC.ai[0] = 0f;
					NPC.ai[1] = 0f;
				}
			}
			NPC.rotation -= MathHelper.ToRadians(45f);
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo) {
			if (Main.rand.NextBool(3)) {
				target.AddBuff(BuffID.Confused, 120);
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.NormalvsExpert(ItemID.Nazar, 100, 50));
        }

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			SpriteEffects spriteEffects = (SpriteEffects)0;
			if (NPC.spriteDirection == 1)
			{
				spriteEffects = (SpriteEffects)1;
			}
			float num35 = 0f;
			float num36 = Main.NPCAddHeight(NPC);
			Vector2 halfSize = new Vector2(TextureAssets.Npc[Type].Width() / 2, TextureAssets.Npc[Type].Height() / Main.npcFrameCount[Type] / 2);
			spriteBatch.Draw(TextureAssets.Npc[Type].Value, new Vector2(NPC.position.X - screenPos.X + (float)(NPC.width / 2) - (float)TextureAssets.Npc[Type].Width() * NPC.scale / 2f + halfSize.X * NPC.scale, NPC.position.Y - screenPos.Y + (float)NPC.height - (float)TextureAssets.Npc[Type].Height() * NPC.scale / (float)Main.npcFrameCount[Type] + 4f + halfSize.Y * NPC.scale + num36 + num35), NPC.frame, Color.White, NPC.rotation, halfSize, NPC.scale, spriteEffects, 0f);
		}

		public override void FindFrame(int frameHeight)
		{
			if (NPC.ai[0] == 2f)
			{
				NPC.frameCounter = 0.0;
				NPC.frame.Y = 0;
				return;
			}
			NPC.frameCounter += 1.0;
			if (NPC.frameCounter >= 4.0)
			{
				NPC.frameCounter = 0.0;
				NPC.frame.Y += frameHeight;
				if (NPC.frame.Y / frameHeight >= Main.npcFrameCount[NPC.type])
				{
					NPC.frame.Y = 0;
				}
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            //if (spawnInfo.Player.InModBiome(ModContent.GetInstance<ConfectionBiome>()) && !spawnInfo.AnyInvasionActive() && Main.hardMode && (spawnInfo.Player.ZoneRockLayerHeight || spawnInfo.Player.ZoneUnderworldHeight)) {
            //    return 0.08f;
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
					int dustID = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Smoke, 0f, 0f, 0, default(Color), 1.5f);
					Main.dust[dustID].noGravity = true;
				}
			}
			else
			{
				for (int i = 0; i < 20; i++)
				{
					int dustID = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Smoke, 0f, 0f, 0, default(Color), 1.5f);
					Dust dust = Main.dust[dustID];
					dust.velocity *= 2f;
					dust.noGravity = true;
				}
				int goreID = Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.position.X, NPC.position.Y + (float)(NPC.height / 2) - 10f), new Vector2(Main.rand.Next(-2, 3), Main.rand.Next(-2, 3)), 61, NPC.scale);
				Gore gore = Main.gore[goreID];
				gore.velocity *= 0.5f;
				goreID = Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.position.X, NPC.position.Y + (float)(NPC.height / 2) - 10f), new Vector2(Main.rand.Next(-2, 3), Main.rand.Next(-2, 3)), 61, NPC.scale);
				gore = Main.gore[goreID];
				gore.velocity *= 0.5f;
				goreID = Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.position.X, NPC.position.Y + (float)(NPC.height / 2) - 10f), new Vector2(Main.rand.Next(-2, 3), Main.rand.Next(-2, 3)), 61, NPC.scale);
				gore = Main.gore[goreID];
				gore.velocity *= 0.5f;
			}
		}
    }
}