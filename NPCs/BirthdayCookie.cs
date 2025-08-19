using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
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
using TheConfectionRebirth.Items.Armor.BirthdayOutfit;
using TheConfectionRebirth.Items.Armor.CookieOutfit;
using TheConfectionRebirth.Items.Banners;

namespace TheConfectionRebirth.NPCs
{
    public class BirthdayCookie : ModNPC
    {
		public override void SetDefaults()
        {
			NPC.aiStyle = 26;
			NPC.knockBackResist = 0.3f;
			NPC.value = 1500f;
			NPC.damage = 58;
			NPC.defense = 30;
			NPC.width = 44;
            NPC.height = 44;
            NPC.lifeMax = 360;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath6;
            AIType = NPCID.Unicorn;
            Banner = ModContent.NPCType<Rollercookie>();
            BannerItem = ModContent.ItemType<RollerCookieBanner>();
            SpawnModBiomes = new int[1] { ModContent.GetInstance<ConfectionBiome>().Type };
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
			bestiaryEntry.UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[ModContent.NPCType<Rollercookie>()], quickUnlock: true);
			ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[Type] = ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[ModContent.NPCType<Rollercookie>()];

			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                
                new FlavorTextBestiaryInfoElement("Mods.TheConfectionRebirth.Bestiary.BirthdayCookie")
            });
        }

		public override void AI()
        {
            NPC.rotation += NPC.velocity.X * 0.05f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CookieDough>(), maximumDropped: 2));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ChocolateChunk>(), 100));
            npcLoot.Add(ItemDropRule.OneFromOptionsNotScalingWithLuck(1, ModContent.ItemType<TopCake>(), ModContent.ItemType<BirthdaySuit>(), ModContent.ItemType<RightTrousers>()));
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
					Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<BirthdayDust>(), 2.5f * (float)hit.HitDirection, -2.5f);
				}
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("BirthdayCookieGore1").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("BirthdayCookieGore2").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("BirthdayCookieGore3").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("BirthdayCookieGore4").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("BirthdayCookieGore5").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("BirthdayCookieGore6").Type);
			}
			else
			{
				for (int i = 0; i < hit.Damage / (double)NPC.lifeMax * 10.0; i++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<BirthdayDust>(), hit.HitDirection, -1f);
				}
			}
		}
	}
}
