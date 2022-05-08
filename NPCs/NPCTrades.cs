using TheConfectionRebirth.Dusts;
using TheConfectionRebirth.Items;
using TheConfectionRebirth.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth;

namespace TheConfectionRebirth.NPCs
{
	public class NPCTrades : GlobalNPC
	{
	public override void SetupShop(int type, Chest shop, ref int nextSlot) {
			if (type == NPCID.Dryad && Main.hardMode) {
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Placeable.CreamBeans>());
				shop.item[nextSlot].shopCustomPrice = 2000;
				nextSlot++;
			}
			else if (type == NPCID.Steampunker && Main.hardMode) {
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.CreamSolution>());
				shop.item[nextSlot].shopCustomPrice = 500;
				nextSlot++;
			}
			else if (type == NPCID.Wizard) {
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Weapons.Kazoo>());
				shop.item[nextSlot].shopCustomPrice = 10000;
				nextSlot++;
			}
		}
    }
}