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
using Terraria.ModLoader;
using TheConfectionRebirth.Backgrounds;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.ModSupport;

namespace TheConfectionRebirth
{
    public class TheConfectionRebirth : Mod
    {
        internal const int bgVarAmount = 3;

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
						ConfectionWorld.ConfectionSurfaceBG[1] = Main.rand.Next(bgVarAmount);

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
                        ConfectionWorld.ConfectionSurfaceBG[1] = Main.rand.Next(bgVarAmount);

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
                        ConfectionWorld.ConfectionSurfaceBG[1] = Main.rand.Next(bgVarAmount);

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
                            ConfectionWorld.ConfectionSurfaceBG[3] = Main.rand.Next(bgVarAmount);

                        Texture2D texture = (style as IBackground).GetUltraFarTexture(ConfectionWorld.ConfectionSurfaceBG[3]).Value;

                        Main.spriteBatch.Draw(texture,
							new Vector2((int)typeof(Main).GetField("bgStartX", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).GetValue(Main.instance) * 0.8f % Main.screenWidth
								+ (int)typeof(Main).GetField("bgWidthScaled", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).GetValue(null) * i,
								(int)typeof(Main).GetField("bgTopY", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).GetValue(Main.instance) - 240),
							new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)),
							(Color)typeof(Main).GetField("ColorOfSurfaceBackgroundsModified", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).GetValue(null), 0f, default(Vector2),
							(float)typeof(Main).GetField("bgScale", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).GetValue(null), 0, 0f);
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
						ConfectionWorld.ConfectionSurfaceBG[2] = Main.rand.Next(bgVarAmount);

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
                        ConfectionWorld.ConfectionSurfaceBG[2] = Main.rand.Next(bgVarAmount);

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
                        ConfectionWorld.ConfectionSurfaceBG[2] = Main.rand.Next(bgVarAmount);

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
						ConfectionWorld.ConfectionSurfaceBG[0] = Main.rand.Next(bgVarAmount);

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
                        ConfectionWorld.ConfectionSurfaceBG[0] = Main.rand.Next(bgVarAmount);

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
                        ConfectionWorld.ConfectionSurfaceBG[0] = Main.rand.Next(bgVarAmount);

                    return (style as IBackground).GetFarTexture(ConfectionWorld.ConfectionSurfaceBG[0]).Value.Height;
                }
                return v;
            });
        }
    }
}
