using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using TheConfectionRebirth.Items.Placeable;

namespace TheConfectionRebirth.Items
{
	public class ConfectionBiomeKey : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 1;
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<ConfectionBiomeChestItem>();
		}

		public override void SetDefaults()
		{
			Item.width = 14;
			Item.height = 20;
			Item.maxStack = Item.CommonMaxStack;
			Item.rare = ItemRarityID.Yellow;
		}

		public override bool ConsumeItem(Player player)
		{
			return NPC.downedPlantBoss;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			int index = tooltips.FindLastIndex(tt => (tt.Mod.Equals("Terraria") || tt.Mod.Equals(Mod.Name)) && tt.Name.Equals("Tooltip0"));
			if (!NPC.downedPlantBoss)
			{
				var thing = new TooltipLine(Mod, "Tooltip0", Language.GetTextValue("LegacyTooltip.59"));
				tooltips.Insert(index, thing);
			}
			else
			{
				var thing = new TooltipLine(Mod, "Tooltip0", Language.GetTextValue("Mods.TheConfectionRebirth.Items.ConfectionBiomeKey.KeyToolTip"));
				tooltips.Insert(index, thing);
			}
		}
	}
}
