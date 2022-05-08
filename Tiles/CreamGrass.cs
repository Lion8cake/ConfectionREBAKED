using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Tiles
{
    public class CreamGrass : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
			this.SetModTree(new Trees.CreamTree());
            Main.tileMerge[Type][Mod.Find<ModTile>("CreamGrass").Type] = true;
			Main.tileMerge[Type][Mod.Find<ModTile>("CookieBlock").Type] = true;
            Main.tileBlendAll[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBrick[base.Type] = true;
		    Main.tileSolid[base.Type] = true;
		    Main.tileBlockLight[base.Type] = true;
		    TileID.Sets.Grass[base.Type] = true;
		    TileID.Sets.ChecksForMerge[base.Type] = true;
            AddMapEntry(new Color(235, 207, 150));
            SoundType = 0;
            SoundStyle = 2;
            ItemDrop = Mod.Find<ModItem>("CookieBlock").Type;
        }
        /*public static bool PlaceObject(int x, int y, int type, bool mute = false, int style = 0, int alternate = 0, int random = -1, int direction = -1)
        {
            TileObject objectData;
            if (!TileObject.CanPlace(x, y, type, style, direction, out objectData, false, false))
                return false;
            objectData.random = random;
            if (TileObject.Place(objectData) && !mute)
                WorldGen.SquareTileFrame(x, y, true);
            return false;
        }
        public override void RandomUpdate(int i, int j)
        {
            if (Framing.GetTileSafely(i, j - 1).active())
                return;
            switch (Main.rand.Next(14))
            {
                case 0:
                    PlaceObject(i, j - 1, this.mod.Find<ModTile>("CreamGrass1").Type, false, 0, 0, -1, -1);
                    NetMessage.SendObjectPlacment(-1, i, j - 1, this.mod.Find<ModTile>("CreamGrass1").Type, 0, 0, -1, -1);
                    break;
                case 1:
                    PlaceObject(i, j - 1, this.mod.Find<ModTile>("CreamGrass2").Type, false, 0, 0, -1, -1);
                    NetMessage.SendObjectPlacment(-1, i, j - 1, this.mod.Find<ModTile>("CreamGrass2").Type, 0, 0, -1, -1);
                    break;
                case 2:
                    PlaceObject(i, j - 1, this.mod.Find<ModTile>("CreamGrass3").Type, false, 0, 0, -1, -1);
                    NetMessage.SendObjectPlacment(-1, i, j - 1, this.mod.Find<ModTile>("CreamGrass3").Type, 0, 0, -1, -1);
                    break;
                case 3:
                    PlaceObject(i, j - 1, this.mod.Find<ModTile>("CreamGrass4").Type, false, 0, 0, -1, -1);
                    NetMessage.SendObjectPlacment(-1, i, j - 1, this.mod.Find<ModTile>("CreamGrass4").Type, 0, 0, -1, -1);
                    break;
				case 4:
                    PlaceObject(i, j - 1, this.mod.Find<ModTile>("CreamGrass5").Type, false, 0, 0, -1, -1);
                    NetMessage.SendObjectPlacment(-1, i, j - 1, this.mod.Find<ModTile>("CreamGrass5").Type, 0, 0, -1, -1);
                    break;
				case 5:
                    PlaceObject(i, j - 1, this.mod.Find<ModTile>("CreamGrass6").Type, false, 0, 0, -1, -1);
                    NetMessage.SendObjectPlacment(-1, i, j - 1, this.mod.Find<ModTile>("CreamGrass6").Type, 0, 0, -1, -1);
                    break;
				case 6:
                    PlaceObject(i, j - 1, this.mod.Find<ModTile>("CreamGrass7").Type, false, 0, 0, -1, -1);
                    NetMessage.SendObjectPlacment(-1, i, j - 1, this.mod.Find<ModTile>("CreamGrass7").Type, 0, 0, -1, -1);
                    break;
				case 7:
                    PlaceObject(i, j - 1, this.mod.Find<ModTile>("CreamGrass8").Type, false, 0, 0, -1, -1);
                    NetMessage.SendObjectPlacment(-1, i, j - 1, this.mod.Find<ModTile>("CreamGrass8").Type, 0, 0, -1, -1);
                    break;
				case 8:
                    PlaceObject(i, j - 1, this.mod.Find<ModTile>("CreamGrass9").Type, false, 0, 0, -1, -1);
                    NetMessage.SendObjectPlacment(-1, i, j - 1, this.mod.Find<ModTile>("CreamGrass9").Type, 0, 0, -1, -1);
                    break;
				case 9:
                    PlaceObject(i, j - 1, this.mod.Find<ModTile>("CreamGrass10").Type, false, 0, 0, -1, -1);
                    NetMessage.SendObjectPlacment(-1, i, j - 1, this.mod.Find<ModTile>("CreamGrass10").Type, 0, 0, -1, -1);
                    break;
				case 10:
                    PlaceObject(i, j - 1, this.mod.Find<ModTile>("CreamGrass11").Type, false, 0, 0, -1, -1);
                    NetMessage.SendObjectPlacment(-1, i, j - 1, this.mod.Find<ModTile>("CreamGrass11").Type, 0, 0, -1, -1);
                    break;
				case 11:
                    PlaceObject(i, j - 1, this.mod.Find<ModTile>("CreamGrass12").Type, false, 0, 0, -1, -1);
                    NetMessage.SendObjectPlacment(-1, i, j - 1, this.mod.Find<ModTile>("CreamGrass12").Type, 0, 0, -1, -1);
                    break;
                default:
                    PlaceObject(i, j - 1, this.mod.Find<ModTile>("YumDrop").Type, false, 0, 0, -1, -1);
                    NetMessage.SendObjectPlacment(-1, i, j - 1, this.mod.Find<ModTile>("YumDrop").Type, 0, 0, -1, -1);
                    break;
            }
        }*/
		
		/*public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			if (fail && !effectOnly)
			{
				Main.tile[i, j].type = (ushort)ModContent.TileType<CookieBlock>();
			}
		}*/
    }
}