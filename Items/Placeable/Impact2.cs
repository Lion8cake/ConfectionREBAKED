using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Placeable
{
    public class Impact2 : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 44;
            Item.height = 24;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = 1;
            Item.consumable = true;
            Item.value = 5000;
            Item.createTile = ModContent.TileType<Tiles.Impact2>();
            Item.placeStyle = 0;
        }
    }
}