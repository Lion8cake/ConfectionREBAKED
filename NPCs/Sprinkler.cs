using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Items.Banners;
using static Humanizer.On;

namespace TheConfectionRebirth.NPCs
{

    public class Sprinkler : ModNPC
    {
        private Player player;

        private sbyte Index;

        public static Asset<Texture2D>[][] Assets = null;

        public override void Load()
        {
            Asset<Texture2D> wtf = ModContent.Request<Texture2D>(Texture);
            VariationManager<Sprinkler>.AddGroup("Normal", wtf);
            VariationManager<Sprinkler>.AddGroup("Corn", wtf, () => false && Main.halloween);
            VariationManager<Sprinkler>.AddGroup("Eye", wtf, () => Main.halloween);
            VariationManager<Sprinkler>.AddGroup("Gift", wtf, () => Main.xMas);

            if (Main.dedServ)
                return;

            Assets = new Asset<Texture2D>[VariationManager<Sprinkler>.Count][];
            for (int i = 0; i < Assets.GetLength(0); i++)
            {
                Assets[i] = new Asset<Texture2D>[2];
                for (int j = 0; j < 2; j++)
                {
                    Assets[i][j] = ModContent.Request<Texture2D>($"TheConfectionRebirth/NPCs/Sprinkler/Sprinkler_{i}_{j}");
                }
            }
        }

		public override void Unload()
		{
			VariationManager<Sprinkler>.Clear();
            Assets = null;
		}

		public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 2;

            NPCID.Sets.NPCBestiaryDrawModifiers value = new(0)
            {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
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
            NPC.value = 60f;
            // npc.noGravity = false;
            NPC.knockBackResist = 0f;
            NPC.aiStyle = 0;
            AIType = -1;
            AnimationType = NPCID.BlueSlime;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<SprinklingBanner>();
            SpawnModBiomes = new int[1] { ModContent.GetInstance<ConfectionBiome>().Type };
            Index = -1;
			NPC.gfxOffY = 4;
        }

		public override void AI() {
			NPC.TargetClosest();
			float num281 = 12f;
			Vector2 vector32 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
			float num282 = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) - vector32.X;
			float num283 = Main.player[NPC.target].position.Y - vector32.Y;
			float num284 = (float)Math.Sqrt(num282 * num282 + num283 * num283);
			num284 = num281 / num284;
			num282 *= num284;
			num283 *= num284;
			if (NPC.directionY < 0) {
				if (NPC.velocity.X != 0f) {
					NPC.velocity.X *= 0.9f;
					if ((double)NPC.velocity.X > -0.1 || (double)NPC.velocity.X < 0.1) {
						NPC.netUpdate = true;
						NPC.velocity.X = 0f;
					}
				}
			}
			if (NPC.ai[0] > 0f) {
				NPC.ai[0] -= 1f;
			}

			if (Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height)) {
				if (Main.netMode != 1 && NPC.ai[0] == 0f) {
					NPC.ai[0] = 200f;
					int num285 = 10;
					int num286 = ModContent.ProjectileType<Projectiles.SprinklingBallSmall>();
					int num287 = Projectile.NewProjectile(NPC.GetSource_FromAI(), vector32.X, vector32.Y, num282, num283, num286, num285, 0f, Main.myPlayer);
					Main.projectile[num287].ai[0] = 2f;
					Main.projectile[num287].timeLeft = 300;
					Main.projectile[num287].friendly = false;
					Main.projectile[num287].frame = Index;
					NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, num287);
					NPC.netUpdate = true;
					SoundEngine.PlaySound(SoundID.Item5, NPC.position);
				}
				if (Main.netMode != 1 && NPC.ai[0] == 30f) {
					int num285 = 10;
					int num286 = ModContent.ProjectileType<Projectiles.SprinklingBallLarge>();
					int num287 = Projectile.NewProjectile(NPC.GetSource_FromAI(), vector32.X, vector32.Y, num282, num283, num286, num285, 0f, Main.myPlayer);
					Main.projectile[num287].ai[0] = 2f;
					Main.projectile[num287].timeLeft = 300;
					Main.projectile[num287].friendly = false;
					Main.projectile[num287].frame = Index;
					NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, num287);
					NPC.netUpdate = true;
					SoundEngine.PlaySound(SoundID.Item5, NPC.position);
				}
				if (Main.netMode != 1 && NPC.ai[0] == 15f) {
					int num285 = 10;
					int num286 = ModContent.ProjectileType<Projectiles.SprinklingBall>();
					int num287 = Projectile.NewProjectile(NPC.GetSource_FromAI(), vector32.X, vector32.Y, num282, num283, num286, num285, 0f, Main.myPlayer);
					Main.projectile[num287].ai[0] = 2f;
					Main.projectile[num287].timeLeft = 300;
					Main.projectile[num287].friendly = false;
					Main.projectile[num287].frame = Index;
					NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, num287);
					NPC.netUpdate = true;
					SoundEngine.PlaySound(SoundID.Item5, NPC.position);
				}
			}
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

                new FlavorTextBestiaryInfoElement("Mods.TheConfectionRebirth.Bestiary.Sprinkler")
            });
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.damage = (int)(NPC.damage * 0.2f);
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                Vector2 spawnAt = NPC.Center + new Vector2(0f, NPC.height / 2f);
                int index = NPC.NewNPC(NPC.GetSource_FromAI(), (int)spawnAt.X, (int)spawnAt.Y, ModContent.NPCType<Sprinkling>());
                (Main.npc[index].ModNPC as Sprinkling).Index = Index;
                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendData(MessageID.SyncNPC, number: index);
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.InModBiome(ModContent.GetInstance<ConfectionBiome>()) && !spawnInfo.AnyInvasionActive() && !spawnInfo.Player.ZoneDirtLayerHeight && !spawnInfo.Player.ZoneRockLayerHeight)
            {
                return 1f;
            }
            return 0f;
        }

		public override bool PreAI()
        {
            if (Index == -1)
            {
                Index = VariationManager<Sprinkler>.GetRandomGroup().Index;

                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendData(MessageID.SyncNPC, number: NPC.whoAmI);
            }

            return true;
        }

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture;
            Rectangle frame = NPC.frame;
            Vector2 pos = NPC.Center - screenPos;
            pos.Y += NPC.gfxOffY + 4f;

            int index = Utils.Clamp(Index, 0, 4);
            if (index == 4)
                index = 0;

            int frameOff = (NPC.frame.Y != 0).ToInt() * 2;
            Texture2D front = Assets[index][1].Value;
            texture = Assets[index][0].Value;

            spriteBatch.Draw(texture, pos + new Vector2(0f, frameOff), new(0, 0, 42, 24), drawColor, NPC.rotation, frame.Size() * 0.5f, NPC.scale, 0, 0f);
            spriteBatch.Draw(front, pos, frame, drawColor, NPC.rotation, frame.Size() * 0.5f, NPC.scale, 0, 0f);
            return false;
        }

        public override void SendExtraAI(BinaryWriter writer) => writer.Write(Index);

        public override void ReceiveExtraAI(BinaryReader reader) => Index = reader.ReadSByte();
    }
}