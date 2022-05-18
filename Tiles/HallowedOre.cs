using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Tiles
{
    public class HallowedOre : ModTile
    {
        public override void SetStaticDefaults()
        {
            TileID.Sets.Ore[Type] = true;
            Main.tileSpelunker[Type] = true;
            Main.tileOreFinderPriority[Type] = 680;
            Main.tileShine2[Type] = true;
            Main.tileShine[Type] = 975;
            Main.tileMergeDirt[Type] = true;
            Main.tileMerge[Type][Mod.Find<ModTile>("Creamstone").Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;

            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Hallowed Ore");
            AddMapEntry(new Color(180, 180, 204), name);

            DustType = 84;
            ItemDrop = ModContent.ItemType<Items.Placeable.HallowedOre>();
            SoundType = SoundID.Tink;
            SoundStyle = 1;
            MineResist = 4f;
            MinPick = 180;
        }
    }
}