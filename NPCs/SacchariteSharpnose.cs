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
			NPC.width = 28;
			NPC.height = 44;
			NPC.aiStyle = 87;
			NPC.damage = 90;
			NPC.defense = 34;
			NPC.lifeMax = 3500;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.NPCDeath6;
			NPC.value = 30000f;
			NPC.knockBackResist = 0.1f;
			NPC.rarity = 5;

			Banner = Type;
            BannerItem = ModContent.ItemType<CreamSwollowerBanner>();
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
			float num305 = 0f;
			float num306 = Main.NPCAddHeight(NPC);
			Vector2 halfSize = new((float)(TextureAssets.Npc[Type].Width() / 2), (float)(TextureAssets.Npc[Type].Height() / Main.npcFrameCount[Type] / 2));
			SpriteEffects spriteEffects = (SpriteEffects)0;
			if (NPC.spriteDirection == 1)
			{
				spriteEffects = (SpriteEffects)1;
			}

			Texture2D value74 = TextureAssets.Npc[Type].Value;
			Color color41 = Color.White;
			float amount2 = 0f;
			float amount3 = 0f;
			int num268 = 0;
			int num269 = 0;
			int num270 = 1;
			int num271 = 15;
			float scale17 = NPC.scale;
			float value75 = NPC.scale;
			int num273 = 0;
			float num274 = 0f;
			float num275 = 0f;
			float num276 = 0f;
			Color color42 = drawColor;
			Vector2 origin16 = halfSize;

			num268 = 6;
			num269 = 3;
			num271 = num268 * 2;
			
			for (int num289 = num270; num289 < num268; num289 += num269)
			{
				Color value77 = color42;
				value77 = Color.Lerp(value77, color41, amount2);
				value77 = NPC.GetAlpha(value77);
				value77 *= (float)(num268 - num289) / (float)num271;
				float scale18 = MathHelper.Lerp(scale17, value75, 1f - (float)(num268 - num289) / (float)num271);
				Vector2 position26 = NPC.oldPos[num289] + new Vector2((float)NPC.width, (float)NPC.height) / 2f - screenPos;
				position26 -= new Vector2((float)value74.Width, (float)(value74.Height / Main.npcFrameCount[Type])) * NPC.scale / 2f;
				position26 += halfSize * NPC.scale + new Vector2(0f, num305 + num306 + NPC.gfxOffY);
				spriteBatch.Draw(value74, position26, (Rectangle?)NPC.frame, value77, NPC.rotation, halfSize, scale18, spriteEffects, 0f);
			}
			for (int num290 = 0; num290 < num273; num290++)
			{
				Color value79 = drawColor;
				value79 = Color.Lerp(value79, color41, amount2);
				value79 = NPC.GetAlpha(value79);
				value79 = Color.Lerp(value79, color41, amount3);
				value79 *= 1f - num274;
				Vector2 position27 = NPC.Center + ((float)num290 / (float)num273 * ((float)Math.PI * 2f) + NPC.rotation + num276).ToRotationVector2() * num275 * num274 - screenPos;
				position27 -= new Vector2((float)value74.Width, (float)(value74.Height / Main.npcFrameCount[Type])) * NPC.scale / 2f;
				position27 += halfSize * NPC.scale + new Vector2(0f, num305 + num306 + NPC.gfxOffY);
				spriteBatch.Draw(value74, position27, (Rectangle?)NPC.frame, value79, NPC.rotation, origin16, NPC.scale, spriteEffects, 0f);
			}
			Vector2 vector68 = NPC.Center - screenPos;
			vector68 -= new Vector2((float)value74.Width, (float)(value74.Height / Main.npcFrameCount[Type])) * NPC.scale / 2f;
			vector68 += halfSize * NPC.scale + new Vector2(0f, num305 + num306 + NPC.gfxOffY);
			spriteBatch.Draw(value74, vector68, (Rectangle?)NPC.frame, NPC.GetAlpha(color42), NPC.rotation, origin16, NPC.scale, spriteEffects, 0f);
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
