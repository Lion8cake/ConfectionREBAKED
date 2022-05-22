using TheConfectionRebirth.Tiles;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;

namespace TheConfectionRebirth.Items.Placeable
{
	public class ConfectionUGMusicBox : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Music Box (Underground Confection)");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			
			MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Sounds/Music/ConfectionUnderground"), ModContent.ItemType<Items.Placeable.ConfectionUGMusicBox>(), ModContent.TileType<Tiles.ConfectionUGMusicBox>());
		}

		public override void SetDefaults() {
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.ConfectionUGMusicBox>();
			Item.width = 24;
			Item.height = 24;
			Item.rare = ItemRarityID.LightRed;
			Item.value = 100000;
			Item.accessory = true;
		}
	}
}
