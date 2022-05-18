using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items
{
    public class CookieCarp : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }

        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 12;
            Item.value = 12500;
            Item.rare = ItemRarityID.Green;
            Item.maxStack = 999;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(this, 1).AddIngredient(ItemID.BottledWater, 1).AddIngredient(ItemID.Shiverthorn, 1).AddTile(TileID.AlchemyTable).ReplaceResult(ItemID.LovePotion);
            CreateRecipe(1).AddIngredient(this, 2).AddTile(96).ReplaceResult(ItemID.CookedFish);
        }
    }
}