﻿using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Items;
using static Terraria.WorldGen;
using Microsoft.Xna.Framework;

namespace TheConfectionRebirth.Tiles.Trees
{
    public class CreamPalmTree : ModPalmTree
    {
        public override TreePaintingSettings TreeShaderSettings => new TreePaintingSettings
        {
            UseSpecialGroups = true,
            SpecialGroupMinimalHueValue = 11f / 72f,
            SpecialGroupMaximumHueValue = 0.25f,
            SpecialGroupMinimumSaturationValue = 0.88f,
            SpecialGroupMaximumSaturationValue = 1f
        };

        public override void SetStaticDefaults()
        {
            GrowsOnTileId = [ModContent.TileType<Creamsand>()];
        }

        public override Asset<Texture2D> GetTexture()
        {
            return ModContent.Request<Texture2D>("TheConfectionRebirth/Tiles/Trees/CreamPalmTree");
        }

        public override int SaplingGrowthType(ref int style)
        {
            style = 1;
            return ModContent.TileType<CreamSapling>();
        }

        public override Asset<Texture2D> GetOasisTopTextures()
        {
            return ModContent.Request<Texture2D>("TheConfectionRebirth/Tiles/Trees/CreamPalmOasisTree_Tops");
        }

        public override Asset<Texture2D> GetTopTextures()
        {
            return ModContent.Request<Texture2D>("TheConfectionRebirth/Tiles/Trees/CreamPalmTree_Tops");
        }

		public override int TreeLeaf() {
			return ModContent.GoreType<CreamTreeLeaf>();
		}

		public override bool Shake(int x, int y, ref bool createLeaves) {
			if (Main.getGoodWorld && genRand.NextBool(17)) {
				Projectile.NewProjectile(new EntitySource_ShakeTree(x, y), x * 16, y * 16, (float)Main.rand.Next(-100, 101) * 0.002f, 0f, ProjectileID.Bomb, 0, 0f, Main.myPlayer, 16f, 16f);
			}
			else if (genRand.NextBool(7)) {
				Item.NewItem(new EntitySource_ShakeTree(x, y), x * 16, y * 16, 16, 16, ItemID.Acorn, genRand.Next(1, 3));
			}
			else if (genRand.NextBool(35) && Main.halloween) {
				Item.NewItem(new EntitySource_ShakeTree(x, y), x * 16, y * 16, 16, 16, ItemID.RottenEgg, genRand.Next(1, 3));
			}
			else if (genRand.NextBool(12)) {
				Item.NewItem(GetItemSource_FromTreeShake(x, y), x * 16, y * 16, 16, 16, ModContent.ItemType<Items.Placeable.CreamWood>(), genRand.Next(1, 4));
			}
			else if (genRand.NextBool(20)) {
				int type = ItemID.CopperCoin;
				int num2 = genRand.Next(50, 100);
				if (genRand.NextBool(30)) {
					type = ItemID.GoldCoin;
					num2 = 1;
					if (genRand.NextBool(5)) {
						num2++;
					}
					if (genRand.NextBool(10)) {
						num2++;
					}
				}
				else if (genRand.NextBool(10)) {
					type = ItemID.SilverCoin;
					num2 = genRand.Next(1, 21);
					if (genRand.NextBool(3)) {
						num2 += genRand.Next(1, 21);
					}
					if (genRand.NextBool(4)) {
						num2 += genRand.Next(1, 21);
					}
				}
				Item.NewItem(GetItemSource_FromTreeShake(x, y), x * 16, y * 16, 16, 16, type, num2);
			}
			else if (genRand.NextBool(15)) {
				int type2 = Main.rand.NextFromList(new short[4] { (short)ModContent.NPCType<NPCs.Pip>(), (short)ModContent.NPCType<NPCs.Birdnana>(), NPCID.Squirrel, NPCID.SquirrelRed });
				if (Player.GetClosestRollLuck(x, y, NPC.goldCritterChance) == 0f) {
					type2 = ((!genRand.NextBool(2)) ? NPCID.SquirrelGold : NPCID.GoldBird);
				}
				NPC.NewNPC(new EntitySource_ShakeTree(x, y), x * 16, y * 16, type2);
			}
			else if (genRand.NextBool(50) && !Main.dayTime) {
				int type3 = Main.rand.NextFromList(new short[3] { NPCID.FairyCritterPink, NPCID.FairyCritterGreen, NPCID.FairyCritterBlue });
				if (Main.tenthAnniversaryWorld && !Main.rand.NextBool(4)) {
					type3 = NPCID.FairyCritterPink;
				}
				NPC.NewNPC(new EntitySource_ShakeTree(x, y), x * 16, y * 16, type3);
			}
			else if (genRand.NextBool(50)) {
				Point point;
				for (int l = 0; l < 5; l++) {
					point = new(x + Main.rand.Next(-2, 2), y - 1 + Main.rand.Next(-2, 2));
					int type4 = ((Player.GetClosestRollLuck(x, y, NPC.goldCritterChance) != 0f) ? Main.rand.NextFromList(new short[2] { (short)ModContent.NPCType<NPCs.Pip>(), (short)ModContent.NPCType<NPCs.Birdnana>() }) : NPCID.GoldBird);
					NPC obj3 = Main.npc[NPC.NewNPC(new EntitySource_ShakeTree(x, y), point.X * 16, point.Y * 16, type4)];
					obj3.velocity = Main.rand.NextVector2CircularEdge(3f, 3f);
					obj3.netUpdate = true;
				}
			}
			else if (genRand.NextBool(20)&& !IsPalmOasisTree(x)) {
				NPC.NewNPC(new EntitySource_ShakeTree(x, y), x * 16, y * 16, NPCID.Seagull2);
			}
			else if (genRand.NextBool(20) && !Main.raining && !NPC.TooWindyForButterflies && Main.dayTime) {
				int type5 = ModContent.NPCType<NPCs.GrumbleBee>();
				if (Player.GetClosestRollLuck(x, y, NPC.goldCritterChance) == 0f) {
					type5 = NPCID.GoldButterfly;
				}
				NPC.NewNPC(new EntitySource_ShakeTree(x, y), x * 16, y * 16, type5);
			}
			else if (genRand.NextBool(12) && !IsPalmOasisTree(x)) {
				int secondaryItemStack = ((!genRand.NextBool(2)) ? ItemID.Coconut : ItemID.Banana);
				Item.NewItem(GetItemSource_FromTreeShake(x, y), x * 16, y * 16, 16, 16, secondaryItemStack);
			}
			else if (genRand.NextBool(12)) {
				int secondaryItemStack = ((!genRand.NextBool(2)) ? ModContent.ItemType<Cherimoya>() : ModContent.ItemType<CocoaBeans>());
				Item.NewItem(GetItemSource_FromTreeShake(x, y), x * 16, y * 16, 16, 16, secondaryItemStack);
			}
			if (Main.netMode == NetmodeID.Server) {
				NetMessage.SendData(MessageID.SpecialFX, -1, -1, null, 1, x, y, 1f, ModContent.GoreType<CreamTreeLeaf>());
			}
			return false;
		}

		public override int DropWood()
        {
			return ModContent.ItemType<Items.Placeable.CreamWood>();
        }
    }
}
