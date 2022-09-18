using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace TheConfectionRebirth.Items.Placeable
{
    public class LinedConfectionGemWall : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 400;
        }

        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 12;
            Item.maxStack = 999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 7;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createWall = ModContent.WallType<Walls.GraveyardWalls.LinedConfectionGemWall>();
        }

        public override void AddRecipes()
        {
            CreateRecipe(4).AddIngredient(ModContent.ItemType<Creamstone>()).AddCondition(Recipe.Condition.InGraveyardBiome).AddTile(TileID.WorkBenches).Register();
        }
    }
}