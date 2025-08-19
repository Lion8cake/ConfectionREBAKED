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
            Main.tileSolid[Type] = false;
            Main.tileLighted[Type] = true;
            TheConfectionRebirth.tileMerge[Type, ModContent.TileType<Creamstone>()] = true;
			TheConfectionRebirth.tileMerge[Type, ModContent.TileType<SacchariteBlock>()] = true;
            DustType = ModContent.DustType<SacchariteCrystals>();
            HitSound = SoundID.Item27;
			RegisterItemDrop(ModContent.ItemType<Items.EnchantedSaccharite>());
			Main.tileOreFinderPriority[Type] = 675;
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(181, 196, 240), name);
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b) {
			r = (float)(0.181f);
			g = (float)(0.196f);
			b = (float)(0.24f);
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
}