using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.IO;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Items;
using TheConfectionRebirth.Items.Armor;
using TheConfectionRebirth.Items.Banners;
using TheConfectionRebirth.Pets.RollerCookiePet;

namespace TheConfectionRebirth.NPCs
{
    public class Rollercookie : ModNPC
    {
        private enum Variation : byte
        {
            None,
            Normal,
            Halloween,
            Christmas,
            Easter,
            Birthday,
            FakeBirthday,
            Fox,
            Blueberry,
        }

        private Variation variation;
        private static int[] fax;

		private class CookieVariationDrop : IItemDropRuleCondition
		{
			private readonly Variation variation;
			private readonly string d;

			public CookieVariationDrop(Variation variation, string desc = null)
			{
				this.variation = variation;
				d = desc;
			}

			public bool CanDrop(DropAttemptInfo info) => !info.IsInSimulation && info.npc is not null && info.npc.ModNPC is Rollercookie c && c.variation == variation;

			public bool CanShowItemDropInUI() => true;

			public string GetConditionDescription() => d;
		}

		public override void Load()
		{
            fax = new int[Main.maxNPCs];
			Array.Fill(fax, -1);
		}

		public override void Unload() => fax = null;

		public override void SetDefaults()
        {
            NPC.width = 44;
            NPC.height = 44;
            NPC.damage = 58;
            NPC.defense = 17;
            NPC.lifeMax = 360;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.value = 60f;
            // npc.noGravity = false;
            // npc.noTileCollide = false;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = 26;
            AIType = NPCID.Unicorn;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<RollerCookieBanner>();
            SpawnModBiomes = new int[1] { ModContent.GetInstance<ConfectionBiomeSurface>().Type };
            variation = Variation.None;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

                new FlavorTextBestiaryInfoElement("Mods.TheConfectionRebirth.Bestiary.Rollercookie")
            });
        }

		public override bool PreAI()
		{
            if (variation == Variation.None)
			{
                variation = Variation.Normal;
                if (Main.halloween)
                    variation = Variation.Halloween;
                if (Main.xMas)
                    variation = Variation.Christmas;
                if (ConfectionWorld.IsEaster)
                    variation = Variation.Easter;
                if (Main.rand.NextFloat() < 0.002f || TheConfectionRebirth.OurFavoriteDay && Main.rand.NextFloat() < 0.075f)
                    variation = Variation.Birthday;
                if (Main.rand.NextFloat() < 0.001f || TheConfectionRebirth.OurFavoriteDay && Main.rand.NextFloat() < 0.0375f)
                    variation = Variation.Blueberry;
                if (DateTime.Now.Day.Equals(11) && DateTime.Now.Month.Equals(12))
                    variation = Main.rand.NextBool() && variation != Variation.Birthday ? Variation.FakeBirthday : Main.rand.NextBool() ? Variation.Blueberry : Variation.Fox;

                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendData(MessageID.SyncNPC, number: NPC.whoAmI);
			}

			return true;
		}

		public override void AI()
        {
            NPC.rotation += NPC.velocity.X * 0.05f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CookieDough>(), maximumDropped: 2));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ChocolateChunk>(), 100));

            LeadingConditionRule rule = new(new CookieVariationDrop(Variation.Birthday));
            rule.OnFailedConditions(ItemDropRule.OneFromOptionsNotScalingWithLuck(20, ModContent.ItemType<CookieMask>(), ModContent.ItemType<CookieShirt>(), ModContent.ItemType<CookiePants>()));
            rule.OnSuccess(ItemDropRule.OneFromOptionsNotScalingWithLuck(2, ModContent.ItemType<TopCake>(), ModContent.ItemType<BirthdaySuit>(), ModContent.ItemType<RightTrousers>()));
            npcLoot.Add(rule);
        }

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.IsABestiaryIconDummy)
            {
                Vector2 pos = NPC.Center - screenPos;
                Rectangle frame = new(0, 0, 66, 64);
                pos.Y += NPC.gfxOffY - 6f;
                spriteBatch.Draw(TextureAssets.Npc[Type].Value, pos, frame, drawColor, NPC.rotation, frame.Size() * 0.5f, NPC.scale, DS.FlipTex(NPC.direction), 0f);
                return true;
			}

            DS.DrawNPC(NPC, TextureAssets.Npc[Type].Value, spriteBatch, screenPos, drawColor);
            return false;
		}

		public override void FindFrame(int frameHeight)
        {
            if (fax[NPC.whoAmI] == -1)
            {
                int x = 0;
                if (variation == Variation.Normal)
                    x = Main.rand.Next(9) * 66;
                else if (variation == Variation.Halloween)
                    x = 264 + Main.rand.Next(3) * 66;
                else if (variation == Variation.Christmas)
                    x = Main.rand.Next(3) * 66;
                else if (variation == Variation.Birthday || variation == Variation.FakeBirthday)
                    x = 198;
                else if (variation == Variation.Easter)
                    x = 132;
                else if (variation == Variation.Blueberry)
                    x = 528;
                else if (variation == Variation.Fox)
                    x = 462;
                fax[NPC.whoAmI] = x;
            }

            NPC.frame = new(fax[NPC.whoAmI], variation == Variation.Normal ? 0 : 64, 66, 64);
        }

		public override void SendExtraAI(BinaryWriter writer) => writer.Write((byte)variation);

		public override void ReceiveExtraAI(BinaryReader reader) => variation = (Variation)reader.ReadByte();

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.InModBiome(ModContent.GetInstance<ConfectionBiomeSurface>()) && !spawnInfo.AnyInvasionActive())
            {
                return 0.1f;
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

                if (variation == Variation.Birthday)
                {
                    Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), Mod.Find<ModGore>("BirthdayCookieGore").Type);
                    Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), Mod.Find<ModGore>("BirthdayCookieGore").Type);
                }
                else
                {
                    Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), Mod.Find<ModGore>("RollercookieGore1").Type);
                    Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), Mod.Find<ModGore>("RollercookieGore2").Type);
                }

                fax[NPC.whoAmI] = -1;
            }
        }

		public override void ModifyTypeName(ref string typeName)
		{
			if (variation == Variation.Birthday)
			{
                typeName = Language.GetTextValue("Mods.TheConfectionRebirth.NPCName.BirthdayCookie");
			}
		}
	}
}
