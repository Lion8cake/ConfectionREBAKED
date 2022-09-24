using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Biomes;

namespace TheConfectionRebirth.NPCs
{
    public class NPCTrades : GlobalNPC
    {
        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            if (type == NPCID.Dryad && Main.hardMode)
            {
                if (type == NPCID.Dryad && Main.moonPhase == 0 && Main.hardMode || type == NPCID.Dryad && Main.moonPhase == 1 && Main.hardMode)
                {
                    shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Placeable.PottedConfectionCedar>());
                    shop.item[nextSlot].shopCustomPrice = 30000;
                    nextSlot++;
                }
                if (type == NPCID.Dryad && Main.moonPhase == 2 && Main.hardMode || type == NPCID.Dryad && Main.moonPhase == 3 && Main.hardMode)
                {
                    shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Placeable.PottedConfectionTree>());
                    shop.item[nextSlot].shopCustomPrice = 30000;
                    nextSlot++;
                }
                if (type == NPCID.Dryad && Main.moonPhase == 4 && Main.hardMode || type == NPCID.Dryad && Main.moonPhase == 5 && Main.hardMode)
                {
                    shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Placeable.PottedConfectionPalm>());
                    shop.item[nextSlot].shopCustomPrice = 30000;
                    nextSlot++;
                }
                if (type == NPCID.Dryad && Main.moonPhase == 6 && Main.hardMode || type == NPCID.Dryad && Main.moonPhase == 7 && Main.hardMode)
                {
                    shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Placeable.PottedConfectionBamboo>());
                    shop.item[nextSlot].shopCustomPrice = 30000;
                    nextSlot++;
                }
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Placeable.CreamBeans>());
                shop.item[nextSlot].shopCustomPrice = 2000;
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<Pets.CreamyFoxPet.CreamFoxPetItem>());
                shop.item[nextSlot].shopCustomPrice = 1000000;
                nextSlot++;
            }
            else if (type == NPCID.Steampunker && Main.hardMode)
            {
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.CreamSolution>());
                shop.item[nextSlot].shopCustomPrice = 500;
                nextSlot++;
            }
            else if (type == NPCID.Wizard)
            {
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Kazoo>());
                shop.item[nextSlot].shopCustomPrice = 10000;
                nextSlot++;
            }
        }
    }
}