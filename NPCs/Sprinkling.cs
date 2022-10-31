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
using ReLogic.Content;
using Terraria.GameContent;

namespace TheConfectionRebirth.NPCs
{
    public class Sprinkling : ModNPC
    {
        internal sbyte Index;

        public static Asset<Texture2D>[][] Assets;

        public override void Load()
        {
            Asset<Texture2D> wtf = ModContent.Request<Texture2D>(Texture);
            VariationManager<Sprinkling>.AddGroup("Normal", wtf);
            VariationManager<Sprinkling>.AddGroup("Corn", wtf, () => false && Main.halloween);
            VariationManager<Sprinkling>.AddGroup("Eye", wtf, () => Main.halloween);
            VariationManager<Sprinkling>.AddGroup("Gift", wtf, () => Main.xMas);

            if (Main.dedServ)
                return;

            Assets = new Asset<Texture2D>[VariationManager<Sprinkling>.Count][];
            for (int i = 0; i < Assets.GetLength(0); i++)
            {
                Assets[i] = new Asset<Texture2D>[3];
                for (int j = 0; j < 3; j++)
                {
                    Assets[i][j] = ModContent.Request<Texture2D>($"TheConfectionRebirth/NPCs/Sprinkler/Sprinkling_{i}_{j}");
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
            Main.npcFrameCount[NPC.type] = 10;
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, new(0)
            {
                Position = new(2, 6f),
                PortraitPositionXOverride = 2f,
                PortraitPositionYOverride = 4f
            });
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

                new FlavorTextBestiaryInfoElement("Mods.TheConfectionRebirth.Bestiary.Sprinkling")
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Sprinkles>(), 1, 1, 3));
            npcLoot.Add(ItemDropRule.Common(ItemID.FastClock, 100));
            npcLoot.Add(ItemDropRule.Common(ItemID.Megaphone, 100));
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
            Texture2D texture = TextureAssets.Npc[Type].Value;
            Rectangle frame = NPC.frame;
            Vector2 pos = NPC.Center - screenPos;
            pos.Y += NPC.gfxOffY - 4f;
            if (NPC.IsABestiaryIconDummy)
            {
				spriteBatch.Draw(texture, pos, frame, drawColor, NPC.rotation, NPC.frame.Size() * 0.5f, NPC.scale, DS.FlipTex(NPC.direction), 0f);
                return true;
            }

            int index = Utils.Clamp(Index, 0, 4);
            if (index == 4)
                index = 0;

            Texture2D back = Assets[index][2].Value;
            Texture2D front = Assets[index][1].Value;
            texture = Assets[index][0].Value;
            frame.Y %= front.Height;

            spriteBatch.Draw(back, pos, frame, drawColor, NPC.rotation, frame.Size() * 0.5f, NPC.scale, DS.FlipTex(NPC.direction), 0f);
            spriteBatch.Draw(texture, pos, new(0, 0, 58, 50), drawColor, NPC.rotation, new(29, 25), NPC.scale, DS.FlipTex(NPC.direction), 0f);
            spriteBatch.Draw(front, pos, frame, drawColor, NPC.rotation, frame.Size() * 0.5f, NPC.scale, DS.FlipTex(NPC.direction), 0f);
            return false;
        }

        public override void SendExtraAI(BinaryWriter writer) => writer.Write(Index);

        public override void ReceiveExtraAI(BinaryReader reader) => Index = reader.ReadSByte();
    }
}