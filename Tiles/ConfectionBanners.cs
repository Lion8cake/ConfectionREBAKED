using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
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
            LocalizedText name = CreateMapEntryName();
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
                case 25:
                    item = "Dudley";
                    break;
                case 26:
                    item = "Prickster";
                    break;
                case 27:
                    item = "CherryBomber";
                    break;
                case 28:
                    item = "ConfectionMimic";
                    break;
                default:
                    return;

            }
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (closer)
            {
                Player player = Main.LocalPlayer;
                int style = Main.tile[i, j].TileFrameX / 18;
                string type;
                switch (style)
                {
                    case 0:
                        type = "Rollercookie";
                        break;
                    case 1:
                        type = "Sprinkling";
                        break;
                    case 2:
                        type = "ParfaitSlime";
                        break;
                    case 3:
                        type = "SherbetSlime";
                        break;
                    case 4:
                        type = "WildWilly";
                        break;
                    case 5:
                        type = "Hunger";
                        break;
                    case 6:
                        type = "SweetGummy";
                        break;
                    case 7:
                        type = "CrazyCone";
                        break;
                    case 9:
                        type = "MintJr";
                        break;
                    case 10:
                        type = "Birdnana";
                        break;
                    case 11:
                        type = "ChocolateBunny";
                        break;
                    case 12:
                        type = "ChocolateFrog";
                        break;
                    case 13:
                        type = "Pip";
                        break;
                    case 14:
                        type = "CherryBug";
                        break;
                    case 15:
                        type = "StripedPigron";
                        break;
                    case 16:
                        type = "FoaminFloat";
                        break;
                    case 17:
                        type = "CrookedCookie";
                        break;
                    case 18:
                        type = "Iscreamer";
                        break;
                    case 19:
                        type = "CreamsandWitchPhase2";
                        break;
                    case 20:
                        type = "TheUnfirm";
                        break;
                    case 21:
                        type = "GummyWyrm";
                        break;
                    case 22:
                        type = "CreamSwollower";
                        break;
                    case 23:
                        type = "IcecreamGal";
                        break;
                    case 24:
                        type = "Meowzer";
                        break;
                    case 25:
                        type = "Dudley";
                        break;
                    case 26:
                        type = "Prickster";
                        break;
                    //case 27:
                     //   type = "CherryBomber";
                     //   break;
                    case 28:
                        type = "BigMimicConfection";
                        break;
                    default:
                        return;
                }
                Main.SceneMetrics.NPCBannerBuff[Mod.Find<ModNPC>(type).Type] = true;
                Main.SceneMetrics.hasBanner = true;
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
