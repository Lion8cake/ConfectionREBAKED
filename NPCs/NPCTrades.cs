using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Items.Placeable;

namespace TheConfectionRebirth.NPCs
{
    public class NPCTrades : GlobalNPC
    {
        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            if (type == NPCID.Dryad && Main.hardMode)
            {
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<CreamgrassWall>());
                nextSlot++;

                int pot = ModContent.ItemType<PottedConfectionCedar>();
                switch (Main.moonPhase)
				{
                    case 2:
                    case 3:
                        pot = ModContent.ItemType<PottedConfectionTree>();
                        break;
                    case 4:
                    case 6:
                        pot = ModContent.ItemType<PottedConfectionPalm>();
                        break;
                    case 7:
                    case 8:
                        pot = ModContent.ItemType<PottedConfectionBamboo>();
                        break;
                }
                shop.item[nextSlot].SetDefaults(pot);
                nextSlot++;

                shop.item[nextSlot].SetDefaults(ModContent.ItemType<CreamBeans>());
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<Pets.CreamyFoxPet.CreamFoxPetItem>());
                nextSlot++;
            }
            else if (type == NPCID.Steampunker && Main.hardMode)
            {
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.CreamSolution>());
                nextSlot++;
            }
            else if (type == NPCID.Wizard)
            {
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Kazoo>());
                nextSlot++;
            }
        }
    }
}