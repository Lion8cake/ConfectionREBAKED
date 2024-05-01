using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.Enums;

namespace TheConfectionRebirth.Items.Placeable {
	[LegacyName("SoulofDelightinaBottleItem")]
	internal class SoulofDelightinaBottle : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.SoulBottleLight);
            Item.createTile = ModContent.TileType<Tiles.SoulofDelightinaBottle>();
        }

		public override void AddRecipes() {
			CreateRecipe(1)
				.AddIngredient(ModContent.ItemType<Items.SoulofDelight>())
				.AddIngredient(ItemID.Bottle)
				.Register();
		}
	}
}
