using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour.HookGen;
using ReLogic.Content;
using System;
using System.Reflection;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using TheConfectionRebirth.Backgrounds;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.ModSupport;

namespace TheConfectionRebirth
{
    public class TheConfectionRebirth : Mod
    {
        internal const int bgVarAmount = 4;

        private static MethodInfo Limits = null;

        private static event ILContext.Manipulator ModifyLimits
		{
			add => HookEndpointManager.Modify(Limits, value);
			remove => HookEndpointManager.Unmodify(Limits, value);
		}

		private static MethodInfo Limits2 = null;

        private static event ILContext.Manipulator ModifyLimits2
		{
			add => HookEndpointManager.Modify(Limits2, value);
			remove => HookEndpointManager.Unmodify(Limits2, value);
		}

		private static MethodInfo Limits3 = null;

        private static event ILContext.Manipulator ModifyLimits3
		{
			add => HookEndpointManager.Modify(Limits3, value);
			remove => HookEndpointManager.Unmodify(Limits3, value);
		}

		public override void Load()
        {
            if (ModLoader.TryGetMod("Wikithis", out Mod wikithis))
                wikithis.Call("AddModURL", this, "terrariamods.fandom.com$Confection_Rebaked");

            var UIMods = typeof(Main).Assembly.GetType("Terraria.ModLoader." + nameof(SurfaceBackgroundStylesLoader));
            Limits = UIMods.GetMethod(nameof(SurfaceBackgroundStylesLoader.DrawMiddleTexture));
            ModifyLimits += TheConfectionRebirth_ModifyLimits;
            Limits2 = UIMods.GetMethod(nameof(SurfaceBackgroundStylesLoader.DrawFarTexture));
            ModifyLimits2 += TheConfectionRebirth_ModifyLimits2;
            Limits3 = UIMods.GetMethod(nameof(SurfaceBackgroundStylesLoader.DrawCloseBackground));
            ModifyLimits3 += TheConfectionRebirth_ModifyLimits3;

            float[] _flashPower = (float[])typeof(BackgroundChangeFlashInfo).GetField("_flashPower", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(WorldGen.BackgroundsCache);
            int[] _variations = (int[])typeof(BackgroundChangeFlashInfo).GetField("_variations", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(WorldGen.BackgroundsCache);
            float[] newFlashPower = new float[_flashPower.Length + bgVarAmount];
            int[] newVariations = new int[_variations.Length + bgVarAmount];
            _cacheIndexes = new int[bgVarAmount];
            for (int i = 0; i < newFlashPower.Length; i++)
			{
                if (i >= _flashPower.Length)
				{
                    newFlashPower[i] = 0f;
                    newVariations[i] = 0;
					_cacheIndexes[i - _flashPower.Length] = i;
                    continue;
				}

                newFlashPower[i] = _flashPower[i];
                newVariations[i] = _variations[i];
			}
            typeof(BackgroundChangeFlashInfo).GetField("_flashPower", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(WorldGen.BackgroundsCache, newFlashPower);
            typeof(BackgroundChangeFlashInfo).GetField("_variations", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(WorldGen.BackgroundsCache, newVariations);

			On.Terraria.GameContent.BackgroundChangeFlashInfo.UpdateCache += BackgroundChangeFlashInfo_UpdateCache;
            On.Terraria.WorldGen.RandomizeBackgroundBasedOnPlayer += WorldGen_RandomizeBackgroundBasedOnPlayer;

			On.Terraria.Main.DrawSurfaceBG_BackMountainsStep1 += Main_DrawSurfaceBG_BackMountainsStep1;
        }

        private static double _backgroundTopMagicNumberCache;
        private static int _pushBGTopHackCache;
		private static void Main_DrawSurfaceBG_BackMountainsStep1(On.Terraria.Main.orig_DrawSurfaceBG_BackMountainsStep1 orig, Main self, double backgroundTopMagicNumber, float bgGlobalScaleMultiplier, int pushBGTopHack)
		{
            _backgroundTopMagicNumberCache = backgroundTopMagicNumber;
            _pushBGTopHackCache = pushBGTopHack;
            orig(self, backgroundTopMagicNumber, bgGlobalScaleMultiplier, pushBGTopHack);
		}

		private static int[] _cacheIndexes;
		private static void BackgroundChangeFlashInfo_UpdateCache(On.Terraria.GameContent.BackgroundChangeFlashInfo.orig_UpdateCache orig, BackgroundChangeFlashInfo self)
		{
            orig(self);

            for (int i = 0; i < bgVarAmount; i++)
                typeof(BackgroundChangeFlashInfo).GetMethod("UpdateVariation", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { _cacheIndexes[i], ConfectionWorld.ConfectionSurfaceBG[i] });
		}

		private void WorldGen_RandomizeBackgroundBasedOnPlayer(On.Terraria.WorldGen.orig_RandomizeBackgroundBasedOnPlayer orig, UnifiedRandom random, Player player)
		{
            if (ModContent.GetInstance<ConfectionBiomeTileCount>().confectionBlockCount >= 120)
			{
                int[] rand = new int[4] { Main.rand.Next(bgVarAmount), Main.rand.Next(bgVarAmount), Main.rand.Next(bgVarAmount), Main.rand.Next(bgVarAmount) };
                for (int i = 0; i < bgVarAmount; i++)
				{
                    while (ConfectionWorld.ConfectionSurfaceBG[i] == rand[i])
                    {
                        rand[i] = Main.rand.Next(bgVarAmount);
                        if (Main.rand.Next(1000000000 + Main.rand.Next(5002254)) < 7752 + Main.rand.Next(7752))
                            ConfectionWorld.Secret = true;
                    }

                    ConfectionWorld.ConfectionSurfaceBG[i] = rand[i];
                    typeof(BackgroundChangeFlashInfo).GetMethod("UpdateVariation", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(WorldGen.BackgroundsCache, new object[] { _cacheIndexes[i], rand[i] });
                }

                if (Main.netMode == NetmodeID.MultiplayerClient)
                    NetMessage.SendData(MessageID.WorldData);
            }

            orig(random, player);
		}

		public override void Unload()
        {
            if (Limits != null)
                ModifyLimits -= TheConfectionRebirth_ModifyLimits;
            Limits = null;
            if (Limits2 != null)
                ModifyLimits2 -= TheConfectionRebirth_ModifyLimits2;
            Limits2 = null;
            if (Limits3 != null)
                ModifyLimits3 -= TheConfectionRebirth_ModifyLimits3;
            Limits3 = null;

            _backgroundTopMagicNumberCache = 0;
            _pushBGTopHackCache = 0;
            _cacheIndexes = null;
        }

        public override void PostSetupContent()
        {
            SummonersShineThoughtBubble.PostSetupContent();
            StackableBuffData.PostSetupContent();
            ModSupport.ModSupportBaseClass.HookAll();
        }

        // middle
        private void TheConfectionRebirth_ModifyLimits(ILContext il)
        {
            ILCursor c = new(il);
            if (!c.TryGotoNext(i => i.MatchCallvirt(typeof(Asset<Texture2D>).GetMethod("get_Value"))))
                return;

            c.Index++;
            c.Emit(OpCodes.Ldloc, 1);
            c.EmitDelegate<Func<Texture2D, ModSurfaceBackgroundStyle, Texture2D>>((value, style) =>
            {
                if (style is IBackground)
                {
                    if (ConfectionWorld.ConfectionSurfaceBG[1] == -1)
                    {
                        ConfectionWorld.ConfectionSurfaceBG[1] = Main.rand.Next(bgVarAmount);
                        if (Main.rand.Next(1000000000 + Main.rand.Next(5002254)) < 7752 + Main.rand.Next(7752))
                            ConfectionWorld.Secret = true;
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                            NetMessage.SendData(MessageID.WorldData);
                    }

                    return (style as IBackground).GetMidTexture(ConfectionWorld.ConfectionSurfaceBG[1]).Value;
                }
                return value;
            });

            if (!c.TryGotoNext(MoveType.After,
                i => i.MatchLdsfld<Main>(nameof(Main.backgroundWidth)),
                i => i.MatchLdloc(4),
                i => i.MatchLdelemI4()))
                return;

            c.Emit(OpCodes.Ldloc, 1);
            c.EmitDelegate<Func<int, ModSurfaceBackgroundStyle, int>>((v, style) =>
            {
                if (style is IBackground)
                {
                    if (ConfectionWorld.ConfectionSurfaceBG[1] == -1)
                    {
                        ConfectionWorld.ConfectionSurfaceBG[1] = Main.rand.Next(bgVarAmount);
                        if (Main.rand.Next(1000000000 + Main.rand.Next(5002254)) < 7752 + Main.rand.Next(7752))
                            ConfectionWorld.Secret = true;
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                            NetMessage.SendData(MessageID.WorldData);
                    }

                    return (style as IBackground).GetFarTexture(ConfectionWorld.ConfectionSurfaceBG[1]).Value.Width;
                }
                return v;
            });

            if (!c.TryGotoNext(MoveType.After,
                i => i.MatchLdsfld<Main>(nameof(Main.backgroundHeight)),
                i => i.MatchLdloc(4),
                i => i.MatchLdelemI4()))
                return;

            c.Emit(OpCodes.Ldloc, 1);
            c.EmitDelegate<Func<int, ModSurfaceBackgroundStyle, int>>((v, style) =>
            {
                if (style is IBackground)
                {
                    if (ConfectionWorld.ConfectionSurfaceBG[1] == -1)
                    {
                        ConfectionWorld.ConfectionSurfaceBG[1] = Main.rand.Next(bgVarAmount);
                        if (Main.rand.Next(1000000000 + Main.rand.Next(5002254)) < 7752 + Main.rand.Next(7752))
                            ConfectionWorld.Secret = true;
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                            NetMessage.SendData(MessageID.WorldData);
                    }

                    return (style as IBackground).GetFarTexture(ConfectionWorld.ConfectionSurfaceBG[1]).Value.Height;
                }
                return v;
            });
        }

        // far and super far
        private void TheConfectionRebirth_ModifyLimits2(ILContext il)
        {
            ILCursor c = new(il);

            if (!c.TryGotoNext(i => i.MatchLdloc(3),
                i => i.MatchLdcR4(0)))
                return;

            c.Emit(OpCodes.Ldloc, 1);
            c.EmitDelegate<Action<ModSurfaceBackgroundStyle>>((style) =>
            {
                if (style == null || style is not IBackground)
                    return;

                int slot = style.Slot;
                float alpha = Main.bgAlphaFarBackLayer[slot];
				if (alpha > 0f)
				{
					for (int i = 0; i < (int)typeof(Main).GetField("bgLoops", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).GetValue(Main.instance); i++)
                    {
                        if (ConfectionWorld.ConfectionSurfaceBG[3] == -1)
                        {
                            ConfectionWorld.ConfectionSurfaceBG[3] = Main.rand.Next(bgVarAmount);
                            if (Main.rand.Next(1000000000 + Main.rand.Next(5002254)) < 7752 + Main.rand.Next(7752))
                                ConfectionWorld.Secret = true;
                            if (Main.netMode == NetmodeID.MultiplayerClient)
                                NetMessage.SendData(MessageID.WorldData);
                        }

                        float bgParallax = 0.1f;
                        Texture2D texture = (style as IBackground).GetUltraFarTexture(ConfectionWorld.ConfectionSurfaceBG[3]).Value;
                        if (texture is null)
                            return;

                        Color ColorOfSurfaceBackgroundsModified = (Color)typeof(Main).GetField("ColorOfSurfaceBackgroundsModified", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
                        float scAdj = (float)typeof(Main).GetField("scAdj", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(Main.instance);
                        int bgWidthScaled = (int)typeof(Main).GetField("bgWidthScaled", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
                        //int bgTopY = (int)typeof(Main).GetField("bgTopY", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).GetValue(Main.instance);
                        int bgTopY = !Main.gameMenu ? (int)(_backgroundTopMagicNumberCache * 1300.0 + 1005.0 + (int)scAdj + _pushBGTopHackCache + 40) : 75 + _pushBGTopHackCache;
                        
        float bgScale = (float)typeof(Main).GetField("bgScale", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
                        int bgStartX = (int)(0.0 - Math.IEEERemainder((double)Main.screenPosition.X * bgParallax, bgWidthScaled) - (bgWidthScaled / 2));

                        Main.spriteBatch.Draw(texture,
							new Vector2(bgStartX + bgWidthScaled * i, bgTopY - 20),
							new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)),
							ColorOfSurfaceBackgroundsModified, 0f, default,
							bgScale, 0, 0f);
                    }
                }
            });

            if (!c.TryGotoNext(i => i.MatchCallvirt(typeof(Asset<Texture2D>).GetMethod("get_Value"))))
                return;

            c.Index++;
            c.Emit(OpCodes.Ldloc, 1);
            c.EmitDelegate<Func<Texture2D, ModSurfaceBackgroundStyle, Texture2D>>((value, style) =>
            {
                if (style is IBackground)
                {
                    if (ConfectionWorld.ConfectionSurfaceBG[2] == -1)
                    {
                        ConfectionWorld.ConfectionSurfaceBG[2] = Main.rand.Next(bgVarAmount);
                        if (Main.rand.Next(1000000000 + Main.rand.Next(5002254)) < 7752 + Main.rand.Next(7752))
                            ConfectionWorld.Secret = true;
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                            NetMessage.SendData(MessageID.WorldData);
                    }

                    return (style as IBackground).GetFarTexture(ConfectionWorld.ConfectionSurfaceBG[2]).Value;
                }
                return value;
            });

            if (!c.TryGotoNext(MoveType.After,
                i => i.MatchLdsfld<Main>(nameof(Main.backgroundWidth)),
                i => i.MatchLdloc(4),
                i => i.MatchLdelemI4()))
                return;

            c.Emit(OpCodes.Ldloc, 1);
            c.EmitDelegate<Func<int, ModSurfaceBackgroundStyle, int>>((v, style) =>
            {
                if (style is IBackground)
                {
                    if (ConfectionWorld.ConfectionSurfaceBG[2] == -1)
                    {
                        ConfectionWorld.ConfectionSurfaceBG[2] = Main.rand.Next(bgVarAmount);
                        if (Main.rand.Next(1000000000 + Main.rand.Next(5002254)) < 7752 + Main.rand.Next(7752))
                            ConfectionWorld.Secret = true;
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                            NetMessage.SendData(MessageID.WorldData);
                    }

                    return (style as IBackground).GetFarTexture(ConfectionWorld.ConfectionSurfaceBG[2]).Value.Width;
                }
                return v;
            });

            if (!c.TryGotoNext(MoveType.After,
                i => i.MatchLdsfld<Main>(nameof(Main.backgroundHeight)),
                i => i.MatchLdloc(4),
                i => i.MatchLdelemI4()))
                return;

            c.Emit(OpCodes.Ldloc, 1);
            c.EmitDelegate<Func<int, ModSurfaceBackgroundStyle, int>>((v, style) =>
            {
                if (style is IBackground)
                {
                    if (ConfectionWorld.ConfectionSurfaceBG[2] == -1)
                    {
                        ConfectionWorld.ConfectionSurfaceBG[2] = Main.rand.Next(bgVarAmount);
                        if (Main.rand.Next(1000000000 + Main.rand.Next(5002254)) < 7752 + Main.rand.Next(7752))
                            ConfectionWorld.Secret = true;
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                            NetMessage.SendData(MessageID.WorldData);
                    }

                    return (style as IBackground).GetFarTexture(ConfectionWorld.ConfectionSurfaceBG[2]).Value.Height;
                }
                return v;
            });
        }

        // close
        private void TheConfectionRebirth_ModifyLimits3(ILContext il)
        {
            ILCursor c = new(il);
            if (!c.TryGotoNext(i => i.MatchCallvirt(typeof(Asset<Texture2D>).GetMethod("get_Value"))))
                return;

            c.Index++;
            c.Emit(OpCodes.Ldloc, 0);
            c.EmitDelegate<Func<Texture2D, ModSurfaceBackgroundStyle, Texture2D>>((value, style) =>
            {
                if (style is IBackground)
                {
                    if (ConfectionWorld.ConfectionSurfaceBG[0] == -1)
                    {
                        ConfectionWorld.ConfectionSurfaceBG[0] = Main.rand.Next(bgVarAmount);
                        if (Main.rand.Next(1000000000 + Main.rand.Next(5002254)) < 7752 + Main.rand.Next(7752))
                            ConfectionWorld.Secret = true;
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                            NetMessage.SendData(MessageID.WorldData);
                    }
                    return (style as IBackground).GetCloseTexture(ConfectionWorld.ConfectionSurfaceBG[0]).Value;
                }
                return value;
            });

            if (!c.TryGotoNext(MoveType.After,
                i => i.MatchLdsfld<Main>(nameof(Main.backgroundWidth)),
                i => i.MatchLdloc(3),
                i => i.MatchLdelemI4()))
                return;

            c.Emit(OpCodes.Ldloc, 0);
            c.EmitDelegate<Func<int, ModSurfaceBackgroundStyle, int>>((v, style) =>
            {
                if (style is IBackground)
                {
                    if (ConfectionWorld.ConfectionSurfaceBG[0] == -1)
                    {
                        ConfectionWorld.ConfectionSurfaceBG[0] = Main.rand.Next(bgVarAmount);
                        if (Main.rand.Next(1000000000 + Main.rand.Next(5002254)) < 7752 + Main.rand.Next(7752))
                            ConfectionWorld.Secret = true;
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                            NetMessage.SendData(MessageID.WorldData);
                    }
                    return (style as IBackground).GetFarTexture(ConfectionWorld.ConfectionSurfaceBG[0]).Value.Width;
                }
                return v;
            });

            if (!c.TryGotoNext(MoveType.After,
                i => i.MatchLdsfld<Main>(nameof(Main.backgroundHeight)),
                i => i.MatchLdloc(3),
                i => i.MatchLdelemI4()))
                return;

            c.Emit(OpCodes.Ldloc, 0);
            c.EmitDelegate<Func<int, ModSurfaceBackgroundStyle, int>>((v, style) =>
            {
                if (style is IBackground)
                {
                    if (ConfectionWorld.ConfectionSurfaceBG[0] == -1)
                    {
                        ConfectionWorld.ConfectionSurfaceBG[0] = Main.rand.Next(bgVarAmount);
                        if (Main.rand.Next(1000000000 + Main.rand.Next(5002254)) < 7752 + Main.rand.Next(7752))
                            ConfectionWorld.Secret = true;
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                            NetMessage.SendData(MessageID.WorldData);
                    }
                    Texture2D texture2 = TextureAssets.MagicPixel.Value;
                    Color color = Color.Black * WorldGen.BackgroundsCache.GetFlashPower(_cacheIndexes[0]);
                    Main.spriteBatch.Draw(texture2, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), color);

                    return (style as IBackground).GetFarTexture(ConfectionWorld.ConfectionSurfaceBG[0]).Value.Height;
                }
                return v;
            });
        }
    }
}
