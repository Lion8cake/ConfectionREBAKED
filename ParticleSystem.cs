using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Renderers;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth
{
    public class ParticleSystem : ModSystem
    {
        static List<Particle> Particles;
        static readonly int MaxParticles = 3000;
        public override void Load()
        {
            Particles = new List<Particle>(MaxParticles);
            On_Main.DrawCapture += On_Main_DrawCapture;
            On_Main.DrawDust += On_Main_DrawDust;
        }

		public override void Unload() {
			Particles.Clear();
			On_Main.DrawCapture -= On_Main_DrawCapture;
			On_Main.DrawDust -= On_Main_DrawDust;
		}

		private void On_Main_DrawDust(On_Main.orig_DrawDust orig, Main self)
        {
            orig.Invoke(self);
            DrawParticles();
        }
        private void On_Main_DrawCapture(On_Main.orig_DrawCapture orig, Main self, Rectangle area, Terraria.Graphics.Capture.CaptureSettings settings)
        {
            orig.Invoke(self, area, settings);
            DrawParticles();
        }

        
        public static void DrawParticles()
        {
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);

            SpriteBatch spriteBatch = Main.spriteBatch;

            for (int i = 0; i < Particles.Count; i++)
            {
                Particle particle = Particles.ElementAt(i);
                if (particle.Active)
                {
                    particle.Draw(spriteBatch);
                }
            }

            for (int i = 0; i < Particles.Count; i++)
            {
                Particle particle = Particles.ElementAt(i);
                if (particle.Active)
                {
                    particle.PostDraw(spriteBatch);
                }
            }

            Main.spriteBatch.End();
        }

		public override void PostUpdateDusts() {
			for (int i = 0; i < Particles.Count; i++) {
				Particle particle = Particles.ElementAt(i);
				if (particle.Active) {
					if (particle.TimeInWorld == 0) {
						particle.OnSpawn();
					}
					particle.Update();
				}
				else {
					Particles.Remove(Particles.ElementAt(i));
				}
			}
		}

		public static Particle AddParticle(Particle type, Vector2 position, Vector2 velocity, Color color, float AI1 = 0, float AI2 = 0, float AI3 = 0)
        {
            if (Particles.Count == MaxParticles)
            {
                Particles.Remove(Particles.First());
            }
            Particles.Add(type);
            Particles.Last().Position = position;
            Particles.Last().Velocity = velocity;
            Particles.Last().Color = color;
            Particles.Last().AI1 = AI1;
            Particles.Last().AI2 = AI2;
            Particles.Last().AI3 = AI3;

            return Particles.Last();
        }
    }
    public abstract class Particle
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public int TimeInWorld;
        public float AI1;
        public float AI2;
        public float AI3;
        public bool Active = true;
        public byte Type;
        public Color Color;
        public virtual void Update()
        {
        }
        public virtual void OnSpawn()
        {

        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
        }
        public virtual void PostDraw(SpriteBatch spriteBatch)
        {
        }
    }

	public class NeapoliniteSlash : Particle {

		protected int backwardstimer = 5;

		public override void Update() {
			TimeInWorld += 1;
			if (Main.timeForVisualEffects % 4 == 0) {
				TimeInWorld += 1;
			}

			if (TimeInWorld > 20)
				Active = false;
			if (TimeInWorld > 15) {
				backwardstimer--;
			}
			AI1 -= 0.1f;
		}

		public override void OnSpawn() {
			for (int i = 0; i < 30; i++) {
				Dust dust1 = Dust.NewDustPerfect(Position, ModContent.DustType<NeapoliniteVanillaDust>(), new Vector2(0, 0), 0, default, 1.5f);
				dust1.noGravity = true;
				if (i < 15) {
					dust1.velocity.X += 7;
				}
				else {
					dust1.velocity.X -= 7;
				}
				dust1.velocity.Y += Main.rand.Next(-100, 100) / 100;
				Dust dust2 = Dust.NewDustPerfect(Position, ModContent.DustType<NeapoliniteStrawberryDust>(), new Vector2(0, 0), 0, default, 1.5f);
				dust2.noGravity = true;
				if (i < 15) {
					dust2.velocity += new Vector2(5, 5);
				}
				else {
					dust2.velocity -= new Vector2(5, 5);
				}
				dust2.velocity += new Vector2(Main.rand.Next(-100, 100) / 100, Main.rand.Next(-100, 100) / 100);
				Dust dust3 = Dust.NewDustPerfect(Position, ModContent.DustType<NeapoliniteChocolateDust>(), new Vector2(0, 0), 0, default, 1.5f);
				dust3.noGravity = true;
				if (i < 15) {
					dust3.velocity += new Vector2(-5, 5);
				}
				else {
					dust3.velocity += new Vector2(5, -5);
				}
				dust2.velocity += new Vector2(Main.rand.Next(-100, 100) / 100, Main.rand.Next(-100, 100) / 100);
			}
		}

		public override void Draw(SpriteBatch spriteBatch) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("TheConfectionRebirth/Assets/DoubleSparkle");
			//Texture2D texture2 = TextureAssets.Extra[98].Value;
			Rectangle frame = texture.Frame();
			Vector2 frameOrigin = frame.Size() / 2f;
			Vector2 DrawPos = Position - Main.screenPosition;
			Color color1 = new(224, 92, 165); //Srawberry
			Color color2 = new(153, 96, 62); //Chocolate
			Color color3 = new(230, 196, 125); //Vanilla
			Vector2 Size = new((float)Math.Sin((float)(TimeInWorld + 3) * 0.06f) * 1f, (float)Math.Sin((float)(TimeInWorld + 3) * 0.06f) * 1f);
			if (TimeInWorld < 10) {
				Size = new((float)Math.Sin((float)(TimeInWorld + 3) * 0.06f) * 1f, (float)Math.Sin((float)(TimeInWorld + 3) * 0.06f) * 1f);
			}
			if (TimeInWorld > 15) {
				Size = new((float)Math.Sin((float)(backwardstimer + 3) * 0.06f) * 1f, (float)Math.Sin((float)(backwardstimer + 3) * 0.06f) * 1f);
			}
			spriteBatch.Draw(texture, DrawPos, frame, color1, -MathHelper.PiOver4, frameOrigin, Size, SpriteEffects.None, 0);
			spriteBatch.Draw(texture, DrawPos, frame, color2, MathHelper.PiOver4, frameOrigin, Size, SpriteEffects.None, 0);
			spriteBatch.Draw(texture, DrawPos, frame, color3, MathHelper.PiOver2, frameOrigin, Size, SpriteEffects.None, 0);
			spriteBatch.Draw(texture, DrawPos, frame, new Color(255, 255, 255, 0) * 0.3f, -MathHelper.PiOver4, frameOrigin, new Vector2(1.4f, (float)Math.Sin((float)(TimeInWorld + 1) * 0.20f) * 2f) * 0.7f, SpriteEffects.None, 0);
			spriteBatch.Draw(texture, DrawPos, frame, new Color(255, 255, 255, 0) * 0.3f, MathHelper.PiOver4, frameOrigin, new Vector2(1.4f, (float)Math.Sin((float)(TimeInWorld + 1) * 0.20f) * 2f) * 0.7f, SpriteEffects.None, 0);
			spriteBatch.Draw(texture, DrawPos, frame, new Color(255, 255, 255, 0) * 0.3f, MathHelper.PiOver2, frameOrigin, new Vector2(1.4f, (float)Math.Sin((float)(TimeInWorld + 1) * 0.20f) * 2f) * 0.7f, SpriteEffects.None, 0);
		}
	}

	public class TrueNeapoliniteSlash : Particle {

		protected int backwardstimer = 5;

		protected Vector2 BlueOffset = new Vector2(1, 0);

		public override void Update() {
			TimeInWorld += 1;
			if (Main.timeForVisualEffects % 4 == 0) {
				TimeInWorld += 1;
			}

			if (TimeInWorld > 20)
				Active = false;
			if (TimeInWorld > 15) {
				backwardstimer--;
			}
			AI1 -= 0.1f;
		}

		public override void OnSpawn() {
			for (int i = 0; i < 30; i++) {
				Dust dust1 = Dust.NewDustPerfect(Position, ModContent.DustType<NeapoliniteVanillaDust>(), new Vector2(0, 0), 0, default, 1.5f);
				dust1.noGravity = true;
				if (i < 15) {
					dust1.velocity.X += 7;
				}
				else {
					dust1.velocity.X -= 7;
				}
				dust1.velocity.Y += Main.rand.Next(-100, 100) / 100;
				Dust dust2 = Dust.NewDustPerfect(Position, ModContent.DustType<NeapoliniteStrawberryDust>(), new Vector2(0, 0), 0, default, 1.5f);
				dust2.noGravity = true;
				if (i < 15) {
					dust2.velocity += new Vector2(5, 5);
				}
				else {
					dust2.velocity -= new Vector2(5, 5);
				}
				dust2.velocity += new Vector2(Main.rand.Next(-100, 100) / 100, Main.rand.Next(-100, 100) / 100);
				Dust dust3 = Dust.NewDustPerfect(Position, ModContent.DustType<NeapoliniteChocolateDust>(), new Vector2(0, 0), 0, default, 1.5f);
				dust3.noGravity = true;
				if (i < 15) {
					dust3.velocity += new Vector2(-5, 5);
				}
				else {
					dust3.velocity += new Vector2(5, -5);
				}
				dust3.velocity += new Vector2(Main.rand.Next(-100, 100) / 100, Main.rand.Next(-100, 100) / 100);
				Dust dust4 = Dust.NewDustPerfect(Position + BlueOffset, ModContent.DustType<NeapoliniteSacchariteDust>(), new Vector2(0, 0), 0, default, 1.5f);
				dust4.noGravity = true;
				if (i < 15) {
					dust4.velocity.Y += 7;
				}
				else {
					dust4.velocity.Y -= 7;
				}
				dust4.velocity.X += Main.rand.Next(-100, 100) / 100;
				Dust dust5 = Dust.NewDustPerfect(Position - BlueOffset, ModContent.DustType<NeapoliniteSacchariteDust>(), new Vector2(0, 0), 0, default, 1.5f);
				dust5.noGravity = true;
				if (i < 15) {
					dust5.velocity.Y += 7;
				}
				else {
					dust5.velocity.Y -= 7;
				}
				dust5.velocity.X += Main.rand.Next(-100, 100) / 100;
			}
		}

		public override void Draw(SpriteBatch spriteBatch) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("TheConfectionRebirth/Assets/DoubleSparkle");
			//Texture2D texture2 = TextureAssets.Extra[98].Value;
			Rectangle frame = texture.Frame();
			Vector2 frameOrigin = frame.Size() / 2f;
			Vector2 DrawPos = Position - Main.screenPosition;
			Color color1 = new(224, 92, 165); //Srawberry
			Color color2 = new(153, 96, 62); //Chocolate
			Color color3 = new(230, 196, 125); //Vanilla
			Color color4 = new(32, 174, 221); //Saccharite
			Vector2 Size = new((float)Math.Sin((float)(TimeInWorld + 3) * 0.06f) * 1f, (float)Math.Sin((float)(TimeInWorld + 3) * 0.06f) * 1f);
			if (TimeInWorld < 10) {
				Size = new((float)Math.Sin((float)(TimeInWorld + 3) * 0.06f) * 1f, (float)Math.Sin((float)(TimeInWorld + 3) * 0.06f) * 1f);
			}
			if (TimeInWorld > 15) {
				Size = new((float)Math.Sin((float)(backwardstimer + 3) * 0.06f) * 1f, (float)Math.Sin((float)(backwardstimer + 3) * 0.06f) * 1f);
			}
			spriteBatch.Draw(texture, DrawPos, frame, color1, -MathHelper.PiOver4, frameOrigin, Size, SpriteEffects.None, 0);
			spriteBatch.Draw(texture, DrawPos, frame, color2, MathHelper.PiOver4, frameOrigin, Size, SpriteEffects.None, 0);
			spriteBatch.Draw(texture, DrawPos, frame, color3, MathHelper.PiOver2, frameOrigin, Size, SpriteEffects.None, 0);
			spriteBatch.Draw(texture, DrawPos + BlueOffset, frame, color4, 0, frameOrigin, Size, SpriteEffects.None, 0);
			spriteBatch.Draw(texture, DrawPos - BlueOffset, frame, color4, 0, frameOrigin, Size, SpriteEffects.None, 0);
			spriteBatch.Draw(texture, DrawPos, frame, new Color(255, 255, 255, 0) * 0.3f, -MathHelper.PiOver4, frameOrigin, new Vector2(1.4f, (float)Math.Sin((float)(TimeInWorld + 1) * 0.20f) * 2f) * 0.7f, SpriteEffects.None, 0);
			spriteBatch.Draw(texture, DrawPos, frame, new Color(255, 255, 255, 0) * 0.3f, MathHelper.PiOver4, frameOrigin, new Vector2(1.4f, (float)Math.Sin((float)(TimeInWorld + 1) * 0.20f) * 2f) * 0.7f, SpriteEffects.None, 0);
			spriteBatch.Draw(texture, DrawPos, frame, new Color(255, 255, 255, 0) * 0.3f, MathHelper.PiOver2, frameOrigin, new Vector2(1.4f, (float)Math.Sin((float)(TimeInWorld + 1) * 0.20f) * 2f) * 0.7f, SpriteEffects.None, 0);
			spriteBatch.Draw(texture, DrawPos + BlueOffset, frame, new Color(255, 255, 255, 0) * 0.3f, 0, frameOrigin, new Vector2(1.4f, (float)Math.Sin((float)(TimeInWorld + 1) * 0.20f) * 2f) * 0.7f, SpriteEffects.None, 0);
			spriteBatch.Draw(texture, DrawPos - BlueOffset, frame, new Color(255, 255, 255, 0) * 0.3f, 0, frameOrigin, new Vector2(1.4f, (float)Math.Sin((float)(TimeInWorld + 1) * 0.20f) * 2f) * 0.7f, SpriteEffects.None, 0);
		}
	}

	public class BloodyNeedle : Particle {
		protected int MoveToCentre = 10;

		public override void Update() {
			TimeInWorld += 1;
			if (Main.timeForVisualEffects % 4 == 0) {
				TimeInWorld += 1;
			}

			if (TimeInWorld > 26)
				Active = false;
			AI1 -= 0.1f;

			if (MoveToCentre > 0) {
				MoveToCentre--;
			}
		}

		public override void Draw(SpriteBatch spriteBatch) {
			Texture2D texture = TextureAssets.Extra[98].Value;
			Rectangle frame = texture.Frame();
			Vector2 frameOrigin = frame.Size() / 2f;
			Color color = new(223, 22, 49, TimeInWorld * 10);
			Color color2 = new Color(255, 255, 255, TimeInWorld * 10)/* * 0.3f*/;
			float realscale = 0.45f;
			Vector2 size = new Vector2(1f, 2.5f) * realscale;
			Vector2 size2 = new Vector2(0.8f, 2.3f) * realscale;

			Vector2 DrawPosLtoR = Position - Main.screenPosition + new Vector2(-MoveToCentre * 2, -MoveToCentre * 2);
			Vector2 DrawPosRtoL = Position - Main.screenPosition - new Vector2(-MoveToCentre * 2, MoveToCentre * 2);

			spriteBatch.Draw(texture, DrawPosLtoR, frame, color, -MathHelper.PiOver4, frameOrigin, size, SpriteEffects.None, 0);
			spriteBatch.Draw(texture, DrawPosLtoR, frame, color2, -MathHelper.PiOver4, frameOrigin, size2, SpriteEffects.None, 0);

			spriteBatch.Draw(texture, DrawPosRtoL, frame, color, MathHelper.PiOver4, frameOrigin, size, SpriteEffects.None, 0);
			spriteBatch.Draw(texture, DrawPosRtoL, frame, color2, MathHelper.PiOver4, frameOrigin, size2, SpriteEffects.None, 0);
		}
	}

	public class TrueBloodyNeedle : Particle {
		protected int MoveToCentre = 10;
		protected int CentreNeedleMove = 10;

		public override void Update() {
			TimeInWorld += 1;
			if (Main.timeForVisualEffects % 4 == 0) {
				TimeInWorld += 1;
			}

			if (TimeInWorld > 26)
				Active = false;
			AI1 -= 0.1f;

			if (MoveToCentre > 0) {
				MoveToCentre--;
			}

			if (CentreNeedleMove > 0 && TimeInWorld > 16) {
				CentreNeedleMove -= 2;
			}
		}

		public override void Draw(SpriteBatch spriteBatch) {
			Texture2D texture = TextureAssets.Extra[98].Value;
			Rectangle frame = texture.Frame();
			Vector2 frameOrigin = frame.Size() / 2f;
			Color color = new(223, 22, 49, TimeInWorld * 10);
			Color color2 = new Color(255, 255, 255, TimeInWorld * 10);
			Color color3 = new Color(244, 191, 90, TimeInWorld * 10);
			float realscale = 0.45f;
			Vector2 size = new Vector2(1f, 2.5f) * realscale;
			Vector2 size2 = new Vector2(0.8f, 2.3f) * realscale;
			float realscale2 = 0.65f;
			Vector2 size3 = new Vector2(1f, 2.5f) * realscale2;
			Vector2 size4 = new Vector2(0.8f, 2.3f) * realscale2;

			Vector2 DrawPosLtoR = Position - Main.screenPosition + new Vector2(-MoveToCentre * 2, -MoveToCentre * 2);
			Vector2 DrawPosRtoL = Position - Main.screenPosition - new Vector2(-MoveToCentre * 2, MoveToCentre * 2);
			Vector2 DrawPosTtoB = Position - Main.screenPosition - new Vector2(0, CentreNeedleMove * 2);

			spriteBatch.Draw(texture, DrawPosLtoR, frame, color, -MathHelper.PiOver4, frameOrigin, size, SpriteEffects.None, 0);
			spriteBatch.Draw(texture, DrawPosLtoR, frame, color2, -MathHelper.PiOver4, frameOrigin, size2, SpriteEffects.None, 0);

			spriteBatch.Draw(texture, DrawPosRtoL, frame, color, MathHelper.PiOver4, frameOrigin, size, SpriteEffects.None, 0);
			spriteBatch.Draw(texture, DrawPosRtoL, frame, color2, MathHelper.PiOver4, frameOrigin, size2, SpriteEffects.None, 0);

			spriteBatch.Draw(texture, DrawPosTtoB, frame, color3, 0, frameOrigin, size3, SpriteEffects.None, 0);
			spriteBatch.Draw(texture, DrawPosTtoB, frame, color2, 0, frameOrigin, size4, SpriteEffects.None, 0);
		}
	}
}
