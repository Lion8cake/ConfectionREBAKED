using Microsoft.Xna.Framework;
using System;
using System.Reflection;
using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace TheConfectionRebirth.Tiles.ExxoAvalonOrigins;

public class ConfectedAltar : ModTile
{
    public Mod avalon;

    public bool SuperHardmode
	{
        get
		{
            Type w = avalon.GetType().Assembly.GetType("AvalonTesting.AvalonTestingWorld");
            object m = typeof(ModContent).GetMethod(nameof(ModContent.GetInstance)).MakeGenericMethod(w).Invoke(null, Array.Empty<object>());
            return (bool)m.GetType().GetProperty("SuperHardmode", BindingFlags.Public | BindingFlags.Instance).GetMethod.Invoke(w, Array.Empty<object>());
		}
	}

	public override bool IsLoadingEnabled(Mod mod)
	{
		return ModLoader.TryGetMod("AvalonTesting", out avalon);
	}

	public override void SetStaticDefaults()
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

	public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
	{
		float brightness = Main.rand.Next(-5, 6) * 0.0025f;
		r = 1f + brightness;
		g = 0.9f + (brightness * 2f);
		b = 0f;
	}

	public override bool CanExplode(int i, int j)
    {
        return false;
	}

	public override bool CanKillTile(int i, int j, ref bool blockDamaged)
	{
		if (!SuperHardmode && !Main.hardMode)
		{
			blockDamaged = false;
		}

		return SuperHardmode && Main.hardMode;
	}

	public override void KillMultiTile(int i, int j, int frameX, int frameY)
	{
		if (SuperHardmode && Main.hardMode)
		{
			SmashHallowAltar(i, j);
		}
	}

	public override void NearbyEffects(int i, int j, bool closer)
	{
		if (Main.rand.Next(60) == 1)
		{
			int num162 = Dust.NewDust(new Vector2(i * 16, j * 16), 16, 16, DustID.HallowedWeapons, 0f, 0f, 0, default,
				1.5f);
			Main.dust[num162].noGravity = true;
			Main.dust[num162].velocity *= 1f;
		}
	}

	public static void SmashHallowAltar(int i, int j)
    {
        ModLoader.TryGetMod("AvalonTesting", out Mod mod);
        mod.GetType().Assembly.GetType("AvalonTesting.Tiles.HallowedAltar").GetMethod("SmashHallowAltar", BindingFlags.Public | BindingFlags.Static).Invoke(null, new object[] { i, j });
    }
}
