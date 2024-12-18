using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static TheConfectionRebirth.NPCs.ConfectionGlobalNPC;

namespace TheConfectionRebirth.Items
{
	public class ConfectionGlobalItem : GlobalItem
	{
		public override bool? UseItem(Item item, Player player)
		{
			int type = item.type;
			int tileType = 0;
			if (type == ItemID.GreenMoss)
			{
				tileType = ModContent.TileType<Tiles.CreamstoneMossGreen>();
			}
			if (type == ItemID.BrownMoss)
			{
				tileType = ModContent.TileType<Tiles.CreamstoneMossBrown>();
			}
			if (type == ItemID.RedMoss)
			{
				tileType = ModContent.TileType<Tiles.CreamstoneMossRed>();
			}
			if (type == ItemID.BlueMoss)
			{
				tileType = ModContent.TileType<Tiles.CreamstoneMossBlue>();
			}
			if (type == ItemID.PurpleMoss)
			{
				tileType = ModContent.TileType<Tiles.CreamstoneMossPurple>();
			}
			if (type == ItemID.LavaMoss)
			{
				tileType = ModContent.TileType<Tiles.CreamstoneMossLava>();
			}
			if (type == ItemID.KryptonMoss)
			{
				tileType = ModContent.TileType<Tiles.CreamstoneMossKrypton>();
			}
			if (type == ItemID.XenonMoss)
			{
				tileType = ModContent.TileType<Tiles.CreamstoneMossXenon>();
			}
			if (type == ItemID.ArgonMoss)
			{
				tileType = ModContent.TileType<Tiles.CreamstoneMossArgon>();
			}
			if (type == ItemID.VioletMoss)
			{
				tileType = ModContent.TileType<Tiles.CreamstoneMossNeon>();
			}
			if (type == ItemID.RainbowMoss)
			{
				tileType = ModContent.TileType<Tiles.CreamstoneMossHelium>();
			}
			if (tileType != 0)
			{
				Tile tile = Framing.GetTileSafely(Player.tileTargetX, Player.tileTargetY);
				if (tile.HasTile && (tile.TileType == ModContent.TileType<Tiles.Creamstone>()) && player.IsInTileInteractionRange(Player.tileTargetX, Player.tileTargetY, Terraria.DataStructures.TileReachCheckSettings.Simple))
				{
					Main.tile[Player.tileTargetX, Player.tileTargetY].TileType = (ushort)tileType;
					WorldGen.SquareTileFrame(Player.tileTargetX, Player.tileTargetY, true);
					SoundEngine.PlaySound(SoundID.Dig, player.position);
					return true;
				}
			}
			return null;
		}

		public override bool IsAnglerQuestAvailable(int type)
		{
			if (ConfectionWorldGeneration.confectionorHallow)
			{
				if (type == ItemID.UnicornFish)
				{
					return false;
				}
				if (type == ItemID.Pixiefish)
				{
					return false;
				}
				if (type == ItemID.MirageFish)
				{
					return false;
				}
			}
			return true;
		}

		public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
		{
			if (item.type == ItemID.WallOfFleshBossBag)
			{
				itemLoot.Remove(FindHammer(itemLoot));
				//Moved the hammer outside of the WoF bag
			}
			if (item.type == ItemID.TwinsBossBag || item.type == ItemID.DestroyerBossBag || item.type == ItemID.SkeletronPrimeBossBag)
			{
				itemLoot.Remove(FindHallowedBars(itemLoot));

				DrunkWorldIsNotActive NotDrunk = new DrunkWorldIsNotActive();

				LeadingConditionRule ConfectionCondition = new LeadingConditionRule(new ConfectionDropRule());
				ConfectionCondition.OnSuccess(ItemDropRule.ByCondition(NotDrunk, ModContent.ItemType<Items.Placeable.NeapoliniteOre>(), 1, 15 * 5, 30 * 5));
				itemLoot.Add(ConfectionCondition);

				LeadingConditionRule HallowCondition = new LeadingConditionRule(new HallowDropRule());
				HallowCondition.OnSuccess(ItemDropRule.ByCondition(NotDrunk, ModContent.ItemType<Items.Placeable.HallowedOre>(), 1, 15 * 5, 30 * 5));
				itemLoot.Add(HallowCondition);

				LeadingConditionRule DrunkCondition = new LeadingConditionRule(new DrunkWorldIsActive());
				DrunkCondition.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Placeable.HallowedOre>(), 1, 8 * 5, 15 * 5));
				DrunkCondition.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Placeable.NeapoliniteOre>(), 1, 8 * 5, 15 * 5));
				itemLoot.Add(DrunkCondition);
			}
		}

		private static IItemDropRule FindHallowedBars(ItemLoot loot)
		{
			foreach (IItemDropRule item in loot.Get(false))
			{
				CommonDrop c = (CommonDrop)(object)((item is CommonDrop) ? item : null);
				if (c != null && c.itemId == ItemID.HallowedBar)
				{
					return (IItemDropRule)(object)c;
				}
			}
			return null;
		}

		private static IItemDropRule FindHammer(ItemLoot loot)
		{
			foreach (IItemDropRule item in loot.Get(false))
			{
				CommonDrop c = (CommonDrop)(object)((item is CommonDrop) ? item : null);
				if (c != null && c.itemId == ItemID.Pwnhammer)
				{
					return (IItemDropRule)(object)c;
				}
			}
			return null;
		}
	}
}
