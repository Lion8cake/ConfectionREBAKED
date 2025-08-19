using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace TheConfectionRebirth.Items.Placeable
{
	[LegacyName("SoulofSpiteinaBottleItem")]
	internal class SoulofSpiteinaBottle : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.SoulBottleNight);
			Item.placeStyle = 0;
            Item.createTile = ModContent.TileType<Tiles.SoulofSpiteinaBottle>();
        }

		public override void AddRecipes() {
			CreateRecipe(1)
				.AddIngredient(ModContent.ItemType<Items.SoulofSpite>())
				.AddIngredient(ItemID.Bottle)
				.Register();
		}
	}
}
