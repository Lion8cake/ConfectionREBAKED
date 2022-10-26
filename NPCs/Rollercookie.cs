using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
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
            None = 0,
            Normal = 1,
            Snow = 2,
            Birthday = 3
        }

        private Variation variation;

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

        private static Asset<Texture2D> BirthdayCookieTexture;
        private static Asset<Texture2D> SantaCookieTexture;

        public override void Load()
		{
            BirthdayCookieTexture = ModContent.Request<Texture2D>(Texture + "_Birthday");
            SantaCookieTexture = ModContent.Request<Texture2D>(Texture + "_Santa");
        }

		public override void Unload() => BirthdayCookieTexture = SantaCookieTexture = null;

		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Roller Cookie");
        }

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

                new FlavorTextBestiaryInfoElement("A cookie brought to life in the magical biome that was created from the spirits of light and night")
            });
        }

		public override bool PreAI()
		{
            if (variation == Variation.None)
			{
                variation = Variation.Normal;
                if (Main.SceneMetrics.EnoughTilesForSnow)
                    variation = Variation.Snow;
                if (Main.rand.NextFloat() < 0.002f || TheConfectionRebirth.OurFavoriteDay && Main.rand.NextFloat() < 0.05f)
                    variation = Variation.Birthday;

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

            LeadingConditionRule rule = new(new CookieVariationDrop(Variation.Normal));
            rule.OnSuccess(ItemDropRule.OneFromOptionsNotScalingWithLuck(20, ModContent.ItemType<CookieMask>(), ModContent.ItemType<CookieShirt>(), ModContent.ItemType<CookiePants>()));
            rule.OnFailedConditions(ItemDropRule.OneFromOptionsNotScalingWithLuck(1, ModContent.ItemType<TopCake>(), ModContent.ItemType<BirthdaySuit>(), ModContent.ItemType<RightTrousers>()));
            npcLoot.Add(rule);
        }

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.IsABestiaryIconDummy)
                return true;

            Texture2D texture = TextureAssets.Npc[Type].Value;
            if (variation is Variation.Snow)
                texture = SantaCookieTexture.Value;
            else if (variation is Variation.Birthday)
                texture = BirthdayCookieTexture.Value;

            DS.DrawNPC(NPC, texture, spriteBatch, screenPos, drawColor);
            return false;
		}

		public override void SendExtraAI(BinaryWriter writer) => writer.Write((byte)variation);

		public override void ReceiveExtraAI(BinaryReader reader) => variation = (Variation)reader.ReadByte();

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneOverworldHeight && spawnInfo.Player.InModBiome(ModContent.GetInstance<ConfectionBiomeSurface>()) && !spawnInfo.Player.ZoneOldOneArmy && !spawnInfo.Player.ZoneTowerNebula && !spawnInfo.Player.ZoneTowerSolar && !spawnInfo.Player.ZoneTowerStardust && !spawnInfo.Player.ZoneTowerVortex && !spawnInfo.Invasion)
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
