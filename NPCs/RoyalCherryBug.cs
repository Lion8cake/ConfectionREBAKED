using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Dusts;
using TheConfectionRebirth.Items.Banners;

namespace TheConfectionRebirth.NPCs
{
	public class RoyalCherryBug : ModNPC
	{
		public override void SetStaticDefaults() 
		{
			Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.LightningBug];
			Main.npcCatchable[Type] = true;
			NPCID.Sets.CountsAsCritter[Type] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
		}

		public override void SetDefaults() 
		{
			NPC.width = 12;
			NPC.height = 12;
			NPC.aiStyle = -1;
			NPC.damage = 0;
			NPC.defense = 0;
			NPC.lifeMax = 5;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.npcSlots = 0.2f;
			NPC.noGravity = true;
			NPC.catchItem = ModContent.ItemType<Items.RoyalCherryBug>();
			SpawnModBiomes = new int[1] { ModContent.GetInstance<ConfectionBiome>().Type };
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) 
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
				new BestiaryBackground(ModContent.Request<Texture2D>("TheConfectionRebirth/Biomes/ConfectionBiomeMapBackground"), new Color(35, 40, 40)),
				new BestiaryBackgroundOverlay(Main.Assets.Request<Texture2D>("Images/MapBGOverlay4"), Color.White),
				new FlavorTextBestiaryInfoElement("Mods.TheConfectionRebirth.Bestiary.RoyalCherryBug")
			});
		}

		public override bool? CanBeHitByProjectile(Projectile projectile) 
		{
			if (NPC.ai[2] > 0) {
				return null;
			}
			else {
				return false;
			}
		}

		public override void AI()
		{
			float num12 = NPC.ai[0];
			float num13 = NPC.ai[1];
			if (Main.netMode != 1) {
				NPC.localAI[0] -= 1f;
				if (NPC.ai[3] == 0f) {
					NPC.ai[3] = (float)Main.rand.Next(75, 111) * 0.01f;
				}
				if (NPC.localAI[0] <= 0f) {
					NPC.TargetClosest();
					NPC.localAI[0] = Main.rand.Next(60, 180);
					float num15 = Math.Abs(NPC.Center.X - Main.player[NPC.target].Center.X);
					if (num15 > 700f && NPC.localAI[3] == 0f) {
						float num16 = (float)Main.rand.Next(50, 151) * 0.01f;
						if (num15 > 1000f) {
							num16 = (float)Main.rand.Next(150, 201) * 0.01f;
						}
						else if (num15 > 850f) {
							num16 = (float)Main.rand.Next(100, 151) * 0.01f;
						}
						int num17 = NPC.direction * Main.rand.Next(100, 251);
						int num18 = Main.rand.Next(-50, 51);
						if (NPC.position.Y > Main.player[NPC.target].position.Y - 100f) {
							num18 -= Main.rand.Next(100, 251);
						}
						float num19 = num16 / (float)Math.Sqrt(num17 * num17 + num18 * num18);
						num12 = (float)num17 * num19;
						num13 = (float)num18 * num19;
					}
					else {
						NPC.localAI[3] = 1f;
						float num20 = (float)Main.rand.Next(5, 151) * 0.01f;
						int num21 = Main.rand.Next(-100, 101);
						int num22 = Main.rand.Next(-100, 101);
						float num23 = num20 / (float)Math.Sqrt(num21 * num21 + num22 * num22);
						num12 = (float)num21 * num23;
						num13 = (float)num22 * num23;
					}
					NPC.netUpdate = true;
				}
			}
			NPC.scale = NPC.ai[3];
			if (NPC.localAI[2] > 0f) {
				int i3 = (int)NPC.Center.X / 16;
				int j3 = (int)NPC.Center.Y / 16;
				if (NPC.localAI[2] > 3f) {
					Lighting.AddLight(i3, j3, 1.77f * NPC.scale, 1.12f * NPC.scale, 0.71f * NPC.scale);
				}
				NPC.localAI[2] -= 1f;
			}
			else if (NPC.localAI[1] > 0f) {
				NPC.localAI[1] -= 1f;
			}
			else {
				NPC.localAI[1] = Main.rand.Next(30, 180);
				if (!Main.dayTime || (double)(NPC.position.Y / 16f) > Main.worldSurface + 10.0) {
					NPC.localAI[2] = Main.rand.Next(10, 30);
				}
			}
			int num30 = 80;
			NPC.velocity.X = (NPC.velocity.X * (float)(num30 - 1) + num12) / (float)num30;
			NPC.velocity.Y = (NPC.velocity.Y * (float)(num30 - 1) + num13) / (float)num30;
			if (NPC.velocity.Y > 0f) {
				int num31 = 4;
				int num32 = (int)NPC.Center.X / 16;
				int num33 = (int)NPC.Center.Y / 16;
				for (int num34 = num33; num34 < num33 + num31; num34++) {
					if (WorldGen.InWorld(num32, num34, 2) && Main.tile[num32, num34] != null && ((Main.tile[num32, num34].HasUnactuatedTile && Main.tileSolid[Main.tile[num32, num34].TileType]) || Main.tile[num32, num34].LiquidAmount > 0)) {
						num13 *= -1f;
						if (NPC.velocity.Y > 0f) {
							NPC.velocity.Y *= 0.9f;
						}
					}
				}
			}
			if (NPC.velocity.Y < 0f) {
				int num35 = 30;
				bool flag59 = false;
				int num37 = (int)NPC.Center.X / 16;
				int num38 = (int)NPC.Center.Y / 16;
				for (int num39 = num38; num39 < num38 + num35; num39++) {
					if (WorldGen.InWorld(num37, num39, 2) && Main.tile[num37, num39] != null && Main.tile[num37, num39].HasUnactuatedTile && Main.tileSolid[Main.tile[num37, num39].TileType]) {
						flag59 = true;
					}
				}
				if (!flag59) {
					num13 *= -1f;
					if (NPC.velocity.Y < 0f) {
						NPC.velocity.Y *= 0.9f;
					}
				}
			}
			if (NPC.collideX) {
				num12 = ((!(NPC.velocity.X < 0f)) ? (0f - Math.Abs(num12)) : Math.Abs(num12));
				NPC.velocity.X *= -0.2f;
			}
			if (NPC.velocity.X < 0f) {
				NPC.direction = -1;
			}
			if (NPC.velocity.X > 0f) {
				NPC.direction = 1;
			}
			NPC.ai[0] = num12;
			NPC.ai[1] = num13;
			if (Main.dayTime) {
				NPC.alpha += 2;
			}
			else if (NPC.alpha > 0) {
				NPC.alpha -= 2;
			}
			if (NPC.alpha >= 255) {
				NPC.active = false;
			}

			if (NPC.ai[2] == 0) {
				NPC.ai[2] = 1;
			}
			bool playerIsNear = false;
			for (int i = 0; i < Main.player.Length; i++) {
				if (Main.player[i].Center.Distance(NPC.Center) < 250) {
					playerIsNear = true;
					break;
				}
			}
			if (playerIsNear && NPC.ai[2] < 1) {
				NPC.ai[2] += 0.02f;
			}
			else if (NPC.ai[2] >= 0) {
				NPC.ai[2] -= 0.02f;
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) 
		{
			Texture2D ring = TextureAssets.Extra[ExtrasID.KeybrandRing].Value;
			spriteBatch.Draw(ring, NPC.Center - Main.screenPosition - new Vector2(0, 6), new Rectangle(0, 0, ring.Width, ring.Height), new Color(128, 34, 12, 0) * NPC.ai[2] * NPC.Opacity, (float)Main.timeForVisualEffects * 0.001f, ring.Size() / 2f, (0.6f + (float)Math.Sin(Main.timeForVisualEffects * 0.05f) * 0.1f) * NPC.ai[2], SpriteEffects.None, 0);
			spriteBatch.Draw(ring, NPC.Center - Main.screenPosition - new Vector2(0, 6), new Rectangle(0, 0, ring.Width, ring.Height), new Color(128, 34, 64, 0) * NPC.ai[2] * NPC.Opacity, (float)Main.timeForVisualEffects * 0.001f, ring.Size() / 2f, (0.5f + (float)Math.Sin(Main.timeForVisualEffects * 0.05f) * 0.14f) * NPC.ai[2], SpriteEffects.None, 0);
			return true;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) 
		{
			Texture2D Sparkle = TextureAssets.Extra[ExtrasID.SharpTears].Value;
			for (int i = 0; i < 2; i++)
				spriteBatch.Draw(Sparkle, NPC.Center - Main.screenPosition - new Vector2(0, 4), new Rectangle(0, 0, Sparkle.Width, Sparkle.Height), new Color(138, 64, 94, 0) * NPC.Opacity * NPC.ai[2] * (float)(Math.Sin(Main.timeForVisualEffects * 0.1f) * 0.5f + 0.5f), (i * MathHelper.PiOver2) + (float)Main.timeForVisualEffects * -0.01f, Sparkle.Size() / 2f,
					new Vector2((1.1f + (float)Math.Sin(Main.timeForVisualEffects * 0.03f) * 0.3f) * NPC.ai[2], (0.7f + (float)Math.Sin(Main.timeForVisualEffects * 0.1f) * 0.7f) * NPC.ai[2]), SpriteEffects.None, 0);
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.spriteDirection = NPC.direction;
			NPC.frameCounter += 1.0;
			if (NPC.frameCounter < 4.0)
			{
				NPC.frame.Y = 0;
			}
			else
			{
				NPC.frame.Y = frameHeight;
				if (NPC.frameCounter >= 7.0)
				{
					NPC.frameCounter = 0.0;
				}
			}
			if (NPC.localAI[2] <= 0f)
			{
				NPC.frame.Y += frameHeight * 2;
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return ConfectionGlobalNPC.SpawnNPC_ConfectionNPC(spawnInfo, Type);
		}

		public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode == NetmodeID.Server)
			{
				return;
			}

			if (NPC.life <= 0)
			{
				for (int i = 0; i < 6; i++)
				{
					int dustID = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.FireflyHit, 2 * hit.HitDirection, -2f);
					if (Main.rand.NextBool(2))
					{
						Main.dust[dustID].noGravity = true;
						Main.dust[dustID].scale = 1.5f * NPC.scale;
					}
					else
					{
						Main.dust[dustID].scale = 0.8f * NPC.scale;
					}
				}
			}
		}
	}
}
