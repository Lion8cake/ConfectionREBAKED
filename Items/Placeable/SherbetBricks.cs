using TheConfectionRebirth.Tiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace TheConfectionRebirth.Items.Placeable
{
  public class SherbetBricks : ModItem
  {
	public override void SetStaticDefaults()
	{
		DisplayName.SetDefault("Sherbet Brick");
		CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
	}

	public override void SetDefaults()
	{
		Item.width = 32;
		Item.height = 16;
		Item.maxStack = 999;
		Item.useTurn = true;
		Item.autoReuse = true;
		Item.noUseGraphic = true;
		Item.useAnimation = 15;
		Item.useTime = 10;
		Item.useStyle = 1;
		Item.value = Item.buyPrice(0, 0, 15);
		Item.consumable = true;
		Item.createTile = ModContent.TileType<Tiles.SherbetBricks>();
	}
  } 
}
