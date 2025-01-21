using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;
using System;
using System.Reflection;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Items;
using TheConfectionRebirth.Items.Banners;

namespace TheConfectionRebirth.NPCs
{
	[LegacyName("CreamSwollower")]
	public class SacchariteSharpnose : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 4;
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, new NPCID.Sets.NPCBestiaryDrawModifiers
            {
				Position = new Vector2(35f, -3f),
				PortraitPositionXOverride = 0f
			});
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
			NPCID.Sets.TrailingMode[Type] = 6;
        }

        public override void SetDefaults()
        {
			NPC.noGravity = true;
			NPC.width = 100;
			NPC.height = 24;
			NPC.aiStyle = 103;
			NPC.damage = 54;
			NPC.defense = 26;
			NPC.lifeMax = 450;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = 400f;
			NPC.knockBackResist = 0.7f;
			NPC.behindTiles = true;
			Banner = Type;
            BannerItem = ModContent.ItemType<SacchariteSharpnoseBanner>();
            SpawnModBiomes = new int[1] { ModContent.GetInstance<SandConfectionSurfaceBiome>().Type };
        }

		public override void FindFrame(int frameHeight)
		{
			NPC.spriteDirection = NPC.direction;
			NPC.frameCounter += 1.0;
			if (NPC.frameCounter >= 16.0)
			{
				NPC.frameCounter = 0.0;
			}
			NPC.frame.Y = frameHeight * (int)(NPC.frameCounter / 4.0);
		}

		public override bool? CanFallThroughPlatforms() 
        {
			return true;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Vector2 frameSize = new Vector2(NPC.frame.Width, NPC.frame.Height);
			Vector2 halfSize = new(frameSize.X / 2, frameSize.Y / 2);

			SpriteEffects spriteEffects = SpriteEffects.None;
			if (NPC.spriteDirection == 1)
			{
				spriteEffects = SpriteEffects.FlipHorizontally;
			}

			int trailAmount = 6;
			int trailIncrement = trailAmount / 2;
			float trailColorMod = trailAmount * 2;

			for (int i = 1; i < trailAmount; i += trailIncrement)
			{
				Color color = NPC.GetAlpha(drawColor);
				color *= (trailAmount - i) / trailColorMod;
				Vector2 posTrail = NPC.oldPos[i] + new Vector2(NPC.width, NPC.height) / 2f - screenPos;
				posTrail -= frameSize * NPC.scale / 2f;
				posTrail += halfSize * NPC.scale + new Vector2(0f, NPC.gfxOffY);
				spriteBatch.Draw(TextureAssets.Npc[Type].Value, posTrail, NPC.frame, color, NPC.rotation, halfSize, NPC.scale, spriteEffects, 0f);
			}
			Vector2 pos = NPC.Center - screenPos;
			pos -= frameSize * NPC.scale / 2f;
			pos += halfSize * NPC.scale + new Vector2(0f, NPC.gfxOffY);
			spriteBatch.Draw(TextureAssets.Npc[Type].Value, pos, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, halfSize, NPC.scale, spriteEffects, 0f);
			return false;
		}

		public override Color? GetAlpha(Color drawColor) {
			float num = (float)(255 - NPC.alpha) / 255f;
			int num2 = (int)(drawColor.R * num);
			int num3 = (int)(drawColor.G * num);
			int num4 = (int)(drawColor.B * num);
			int num5 = drawColor.A - NPC.alpha;
			if (num2 + num3 + num4 > 10 && num2 + num3 + num4 >= 60) 
            {
				num2 *= 2;
				num3 *= 2;
				num4 *= 2;
				if (num2 > 255) 
                {
					num2 = 255;
				}
				if (num3 > 255) 
                {
					num3 = 255;
				}
				if (num4 > 255) 
                {
					num4 = 255;
				}
			}
			return new Color(num2, num3, num4, num5);
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Sandstorm,

                new FlavorTextBestiaryInfoElement("Mods.TheConfectionRebirth.Bestiary.SacchariteSharpnose")
            });
        }

		public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
			npcLoot.Add(ItemDropRule.Food(ItemID.Nachos, 30));
			npcLoot.Add(ItemDropRule.Common(ItemID.SharkFin, 8));
            npcLoot.Add(ItemDropRule.ByCondition(new Conditions.WindyEnoughForKiteDrops(), ItemID.KiteSandShark, 25));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CreamPuff>(), 25));
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            //if (spawnInfo.Player.InModBiome(ModContent.GetInstance<ConfectionBiome>()) && !spawnInfo.AnyInvasionActive() && Main.hardMode && spawnInfo.Player.ZoneDesert && spawnInfo.Player.ZoneSandstorm) {
            //    return 0.5f;
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
				for (int i = 0; (double)i < hit.Damage / (double)NPC.lifeMax * 150.0; i++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f);
				}
			}
			else
			{
				for (int i = 0; i < 75; i++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 2 * hit.HitDirection, -2f);
				}
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity * 0.8f, Mod.Find<ModGore>("SacchariteSharpnoseGore1").Type);
				Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.position.X + 14f, NPC.position.Y), NPC.velocity * 0.8f, Mod.Find<ModGore>("SacchariteSharpnoseGore2").Type);
				Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.position.X + 14f, NPC.position.Y), NPC.velocity * 0.8f, Mod.Find<ModGore>("SacchariteSharpnoseGore3").Type);
				Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.position.X + 14f, NPC.position.Y), NPC.velocity * 0.8f, Mod.Find<ModGore>("SacchariteSharpnoseGore4").Type);
			}
        }
    }
}
