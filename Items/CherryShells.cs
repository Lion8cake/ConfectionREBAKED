using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria;

namespace TheConfectionRebirth.Items
{
    public class CherryShells : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 5;
        }

        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 12;
            Item.value = 7500;
			Item.rare = ItemRarityID.Blue;
			Item.maxStack = Item.CommonMaxStack;
        }
    }
}