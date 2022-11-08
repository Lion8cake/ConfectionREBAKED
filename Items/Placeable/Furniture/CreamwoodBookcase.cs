using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Placeable.Furniture
{
    public class CreamwoodBookcase : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 20;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.value = Terraria.Item.sellPrice(copper: 60);
            Item.createTile = ModContent.TileType<Tiles.Furniture.CreamwoodBookcase>();
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(null, "CreamWood", 20).AddIngredient(149, 10).AddTile(TileID.Sawmill).Register();
        }
    }
}