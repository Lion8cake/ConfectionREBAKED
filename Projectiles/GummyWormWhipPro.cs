using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.Audio;
using System.IO;

namespace TheConfectionRebirth.Projectiles
{
	public class GummyWormWhipPro : ModProjectile
	{
		internal int Variation;

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.IsAWhip[Type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.width = 18;
			Projectile.height = 18;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ownerHitCheck = true;
			Projectile.extraUpdates = 1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			Projectile.WhipSettings.Segments = 10;
			Projectile.WhipSettings.RangeMultiplier = 1.5f;
		}

		private float Timer
		{
			get => Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}

		private float ChargeTime
		{
			get => Projectile.ai[1];
			set => Projectile.ai[1] = value;
		}

		public override void AI()
		{
			Player owner = Main.player[Projectile.owner];
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

			Projectile.Center = Main.GetPlayerArmPosition(Projectile) + Projectile.velocity * Timer;
			Projectile.spriteDirection = Projectile.velocity.X >= 0f ? 1 : -1;

			/*if (!Charge(owner))
			{
				return;
			}*/

			Timer++;

			float swingTime = owner.itemAnimationMax * Projectile.MaxUpdates;
			if (Timer >= swingTime || owner.itemAnimation <= 0)
			{
				Projectile.Kill();
				return;
			}

			owner.heldProj = Projectile.whoAmI;
			if (Timer == swingTime / 2)
			{
				List<Vector2> points = Projectile.WhipPointsForCollision;
				Projectile.FillWhipControlPoints(Projectile, points);
				SoundEngine.PlaySound(SoundID.Item153, points[^1]);
			}
		}

		/*
		private bool Charge(Player owner)
		{
			if (!owner.channel || ChargeTime >= 120)
			{
				return true;
			}

			ChargeTime++;

			if (ChargeTime % 12 == 0)
				Projectile.WhipSettings.Segments++;

			Projectile.WhipSettings.RangeMultiplier += 1 / 120f;

			owner.itemAnimation = owner.itemAnimationMax;
			owner.itemTime = owner.itemTimeMax;

			return false;
		}
		*/

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
			Projectile.damage = (int)(hit.Damage * 0.7f);
		}

		private static void DrawLine(List<Vector2> list)
		{
			Texture2D texture = TextureAssets.FishingLine.Value;
			Rectangle frame = texture.Frame();
			Vector2 origin = new(frame.Width / 2, 2);

			Vector2 pos = list[0];
			for (int i = 0; i < list.Count - 1; i++)
			{
				Vector2 element = list[i];
				Vector2 diff = list[i + 1] - element;

				float rotation = diff.ToRotation() - MathHelper.PiOver2;
				Color color = Lighting.GetColor(element.ToTileCoordinates(), Main.DiscoColor);
				Vector2 scale = new(1, (diff.Length() + 2) / frame.Height);

				Main.EntitySpriteDraw(texture, pos - Main.screenPosition, frame, color, rotation, origin, scale, SpriteEffects.None, 0);

				pos += diff;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			List<Vector2> list = new();
			Projectile.FillWhipControlPoints(Projectile, list);

			DrawLine(list);

			SpriteEffects flip = Projectile.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

			Main.instance.LoadProjectile(Type);
			Texture2D texture = TextureAssets.Projectile[Type].Value;

			Vector2 pos = list[0];

			for (int i = 0; i < list.Count - 1; i++)
			{
				Rectangle frame = new(Variation * 18, 0, 18, 26);
				Vector2 origin = new(5, 8);
				Vector2 scale = new(1f, 2f);

				if (i == list.Count - 2)
				{
					frame.Y = 74;
					frame.Height = 18;

					Projectile.GetWhipSettings(Projectile, out float timeToFlyOut, out int _, out float _);
					float t = Timer / timeToFlyOut;
					scale.X = MathHelper.Lerp(0.5f, 1.5f, Utils.GetLerpValue(0.1f, 0.7f, t, true) * Utils.GetLerpValue(0.9f, 0.7f, t, true));
				}
				else if (i > 10)
				{
					frame.Y = 58;
					frame.Height = 16;
				}
				else if (i > 5)
				{
					frame.Y = 42;
					frame.Height = 16;
				}
				else if (i > 0)
				{
					frame.Y = 26;
					frame.Height = 16;
				}

				Vector2 element = list[i];
				Vector2 diff = list[i + 1] - element;

				float rotation = diff.ToRotation() - MathHelper.PiOver2;
				Color color = Lighting.GetColor(element.ToTileCoordinates());

				Main.EntitySpriteDraw(texture, pos - Main.screenPosition, frame, color, rotation, origin, scale, flip, 0);

				pos += diff;
			}
			return false;
		}

		public override ModProjectile Clone(Projectile newEntity)
		{
			var v = (GummyWormWhipPro)base.Clone(newEntity);
			v.Variation = Variation;
			return v;
		}

		public override void SendExtraAI(BinaryWriter writer) => writer.Write((byte)Variation);

		public override void ReceiveExtraAI(BinaryReader reader) => Variation = reader.ReadByte();
	}
}
