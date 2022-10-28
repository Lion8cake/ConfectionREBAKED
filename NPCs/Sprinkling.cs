using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Items;
using TheConfectionRebirth.Items.Banners;

namespace TheConfectionRebirth.NPCs
{
    public class Sprinkling : ModNPC
    {
        internal sbyte Index;

        public override void Load()
        {
            VariationManager<Sprinkling>.AddGroup("Normal", ModContent.Request<Texture2D>(Texture));
            /*VariationManager<Sprinkling>.AddGroup("Corn", ModContent.Request<Texture2D>(Texture + "_Corn"), () => Main.halloween);
            VariationManager<Sprinkling>.AddGroup("Eye", ModContent.Request<Texture2D>(Texture + "_Eye"), () => Main.halloween);
            VariationManager<Sprinkling>.AddGroup("Gift", ModContent.Request<Texture2D>(Texture + "_Gift"), () => Main.xMas);*/
        }

        public override void Unload() => VariationManager<Sprinkler>.Clear();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sprinkling");
            Main.npcFrameCount[NPC.type] = 10;
        }

        public override void SetDefaults()
        {
            NPC.width = 36;
            NPC.height = 36;
            NPC.damage = 75;
            NPC.defense = 20;
            NPC.lifeMax = 140;
            NPC.HitSound = SoundID.NPCHit5;
            NPC.DeathSound = SoundID.NPCDeath7;
            NPC.value = 60f;
            NPC.noGravity = true;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = 22;
            AIType = NPCID.Pixie;
            AnimationType = NPCID.Pixie;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<SprinklingBanner>();
            SpawnModBiomes = new int[1] { ModContent.GetInstance<ConfectionBiomeSurface>().Type };
            Index = -1;
        }

        public override bool PreAI()
        {
            if (Index == -1)
            {
                Index = (sbyte)VariationManager<Sprinkling>.GetRandomGroup().Index;

                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendData(MessageID.SyncNPC, number: NPC.whoAmI);
            }

            return true;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

                new FlavorTextBestiaryInfoElement("Sprinkles cursed with the soul power of flight will still try and kill anyone from taking over the confection.")
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Sprinkles>(), 1, 1, 3));
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

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.IsABestiaryIconDummy)
                return true;

            DS.DrawNPC(NPC, VariationManager<Sprinkling>.GetByIndex(Index).Get().Value, spriteBatch, screenPos, drawColor);
            return false;
        }

        public override void SendExtraAI(BinaryWriter writer) => writer.Write(Index);

        public override void ReceiveExtraAI(BinaryReader reader) => Index = reader.ReadSByte();
    }
}