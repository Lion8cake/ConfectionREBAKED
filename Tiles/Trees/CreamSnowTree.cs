using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;
using TheConfectionRebirth.Items;
using TheConfectionRebirth.NPCs.Critters;

namespace TheConfectionRebirth.Tiles.Trees
{
    public class CreamSnowTree : ModTree
    {
        public override void SetStaticDefaults()
        {
            GrowsOnTileId = new int[1] { ModContent.TileType<CreamBlock>() };
        }

		public override int CreateDust() {
			return ModContent.DustType<Dusts.CreamwoodDust>();
		}

		public override Asset<Texture2D> GetTexture()
        {
            return ModContent.Request<Texture2D>("TheConfectionRebirth/Tiles/Trees/CreamSnowTree");
        }

        public override Asset<Texture2D> GetBranchTextures()
        {
            return ModContent.Request<Texture2D>("TheConfectionRebirth/Tiles/Trees/CreamSnowTree_Branches");
        }

        public override Asset<Texture2D> GetTopTextures()
        {
            return ModContent.Request<Texture2D>("TheConfectionRebirth/Tiles/Trees/CreamSnowTree_Tops");
        }

		public override TreePaintingSettings TreeShaderSettings => new TreePaintingSettings {
			UseSpecialGroups = true,
			SpecialGroupMinimalHueValue = 11f / 72f,
			SpecialGroupMaximumHueValue = 0.25f,
			SpecialGroupMinimumSaturationValue = 0.88f,
			SpecialGroupMaximumSaturationValue = 1f
		};

		public override bool Shake(int x, int y, ref bool createLeaves) {
			if (Main.rand.NextBool(7)) {
				Item.NewItem(WorldGen.GetItemSource_FromTreeShake(x, y), x * 16, y * 16, 16, 16, 27, WorldGen.genRand.Next(1, 3));
				return true;
			}
			else if (WorldGen.genRand.Next(20) == 0) {
				int type = 71;
				int num2 = WorldGen.genRand.Next(50, 100);
				if (WorldGen.genRand.Next(30) == 0) {
					type = 73;
					num2 = 1;
					if (WorldGen.genRand.Next(5) == 0) {
						num2++;
					}
					if (WorldGen.genRand.Next(10) == 0) {
						num2++;
					}
				}
				else if (WorldGen.genRand.Next(10) == 0) {
					type = 72;
					num2 = WorldGen.genRand.Next(1, 21);
					if (WorldGen.genRand.Next(3) == 0) {
						num2 += WorldGen.genRand.Next(1, 21);
					}
					if (WorldGen.genRand.Next(4) == 0) {
						num2 += WorldGen.genRand.Next(1, 21);
					}
				}
				Item.NewItem(WorldGen.GetItemSource_FromTreeShake(x, y), x * 16, y * 16, 16, 16, type, num2);
				return true;
			}
			else if (WorldGen.genRand.Next(15) == 0) {
				int type2 = WorldGen.genRand.Next(5) switch {
					0 => 74,
					1 => 297,
					2 => 298,
					3 => 299,
					4 => ModContent.NPCType<NPCs.Critters.Pip>(),
					5 => ModContent.NPCType<NPCs.Critters.Birdnana>(),
					_ => 538,
				};
				if (Player.GetClosestRollLuck(x, y, NPC.goldCritterChance) == 0f) {
					type2 = ((WorldGen.genRand.Next(2) != 0) ? 539 : 442);
				}
				NPC.NewNPC(new EntitySource_ShakeTree(x, y), x * 16, y * 16, type2);
				return true;
			}
			else if (WorldGen.genRand.Next(50) == 0 && !Main.dayTime) {
				int type3 = Main.rand.NextFromList(new short[3] { 583, 584, 585 });
				if (Main.tenthAnniversaryWorld && Main.rand.Next(4) != 0) {
					type3 = 583;
				}
				NPC.NewNPC(new EntitySource_ShakeTree(x, y), x * 16, y * 16, type3);
				return true;
			}
			else if (WorldGen.genRand.Next(50) == 0) {
				for (int l = 0; l < 5; l++) {
					Point point = new Point(x + Main.rand.Next(-2, 2), y - 1 + Main.rand.Next(-2, 2));
					int type4 = ((Player.GetClosestRollLuck(x, y, NPC.goldCritterChance) != 0f) ? Main.rand.NextFromList(new short[3] { 74, 297, 298 }) : 442);
					NPC obj2 = Main.npc[NPC.NewNPC(new EntitySource_ShakeTree(x, y), point.X * 16, point.Y * 16, type4)];
					obj2.velocity = Main.rand.NextVector2CircularEdge(3f, 3f);
					obj2.netUpdate = true;
				}
				return true;
			}
			else if (WorldGen.genRand.Next(20) == 0 && !Main.raining && !NPC.TooWindyForButterflies && Main.dayTime) {
				int type5 = 356;
				if (Player.GetClosestRollLuck(x, y, NPC.goldCritterChance) == 0f) {
					type5 = 444;
				}
				NPC.NewNPC(new EntitySource_ShakeTree(x, y), x * 16, y * 16, type5);
				return true;
			}
			else if (WorldGen.genRand.Next(12) == 0) {
				Item.NewItem(Type: (WorldGen.genRand.Next(2) != 0) ? ModContent.ItemType<Cherimoya>() : 4297, source: WorldGen.GetItemSource_FromTreeShake(x, y), X: x * 16, Y: y * 16, Width: 16, Height: 16);
				return true;
			}
			return false;
		}

		public override int SaplingGrowthType(ref int style) {
			style = 0;
			return ModContent.TileType<Trees.CreamSapling>();
		}

		public override void SetTreeFoliageSettings(Tile tile, ref int xoffset, ref int treeFrame, ref int floorY, ref int topTextureFrameWidth, ref int topTextureFrameHeight) {
		}

		public override int TreeLeaf() {
			return ModContent.GoreType<CreamTreeLeaf>();
		}

		public override int DropWood() {
			return ModContent.ItemType<Items.Placeable.CreamWood>();
		}
	}
}