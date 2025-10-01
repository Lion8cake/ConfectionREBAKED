using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;
using System;
using Terraria;
using Terraria.GameContent.RGB;

namespace TheConfectionRebirth.RGB
{
	public class ConfectionSurfaceShader : ChromaShader
	{
		private readonly Vector4 _skyColor;

		private readonly Vector4 _groundColor;

		private readonly Vector4 _cookieFlowerColor;

		private readonly Vector4 _chocolateFlowerColor;

		private Vector4 _lightColor;

		public override void Update(float elapsedTime)
		{
			_lightColor = Main.ColorOfTheSkies.ToVector4() * 0.75f + Vector4.One * 0.25f;
		}

		[RgbProcessor(EffectDetailLevel.Low)]
		private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				float strength = (float)Math.Sin(time + fragment.GetCanvasPositionOfIndex(i).X) * 0.5f + 0.5f;
				Vector4 color = Vector4.Lerp(_skyColor, _groundColor, strength);
				fragment.SetColor(i, color);
			}
		}

		[RgbProcessor(EffectDetailLevel.High)]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			Vector4 skyLight = _skyColor * _lightColor;
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				Point gridPositionOfIndex = fragment.GetGridPositionOfIndex(i);
				float keyNoise = NoiseHelper.GetDynamicNoise(gridPositionOfIndex.X, gridPositionOfIndex.Y, time / 20f);
				keyNoise = Math.Max(0f, 1f - keyNoise * 5f);
				Vector4 color = skyLight;
				color = (((gridPositionOfIndex.X * 100 + gridPositionOfIndex.Y) % 2 != 0) ? Vector4.Lerp(color, _cookieFlowerColor, keyNoise) : Vector4.Lerp(color, _chocolateFlowerColor, keyNoise));
				float floorY = (float)Math.Sin(canvasPositionOfIndex.X) * 0.3f + 0.7f;
				if (canvasPositionOfIndex.Y > floorY)
				{
					color = _groundColor;
				}
				fragment.SetColor(i, color);
			}
		}

		public ConfectionSurfaceShader()
		{
			Color val = new Color(220, 150, 200); //pink
			_skyColor = val.ToVector4();
			_groundColor = new Vector4(0.8f, 0.75f, 0.6f, 1f); //cream
			_cookieFlowerColor = new Vector4(0.6f, 0.55f, 0.4f, 1f); //brownish-cream
			_chocolateFlowerColor = new Vector4(0.2f, 0.15f, 0.15f, 1f); //brown
		}
	}
}
