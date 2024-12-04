using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TheConfectionRebirth.Items.Placeable
{
	public class YumBlockWall : ModItem
	{
		public override void SetDefaults() {
			Item.width = 12;
			Item.height = 12;
			Item.maxStack = Item.CommonMaxStack;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 7;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createWall = ModContent.WallType<Walls.YumBlockWall>();
		}
	}
}