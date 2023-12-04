using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Items.Banners;

namespace TheConfectionRebirth.NPCs
{

    public class Hunger : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 6;
        }

        public override void SetDefaults()
        {
            NPC.width = 20;
            NPC.height = 20;
            NPC.damage = 80;
            NPC.defense = 22;
            NPC.lifeMax = 500;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.value = 60f;
            NPC.knockBackResist = 0.5f;
			NPC.aiStyle = -1;
            AnimationType = NPCID.PresentMimic;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<HungerBanner>();
            SpawnModBiomes = new int[1] { ModContent.GetInstance<SandConfectionSurfaceBiome>().Type };
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

                new FlavorTextBestiaryInfoElement("Mods.TheConfectionRebirth.Bestiary.Hunger")
            });
        }

		public override void AI() {
			if (NPC.ai[0] == 0f) {
				NPC.TargetClosest();
				if (Main.netMode == 1) {
					return;
				}
				if (NPC.velocity.X != 0f || NPC.velocity.Y < 0f || (double)NPC.velocity.Y > 0.3) {
					NPC.ai[0] = 1f;
					NPC.netUpdate = true;
					return;
				}
				Rectangle rectangle3 = new((int)Main.player[NPC.target].position.X, (int)Main.player[NPC.target].position.Y, Main.player[NPC.target].width, Main.player[NPC.target].height);
				Rectangle val38 = new Rectangle((int)NPC.position.X - 100, (int)NPC.position.Y - 100, NPC.width + 200, NPC.height + 200);
				if (val38.Intersects(rectangle3) || NPC.life < NPC.lifeMax) {
					NPC.ai[0] = 1f;
					NPC.netUpdate = true;
				}
			}
			else if (NPC.velocity.Y == 0f) {
				NPC.ai[2] += 1f;
				int num883 = 20;
				if (NPC.ai[1] == 0f) {
					num883 = 12;
				}
				if (NPC.ai[2] < (float)num883) {
					NPC.velocity.X *= 0.9f;
					return;
				}
				NPC.ai[2] = 0f;
				NPC.TargetClosest();
				if (NPC.direction == 0) {
					NPC.direction = -1;
				}
				NPC.spriteDirection = NPC.direction;
				NPC.ai[1] += 1f;
				if (NPC.ai[1] == 2f) {
					NPC.velocity.X = (float)NPC.direction * 5f;
					NPC.velocity.Y = -12f;
					NPC.ai[1] = 0f;
				}
				else {
					NPC.velocity.X = (float)NPC.direction * 6f;
					NPC.velocity.Y = -6f;
				}
				NPC.netUpdate = true;
			}
			else if (NPC.direction == 1 && NPC.velocity.X < 1f) {
				NPC.velocity.X += 0.1f;
			}
			else if (NPC.direction == -1 && NPC.velocity.X > -1f) {
				NPC.velocity.X -= 0.1f;
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

                for (int i = 0; i < 3; i++)
                {
                    Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), 13);
                    Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), 12);
                    Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), 11);
                    Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), Mod.Find<ModGore>("HungerGore").Type);
                    Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), Mod.Find<ModGore>("HungerGore").Type);
                }
            }
        }
    }
}