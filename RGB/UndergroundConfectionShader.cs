using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;
using System;
using Terraria.GameContent.RGB;

namespace TheConfectionRebirth.RGB
{
	//Keyboard shader for the confection's underground
	public class UndergroundConfectionShader : ChromaShader
	{
		private readonly Vector4 _baseColor;

		private readonly Vector4 _creamstoneColor;

		private readonly Vector4 _sacchariteColor;

		[RgbProcessor(EffectDetailLevel.Low)]
		private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				float strength = (float)Math.Sin(time * 2f + fragment.GetCanvasPositionOfIndex(i).X) * 0.5f + 0.5f;
				Vector4 color = Vector4.Lerp(_creamstoneColor, _sacchariteColor, strength);
				fragment.SetColor(i, color);
			}
		}

		//Draws a ceiling and a floor, then allows saccharite lines to grow either from the top or the bottom depending on whether the row is an odd or even row
		[RgbProcessor(EffectDetailLevel.High)]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				Point gridPositionOfIndex = fragment.GetGridPositionOfIndex(i);
				Vector2 canvasPos = fragment.GetCanvasPositionOfIndex(i);
				Vector4 rockColor = _baseColor;
				float keyNoise = NoiseHelper.GetStaticNoise(gridPositionOfIndex.X);
				keyNoise = (keyNoise * 10f + time * 0.4f) % 10f;
				float noiseStrength = 1f;
				if (keyNoise > 1f)
				{
					noiseStrength = MathHelper.Clamp(1f - (keyNoise - 1.4f), 0f, 1f);
					keyNoise = 1f;
				}
				float floorPos = (float)Math.Sin(canvasPos.X) * 0.5f + 0.9f;
				float celingPos = (float)Math.Sin(canvasPos.X) * 0.1f + 0.1f;
				float sacchariteDir = keyNoise - (1f - canvasPos.Y);
				if (canvasPos.X % 2 == 0)
				{
					sacchariteDir = keyNoise - canvasPos.Y;
				}
				if (sacchariteDir > 0f)
				{
					float sacchStrength = 1f;
					if (sacchariteDir < 0.2f)
					{
						sacchStrength = sacchariteDir * 5f;
					}
					rockColor = Vector4.Lerp(rockColor, _sacchariteColor, sacchStrength * noiseStrength);
				}
				if (canvasPos.Y > floorPos || canvasPos.Y < celingPos)
				{
					rockColor = _creamstoneColor;
				}
				fragment.SetColor(i, rockColor);
			}
		}

		public UndergroundConfectionShader()
		{
			Color color = new Color(0.05f, 0.05f, 0.05f);
			_baseColor = color.ToVector4();
			color = new Color(148, 123, 86);
			_creamstoneColor = color.ToVector4();
			color = new Color(149, 241, 247);
			_sacchariteColor = color.ToVector4();
		}
	}
}
