using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Items.Placeable;

namespace TheConfectionRebirth.NPCs
{
    public class NPCTrades : GlobalNPC
    {
		public override void ModifyShop(NPCShop shop) {
			var confectionworld = new Condition("Mods.TheConfectionRebirth.TheConfection", () => ConfectionWorldGeneration.confectionorHallow);
			if (shop.NpcType == NPCID.Dryad) {
				shop.InsertAfter(ItemID.HallowedGrassEcho, ModContent.ItemType<CreamgrassWall>(), Condition.Hardmode, confectionworld);
				shop.InsertAfter(ItemID.HallowedSeeds, ModContent.ItemType<CreamBeans>(), Condition.Hardmode, confectionworld);

				shop.InsertAfter(ItemID.PottedHallowCedar, ModContent.ItemType<PottedConfectionCedar>(), Condition.Hardmode, confectionworld, Condition.MoonPhaseFull);
				shop.InsertAfter(ItemID.PottedHallowCedar, ModContent.ItemType<PottedConfectionCedar>(), Condition.Hardmode, confectionworld, Condition.MoonPhaseWaningGibbous);
				shop.InsertAfter(ItemID.PottedHallowTree, ModContent.ItemType<PottedConfectionTree>(), Condition.Hardmode, confectionworld, Condition.MoonPhaseThirdQuarter);
				shop.InsertAfter(ItemID.PottedHallowTree, ModContent.ItemType<PottedConfectionTree>(), Condition.Hardmode, confectionworld, Condition.MoonPhaseWaningCrescent);
				shop.InsertAfter(ItemID.PottedHallowPalm, ModContent.ItemType<PottedConfectionPalm>(), Condition.Hardmode, confectionworld, Condition.MoonPhaseNew);
				shop.InsertAfter(ItemID.PottedHallowPalm, ModContent.ItemType<PottedConfectionPalm>(), Condition.Hardmode, confectionworld, Condition.MoonPhaseWaxingCrescent);
				shop.InsertAfter(ItemID.PottedHallowBamboo, ModContent.ItemType<PottedConfectionBamboo>(), Condition.Hardmode, confectionworld, Condition.MoonPhaseFirstQuarter);
				shop.InsertAfter(ItemID.PottedHallowBamboo, ModContent.ItemType<PottedConfectionBamboo>(), Condition.Hardmode, confectionworld, Condition.MoonPhaseWaxingGibbous);


				if (shop.TryGetEntry(ItemID.HallowedGrassEcho, out NPCShop.Entry entry)) {
					entry.AddCondition(confectionworld);
					entry.Disable();
				}
				if (shop.TryGetEntry(ItemID.HallowedSeeds, out NPCShop.Entry entry2)) {
					entry2.AddCondition(confectionworld);
					entry2.Disable();
				}

				if (shop.TryGetEntry(ItemID.PottedHallowCedar, out NPCShop.Entry entry4)) {
					entry4.AddCondition(confectionworld);
					entry4.Disable();
				}
				if (shop.TryGetEntry(ItemID.PottedHallowTree, out NPCShop.Entry entry5)) {
					entry5.AddCondition(confectionworld);
					entry5.Disable();
				}
				if (shop.TryGetEntry(ItemID.PottedHallowPalm, out NPCShop.Entry entry6)) {
					entry6.AddCondition(confectionworld);
					entry6.Disable();
				}
				if (shop.TryGetEntry(ItemID.PottedHallowBamboo, out NPCShop.Entry entry7)) {
					entry7.AddCondition(confectionworld);
					entry7.Disable();
				}
			}
			if (shop.NpcType == NPCID.Steampunker) {
				shop.InsertAfter(ItemID.BlueSolution, ModContent.ItemType<Items.CreamSolution>(), Condition.Hardmode, confectionworld);
			}
			if (shop.NpcType == NPCID.Wizard) {
				shop.InsertAfter(ItemID.Bell, ModContent.ItemType<Items.Kazoo>(), Condition.Hardmode);
				if (shop.TryGetEntry(ItemID.Bell, out NPCShop.Entry entry3)) {
					entry3.AddCondition(confectionworld);
					entry3.Disable();
				}
			}
		}
	}
}