using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Tiles;

namespace TheConfectionRebirth.Items.Placeable
{
    public class CreamBeans : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 9999;
            Item.useStyle = 1;
            Item.useAnimation = 15;
            Item.value = Item.buyPrice(silver: 20);
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.consumable = true;
        }

		public override bool? UseItem(Player player) { //I want to thank DylanDoe21 for this 
			Tile tile = Framing.GetTileSafely(Player.tileTargetX, Player.tileTargetY);

			if ((tile.HasTile && tile.TileType == ModContent.TileType<Tiles.CookieBlock>() || tile.HasTile && tile.TileType == TileID.Dirt) &&
			player.IsInTileInteractionRange(Player.tileTargetX, Player.tileTargetY, TileReachCheckSettings.Simple)) 
			{
				SoundEngine.PlaySound(SoundID.Dig, player.Center);

				Main.tile[Player.tileTargetX, Player.tileTargetY].TileType = (ushort)ModContent.TileType<CreamGrass>();

				return true;
			}

			return false;
		}
	}
}
