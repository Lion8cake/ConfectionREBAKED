using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace TheConfectionRebirth.Items.Placeable
{
    public class SherbetBricks : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
		}

        public override void SetDefaults()
        {
            Item.createTile = ModContent.TileType<Tiles.SherbetBricks>();
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
			Item.width = 12;
			Item.height = 12;
			Item.rare = ItemRarityID.Blue;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color((byte)TheConfectionRebirth.SherbR, (byte)TheConfectionRebirth.SherbG, (byte)TheConfectionRebirth.SherbB, byte.MaxValue);
		}
	}
}
