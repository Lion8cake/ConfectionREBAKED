using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour.HookGen;
using ReLogic.Content;
using System;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;
using TheConfectionRebirth.ModSupport;

namespace TheConfectionRebirth
{
    public class TheConfectionRebirth : Mod
    {
        private static MethodInfo Limits = null;

        private static event ILContext.Manipulator ModifyLimits
        {
            add
            {
                HookEndpointManager.Modify(Limits, value);
            }
            remove
            {
                HookEndpointManager.Unmodify(Limits, value);
            }
        }

        private static MethodInfo Limits2 = null;

        private static event ILContext.Manipulator ModifyLimits2
        {
            add
            {
                HookEndpointManager.Modify(Limits2, value);
            }
            remove
            {
                HookEndpointManager.Unmodify(Limits2, value);
            }
        }

        private static MethodInfo Limits3 = null;

        private static event ILContext.Manipulator ModifyLimits3
        {
            add
            {
                HookEndpointManager.Modify(Limits3, value);
            }
            remove
            {
                HookEndpointManager.Unmodify(Limits3, value);
            }
        }

        public override void Load()
        {
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
            ModifyLimits -= TheConfectionRebirth_ModifyLimits;
            Limits = null;
            ModifyLimits2 -= TheConfectionRebirth_ModifyLimits2;
            Limits2 = null;
            ModifyLimits3 -= TheConfectionRebirth_ModifyLimits3;
            Limits3 = null;
        }

        public override void PostSetupContent()
        {
            SummonersShineThoughtBubble.PostSetupContent();
        }

        // middle
        private void TheConfectionRebirth_ModifyLimits(ILContext il)
        {
            ILCursor c = new(il);
            if (!c.TryGotoNext(i => i.MatchCallvirt(typeof(Asset<Texture2D>).GetMethod("get_Value"))))
                return;

            c.Index++;
            c.Emit(OpCodes.Ldloc, 4);
            c.EmitDelegate<Func<Texture2D, int, Texture2D>>((value, textureSlot) =>
            {
                if (textureSlot == ModContent.Find<ModSurfaceBackgroundStyle>("TheConfectionRebirth/ConfectionSurfaceBackgroundStyle").ChooseMiddleTexture())
                {
                    if (ConfectionWorld.ConfectionSurfaceBG == 0)
                    {
                        return ModContent.Request<Texture2D>("TheConfectionRebirth/Backgrounds/ConfectionSurfaceMid").Value;
                    }
                    else if (ConfectionWorld.ConfectionSurfaceBG == 1)
                    {
                        return ModContent.Request<Texture2D>("TheConfectionRebirth/Backgrounds/ConfectionSurface1Mid").Value;
                    }
                    else if (ConfectionWorld.ConfectionSurfaceBG == 2)
                    {
                        return ModContent.Request<Texture2D>("TheConfectionRebirth/Backgrounds/ConfectionSurface2Mid").Value;
                    }
                }
                return value;
            });
        }

        // far
        private void TheConfectionRebirth_ModifyLimits2(ILContext il)
        {
            ILCursor c = new(il);
            if (!c.TryGotoNext(i => i.MatchCallvirt(typeof(Asset<Texture2D>).GetMethod("get_Value"))))
                return;

            c.Index++;
            c.Emit(OpCodes.Ldloc, 4);
            c.EmitDelegate<Func<Texture2D, int, Texture2D>>((value, textureSlot) =>
            {
                if (textureSlot == ModContent.Find<ModSurfaceBackgroundStyle>("TheConfectionRebirth/ConfectionSurfaceBackgroundStyle").ChooseFarTexture())
                {
                    if (ConfectionWorld.ConfectionSurfaceBG == 0)
                    {
                        return ModContent.Request<Texture2D>("TheConfectionRebirth/Backgrounds/ConfectionSurfaceFar").Value;
                    }
                    if (ConfectionWorld.ConfectionSurfaceBG == 1)
                    {
                        return ModContent.Request<Texture2D>("TheConfectionRebirth/Backgrounds/ConfectionSurface1Far").Value;
                    }
                    if (ConfectionWorld.ConfectionSurfaceBG == 2)
                    {
                        return ModContent.Request<Texture2D>("TheConfectionRebirth/Backgrounds/ConfectionSurface2Far").Value;
                    }
                }
                return value;
            });
        }

        // close
        private void TheConfectionRebirth_ModifyLimits3(ILContext il)
        {
            ILCursor c = new(il);
            if (!c.TryGotoNext(i => i.MatchCallvirt(typeof(Asset<Texture2D>).GetMethod("get_Value"))))
                return;

            c.Index++;
            c.Emit(OpCodes.Ldloc, 4);
            c.Emit(OpCodes.Ldloc, 1);
            c.Emit(OpCodes.Ldloc, 2);
            c.Emit(OpCodes.Ldsfld, typeof(Main).GetField("bgScale", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static));
            c.Emit(OpCodes.Ldsfld, typeof(Main).GetField(nameof(Main.instance)));
            c.Emit(OpCodes.Ldfld, typeof(Main).GetField("bgParallax", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance));
            c.EmitDelegate<Func<Texture2D, int, float, float, float, double, Texture2D>>((value, textureSlot, a, b, scale, bgParallax) =>
            {
                float a2 = a;
                float b2 = b;
                float scale2 = scale;
                double bgParallax2 = bgParallax;
                if (textureSlot == ModContent.Find<ModSurfaceBackgroundStyle>("TheConfectionRebirth/ConfectionSurfaceBackgroundStyle").ChooseCloseTexture(ref scale2, ref bgParallax2, ref a2, ref b2))
                {
                    if (ConfectionWorld.ConfectionSurfaceBG == 0)
                    {
                        return ModContent.Request<Texture2D>("TheConfectionRebirth/Backgrounds/ConfectionSurfaceClose").Value;
                    }
                    if (ConfectionWorld.ConfectionSurfaceBG == 1)
                    {
                        return ModContent.Request<Texture2D>("TheConfectionRebirth/Backgrounds/ConfectionSurface1Close").Value;
                    }
                    if (ConfectionWorld.ConfectionSurfaceBG == 2)
                    {
                        return ModContent.Request<Texture2D>("TheConfectionRebirth/Backgrounds/ConfectionSurface2Close").Value;
                    }
                }
                return value;
            });
        }
    }
}
