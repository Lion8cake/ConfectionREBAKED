using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Dusts;
using TheConfectionRebirth.Items;
using TheConfectionRebirth.Items.Armor.CookieOutfit;
using TheConfectionRebirth.Items.Banners;

namespace TheConfectionRebirth.NPCs
{
    public class Rollercookie : ModNPC
    {
		public override void SetStaticDefaults()
		{
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, new NPCID.Sets.NPCBestiaryDrawModifiers
            {
                Position = new(0, -2f),
                PortraitPositionXOverride = 0f,
                PortraitPositionYOverride = -6f,
            });
		}

		public override void SetDefaults()
        {
			NPC.aiStyle = 26;
			NPC.knockBackResist = 0.3f;
			NPC.value = 1000f;
			NPC.damage = 58;
			NPC.defense = 30;
			NPC.width = 44;
            NPC.height = 44;
            NPC.lifeMax = 360;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath6;
            AIType = NPCID.Unicorn;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<RollerCookieBanner>();
            SpawnModBiomes = new int[1] { ModContent.GetInstance<ConfectionBiome>().Type };
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                
                new FlavorTextBestiaryInfoElement("Mods.TheConfectionRebirth.Bestiary.Rollercookie")
            });
        }

		public override void OnSpawn(IEntitySource source)
		{
			if (Main.rand.NextBool(2))
            {
                if (TheConfectionRebirth.isConfectionerBirthday || Main.rand.NextBool(100))
                {
					NPC.SetDefaults(ModContent.NPCType<BirthdayCookie>());
				}
                else if (TheConfectionRebirth.easter)
                {
                    NPC.localAI[1] = 10;
                }
                else if (Main.halloween)
                {
                    NPC.localAI[1] = 20 + Main.rand.Next(0, 3);
                }
                else if (Main.xMas)
				{
					NPC.localAI[1] = 30 + Main.rand.Next(0, 2);
				}
			}
            else
            {
                NPC.localAI[1] = Main.rand.Next(0, 9);
            }
		}

		public override void AI()
        {
            NPC.rotation += NPC.velocity.X * 0.05f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CookieDough>(), maximumDropped: 2));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ChocolateChunk>(), 100));
            npcLoot.Add(ItemDropRule.OneFromOptionsNotScalingWithLuck(20, ModContent.ItemType<CookieMask>(), ModContent.ItemType<CookieShirt>(), ModContent.ItemType<CookiePants>()));
        }

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
           
			Texture2D texture = TextureAssets.Npc[Type].Value;
			SpriteEffects spriteEffects = (SpriteEffects)0;
			if (NPC.spriteDirection == 1)
			{
				spriteEffects = (SpriteEffects)1;
			}
			Vector2 pos = NPC.Center - screenPos;
            Rectangle frame = NPC.frame;
			Vector2 orig = frame.Size() * new Vector2(0.5f, 0.5f);
			Color color = drawColor;
			Main.spriteBatch.Draw(texture, pos, (Rectangle?)frame, NPC.GetAlpha(color), NPC.rotation, orig, NPC.scale, spriteEffects, 0f);
			return false;
		}

		public override void FindFrame(int frameHeight)
        {
            int x = 0;
            int y = 0;
            int variant = (int)NPC.localAI[1];
            if (variant >= 10)
            {
                y = variant / 10;
            }
            x = variant % 10;

            NPC.frame = new(x * 66, y * 64, 66, 64);
        }

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            //if (spawnInfo.Player.InModBiome(ModContent.GetInstance<ConfectionBiome>()) && !spawnInfo.AnyInvasionActive() && Main.hardMode && !spawnInfo.Player.ZoneRockLayerHeight && !spawnInfo.Player.ZoneDirtLayerHeight) {
            //    return 0.82f;
            //}
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
				for (int i = 0; i < 50; i++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<CookieDust>(), 2.5f * (float)hit.HitDirection, -2.5f);
				}
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("RollercookieGore1").Type);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("RollercookieGore2").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("RollercookieGore3").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("RollercookieGore4").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("RollercookieGore5").Type);
            }
            else
            {
				for (int i = 0; i < hit.Damage / (double)NPC.lifeMax * 10.0; i++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<CookieDust>(), hit.HitDirection, -1f);
				}
			}
        }
	}
}
