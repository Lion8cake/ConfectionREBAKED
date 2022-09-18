using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Walls.GraveyardWalls
{
    public class CreamsandstoneWallArtificial : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            AddMapEntry(new Color(54, 30, 24));
            ItemDrop = ModContent.ItemType<Items.Placeable.CreamsandstoneWall>();
        }
    }
}