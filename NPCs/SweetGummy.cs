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
using TheConfectionRebirth.Items;
using TheConfectionRebirth.Items.Armor.GummyOutfit;
using TheConfectionRebirth.Items.Banners;

namespace TheConfectionRebirth.NPCs
{
	public class SweetGummy : ModNPC
    {
		public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 16;
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, new NPCID.Sets.NPCBestiaryDrawModifiers
            {
                Velocity = 0.5f
            });
        }

        public override void SetDefaults()
        {
			NPC.width = 18;
			NPC.height = 40;
			NPC.aiStyle = 3;
			NPC.damage = 55;
			NPC.defense = 18;
			NPC.lifeMax = 200;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath6;
			NPC.knockBackResist = 0.55f;
			NPC.value = 700f;
			AIType = NPCID.Mummy;
			AnimationType = NPCID.Mummy;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<SweetGummyBanner>();
			SpawnModBiomes = new int[1] { ModContent.GetInstance<SandConfectionSurfaceBiome>().Type };
		}

		public override void OnSpawn(IEntitySource source)
		{
			NPC.localAI[0] = Main.rand.Next(0, 9);
            if (Main.rand.NextBool(2))
            {
                if (TheConfectionRebirth.easter)
                {
					NPC.localAI[0] = 12 + Main.rand.Next(0, 3);
				}
                else if (Main.halloween)
                {
                    NPC.localAI[0] = 9 + Main.rand.Next(0, 3);
                }
            }
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture;
            if (NPC.localAI[0] <= 0) 
            { 
                texture = TextureAssets.Npc[Type].Value;
            }
            else
            {
                texture = ModContent.Request<Texture2D>(Texture + "_" + NPC.localAI[0]).Value;
            }
			SpriteEffects spriteEffects = 0;
			if (NPC.spriteDirection == 1)
			{
				spriteEffects = (SpriteEffects)1;
			}
			Vector2 halfSize = new((float)(TextureAssets.Npc[NPC.type].Width() / 2), (float)(TextureAssets.Npc[NPC.type].Height() / Main.npcFrameCount[NPC.type] / 2));
			Rectangle frame = NPC.frame;
            if (NPC.localAI[0] >= 12)
            {
                //Gummy Bunnies are 6 extra pixels in height, here we manipulate the frame to include the extra pixels
                int height = frame.Height + 6;
				int y = (frame.Y / frame.Height) * (height);
                frame.Y = y;
                frame.Height = height;
            }
			float num305 = 0f;
			float num306 = Main.NPCAddHeight(NPC);
			spriteBatch.Draw(texture, new Vector2(NPC.position.X - screenPos.X + (float)(NPC.width / 2) - (float)texture.Width * NPC.scale / 2f + halfSize.X * NPC.scale, NPC.position.Y - screenPos.Y + (float)NPC.height - (float)texture.Height * NPC.scale / (float)Main.npcFrameCount[NPC.type] + 4f + halfSize.Y * NPC.scale + num306 + num305 + NPC.gfxOffY), (Rectangle?)frame, NPC.GetAlpha(drawColor), NPC.rotation, halfSize, NPC.scale, spriteEffects, 0f);
			return false;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

                new FlavorTextBestiaryInfoElement("Mods.TheConfectionRebirth.Bestiary.SweetGummy")
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<GummyMask>(), 75));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<GummyShirt>(), 75));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<GummyPants>(), 75));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CreamPuff>(), 10));
            npcLoot.Add(ItemDropRule.StatusImmunityItem(ItemID.TrifoldMap, 100));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            //if (spawnInfo.Player.InModBiome(ModContent.GetInstance<ConfectionBiome>()) && !spawnInfo.AnyInvasionActive() && Main.hardMode && spawnInfo.Player.ZoneDesert && !spawnInfo.Player.ZoneRockLayerHeight && !spawnInfo.Player.ZoneUnderworldHeight) {
            //    return 0.31f;
            //}
            return 0f;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (Main.netMode == NetmodeID.Server)
            {
                return;
            }

            if (NPC.life > 0)
            {
                for (int i = 0; (double)i < hit.Damage / (double)NPC.lifeMax * 50.0; i++)
                {
                    int dustID = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Smoke, 0f, 0f, 0, default(Color), 1.5f);
                    Dust dust = Main.dust[dustID];
                    dust.velocity *= 2f;
                    dust.noGravity = true;
                }
            }
            else
            {
                for (int j = 0; j < 20; j++)
                {
                    int dustID = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Smoke, 0f, 0f, 0, default(Color), 1.5f);
                    Dust dust = Main.dust[dustID];
                    dust.velocity *= 2f;
                    dust.noGravity = true;
                }
                int goreID = Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.position.X, NPC.position.Y - 10f), new Vector2((float)hit.HitDirection, 0f), 61, NPC.scale);
                Gore gore = Main.gore[goreID];
                gore.velocity *= 0.3f;
                goreID = Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.position.X, NPC.position.Y + (float)(NPC.height / 2) - 10f), new Vector2((float)hit.HitDirection, 0f), 62, NPC.scale);
                gore = Main.gore[goreID];
                gore.velocity *= 0.3f;
                goreID = Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.position.X, NPC.position.Y + (float)NPC.height - 10f), new Vector2((float)hit.HitDirection, 0f), 63, NPC.scale);
                gore = Main.gore[goreID];
                gore.velocity *= 0.3f;
            }
		}
    }
}
