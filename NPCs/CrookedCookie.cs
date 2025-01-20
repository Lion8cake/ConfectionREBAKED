using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Dusts;
using TheConfectionRebirth.Items.Banners;

namespace TheConfectionRebirth.NPCs
{
    public class CrookedCookie : ModNPC
    {
		public override void SetStaticDefaults()
        {
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, new NPCID.Sets.NPCBestiaryDrawModifiers
            {
                Position = new(0, -5f),
                PortraitPositionYOverride = -20f
            });
        }

		public override void SetDefaults()
        {
            NPC.width = 18;
            NPC.height = 18;
            NPC.damage = 60;
            NPC.defense = 24;
            NPC.lifeMax = 150;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.knockBackResist = 0.5f;
			NPC.aiStyle = -1;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<CrookedCookieBanner>();
            SpawnModBiomes = new int[1] { ModContent.GetInstance<SandConfectionSurfaceBiome>().Type };
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

                new FlavorTextBestiaryInfoElement("Mods.TheConfectionRebirth.Bestiary.CrookedCookie")
            });
        }

        public override void AI()
        {
			
            if (NPC.collideX || NPC.collideY)
            {
				if (NPC.velocity.X != NPC.oldVelocity.X)
				{
					NPC.velocity.X = NPC.oldVelocity.X * -0.9f;
				}
				if (NPC.velocity.Y != NPC.oldVelocity.Y && NPC.oldVelocity.Y > 3f)
				{
					NPC.velocity.Y = NPC.oldVelocity.Y * -0.9f;
				}
			}
			float speed = 12f;
			NPC.TargetClosest();
			Vector2 pos = Main.player[NPC.target].Center - NPC.Center;
			pos.Normalize();
			pos *= speed;
			int offset = 200;
			NPC.velocity.X = (NPC.velocity.X * (float)(offset - 1) + pos.X) / (float)offset;
			if (NPC.velocity.Length() > 16f)
			{
				NPC.velocity.Normalize();
				NPC.velocity *= 16f;
			}
			NPC.ai[0] += 1f;
			if (NPC.ai[0] > 5f)
			{
				NPC.ai[0] = 5f;
				if (NPC.velocity.Y == 0f && NPC.velocity.X != 0f)
				{
					NPC.velocity.X *= 0.97f;
					if ((double)NPC.velocity.X > -0.01 && (double)NPC.velocity.X < 0.01)
					{
						NPC.velocity.X = 0f;
						NPC.netUpdate = true;
					}
				}
				NPC.velocity.Y += 0.2f;
			}
			NPC.rotation += NPC.velocity.X * 0.1f;

			if (NPC.velocity.Y > 16f)
			{
				NPC.velocity.Y = 16f;
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
				for (int i = 0; i < 50; i++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<CreamstoneDust>(), 2.5f * (float)hit.HitDirection, -2.5f);
				}
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("CrookedCookieGore1").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("CrookedCookieGore2").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("CrookedCookieGore3").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("CrookedCookieGore4").Type);
			}
			else
			{
				for (int i = 0; i < hit.Damage / (double)NPC.lifeMax * 10.0; i++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<CreamstoneDust>(), hit.HitDirection, -1f);
				}
			}
		}
    }
}
