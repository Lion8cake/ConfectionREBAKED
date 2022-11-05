using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.IO;
using Terraria;
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
            AIType = 0;
            AnimationType = NPCID.BlueSlime;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<SprinklingBanner>();
            SpawnModBiomes = new int[1] { ModContent.GetInstance<ConfectionBiomeSurface>().Type };
            Index = -1;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

                new FlavorTextBestiaryInfoElement("Mods.TheConfectionRebirth.Bestiary.Sprinkler")
            });
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.damage = (int)(NPC.damage * 0.2f);
        }

        public override void HitEffect(int hitDirection, double damage)
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
            if (spawnInfo.Player.InModBiome(ModContent.GetInstance<ConfectionBiomeSurface>()) && !spawnInfo.Player.ZoneOldOneArmy && !spawnInfo.Player.ZoneTowerNebula && !spawnInfo.Player.ZoneTowerSolar && !spawnInfo.Player.ZoneTowerStardust && !spawnInfo.Player.ZoneTowerVortex && !spawnInfo.Invasion)
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

		public override void AI()
        {
            Target();
            if (player != null && Collision.CanHit(NPC, player) && --NPC.ai[1] <= 0f)
            {
                Shoot();
            }
        }

        private void Target()
        {
            NPC.TargetClosest(false);

            player = NPC.target == -1 ? null : Main.player[NPC.target];
        }

        private void Shoot()
        {
            int type = Mod.Find<ModProjectile>("SprinklingBall").Type;
            Vector2 velocity = player.Center - NPC.Center;
            float magnitude = MathF.Sqrt(velocity.X * velocity.X + velocity.Y * velocity.Y);
            if (magnitude > 0f)
            {
                velocity *= 5f / magnitude;
            }

            int ind = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity, type, NPC.damage, 2f);
            Main.projectile[ind].frame = Index;

            NPC.ai[1] = 200f;
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