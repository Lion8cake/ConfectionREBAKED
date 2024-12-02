using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria.ID;

namespace TheConfectionRebirth.Items.Placeable
{
	[LegacyName("SoulofSpiteinaBottleItem")]
	public class SoulofSpiteinaBottle : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
			Item.width = 12;
			Item.height = 28;
			Item.value = 1000;
			Item.createTile = ModContent.TileType<Tiles.SoulofSpiteinaBottle>();
        }
	}
}
