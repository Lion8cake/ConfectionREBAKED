using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
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
        private sbyte Index;

		public override void Load()
        {
            string[] a = { "Green", "Red", "Blue", "Yellow", "Amber", "Pink", "Onyx", "Diamond", "Purple", "Skeleton", "Bunny", "Headless" };
            Func<bool>[] b = new Func<bool>[a.Length];
            b[9] = b[10] = b[11] = () => Main.halloween;

            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] == "Green")
                {
                    VariationManager<SweetGummy>.AddGroup(a[i], ModContent.Request<Texture2D>(Texture));
                    continue;
				}

                VariationManager<SweetGummy>.AddGroup(a[i], ModContent.Request<Texture2D>(Texture + '_' + a[i]), b[i]);
            }
        }

		public override void Unload() => VariationManager<SweetGummy>.Clear();

		public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 16;
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, new(0)
            {
                Velocity = 0.5f
            });
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
            Index = -1;
        }

        public override bool PreAI()
        {
            if (Index == -1)
            {
                Index = VariationManager<SweetGummy>.GetRandomGroup().Index;

                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendData(MessageID.SyncNPC, number: NPC.whoAmI);
            }

            return true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.IsABestiaryIconDummy)
                return true;

            DS.DrawNPC(NPC, VariationManager<SweetGummy>.GetByIndex(Index).Get().Value, spriteBatch, screenPos, drawColor);
            return false;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

                new FlavorTextBestiaryInfoElement("Mods.TheConfectionRebirth.Bestiary.SweetGummy")
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.LightShard, 10));
            npcLoot.Add(ItemDropRule.NormalvsExpert(ItemID.TrifoldMap, 100, 50));
            npcLoot.Add(ItemDropRule.OneFromOptionsNotScalingWithLuck(95, ModContent.ItemType<GummyMask>(), ModContent.ItemType<GummyShirt>(), ModContent.ItemType<GummyPants>()));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.InModBiome(ModContent.GetInstance<SandConfectionSurfaceBiome>()) && !spawnInfo.AnyInvasionActive())
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

        public override void SendExtraAI(BinaryWriter writer) => writer.Write(Index);

        public override void ReceiveExtraAI(BinaryReader reader) => Index = reader.ReadSByte();
    }
}
