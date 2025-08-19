using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace TheConfectionRebirth.Items.Placeable
{
	[LegacyName("RoyalCherryBugBottleItem")]
	internal class RoyalCherryBugBottle : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.FireflyinaBottle);
            Item.createTile = ModContent.TileType<Tiles.RoyalCherryBugBottle>();
        }

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<Items.RoyalCherryBug>()
				.AddIngredient(ItemID.Bottle)
				.Register();
		}
	}
}
