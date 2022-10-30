using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Placeable.Furniture
{
    public class CreamwoodWorkbench : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Creamwood Workbench");
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
            Item.useStyle = 1;
            Item.consumable = true;
            Item.value = Terraria.Item.sellPrice(copper: 30);
            Item.createTile = Mod.Find<ModTile>("CreamwoodWorkbench").Type;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<Items.Placeable.CreamWood>(), 10).Register();
        }
    }
}
