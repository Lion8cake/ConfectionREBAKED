using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Biomes;

namespace TheConfectionRebirth.NPCs
{
    public class CreamsandWitchPhase1 : ModNPC
    {

        public override void SetStaticDefaults()
        {
            NPCID.Sets.NPCBestiaryDrawModifiers value = new(0)
            {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

        public override void SetDefaults()
        {
            NPC.width = 40;
            NPC.height = 40;
            NPC.damage = 85;
            NPC.defense = 15;
            NPC.lifeMax = 1200;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = 22;
            AIType = NPCID.FloatyGross;
			NPC.rarity = 2;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<SandConfectionSurfaceBiome>().Type };
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.spriteDirection = NPC.direction;
        }

        public override void AI()
        {
            NPC.ai[0] += 1f;
            if (Main.rand.NextBool(1000) && NPC.CountNPCS(ModContent.NPCType<CrookedCookie>()) < 25)
            {
                NPC.ai[0] = 0f;
                int i = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<CrookedCookie>(), 0, NPC.whoAmI);
                Main.npc[i].velocity.X = Main.rand.NextFloat(-0.4f, 0.4f);
                Main.npc[i].velocity.Y = Main.rand.NextFloat(-0.5f, -0.05f);
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    NetMessage.SendData(MessageID.SyncNPC, number: i);
            }
            NPC.ai[0] += 2f;
            if (Main.rand.NextBool(1000) && NPC.CountNPCS(ModContent.NPCType<MintJr>()) < 25)
            {
                NPC.ai[0] = 0f;
                int i = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<MintJr>(), 0, NPC.whoAmI);
                Main.npc[i].velocity.X = Main.rand.NextFloat(-0.4f, 0.4f);
                Main.npc[i].velocity.Y = Main.rand.NextFloat(-0.5f, -0.05f);
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    NetMessage.SendData(MessageID.SyncNPC, number: i);
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.InModBiome(ModContent.GetInstance<ConfectionBiome>()) && !spawnInfo.AnyInvasionActive() && Main.hardMode && spawnInfo.Player.ZoneDesert) {
                return 0.01f;
            }
            return 0f;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (Main.netMode == NetmodeID.Server)
            {
                return;
            }

            if (NPC.life <= 0)
            {
                Vector2 spawnAt = NPC.Center + new Vector2(0f, NPC.height / 2f);
                NPC.NewNPC(NPC.GetSource_FromAI(), (int)spawnAt.X, (int)spawnAt.Y, ModContent.NPCType<CreamsandWitchPhase2>());

                var entitySource = NPC.GetSource_Death();

                for (int i = 0; i < 1; i++)
                {
                    Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), Mod.Find<ModGore>("CreamsandWitchBroomGore1").Type);
                    Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), Mod.Find<ModGore>("CreamsandWitchBroomGore2").Type);
                }
            }
        }
    }
}
