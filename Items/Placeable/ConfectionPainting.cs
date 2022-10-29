using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Placeable
{
    public class ConfectionPainting : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Deserted Land");
            Tooltip.SetDefault("'S. Bobble'");
            SacrificeTotal = 1;
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
            Item.value = 10000;
            Item.createTile = ModContent.TileType<Tiles.ConfectionPainting>();
            Item.placeStyle = 0;
        }
    }
}