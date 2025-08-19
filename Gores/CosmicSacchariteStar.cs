
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Gores
{
	public class CosmicSacchariteStar : ModGore
	{
		public override bool Update(Gore gore)
		{
			if (gore.sticky)
			{
				float num = gore.velocity.Length();
				if (num > 32f)
				{
					gore.velocity *= 32f / num;
				}
			}

			gore.velocity.Y *= 0.98f;
			gore.velocity.X *= 0.98f;
			gore.scale -= 0.01f;
			if (gore.scale < 0.1)
			{
				gore.scale = 0.1f;
				gore.alpha = 255;
			}

			gore.position += gore.velocity;

			if (gore.alpha >= 255)
			{
				gore.active = false;
			}

			if (gore.light > 0f)
			{
				float num32 = gore.light * gore.scale;
				float num33 = gore.light * gore.scale;
				float num35 = gore.light * gore.scale;

				num32 *= 0.2f;
				num33 *= 0.5f;
				num35 *= 0.8f;

				if (TextureAssets.Gore[gore.type].IsLoaded)
				{
					Lighting.AddLight((int)((gore.position.X + (float)TextureAssets.Gore[gore.type].Width() * gore.scale / 2f) / 16f), (int)((gore.position.Y + (float)TextureAssets.Gore[gore.type].Height() * gore.scale / 2f) / 16f), num32, num33, num35);
				}
				else
				{
					Lighting.AddLight((int)((gore.position.X + 32f * gore.scale / 2f) / 16f), (int)((gore.position.Y + 32f * gore.scale / 2f) / 16f), num32, num33, num35);
				}
			}
			return false;
		}

		public override void OnSpawn(Gore gore, IEntitySource source)
		{
			gore.sticky = false;
			gore.alpha = 100;
			gore.scale = 0.7f;
			gore.light = 1f;
		}

		public override Color? GetAlpha(Gore gore, Color lightColor)
		{
			float num = (float)(255 - gore.alpha) / 255f;
			int r;
			int g;
			int b;
			r = lightColor.R;
			g = lightColor.G;
			b =	lightColor.B;
			int num2 = lightColor.A - gore.alpha;
			if (num2 < 0)
			{
				num2 = 0;
			}
			if (num2 > 255)
			{
				num2 = 255;
			}
			return new Color(r, g, b, num2);
		}
	}
}
