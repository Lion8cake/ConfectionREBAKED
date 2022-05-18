using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items
{
    public class Sprinkles : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
        }

        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 12;
            Item.value = 500;
            Item.rare = 1;
            Item.maxStack = 99;
        }

        public override void AddRecipes()
        {
            CreateRecipe(3).AddIngredient(this, 3).AddIngredient(ModContent.ItemType<Items.Placeable.Saccharite>(), 1).AddIngredient(ItemID.BottledWater, 3).AddTile(TileID.AlchemyTable).ReplaceResult(ItemID.GreaterHealingPotion);
        }
    }
}