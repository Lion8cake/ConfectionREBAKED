using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Dusts;
using TheConfectionRebirth.Items.Banners;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System;

namespace TheConfectionRebirth.NPCs.Critters
{
    internal class RoyalCherryBug : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Firefly];
            Main.npcCatchable[NPC.type] = true;
			NPCID.Sets.CountsAsCritter[Type] = true;
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, new(0)
            {
                Position = new(0, 8f),
                Velocity = 0.5f
            });
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.Firefly);
            NPC.catchItem = (short)ModContent.ItemType<RoyalCherryBugItem>();
            NPC.aiStyle = 64;
            NPC.friendly = true;
            AIType = NPCID.Firefly;
            AnimationType = NPCID.Firefly;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<ConfectionBiome>().Type };
			NPC.dontTakeDamageFromHostiles = true;
			NPC.lavaImmune = true;
			NPC.rarity = 4;
        }

		public override bool CanBeHitByNPC(NPC attacker) {
			return false;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,

                new FlavorTextBestiaryInfoElement("Mods.TheConfectionRebirth.Bestiary.RoyalCherryBug")
            });
        }

        public override bool? CanBeHitByItem(Player player, Item item)
        {
            return true;
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            return false;
        }

		public override void HitEffect(NPC.HitInfo hit) {
			if (NPC.life <= 0) {
				if (!NPC.AnyNPCs(NPCID.HallowBoss)) {
					int index = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)(NPC.Center.Y / 1.02), NPCID.HallowBoss);
					if (Main.netMode == NetmodeID.Server) {
						NetMessage.SendData(MessageID.SyncNPC, number: index);
					}
				}
				if (Main.netMode != NetmodeID.Server) {
					for (int i = 0; i < 10; i++) {
						Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<CritterBlood>());
					}
				}
			}
		}

		public override void AI() {
			Lighting.AddLight(NPC.position, new Vector3(1.77f, 1.12f, 0.71f));
			if (Main.dayTime) {
				NPC.alpha += 2;
			}
			else {
				NPC.alpha -= 2;
			}
			if (NPC.alpha >= 255) {
				NPC.active = false;
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneOverworldHeight && !Main.dayTime && spawnInfo.Player.InModBiome(ModContent.GetInstance<ConfectionBiome>()) && NPC.downedPlantBoss && Main.hardMode)
            {
                return 0.1f;
            }
            return 0f;
        }
    }

    internal class RoyalCherryBugItem : ModItem
    {
        public override void SetStaticDefaults() => Item.ResearchUnlockCount = 5;

        public override void SetDefaults()
        {
            Item.useStyle = 1;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.width = 12;
            Item.height = 12;
            Item.noUseGraphic = true;
            Item.makeNPC = (short)ModContent.NPCType<RoyalCherryBug>();
			Item.rare = ItemRarityID.Orange;
        }
    }
}
