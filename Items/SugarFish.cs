using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items
{
    public class SugarFish : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }

        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 12;
            Item.value = 150000;
            Item.rare = ItemRarityID.LightRed;
            Item.maxStack = 9999;
        }

        public override void AddRecipes()
        {
            Recipe.Create(ItemID.TeleportationPotion)
                .AddIngredient(this, 1)
                .AddIngredient(ItemID.BottledWater, 1)
                .AddIngredient(ItemID.Blinkroot, 1)
                .AddIngredient(ItemID.Fireblossom, 1)
                .AddTile(TileID.AlchemyTable)
                .Register();
            Recipe.Create(ItemID.CookedFish)
                .AddIngredient(this, 2)
                .AddTile(TileID.CookingPots)
                .Register();
        }
    }
}