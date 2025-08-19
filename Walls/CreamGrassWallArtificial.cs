using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Walls
{
    public class CreamGrassWallArtificial : ModWall
    {
        public override void SetStaticDefaults()
        {
			WallID.Sets.Conversion.Grass[Type] = true;
			Main.wallHouse[Type] = true;
            DustType = ModContent.DustType<CreamGrassDust>();
            AddMapEntry(new Color(100, 85, 54));
			RegisterItemDrop(ModContent.ItemType<Items.Placeable.CreamgrassWall>());
        }
    }
}