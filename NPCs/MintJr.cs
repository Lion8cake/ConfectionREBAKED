using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Dusts;
using TheConfectionRebirth.Items.Banners;

namespace TheConfectionRebirth.NPCs
{
    public class MintJr : ModNPC
    {
		public override void SetStaticDefaults()
        {
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, new(0)
            {
                Position = new(0, 8f)
            });
        }

		public override void SetDefaults()
        {
            NPC.width = 18;
            NPC.height = 18;
            NPC.damage = 60;
            NPC.defense = 20;
            NPC.lifeMax = 120;
            NPC.HitSound = SoundID.NPCHit7;
            NPC.DeathSound = SoundID.NPCDeath19;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.knockBackResist = 0.5f;
			NPC.aiStyle = -1;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<MintJrBanner>();
            SpawnModBiomes = new int[1] { ModContent.GetInstance<SandConfectionSurfaceBiome>().Type };
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

                new FlavorTextBestiaryInfoElement("Mods.TheConfectionRebirth.Bestiary.MintJr")
            });
        }

		public override void AI() {
			if (NPC.target < 0 || NPC.target <= 255 || Main.player[NPC.target].dead) {
				NPC.TargetClosest();
			}
			NPCAimedTarget targetData = NPC.GetTargetData();
			bool flag = false;
			if (targetData.Type == NPCTargetType.Player) {
				flag = Main.player[NPC.target].dead;
			}
			float num = 2f;
			float num12 = 0.06f;
			Vector2 vector = new(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
			float num23 = targetData.Position.X + (float)(targetData.Width / 2);
			float num24 = targetData.Position.Y + (float)(targetData.Height / 2);
			num23 = (int)(num23 / 8f) * 8;
			num24 = (int)(num24 / 8f) * 8;
			vector.X = (int)(vector.X / 8f) * 8;
			vector.Y = (int)(vector.Y / 8f) * 8;
			num23 -= vector.X;
			num24 -= vector.Y;
			float num25 = (float)Math.Sqrt(num23 * num23 + num24 * num24);
			if (num25 == 0f) {
				num23 = NPC.velocity.X;
				num24 = NPC.velocity.Y;
			}
			else {
				num25 = num / num25;
				num23 *= num25;
				num24 *= num25;
			}

			bool flag4 = true;
			
			if (flag) {
				num23 = (float)NPC.direction * num / 2f;
				num24 = (0f - num) / 2f;
			}
			if (NPC.velocity.X < num23) {
				NPC.velocity.X += num12;
				if (flag4 && NPC.velocity.X < 0f && num23 > 0f) {
					NPC.velocity.X += num12;
				}
			}
			else if (NPC.velocity.X > num23) {
				NPC.velocity.X -= num12;
				if (flag4 && NPC.velocity.X > 0f && num23 < 0f) {
					NPC.velocity.X -= num12;
				}
			}
			if (NPC.velocity.Y < num24) {
				NPC.velocity.Y += num12;
				if (flag4 && NPC.velocity.Y < 0f && num24 > 0f) {
					NPC.velocity.Y += num12;
				}
			}
			else if (NPC.velocity.Y > num24) {
				NPC.velocity.Y -= num12;
				if (flag4 && NPC.velocity.Y > 0f && num24 < 0f) {
					NPC.velocity.Y -= num12;
				}
			}
			if (num23 > 0f) {
				NPC.spriteDirection = 1;
				NPC.rotation = (float)Math.Atan2(num24, num23);
			}
			else if (num23 < 0f) {
				NPC.spriteDirection = -1;
				NPC.rotation = (float)Math.Atan2(num24, num23) + 3.14f;
			}

			float num7 = 0.7f;
			if (NPC.collideX) {
				NPC.netUpdate = true;
				NPC.velocity.X = NPC.oldVelocity.X * (0f - num7);
				if (NPC.direction == -1 && NPC.velocity.X > 0f && NPC.velocity.X < 2f) {
					NPC.velocity.X = 2f;
				}
				if (NPC.direction == 1 && NPC.velocity.X < 0f && NPC.velocity.X > -2f) {
					NPC.velocity.X = -2f;
				}
			}
			if (NPC.collideY) {
				NPC.netUpdate = true;
				NPC.velocity.Y = NPC.oldVelocity.Y * (0f - num7);
				if (NPC.velocity.Y > 0f && (double)NPC.velocity.Y < 1.5) {
					NPC.velocity.Y = 2f;
				}
				if (NPC.velocity.Y < 0f && (double)NPC.velocity.Y > -1.5) {
					NPC.velocity.Y = -2f;
				}
			}
			NPC.position += NPC.netOffset;
			
			NPC.position -= NPC.netOffset;

			if (((NPC.velocity.X > 0f && NPC.oldVelocity.X < 0f) || (NPC.velocity.X < 0f && NPC.oldVelocity.X > 0f) || (NPC.velocity.Y > 0f && NPC.oldVelocity.Y < 0f) || (NPC.velocity.Y < 0f && NPC.oldVelocity.Y > 0f)) && !NPC.justHit) {
				NPC.netUpdate = true;
			}
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo) {
			if (Main.rand.NextBool(2)) {
				target.AddBuff(BuffID.Slow, 240);
			}
		}

		public override void HitEffect(NPC.HitInfo hit)
        {
			if (Main.netMode == NetmodeID.Server) {
				return;
			}

			if (NPC.life <= 0) {
				for (int i = 0; i < 25; i++) {
					Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<FudgeDust>());
				}
			}
		}
    }
}
