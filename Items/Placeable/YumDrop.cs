using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Placeable
{
    public class YumDrop : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 25;
        }

        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 12;
            Item.value = Item.sellPrice(silver: 1);
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.White;
            Item.maxStack = Item.CommonMaxStack;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 7; 
            Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.YumBlock>();
		}
    }
}
