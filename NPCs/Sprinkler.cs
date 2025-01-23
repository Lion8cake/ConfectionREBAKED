using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Dusts;
using TheConfectionRebirth.Items.Banners;

namespace TheConfectionRebirth.NPCs
{
	public class Sprinkler : ModNPC
	{
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 2;
		}

		public override void SetDefaults()
		{
			NPC.width = 42;
			NPC.height = 30;
			NPC.damage = 70;
			NPC.defense = 22;
			NPC.lifeMax = 120;
			NPC.HitSound = SoundID.NPCHit5;
			NPC.DeathSound = SoundID.NPCDeath7;
			NPC.knockBackResist = 0f;
			NPC.aiStyle = 0;
			AIType = -1;
			AnimationType = NPCID.BlueSlime;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<SprinklingBanner>();
			SpawnModBiomes = new int[1] { ModContent.GetInstance<ConfectionBiome>().Type };
			NPC.gfxOffY = 4;
			DrawOffsetY = 4;
		}

		public override void OnSpawn(IEntitySource source)
		{
			//spawn variants
			if (Main.rand.NextBool(2))
			{
				int type = Type;
				if (Main.halloween)
				{
					type = Main.rand.Next(0, 2) == 1 ? ModContent.NPCType<Sprinkler_Halloween1>() : ModContent.NPCType<Sprinkler_Halloween2>();
				}
				else if (Main.xMas)
				{
					type = ModContent.NPCType<Sprinkler_Xmas>();
				}
				if (type != Type)
				{
					NPC.Transform(type);
				}
			}
		}

		public override void AI() 
		{
			SprinklerAI_Variantion(0);
		}

		internal void SprinklerAI_Variantion(int variant)
		{
			NPC.TargetClosest();
			float power = 12f;
			float launchX = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) - NPC.Center.X;
			float launchY = Main.player[NPC.target].position.Y - NPC.Center.Y;
			float trijectory = (float)Math.Sqrt(launchX * launchX + launchY * launchY);
			trijectory = power / trijectory;
			launchX *= trijectory;
			launchY *= trijectory;
			if (NPC.directionY < 0)
			{
				if (NPC.velocity.X != 0f)
				{
					NPC.velocity.X *= 0.9f;
					if ((double)NPC.velocity.X > -0.1 || (double)NPC.velocity.X < 0.1)
					{
						NPC.netUpdate = true;
						NPC.velocity.X = 0f;
					}
				}
			}
			if (NPC.ai[0] > 0f)
			{
				NPC.ai[0] -= 1f;
			}

			if (Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height))
			{
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					if (NPC.ai[0] == 0f)
					{
						NPC.ai[0] = 200f;
					}
					int damage = 55;
					if (NPC.ai[0] == 30f)
					{
						int type = ModContent.ProjectileType<Projectiles.SprinklerBall>();
						int projID = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, launchX, launchY * 2f, type, damage, 0f, Main.myPlayer);
						Projectile proj = Main.projectile[projID];
						proj.ai[0] = 2f;
						proj.timeLeft = 300;
						proj.friendly = false;
						proj.frame = variant;
						NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, projID);
						NPC.netUpdate = true;
						SoundEngine.PlaySound(SoundID.Item5, NPC.position);
					}
					if (NPC.ai[0] == 45f)
					{
						int typeMed = ModContent.ProjectileType<Projectiles.SprinklerBall>();
						int projID = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, launchX, launchY * 2f, typeMed, damage, 0f, Main.myPlayer);
						Projectile proj = Main.projectile[projID];
						proj.ai[0] = 1f;
						proj.timeLeft = 300;
						proj.friendly = false;
						proj.frame = variant;
						NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, projID);
						NPC.netUpdate = true;
						SoundEngine.PlaySound(SoundID.Item5, NPC.position);
					}
					if (NPC.ai[0] == 60f)
					{
						int typeLar = ModContent.ProjectileType<Projectiles.SprinklerBall>();
						int projID = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, launchX, launchY * 2f, typeLar, damage, 0f, Main.myPlayer);
						Projectile proj = Main.projectile[projID];
						proj.ai[0] = 0f;
						proj.timeLeft = 300;
						proj.friendly = false;
						proj.frame = variant;
						NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, projID);
						NPC.netUpdate = true;
						SoundEngine.PlaySound(SoundID.Item5, NPC.position);
					}
				}
			}
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[ModContent.NPCType<Sprinkling>()], quickUnlock: true);
			ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[Type] = ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[ModContent.NPCType<Sprinkling>()];

			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

				new FlavorTextBestiaryInfoElement("Mods.TheConfectionRebirth.Bestiary.Sprinkler")
			});
		}

		public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
		{
			NPC.damage = (int)(NPC.damage * 0.2f);
		}

		public override void OnKill()
		{
			Vector2 spawnAt = NPC.Center + new Vector2(0f, NPC.height / 2f);
			int index = NPC.NewNPC(NPC.GetSource_FromAI(), (int)spawnAt.X, (int)spawnAt.Y, ModContent.NPCType<Sprinkling>());
			if (Main.netMode == NetmodeID.Server)
				NetMessage.SendData(MessageID.SyncNPC, number: index);
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

			for (int i = 0; i < hit.Damage / (double)NPC.lifeMax * 10.0; i++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<SprinklingDust>(), 2.5f * (float)hit.HitDirection, -2.5f);
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			return SprinklerDrawing(0, spriteBatch, drawColor, screenPos);
		}

		internal bool SprinklerDrawing(int variant, SpriteBatch spriteBatch, Color drawColor, Vector2 screenPos)
		{
			Texture2D texture;
			Rectangle frame = NPC.frame;
			Vector2 pos = NPC.Center - screenPos;
			pos.Y += NPC.gfxOffY + 4f;

			int frameOff = (NPC.frame.Y != 0).ToInt() * 2;
			Texture2D front = ModContent.Request<Texture2D>($"TheConfectionRebirth/NPCs/Sprinkler/Sprinkler_" + variant + "_1").Value;
			texture = ModContent.Request<Texture2D>($"TheConfectionRebirth/NPCs/Sprinkler/Sprinkler_" + variant + "_0").Value;

			spriteBatch.Draw(texture, pos + new Vector2(0f, frameOff), new(0, 0, 42, 24), drawColor, NPC.rotation, frame.Size() * 0.5f, NPC.scale, 0, 0f);
			spriteBatch.Draw(front, pos, frame, drawColor, NPC.rotation, frame.Size() * 0.5f, NPC.scale, 0, 0f);
			return false;
		}
	}
}