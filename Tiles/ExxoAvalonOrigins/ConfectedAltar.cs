using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace TheConfectionRebirth.Tiles.ExxoAvalonOrigins;

public class ConfectedAltar : ModTile
{
    public override void SetStaticDefaults()
    {
        if (ModLoader.TryGetMod("AvalonTesting", out Mod mod))
        {
            AddMapEntry(new Color(255, 216, 0), LanguageManager.Instance.GetText("Confected Altar"));
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
            TileObjectData.addTile(Type);
            Main.tileHammer[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            DustType = DustID.HallowedWeapons;
            TileID.Sets.PreventsTileRemovalIfOnTopOfIt[Type] = true;
            TileID.Sets.InteractibleByNPCs[Type] = true;
        }
    }

    public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
    {
        if (ModLoader.TryGetMod("AvalonTesting", out Mod mod))
        {
            float brightness = Main.rand.Next(-5, 6) * 0.0025f;
            r = 1f + brightness;
            g = 0.9f + (brightness * 2f);
            b = 0f;
        }
    }

    public override bool CanExplode(int i, int j)
    {
        return false;
    }

    public override bool CanKillTile(int i, int j, ref bool blockDamaged)
    {
        if (ModLoader.TryGetMod("AvalonTesting", out Mod mod))
        {
            if (!ModContent.GetInstance<AvalonTesting.AvalonTestingWorld>().SuperHardmode && !Main.hardMode)
            {
                blockDamaged = false;
            }

            return ModContent.GetInstance<AvalonTesting.AvalonTestingWorld>().SuperHardmode && Main.hardMode;
        }
        return false;
    }

    public override void KillMultiTile(int i, int j, int frameX, int frameY)
    {
        if (ModLoader.TryGetMod("AvalonTesting", out Mod mod))
        {
            if (ModContent.GetInstance<AvalonTesting.AvalonTestingWorld>().SuperHardmode && Main.hardMode)
            {
                SmashHallowAltar(i, j);
            }
        }
    }

    public override void NearbyEffects(int i, int j, bool closer)
    {
        if (ModLoader.TryGetMod("AvalonTesting", out Mod mod))
        {
            if (Main.rand.Next(60) == 1)
            {
                int num162 = Dust.NewDust(new Vector2(i * 16, j * 16), 16, 16, DustID.HallowedWeapons, 0f, 0f, 0, default,
                    1.5f);
                Main.dust[num162].noGravity = true;
                Main.dust[num162].velocity *= 1f;
            }
        }
    }

    public static void SmashHallowAltar(int i, int j)
    {
        if (ModLoader.TryGetMod("AvalonTesting", out Mod mod))
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                return;
            }

            if (!ModContent.GetInstance<AvalonTesting.AvalonTestingWorld>().SuperHardmode && !Main.hardMode)
            {
                return;
            }

            if (WorldGen.noTileActions)
            {
                return;
            }

            if (WorldGen.gen)
            {
                return;
            }

            int num = ModContent.GetInstance<AvalonTesting.Systems.ExxoWorldGen>().HallowedAltarCount % 2;
            int num2 = (ModContent.GetInstance<AvalonTesting.Systems.ExxoWorldGen>().HallowedAltarCount / 2) + 1;
            float num3 = Main.maxTilesX / 4200;
            int num4 = 1 - num;
            num3 = (num3 * 310f) - (85 * num);
            num3 *= 0.85f;
            num3 /= num2;
            if (num == 0)
            {
                if (Main.netMode == NetmodeID.SinglePlayer)
                {
                    if (ModContent.GetInstance<AvalonTesting.Systems.ExxoWorldGen>().SHMTier1Ore == AvalonTesting.Systems.ExxoWorldGen.SHMTier1Variant.Tritanorium)
                    {
                        Main.NewText("Your world has been invigorated with Tritanorium!", 117, 158, 107);
                    }
                    else
                    {
                        Main.NewText("Your world has been melted with Pyroscoric!", 187, 35, 0);
                    }
                }
                else if (Main.netMode == NetmodeID.Server)
                {
                    if (ModContent.GetInstance<AvalonTesting.Systems.ExxoWorldGen>().SHMTier1Ore == AvalonTesting.Systems.ExxoWorldGen.SHMTier1Variant.Tritanorium)
                    {
                        ChatHelper.BroadcastChatMessage(
                            NetworkText.FromLiteral("Your world has been invigorated with Tritanorium!"),
                            new Color(117, 158, 107));
                    }
                    else
                    {
                        ChatHelper.BroadcastChatMessage(
                            NetworkText.FromLiteral("Your world has been melted with Pyroscoric!"), new Color(187, 35, 0));
                    }
                }

                //num = ModContent.GetInstance<AvalonTesting.Systems.ExxoWorldGen>().SHMTier1Ore.GetSHMTier1VariantTileOre();
                num3 *= 1.05f;
            }
            else if (num == 1)
            {
                if (Main.netMode == NetmodeID.SinglePlayer)
                {
                    if (ModContent.GetInstance<AvalonTesting.Systems.ExxoWorldGen>().SHMTier2Ore == AvalonTesting.Systems.ExxoWorldGen.SHMTier2Variant.Unvolandite)
                    {
                        Main.NewText("Your world has been blessed with Unvolandite!", 171, 119, 75);
                    }
                    else
                    {
                        Main.NewText("Your world has been blessed with Vorazylcum!", 123, 95, 126);
                    }
                }
                else if (Main.netMode == NetmodeID.Server)
                {
                    if (ModContent.GetInstance<AvalonTesting.Systems.ExxoWorldGen>().SHMTier2Ore == AvalonTesting.Systems.ExxoWorldGen.SHMTier2Variant.Unvolandite)
                    {
                        ChatHelper.BroadcastChatMessage(
                            NetworkText.FromLiteral("Your world has been blessed with Unvolandite!"),
                            new Color(171, 119, 75));
                    }
                    else
                    {
                        ChatHelper.BroadcastChatMessage(
                            NetworkText.FromLiteral("Your world has been blessed with Vorazylcum!"),
                            new Color(123, 95, 126));
                    }
                }

                //num = ModContent.GetInstance<AvalonTesting.Systems.ExxoWorldGen>().SHMTier2Ore.GetSHMTier2VariantTileOre();
            }

            int num11 = 0;
            while (num11 < num3)
            {
                int i2 = WorldGen.genRand.Next(100, Main.maxTilesX - 100);
                double num12 = Main.worldSurface;
                if (num == 1)
                {
                    num12 = Main.rockLayer;
                }

                if (num == 2 || num == 3)
                {
                    num12 = (Main.rockLayer + Main.rockLayer + Main.maxTilesY) / 3.0;
                }

                int j2 = WorldGen.genRand.Next((int)num12, Main.maxTilesY - 150);
                switch (num)
                {
                    case 0:
                        num = ModContent.TileType<AvalonTesting.Tiles.Ores.TritanoriumOre>();
                        break;
                    case 1:
                        num = ModContent.TileType<AvalonTesting.Tiles.Ores.PyroscoricOre>();
                        break;
                    case 2:
                        num = ModContent.TileType<AvalonTesting.Tiles.Ores.UnvolanditeOre>();
                        break;
                    case 3:
                        num = ModContent.TileType<AvalonTesting.Tiles.Ores.VorazylcumOre>();
                        break;
                }

                WorldGen.OreRunner(i2, j2, WorldGen.genRand.Next(5, 9 + num4), WorldGen.genRand.Next(5, 9 + num4),
                    (ushort)num);
                num11++;
            }

            ModContent.GetInstance<AvalonTesting.Systems.ExxoWorldGen>().HallowedAltarCount++;
        }
    }
}
