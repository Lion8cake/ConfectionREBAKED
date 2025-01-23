using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Items;
using TheConfectionRebirth.Items.Armor;
using TheConfectionRebirth.Items.Armor.BirthdayOutfit;
using TheConfectionRebirth.Items.Armor.WonkyOutfit;
using TheConfectionRebirth.Items.Banners;

namespace TheConfectionRebirth.NPCs
{
	public class WildWilly : ModNPC
	{
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 3;
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
			NPC.damage = 30;
			NPC.defense = 20;
			NPC.lifeMax = 800;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath2;
			NPC.knockBackResist = 0.5f;
			NPC.value = 1000f;
			NPC.rarity = 2;
			AIType = NPCID.DoctorBones;
			AnimationType = NPCID.DoctorBones;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<WildWillyBanner>();
			SpawnModBiomes = new int[1] { ModContent.GetInstance<ConfectionBiome>().Type };
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
				new BestiaryBackground(ModContent.Request<Texture2D>("TheConfectionRebirth/Biomes/ConfectionBiomeMapBackground"), new Color(35, 40, 40)),
				new BestiaryBackgroundOverlay(Main.Assets.Request<Texture2D>("Images/MapBGOverlay4"), Color.White),
				new FlavorTextBestiaryInfoElement("Mods.TheConfectionRebirth.Bestiary.WildWilly")
			});
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CcretTicket>(), 10));
			npcLoot.Add(ItemDropRule.OneFromOptionsNotScalingWithLuck(1, ModContent.ItemType<WonkyHat>(), ModContent.ItemType<WonkyCoat>(), ModContent.ItemType<WonkyTrousers>()));
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return ConfectionGlobalNPC.SpawnNPC_ConfectionNPC(spawnInfo, Type);
		}

		public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode == NetmodeID.Server)
			{
				return;
			}

			if (NPC.life > 0)
			{
				for (int num483 = 0; (double)num483 < hit.Damage / (double)NPC.lifeMax * 100.0; num483++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, 5, hit.HitDirection, -1f);
				}
			}
			else
			{
				for (int num486 = 0; num486 < 50; num486++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, 5, 2.5f * (float)hit.HitDirection, -2.5f);
				}
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("WillyGore1").Type, NPC.scale);
				Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.position.X, NPC.position.Y + 20f), NPC.velocity, Mod.Find<ModGore>("WillyGore2").Type, NPC.scale);
				Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.position.X, NPC.position.Y + 20f), NPC.velocity, Mod.Find<ModGore>("WillyGore2").Type, NPC.scale);
				Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.position.X, NPC.position.Y + 34f), NPC.velocity, Mod.Find<ModGore>("WillyGore3").Type, NPC.scale);
				Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.position.X, NPC.position.Y + 34f), NPC.velocity, Mod.Find<ModGore>("WillyGore3").Type, NPC.scale);
			}
		}
	}
}