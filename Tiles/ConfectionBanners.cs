using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace TheConfectionRebirth.Tiles
{
    public class ConfectionBanners : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.SwaysInWindBasic[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.StyleWrapLimit = 111;
            TileObjectData.addTile(Type);
            DustType = -1;
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Banner");
            AddMapEntry(new Color(13, 88, 130), name);
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            int style = frameX / 18;
            string item;
            switch (style)
            {
                case 0:
                    item = "RollerCookieBanner";
                    break;
                case 1:
                    item = "SprinklingBanner";
                    break;
                case 2:
                    item = "ParfaitSlimeBanner";
                    break;
                case 3:
                    item = "SherbetSlimeBanner";
                    break;
                case 4:
                    item = "WildWillyBanner";
                    break;
                case 5:
                    item = "HungerBanner";
                    break;
                case 6:
                    item = "SweetGummyBanner";
                    break;
                case 7:
                    item = "CrazyConeBanner";
                    break;
                case 8:
                    item = "MeetyMummyBanner";
                    break;
                case 9:
                    item = "MintJrBanner";
                    break;
                case 10:
                    item = "BirdnanaBanner";
                    break;
                case 11:
                    item = "ChocolateBunnyBanner";
                    break;
                case 12:
                    item = "ChocolateFrogBanner";
                    break;
                case 13:
                    item = "PipBanner";
                    break;
                case 14:
                    item = "CherryBugBanner";
                    break;
                case 15:
                    item = "StripedPigronBanner";
                    break;
                case 16:
                    item = "FoaminFloatBanner";
                    break;
                case 17:
                    item = "CrookedCookieBanner";
                    break;
                case 18:
                    item = "IscreamerBanner";
                    break;
                case 19:
                    item = "CreamsandWitchBanner";
                    break;
                case 20:
                    item = "TheUnfirmBanner";
                    break;
                case 21:
                    item = "GummyWyrmBanner";
                    break;
                case 22:
                    item = "CreamSwollowerBanner";
                    break;
                case 23:
                    item = "IcecreamGalBanner";
                    break;
                case 24:
                    item = "MeowzerBanner";
                    break;
                default:
                    return;

            }
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 32, Mod.Find<ModItem>(item).Type);
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (closer)
            {
                Player player = Main.LocalPlayer;
                int style = Main.tile[i, j].TileFrameX / 18;
                switch (style)
                {
                    case 0:
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                    case 5:
                        break;
                    case 6:
                        break;
                    case 7:
                        break;
                    case 8:
                        break;
                    case 9:
                        break;
                    case 10:
                        break;
                    case 11:
                        break;
                    case 12:
                        break;
                    case 13:
                        break;
                    case 14:
                        break;
                    case 15:
                        break;
                    case 16:
                        break;
                    case 17:
                        break;
                    case 18:
                        break;
                    case 19:
                        break;
                    case 20:
                        break;
                    case 21:
                        break;
                    case 22:
                        break;
                    case 23:
                        break;
                    case 24:
                        break;
                    default:
                        return;
                }
                /*Player.HasNPCBannerBuff[Mod.Find<ModNPC>(type).Type] = true;
				Player.NPCBanner = true;*/
            }
        }

        public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
        {
            if (i % 2 == 1)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
        }
    }
}
