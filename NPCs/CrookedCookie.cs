using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Items.Banners;

namespace TheConfectionRebirth.NPCs
{
    public class CrookedCookie : ModNPC
    {
		public override void SetStaticDefaults()
        {
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, new(0)
            {
                Position = new(0, -5f),
                PortraitPositionYOverride = -20f
            });
        }

		public override void SetDefaults()
        {
            NPC.width = 18;
            NPC.height = 18;
            NPC.damage = 60;
            NPC.defense = 24;
            NPC.lifeMax = 150;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.knockBackResist = 0.5f;
			NPC.aiStyle = -1;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<CrookedCookieBanner>();
            SpawnModBiomes = new int[1] { ModContent.GetInstance<SandConfectionSurfaceBiome>().Type };
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

                new FlavorTextBestiaryInfoElement("Mods.TheConfectionRebirth.Bestiary.CrookedCookie")
            });
        }

		

        public override void AI()
        {
			float num142 = 12f;
			NPC.TargetClosest();
			Vector2 vector91 = Main.player[NPC.target].Center - NPC.Center;
			vector91.Normalize();
			vector91 *= num142;
			int num144 = 200;
			NPC.velocity.X = (NPC.velocity.X * (float)(num144 - 1) + vector91.X) / (float)num144;
			if (NPC.velocity.Length() > 16f) {
				NPC.velocity.Normalize();
				NPC.velocity *= 16f;
			}
			if (NPC.localAI[0] > 0f) {
				NPC.localAI[0] -= 1f;
			}
			if (NPC.localAI[0] == 0f) {
				NPC.localAI[0] = 60f;
				if (NPC.collideY == true) {
					NPC.velocity.Y -= 8f;
				}
			}
			NPC.rotation += NPC.velocity.X * 0.05f;
			if (NPC.velocity.Y > 16f) {
				NPC.velocity.Y = 16f;
			}
			if (NPC.velocity.X < 0.1f && NPC.velocity.X > -0.1f && NPC.velocity.Y > -0.1f) {
				NPC.localAI[0] = 0f;
			}
		}

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (Main.netMode == NetmodeID.Server)
            {
                return;
            }

            if (NPC.life <= 0)
            {
                var entitySource = NPC.GetSource_Death();

                for (int i = 0; i < 1; i++)
                {
                    Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), Mod.Find<ModGore>("CrookedCookieGore1").Type);
                    Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), Mod.Find<ModGore>("CrookedCookieGore2").Type);
                }
            }
        }
    }
}
