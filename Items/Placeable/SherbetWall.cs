using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace TheConfectionRebirth.Items.Placeable
{
    public class SherbetWall : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 400;
		}

        public override void SetDefaults()
        {
            Item.createWall = ModContent.WallType<Walls.SherbetWall>();
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 7;
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