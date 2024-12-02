using Microsoft.CodeAnalysis.CSharp.Formatting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI;
using Terraria.Graphics.Renderers;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;
using static System.Formats.Asn1.AsnWriter;
using static Terraria.GameContent.Animations.IL_Actions.Sprites;

namespace TheConfectionRebirth
{
	public enum ParticleLayer : byte
	{
		normal = 0,
		UI = 1,
		BehindTiles = 2
	}
	public class ParticleSystem : ModSystem
	{
		static List<Particle> Particles;
		static List<Particle> PreTileParticles;
		const int MaxParticles = 3000;
		static List<Particle> TooltipParticles;
		public override void Load()
		{
			TooltipParticles = new List<Particle>(MaxParticles);
			PreTileParticles = new List<Particle>(MaxParticles);
			Particles = new List<Particle>(MaxParticles);
			//On_Main.DrawCapture += On_Main_DrawCapture;
			On_Main.DrawDust += On_Main_DrawDust;
			On_Main.DrawBackGore += On_Main_DrawBackGore;
		}

		private void On_Main_DrawBackGore(On_Main.orig_DrawBackGore orig, Main self)
		{
			orig.Invoke(self);
			DrawParticlesBehindTiles();
		}

		private void On_Main_DrawDust(On_Main.orig_DrawDust orig, Main self)
		{
			orig.Invoke(self);
			DrawParticles();
		}
		//private void On_Main_DrawCapture(On_Main.orig_DrawCapture orig, Main self, Rectangle area, Terraria.Graphics.Capture.CaptureSettings settings)
		//{
		//    orig.Invoke(self, area, settings);
		//    DrawParticles();
		//}
		public static void DrawUIParticle(Vector2 Linepos)
		{
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.SamplerStateForCursor, null, null, null, Main.UIScaleMatrix);
			for (int i = 0; i < TooltipParticles.Count; i++)
			{
				Particle particle = TooltipParticles[i];
				if (particle.Active)
				{
					particle.DrawInUI(Main.spriteBatch, Linepos);
				}
			}
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.SamplerStateForCursor, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.UIScaleMatrix);
		}
		public static void DrawParticlesBehindTiles()
		{
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
			List<Particle> postParticles = new List<Particle>();
			SpriteBatch spriteBatch = Main.spriteBatch;
			for (int i = 0; i < PreTileParticles.Count; i++)
			{
				Particle particle = PreTileParticles[i];
				if (particle.Active)
				{
					if (particle.HasPartsDrawnAfterOtherParticles)
					{
						postParticles.Add(particle);
					}
					particle.Draw(spriteBatch);
				}
			}
			for (int i = 0; i < postParticles.Count; i++)
			{
				Particle particle = postParticles[i];
				particle.DrawAfter(spriteBatch);
			}
			postParticles.Clear();

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
		}
		public static void DrawParticles()
		{
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);

			List<Particle> postParticles = new List<Particle>();
			SpriteBatch spriteBatch = Main.spriteBatch;
			for (int i = 0; i < Particles.Count; i++)
			{
				Particle particle = Particles[i];
				if (particle.Active)
				{
					if (particle.HasPartsDrawnAfterOtherParticles)
					{
						postParticles.Add(particle);
					}
					particle.Draw(spriteBatch);
				}
			}
			for (int i = 0; i < postParticles.Count; i++)
			{
				Particle particle = postParticles[i];
				particle.DrawAfter(spriteBatch);
			}
			postParticles.Clear();
			Main.spriteBatch.End();
		}
		public override void PostUpdateDusts()
		{
			for (int i = 0; i < Particles.Count; i++)
			{
				Particle particle = Particles[i];
				if (particle.Active)
				{
					particle.TimeInWorld++;
					particle.Update();
				}
				else
				{
					Particles.Remove(Particles[i]);
				}
			}
			for (int i = 0; i < PreTileParticles.Count; i++)
			{
				Particle particle = PreTileParticles[i];
				if (particle.Active)
				{
					particle.TimeInWorld++;
					particle.Update();
				}
				else
				{
					PreTileParticles.Remove(PreTileParticles[i]);
				}
			}
			for (int i = 0; i < TooltipParticles.Count; i++)
			{
				Particle particle = TooltipParticles[i];
				if (particle.Active)
				{
					particle.TimeInWorld++;
					particle.Update();
				}
				else
				{
					TooltipParticles.Remove(TooltipParticles[i]);
				}
			}
		}

		public static void AddParticle(Particle particle, Vector2 Position, Vector2? Velocity = null, Color? color = null, ParticleLayer layer = ParticleLayer.normal)
		{
			particle.position = Position;
			particle.velocity = (Vector2)(Velocity == null ? Vector2.Zero : Velocity);
			particle.color = (Color)(color == null ? Color.White : color);
			switch (layer)
			{
				case ParticleLayer.normal:
					{
						if (Particles.Count == MaxParticles)
						{
							Particles.Remove(Particles[0]);
						}
						Particles.Add(particle);
						break;
					}
				case ParticleLayer.UI:
					{
						TooltipParticles.Add(particle);
						break;
					}
				case ParticleLayer.BehindTiles:
					{
						if (PreTileParticles.Count == MaxParticles)
						{
							PreTileParticles.Remove(PreTileParticles[0]);
						}
						PreTileParticles.Add(particle);
						break;
					}
			}
			particle.OnSpawn();
		}

		public static void DrawExtra98Special(Vector2 position, Color color, float rotation, Vector2 scale, float timingPercent)
		{
			Texture2D texture = TextureAssets.Extra[98].Value;
			Vector2 orig = texture.Size() / 2;
			Rectangle? frame = new Rectangle?();
			SpriteEffects sprite = SpriteEffects.None;
			Color color2 = Color.White * 0.5f * 0.9f;
			color2.A = (byte)(color2.A / 2);
			Color color3 = color2 * 0.5f;
			color *= 0.5f;
			color *= timingPercent;
			Main.EntitySpriteDraw(texture, position, frame, color, rotation, orig, scale, sprite);
			Main.EntitySpriteDraw(texture, position, frame, color3, rotation, orig, scale * 0.6f, sprite);
		}
	}
	public abstract class Particle : ModType
	{
		public Vector2 position;
		public Vector2 velocity;
		public Color color;
		public int TimeInWorld;
		public bool Active = true;
		public virtual bool HasPartsDrawnAfterOtherParticles => false;
		protected override void Register()
		{
			ModTypeLookup<Particle>.Register(this);
		}
		public virtual void Update()
		{
		}
		public virtual void OnSpawn()
		{
		}
		public virtual void Draw(SpriteBatch spriteBatch)
		{
		}
		public virtual void DrawAfter(SpriteBatch spriteBatch)
		{
		}
		public virtual void DrawInUI(SpriteBatch spriteBatch, Vector2 linePos)
		{
		}
	}

	public class Spawn_Sucrosa : Particle {

		public int timeleft = 0;

		public override void Update() {
			if (TimeInWorld < 9)
				timeleft++;
			else
				timeleft--;

			if (TimeInWorld > 15)
				Active = false;
		}

		public override void OnSpawn() {
			int dustAmount = 15;
			for (int i = 0; i < dustAmount; i++) {
				Vector2 velocity = Vector2.Zero;
				if (i < dustAmount / 2)
					velocity.X = 4;
				else
					velocity.X = -4;
				velocity.Y += Main.rand.Next(-100, 100) / 100;
				Dust dust1 = Dust.NewDustPerfect(position, ModContent.DustType<NeapoliniteVanillaDust>(), velocity, 0, default, 1.5f);
				dust1.noGravity = true;

				Vector2 velocity2;
				if (i < dustAmount / 2)
					velocity2 = new Vector2(2f, 2f);
				else
					velocity2 = new Vector2(-2f, -2f);
				velocity2 += new Vector2(Main.rand.Next(-100, 100) / 100, Main.rand.Next(-100, 100) / 100);
				Dust dust2 = Dust.NewDustPerfect(position, ModContent.DustType<NeapoliniteChocolateDust>(), velocity2, 0, default, 1.5f);
				dust2.noGravity = true;

				Vector2 velocity3 = new Vector2(-2f, 2f);
				velocity3 += new Vector2(Main.rand.Next(-100, 100) / 100, Main.rand.Next(-100, 100) / 100);
				Dust dust3 = Dust.NewDustPerfect(position, ModContent.DustType<NeapoliniteStrawberryDust>(), velocity3, 0, default, 1.5f);
				dust3.noGravity = true;
			}
		}

		public override void Draw(SpriteBatch spriteBatch) {
			
			Vector2 pos = position - Main.screenPosition;
			float timeleftMax = 8f;
			float percent = timeleft / timeleftMax;
			float velocityAmount = 0.75f;

			Color color = new Color( 2f, 0.7f, 1.4f, 0.5f); //Strawberry Colors
			Color color2 = new Color(1f, 0.2f, 0.6f, 0.5f);
			Color color3 = new Color(1.2f, 0.8f, 0.6f, 0.5f); //Chocolate Colors
			Color color4 = new Color(0.5f, 0.3f, 0.3f, 0.5f);
			Color color5 = new Color(2f, 1.7f, 1f, 0.5f); //Vanilla Colors
			Color color6 = new Color(1.7f, 1.2f, 0.7f, 0.5f);

			float scaler = 1.25f;
			Vector2 scale = new Vector2(0.5f, 1f) * (timeleft / timeleftMax) * scaler;
			Vector2 scale2 = new Vector2(0.75f, 0.5f) * 1.1f * (timeleft / (timeleftMax / 2)) * scaler;

			float rotation = MathHelper.ToRadians(90f);
			float rotation2 = MathHelper.ToRadians(45f);
			float rotation3 = MathHelper.ToRadians(-45f);

			Vector2 newPos = pos + new Vector2(velocityAmount * TimeInWorld, 0f);
			Vector2 newPos2 = pos - new Vector2(velocityAmount * TimeInWorld, 0f);
			Vector2 newPos3 = pos + new Vector2(velocityAmount * TimeInWorld, velocityAmount * TimeInWorld);
			Vector2 newPos4 = pos - new Vector2(velocityAmount * TimeInWorld, velocityAmount * TimeInWorld);
			Vector2 newPos5 = pos + new Vector2(velocityAmount * TimeInWorld, -(velocityAmount * TimeInWorld));
			Vector2 newPos6 = pos - new Vector2(velocityAmount * TimeInWorld, -(velocityAmount * TimeInWorld));

			//large outline
			ParticleSystem.DrawExtra98Special(newPos, color6, rotation, scale2, percent);
			ParticleSystem.DrawExtra98Special(newPos2, color6, rotation, scale2, percent);
			ParticleSystem.DrawExtra98Special(newPos3, color4, rotation3, scale2, percent);
			ParticleSystem.DrawExtra98Special(newPos4, color4, rotation3, scale2, percent);
			ParticleSystem.DrawExtra98Special(newPos5, color2, rotation2, scale2, percent);
			ParticleSystem.DrawExtra98Special(newPos6, color2, rotation2, scale2, percent);

			//innard
			ParticleSystem.DrawExtra98Special(newPos, color5, rotation, scale, percent);
			ParticleSystem.DrawExtra98Special(newPos2, color5, rotation, scale, percent);
			ParticleSystem.DrawExtra98Special(newPos4, color3, rotation3, scale, percent);
			ParticleSystem.DrawExtra98Special(newPos3, color3, rotation3, scale, percent);
			ParticleSystem.DrawExtra98Special(newPos6, color, rotation2, scale, percent);
			ParticleSystem.DrawExtra98Special(newPos5, color, rotation2, scale, percent);
		}
	}

	public class Spawn_TrueSucrosa : Particle {

		public int timeleft = 0;

		public override void Update()
		{
			if (TimeInWorld < 9)
				timeleft++;
			else
				timeleft--;

			if (TimeInWorld > 15)
				Active = false;
		}

		public override void OnSpawn()
		{
			int dustAmount = 15;
			for (int i = 0; i < dustAmount; i++)
			{
				Vector2 velocity = Vector2.Zero;
				if (i < dustAmount / 2)
					velocity.X = 4;
				else
					velocity.X = -4;
				velocity.Y += Main.rand.Next(-100, 100) / 100;
				Dust dust1 = Dust.NewDustPerfect(position, ModContent.DustType<NeapoliniteVanillaDust>(), velocity, 0, default, 1.5f);
				dust1.noGravity = true;

				Vector2 velocity2;
				if (i < dustAmount / 2)
					velocity2 = new Vector2(2f, 2f);
				else
					velocity2 = new Vector2(-2f, -2f);
				velocity2 += new Vector2(Main.rand.Next(-100, 100) / 100, Main.rand.Next(-100, 100) / 100);
				Dust dust2 = Dust.NewDustPerfect(position, ModContent.DustType<NeapoliniteChocolateDust>(), velocity2, 0, default, 1.5f);
				dust2.noGravity = true;

				Vector2 velocity3 = new Vector2(-2f, 2f);
				velocity3 += new Vector2(Main.rand.Next(-100, 100) / 100, Main.rand.Next(-100, 100) / 100);
				Dust dust3 = Dust.NewDustPerfect(position, ModContent.DustType<NeapoliniteStrawberryDust>(), velocity3, 0, default, 1.5f);
				dust3.noGravity = true;

				Vector2 velocity4 = Vector2.Zero;
				if (i < dustAmount / 2)
					velocity4.Y = 4;
				else
					velocity4.Y = -4;
				velocity4.X += Main.rand.Next(-100, 100) / 100;
				Dust dust4 = Dust.NewDustPerfect(position, ModContent.DustType<NeapoliniteSacchariteDust>(), velocity4, 0, default, 1.5f);
				dust4.noGravity = true;
			}
		}

		public override void Draw(SpriteBatch spriteBatch)
		{

			Vector2 pos = position - Main.screenPosition;
			float timeleftMax = 8f;
			float percent = timeleft / timeleftMax;
			float velocityAmount = 0.75f;

			Color color = new Color(2f, 0.7f, 1.4f, 0.5f); //Strawberry Colors
			Color color2 = new Color(1f, 0.2f, 0.6f, 0.5f);
			Color color3 = new Color(1.2f, 0.8f, 0.6f, 0.5f); //Chocolate Colors
			Color color4 = new Color(0.5f, 0.3f, 0.3f, 0.5f);
			Color color5 = new Color(2f, 1.7f, 1f, 0.5f); //Vanilla Colors
			Color color6 = new Color(1.7f, 1.2f, 0.7f, 0.5f);
			Color color7 = new Color(0.3f, 1.7f, 2.2f, 0.5f); //Saccharite Colors
			Color color8 = new Color(0.3f, 0.5f, 0.9f, 0.5f);

			float scaler = 1.25f;
			Vector2 scale = new Vector2(0.5f, 1f) * (timeleft / timeleftMax) * scaler;
			Vector2 scale2 = new Vector2(0.75f, 0.5f) * 1.1f * (timeleft / (timeleftMax / 2)) * scaler;

			float rotation = MathHelper.ToRadians(90f);
			float rotation2 = MathHelper.ToRadians(45f);
			float rotation3 = MathHelper.ToRadians(-45f);

			Vector2 newPos = pos + new Vector2(velocityAmount * TimeInWorld, 0f);
			Vector2 newPos2 = pos - new Vector2(velocityAmount * TimeInWorld, 0f);
			Vector2 newPos3 = pos + new Vector2(velocityAmount * TimeInWorld, velocityAmount * TimeInWorld);
			Vector2 newPos4 = pos - new Vector2(velocityAmount * TimeInWorld, velocityAmount * TimeInWorld);
			Vector2 newPos5 = pos + new Vector2(velocityAmount * TimeInWorld, -(velocityAmount * TimeInWorld));
			Vector2 newPos6 = pos - new Vector2(velocityAmount * TimeInWorld, -(velocityAmount * TimeInWorld));
			Vector2 newPos7 = pos + new Vector2(0f, velocityAmount * TimeInWorld);
			Vector2 newPos8 = pos - new Vector2(0f, velocityAmount * TimeInWorld);

			//large outline
			ParticleSystem.DrawExtra98Special(newPos, color6, rotation, scale2, percent);
			ParticleSystem.DrawExtra98Special(newPos2, color6, rotation, scale2, percent);
			ParticleSystem.DrawExtra98Special(newPos3, color4, rotation3, scale2, percent);
			ParticleSystem.DrawExtra98Special(newPos4, color4, rotation3, scale2, percent);
			ParticleSystem.DrawExtra98Special(newPos5, color2, rotation2, scale2, percent);
			ParticleSystem.DrawExtra98Special(newPos6, color2, rotation2, scale2, percent);
			ParticleSystem.DrawExtra98Special(newPos7, color8, 0f, scale2 * 1.25f, percent);
			ParticleSystem.DrawExtra98Special(newPos8, color8, 0f, scale2 * 1.25f, percent);

			//innard
			ParticleSystem.DrawExtra98Special(newPos, color5, rotation, scale, percent);
			ParticleSystem.DrawExtra98Special(newPos2, color5, rotation, scale, percent);
			ParticleSystem.DrawExtra98Special(newPos3, color3, rotation3, scale, percent);
			ParticleSystem.DrawExtra98Special(newPos4, color3, rotation3, scale, percent);
			ParticleSystem.DrawExtra98Special(newPos5, color, rotation2, scale, percent);
			ParticleSystem.DrawExtra98Special(newPos6, color, rotation2, scale, percent);
			ParticleSystem.DrawExtra98Special(newPos7, color7, 0f, scale * 1.25f, percent);
			ParticleSystem.DrawExtra98Special(newPos8, color7, 0f, scale * 1.25f, percent);
		}
	}

	public class Spawn_DeathsRaze : Particle {
		public override void Update() {
			if (TimeInWorld > 20)
				Active = false;
		}

		public override void Draw(SpriteBatch spriteBatch) {
			Vector2 pos = position - Main.screenPosition;
			Vector2 pos2 = pos + new Vector2(-30, -45);
			Vector2 pos3 = pos + new Vector2(30, -45);
			float percent = (16 - TimeInWorld) / 4f; 
			float velocityAmount = 3f;

			Color color = new Color(2.55f, 0.7f, 1f, 0.5f); //Blood Color
			Color color2 = new Color(2.23f, 0.22f, 0.49f, 0.5f);

			float time = TimeInWorld < 10 ? TimeInWorld : 10f;
			float scaler = 1.25f;
			Vector2 scale = new Vector2(0.1f, 0.4f) * 2f * scaler;
			Vector2 scale2 = new Vector2(0.25f, 0.5f) * 1.1f * 2f * scaler;

			float rotation = MathHelper.ToRadians(45f);
			float rotation2 = MathHelper.ToRadians(-45f);

			Vector2 newPos = pos2 + new Vector2(velocityAmount * time, velocityAmount * time);
			Vector2 newPos2 = pos3 - new Vector2(velocityAmount * time, -(velocityAmount * time));

			//large outline
			ParticleSystem.DrawExtra98Special(newPos, color2, rotation2, scale2, percent);
			ParticleSystem.DrawExtra98Special(newPos2, color2, rotation, scale2, percent);

			//innard
			ParticleSystem.DrawExtra98Special(newPos, color, rotation2, scale, percent / 2);
			ParticleSystem.DrawExtra98Special(newPos2, color, rotation, scale, percent / 2);
		}
	}

	public class Spawn_TrueDeathsRaze : Particle {
		public override void Update() {
			if (TimeInWorld > 20)
				Active = false;
		}

		public override void Draw(SpriteBatch spriteBatch) {
			Vector2 pos = position - Main.screenPosition;
			Vector2 pos2 = pos + new Vector2(-30, -45);
			Vector2 pos3 = pos + new Vector2(30, -45);
			Vector2 pos4 = pos + new Vector2(0, -60);
			float percent = (16 - TimeInWorld) / 4f;
			float velocityAmount = 3f;
			float ichorNeedleScale = 1.5f;

			Color color = new Color(2.55f, 0.7f, 1f, 0.5f); //Blood Color
			Color color2 = new Color(2.23f, 0.22f, 0.49f, 0.5f);
			Color color3 = new Color(2.5f, 2.3f, 1.7f, 0.5f); //Ichor Color
			Color color4 = new Color(2.55f, 2.24f, 0.5f, 0.75f);

			float time = TimeInWorld < 10 ? TimeInWorld : 10f;
			float scaler = 1.25f;
			Vector2 scale = new Vector2(0.1f, 0.4f) * 2f * scaler;
			Vector2 scale2 = new Vector2(0.25f, 0.5f) * 1.1f * 2f * scaler;

			float rotation = MathHelper.ToRadians(45f);
			float rotation2 = MathHelper.ToRadians(-45f);


			Vector2 newPos = pos2 + new Vector2(velocityAmount * time, velocityAmount * time);
			Vector2 newPos2 = pos3 - new Vector2(velocityAmount * time, -(velocityAmount * time));
			Vector2 newPos3 = pos4 + new Vector2(0, (velocityAmount * ichorNeedleScale) * time);

			//large outline
			ParticleSystem.DrawExtra98Special(newPos, color2, rotation2, scale2, percent);
			ParticleSystem.DrawExtra98Special(newPos2, color2, rotation, scale2, percent);
			ParticleSystem.DrawExtra98Special(newPos3, color4, 0f, scale2 * ichorNeedleScale, percent);

			//innard
			ParticleSystem.DrawExtra98Special(newPos, color, rotation2, scale, percent / 2);
			ParticleSystem.DrawExtra98Special(newPos2, color, rotation, scale, percent / 2);
			ParticleSystem.DrawExtra98Special(newPos3, color3, 0f, scale * ichorNeedleScale, percent / 2);
		}
	}
}