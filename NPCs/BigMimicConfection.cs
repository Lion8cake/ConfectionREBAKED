using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Items;
using TheConfectionRebirth.Items.Weapons;

namespace TheConfectionRebirth.NPCs
{
    public class BigMimicConfection : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 14;
        }

        public override void SetDefaults()
        {
			NPC.width = 28;
			NPC.height = 44;
			NPC.aiStyle = 87;
			NPC.damage = 90;
			NPC.defense = 34;
			NPC.lifeMax = 3500;
			NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.value = 60f;
            NPC.knockBackResist = 0.5f;
			NPC.rarity = 5;
			AIType = NPCID.BigMimicHallow;
            AnimationType = NPCID.BigMimicHallow;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<ConfectionUndergroundBiome>().Type };
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

                new FlavorTextBestiaryInfoElement("Mods.TheConfectionRebirth.Bestiary.BigMimicConfection")
            });
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.InModBiome(ModContent.GetInstance<ConfectionBiome>()) && !spawnInfo.AnyInvasionActive() && Main.hardMode && (spawnInfo.Player.ZoneRockLayerHeight || spawnInfo.Player.ZoneUnderworldHeight))
            {
                return 0.01f;
            }
            return 0f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.OneFromOptionsNotScalingWithLuck(1, ModContent.ItemType<CookieCrumbler>(), ModContent.ItemType<SweetTooth>(), ModContent.ItemType<SweetHook>(), ModContent.ItemType<CreamSpray>()));
            npcLoot.Add(ItemDropRule.Common(ItemID.GreaterHealingPotion, 1, 5, 10));
            npcLoot.Add(ItemDropRule.Common(ItemID.GreaterManaPotion, 1, 5, 15));
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
                }
            }
        }
    }

	public class ConfectionMimicSpawning : ModPlayer {
		public int LastChest;

		public override void PreUpdateBuffs() {
			if (Main.netMode != NetmodeID.MultiplayerClient) {
				if (Player.chest == -1 && LastChest >= 0 && Main.chest[LastChest] != null) {
					int x2 = Main.chest[LastChest].x;
					int y2 = Main.chest[LastChest].y;
					ChestItemSummonCheck(x2, y2, Mod);
				}
				LastChest = Player.chest;
			}
		}

		public override void UpdateAutopause() {
			LastChest = Player.chest;
		}

		public static bool ChestItemSummonCheck(int x, int y, Mod mod) {
			if (Main.netMode == NetmodeID.MultiplayerClient || !Main.hardMode) {
				return false;
			}
			int num = Chest.FindChest(x, y);
			if (num < 0) {
				return false;
			}
			int numberKeyofDelight = 0;
			int numberOtherItems = 0;
			ushort tileType = Main.tile[Main.chest[num].x, Main.chest[num].y].TileType;
			int tileStyle = (int)(Main.tile[Main.chest[num].x, Main.chest[num].y].TileFrameX / 36);
			if (TileID.Sets.BasicChest[tileType] && (tileStyle < 5 || tileStyle > 6)) {
				for (int i = 0; i < 40; i++) {
					if (Main.chest[num].item[i] != null && Main.chest[num].item[i].type > ItemID.None) {
						if (Main.chest[num].item[i].type == ModContent.ItemType<Items.KeyofDelight>()) {
							numberKeyofDelight += Main.chest[num].item[i].stack;
						}
						else {
							numberOtherItems++;
						}
					}
				}
			}
			if (numberOtherItems == 0 && numberKeyofDelight == 1) {
				if (TileID.Sets.BasicChest[Main.tile[x, y].TileType]) {
					if (Main.tile[x, y].TileFrameX % 36 != 0) {
						x--;
					}
					if (Main.tile[x, y].TileFrameY % 36 != 0) {
						y--;
					}
					int number = Chest.FindChest(x, y);
					for (int j = x; j <= x + 1; j++) {
						for (int k = y; k <= y + 1; k++) {
							if (TileID.Sets.BasicChest[Main.tile[j, k].TileType]) {
								Tile tile = Main.tile[j, k];
								tile.HasTile = false;
							}
						}
					}
					for (int l = 0; l < 40; l++) {
						Main.chest[num].item[l] = new Item();
					}
					Chest.DestroyChest(x, y);
					NetMessage.SendData(MessageID.ChestUpdates, -1, -1, null, 1, (float)x, (float)y, 0f, number, 0, 0);
					NetMessage.SendTileSquare(-1, x, y, 3);
				}
				int npcToSpawn = ModContent.NPCType<BigMimicConfection>();
				int npcIndex = NPC.NewNPC(NPC.GetSource_NaturalSpawn(), x * 16 + 16, y * 16 + 32, npcToSpawn, 0, 0f, 0f, 0f, 0f, 255);
				Main.npc[npcIndex].whoAmI = npcIndex;
				NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, npcIndex, 0f, 0f, 0f, 0, 0, 0);
				Main.npc[npcIndex].BigMimicSpawnSmoke();
			}
			return false;
		}
	}
}