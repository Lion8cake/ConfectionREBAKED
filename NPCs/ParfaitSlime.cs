using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.IO;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Dusts;
using TheConfectionRebirth.Items;
using TheConfectionRebirth.Items.Banners;

namespace TheConfectionRebirth.NPCs
{

    public class ParfaitSlime : ModNPC
    {
        private enum Variation : byte
        {
            None = 0,
            Normal = 1,
            Snow = 2
        }

        private Variation variation;

        private static Asset<Texture2D> SnowTexture;

		public override void Load() => SnowTexture = ModContent.Request<Texture2D>(Texture + "_Snow");

		public override void Unload() => SnowTexture = null;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 2;
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, new(0)
            {
                Position = new(2f, 0f)
            });
        }

        public override void SetDefaults()
        {
            NPC.width = 32;
            NPC.height = 32;
            NPC.damage = 70;
            NPC.defense = 16;
            NPC.lifeMax = 200;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 60f;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = 1;
            AIType = NPCID.Crimslime;
            AnimationType = NPCID.Crimslime;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<ParfaitSlimeBanner>();
            SpawnModBiomes = new int[1] { ModContent.GetInstance<ConfectionUndergroundBiome>().Type };
            variation = Variation.None;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

                new FlavorTextBestiaryInfoElement("Mods.TheConfectionRebirth.Bestiary.ParfaitSlime")
            });
        }

		public override bool PreAI()
        {
            if (variation == Variation.None)
            {
                variation = Variation.Normal;
                if (Main.SceneMetrics.EnoughTilesForSnow)
                    variation = Variation.Snow;

                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendData(MessageID.SyncNPC, number: NPC.whoAmI);
            }

            return true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.IsABestiaryIconDummy)
                return true;

            Texture2D texture = TextureAssets.Npc[Type].Value;
            Rectangle f = NPC.frame;
            if (variation is Variation.Snow)
            {
                texture = SnowTexture.Value;
                f.Width = 44;
                f.Height = 34;
                f.Y = (f.Y / 48) * 34;
            }

            spriteBatch.Draw(texture, NPC.Center - screenPos - new Vector2(0f, 4f), f, drawColor, NPC.rotation, f.Size() * 0.5f, NPC.scale, 0, 0f);
            return false;
        }

		public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.Gel, maximumDropped: 3));
            npcLoot.Add(ItemDropRule.Food(ModContent.ItemType<Brownie>(), 150));
            npcLoot.Add(ItemDropRule.NormalvsExpert(ItemID.SlimeStaff, 10000, 7000));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.InModBiome(ModContent.GetInstance<ConfectionUndergroundBiome>()) && !spawnInfo.Player.ZoneOldOneArmy && !spawnInfo.Player.ZoneTowerNebula && !spawnInfo.Player.ZoneTowerSolar && !spawnInfo.Player.ZoneTowerStardust && !spawnInfo.Player.ZoneTowerVortex && !spawnInfo.Invasion)
            {
                return 1.0f;
            }
            return 0f;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (Main.netMode == NetmodeID.Server)
            {
                return;
            }

            if (NPC.life <= 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<CreamDust>());
                }
            }
        }

        public override void SendExtraAI(BinaryWriter writer) => writer.Write((byte)variation);

        public override void ReceiveExtraAI(BinaryReader reader) => variation = (Variation)reader.ReadByte();
    }
}