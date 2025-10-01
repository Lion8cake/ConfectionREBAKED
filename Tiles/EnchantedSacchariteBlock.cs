using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using TheConfectionRebirth.Dusts;
using Terraria.Localization;

namespace TheConfectionRebirth.Tiles
{
    public class EnchantedSacchariteBlock : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileLighted[Type] = true;
			Main.tileSolid[Type] = false;
			Main.tileShine2[Type] = true;
			Main.tileShine[Type] = 4500;
			Main.tileOreFinderPriority[Type] = 675;

			TileID.Sets.ChecksForMerge[Type] = true;

			Main.tileMerge[Type][ModContent.TileType<Creamstone>()] = true;
			Main.tileMerge[Type][ModContent.TileType<SacchariteBlock>()] = true;

			DustType = ModContent.DustType<SacchariteDust>();
			HitSound = SoundID.Item27;
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(181, 196, 240), name);
			RegisterItemDrop(ModContent.ItemType<Items.EnchantedSaccharite>());
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b) {
			r = 0.181f;
			g = 0.196f;
			b = 0.24f;
		}
    }
}