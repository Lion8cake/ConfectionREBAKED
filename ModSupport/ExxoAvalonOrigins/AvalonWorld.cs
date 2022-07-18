using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Chat;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace TheConfectionRebirth.ModSupport.ExxoAvalonOrigins;

public class AvalonWorld : ModSystem
{
    /*public static void GenerateHallowedOre()
    {
        Main.rand ??= new UnifiedRandom((int)DateTime.Now.Ticks);

        double num5 = Main.rockLayer;
        int xloc = -100 + Main.maxTilesX - 100;
        int yloc = -(int)num5 + Main.maxTilesY - 200;
        int sum = xloc * yloc;
        int amount = (sum / 10000) * 10;
        for (int zz = 0; zz < amount; zz++)
        {
            int i2 = WorldGen.genRand.Next(100, Main.maxTilesX - 100);
            double num6 = Main.rockLayer;
            int j2 = WorldGen.genRand.Next((int)num6, Main.maxTilesY - 200);
            WorldGen.OreRunner(i2, j2, WorldGen.genRand.Next(WorldGen.genRand.Next(2, 4), WorldGen.genRand.Next(4, 6)), WorldGen.genRand.Next(WorldGen.genRand.Next(3, 5), WorldGen.genRand.Next(4, 8)), (ushort)ModContent.TileType<Tiles.Ores.HallowedOre>());
        }
        if (Main.netMode == NetmodeID.SinglePlayer)
        {
            Main.NewText("Your world has been blessed with Hallowed Ore!", 220, 170, 0);
        }
        else if (Main.netMode == NetmodeID.Server)
        {
            ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("Your world has been blessed with Hallowed Ore!"), new Color(220, 170, 0));
        }
    }*/

    public override void ModifyHardmodeTasks(List<GenPass> list)
    {
        int index = list.FindIndex(genpass => genpass.Name.Equals("Hardmode Good"));
        list.Insert(index + 1, new PassLegacy("Confection REBAKED: Hardmode Good (Confected Altars)", new WorldGenLegacyMethod(World.ConfectedAltars.Method)));
    }
}
