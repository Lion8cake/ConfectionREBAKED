using Microsoft.Xna.Framework;
using System;
using System.Reflection;
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
    public class CreamSwollower : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 4;
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, new(0)
            {
                Position = new(35f, 10f),
                PortraitPositionXOverride = 0f
            });
        }

        public override void SetDefaults()
        {
			NPC.noGravity = true;
			NPC.width = 100;
			NPC.height = 24;
			NPC.aiStyle = 103;
			NPC.damage = 58;
			NPC.defense = 25;
			NPC.lifeMax = 465;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = 400f;
			NPC.knockBackResist = 0.7f;
			NPC.noTileCollide = true;
			NPC.behindTiles = true;
            AnimationType = NPCID.SandShark;
            Banner = Type;
            BannerItem = ModContent.ItemType<CreamSwollowerBanner>();
            SpawnModBiomes = new int[1] { ModContent.GetInstance<SandConfectionSurfaceBiome>().Type };
        }

		public override bool? CanFallThroughPlatforms() {
			return true;
		}

		public override Color? GetAlpha(Color drawColor) {
			float num = (float)(255 - NPC.alpha) / 255f;
			int num2 = (int)(drawColor.R * num);
			int num3 = (int)(drawColor.G * num);
			int num4 = (int)(drawColor.B * num);
			int num5 = drawColor.A - NPC.alpha;
			if (num2 + num3 + num4 > 10 && num2 + num3 + num4 >= 60) {
				num2 *= 2;
				num3 *= 2;
				num4 *= 2;
				if (num2 > 255) {
					num2 = 255;
				}
				if (num3 > 255) {
					num3 = 255;
				}
				if (num4 > 255) {
					num4 = 255;
				}
			}
			return new Color(num2, num3, num4, num5);
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Sandstorm,

                new FlavorTextBestiaryInfoElement("Mods.TheConfectionRebirth.Bestiary.CreamSwollower")
            });
        }

		public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.SharkFin, 8, 1, 1));
            npcLoot.Add(ItemDropRule.Food(ItemID.Nachos, 30, 1, 1));
            npcLoot.Add(ItemDropRule.Common(ItemID.LightShard, 25, 1, 1));
            npcLoot.Add(ItemDropRule.ByCondition(new Conditions.WindyEnoughForKiteDrops(), ItemID.KiteSandShark, 25, 1, 1, 1));
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.InModBiome(ModContent.GetInstance<ConfectionBiome>()) && !spawnInfo.AnyInvasionActive() && Main.hardMode && spawnInfo.Player.ZoneDesert && spawnInfo.Player.ZoneSandstorm) {
                return 0.5f;
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
                var entitySource = NPC.GetSource_Death();

                for (int i = 0; i < 1; i++)
                {
                    Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), Mod.Find<ModGore>("CreamSwollowerGore1").Type);
                    Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), Mod.Find<ModGore>("CreamSwollowerGore2").Type);
                    Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), Mod.Find<ModGore>("CreamSwollowerGore3").Type);
                    Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), Mod.Find<ModGore>("CreamSwollowerGore4").Type);
                }
            }
        }
    }
}
