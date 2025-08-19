using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria.ID;

namespace TheConfectionRebirth.Items.Placeable.Furniture
{
    public class SacchariteCandelabra : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 22;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
            Item.value = Terraria.Item.sellPrice(silver: 3);
            Item.createTile = ModContent.TileType<Tiles.Furniture.SacchariteCandelabra>();
        }
    }
}
