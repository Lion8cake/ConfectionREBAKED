using CalamityMod;
using CalamityMod.NPCs;
using CalamityMod.Tiles.Ores;
using Microsoft.Xna.Framework;
using MonoMod.Cil;
using MonoMod.Utils;
using System;
using System.Diagnostics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Tiles;
using CalamityHallowedOreTile = CalamityMod.Tiles.Ores.HallowedOre;

namespace TheConfectionRebirth.ModSupport.Calamity;

[JITWhenModsEnabled(TheConfectionRebirth.CalamityModName)]
public sealed class HallowedOreTweaks : ModSystem {
	public override bool IsLoadingEnabled(Mod mod) {
		return TheConfectionRebirth.IsCalamityLoaded;
	}

	public override void OnModLoad() {
		var spawnHardmodeOresMethod = typeof(CalamityGlobalNPC).FindMethod("SpawnMechBossHardmodeOres");

		Debug.Assert(spawnHardmodeOresMethod != null);

		if (spawnHardmodeOresMethod == null)
			return;

		MonoModHooks.Modify(spawnHardmodeOresMethod, static (ILContext ctx) => {
			var c = new ILCursor(ctx);

			try {
				c.GotoNext(i => i.MatchLdstr("Mods.CalamityMod.Status.Progression.HardmodeOreTier4Text"));

				c.EmitNop();
				foreach (var incomingLabel in c.IncomingLabels) {
					incomingLabel.Target = c.Prev;
				}

				c.EmitDelegate(static () => {
					const string neapoliniteOreKey = "Mods.TheConfectionRebirth.Status.Progression.NeapoliniteOreText";
					const string drunkKey = "Mods.TheConfectionRebirth.Status.Progression.BothOresText";
					var messageColor = new Color(50, 255, 130);

					if (Main.drunkWorld || ConfectionModCalling.FargoBoBW) {
						CalamityUtils.SpawnOre(ModContent.TileType<CalamityHallowedOreTile>(),
							12E-05, 0.55f, 0.9f, 3, 8,
							TileID.Pearlstone, TileID.HallowHardenedSand, TileID.HallowSandstone, TileID.HallowedIce);

						CalamityUtils.SpawnOre(ModContent.TileType<NeapoliniteOre>(),
							12E-05, 0.55f, 0.9f, 3, 8,
							ModContent.TileType<Creamstone>(), ModContent.TileType<HardenedCreamsand>(), ModContent.TileType<Creamsandstone>(), ModContent.TileType<BlueIce>());

						CalamityUtils.DisplayLocalizedText(drunkKey, messageColor);
						return true;
					}
					else if (ConfectionWorldGeneration.confectionorHallow) {
						CalamityUtils.SpawnOre(ModContent.TileType<NeapoliniteOre>(),
							12E-05, 0.55f, 0.9f, 3, 8,
							ModContent.TileType<Creamstone>(), ModContent.TileType<HardenedCreamsand>(), ModContent.TileType<Creamsandstone>(), ModContent.TileType<BlueIce>());

						CalamityUtils.DisplayLocalizedText(neapoliniteOreKey, messageColor);
						return true;
					}

					return false;
				});
				c.EmitBrfalse(c.Next.Next);
				c.EmitRet();
			}
			catch (Exception) {
				ModContent.GetInstance<TheConfectionRebirth>().Logger.Error("Something broke in hallowed ore patch, blame lion8cake");
				MonoModHooks.DumpIL(ModContent.GetInstance<TheConfectionRebirth>(), ctx);
			}
		});
	}
}
