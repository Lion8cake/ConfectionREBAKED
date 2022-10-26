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
using TheConfectionRebirth.Items;
using TheConfectionRebirth.Items.Armor;
using TheConfectionRebirth.Items.Banners;

namespace TheConfectionRebirth.NPCs
{
    public class SweetGummy : ModNPC
    {
        private enum Variation : byte
		{
            None,
            Green,
            Red,
            Blue,
            Yellow
		}

        private Variation variation;

        private static Asset<Texture2D>[] VariationTextures = new Asset<Texture2D>[3];

		public override void Load()
        {
            VariationTextures[0] = ModContent.Request<Texture2D>(Texture + "_Red");
            VariationTextures[1] = ModContent.Request<Texture2D>(Texture + "_Blue");
            VariationTextures[2] = ModContent.Request<Texture2D>(Texture + "_Yellow");
        }

		public override void Unload() => VariationTextures = null;

		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sweet Gummy");
            Main.npcFrameCount[NPC.type] = 16;
        }

        public override void SetDefaults()
        {
            NPC.width = 18;
            NPC.height = 40;
            NPC.damage = 60;
            NPC.defense = 26;
            NPC.lifeMax = 180;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath6;
            //Sound 3 and 4 
            NPC.value = 60f;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = 3;
            AIType = NPCID.Mummy;
            AnimationType = NPCID.Mummy;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<SweetGummyBanner>();
            SpawnModBiomes = new int[1] { ModContent.GetInstance<SandConfectionSurfaceBiome>().Type };
        }

        public override bool PreAI()
        {
            if (variation == Variation.None)
            {
                variation = (Variation)Main.rand.Next(1, 5);

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
            if (variation is >= Variation.Red)
                texture = VariationTextures[(byte)variation - 2].Value;

            DS.DrawNPC(NPC, texture, spriteBatch, screenPos, drawColor);
            return false;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

                new FlavorTextBestiaryInfoElement("A mummy infected by the confection turning into a gummy bear. How sweet?")
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CreamPuff>(), 10));
            npcLoot.Add(ItemDropRule.NormalvsExpert(ItemID.TrifoldMap, 100, 50));
            npcLoot.Add(ItemDropRule.OneFromOptionsNotScalingWithLuck(95, ModContent.ItemType<GummyMask>(), ModContent.ItemType<GummyShirt>(), ModContent.ItemType<GummyPants>()));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneOverworldHeight && spawnInfo.Player.InModBiome(ModContent.GetInstance<SandConfectionSurfaceBiome>()) && !spawnInfo.Player.ZoneOldOneArmy && !spawnInfo.Player.ZoneTowerNebula && !spawnInfo.Player.ZoneTowerSolar && !spawnInfo.Player.ZoneTowerStardust && !spawnInfo.Player.ZoneTowerVortex && !spawnInfo.Invasion)
            {
                return 0.31f;
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
                var entitySource = NPC.GetSource_Death();

                for (int i = 0; i < 3; i++)
                {
                    Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), 13);
                    Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), 12);
                    Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), 11);
                }
            }
        }

        public override void SendExtraAI(BinaryWriter writer) => writer.Write((byte)variation);

        public override void ReceiveExtraAI(BinaryReader reader) => variation = (Variation)reader.ReadByte();
    }
}
