using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Gores
{
	public class BanannaPeel : ModGore
	{
		public override void SetStaticDefaults()
		{
			GoreID.Sets.DisappearSpeed[Type] = 6;
		}

		public override void OnSpawn(Gore gore, IEntitySource source)
		{
			gore.drawOffset.Y -= 6;
		}

		public override bool Update(Gore gore)
		{
			float rotationSpeed = 0.1f;
			if (gore.rotation > MathHelper.PiOver2)
			{
				gore.rotation = gore.rotation % MathHelper.PiOver2;
			}
			if (gore.rotation < rotationSpeed && gore.rotation > -rotationSpeed)
			{
				gore.rotation = 0f;
			}

			if (gore.rotation != 0)
			{
				if (gore.rotation > 0)
				{
					gore.rotation -= rotationSpeed;
				}
				else if (gore.rotation < 0)
				{
					gore.rotation += rotationSpeed;
				}
			}

			if (gore.velocity.Y < 10f)
			{
				gore.velocity.Y += 0.25f;
			}
			gore.velocity.X = 0f;

			int hitboxSize = 32;
			if (TextureAssets.Gore[Type].IsLoaded)
			{
				hitboxSize = TextureAssets.Gore[Type].Width();
				if (TextureAssets.Gore[Type].Height() < hitboxSize)
				{
					hitboxSize = TextureAssets.Gore[Type].Height();
				}
			}
			hitboxSize = (int)((float)hitboxSize * 0.6f);
			gore.velocity = Collision.TileCollision(gore.position, gore.velocity, (int)((float)hitboxSize * gore.scale), (int)((float)hitboxSize * gore.scale));
			Vector4 slopeVelPos = Collision.SlopeCollision(gore.position, gore.velocity, hitboxSize, hitboxSize, 0f, fall: true);
			gore.position.X = slopeVelPos.X;
			gore.position.Y = slopeVelPos.Y;
			gore.velocity.X = slopeVelPos.Z;
			gore.velocity.Y = slopeVelPos.W;

			if (gore.timeLeft > 0)
			{
				gore.timeLeft -= GoreID.Sets.DisappearSpeed[Type];
			}
			else
			{
				gore.alpha += GoreID.Sets.DisappearSpeedAlpha[Type];
			}

			if (gore.alpha >= 255)
			{
				gore.active = false;
			}

			gore.position += gore.velocity;
			return false;
		}
	}
}
