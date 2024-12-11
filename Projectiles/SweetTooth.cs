using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Projectiles
{
	public class SweetTooth : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 22;
			Projectile.height = 22;
			Projectile.aiStyle = 75;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.hide = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.ignoreWater = true;
		}

		public override bool PreAI()
		{
			Player player = Main.player[Projectile.owner];
			float num = (float)Math.PI / 2f;
			Vector2 playerPos = player.RotatedRelativePoint(player.MountedCenter);
			int itemTime = 2;
			float additionalRotation = 0f;
			num = 0f;
			if (Projectile.spriteDirection == -1)
			{
				num = (float)Math.PI;
			}
			Projectile.ai[0] += 1f;
			int itemAnimationMax = player.itemAnimationMax;
			Projectile.ai[1] -= 1f;
			bool canShoot = false;
			if (Projectile.ai[1] <= 0f)
			{
				Projectile.ai[1] = itemAnimationMax;
				canShoot = true;
			}
			bool canShoot2 = player.channel && player.HasAmmo(player.inventory[player.selectedItem]) && !player.noItems && !player.CCed;
			if (Projectile.localAI[0] > 0f)
			{
				Projectile.localAI[0] -= 1f;
			}
			if (Projectile.soundDelay <= 0 && canShoot2)
			{
				Projectile.soundDelay = itemAnimationMax;
				if (Projectile.ai[0] != 1f)
				{
					SoundEngine.PlaySound(in SoundID.Item5, Projectile.position);
				}
				Projectile.localAI[0] = 12f;
			}
			if (canShoot && Main.myPlayer == Projectile.owner)
			{
				int arrowCount = 1;
				float arrowSpawnArea = 1.5f;
				if (canShoot2)
				{
					player.PickAmmo(player.inventory[player.selectedItem], out int shoot, out float shootSpeed, out int damage, out float knockBack, out int ammoID);
					IEntitySource source = player.GetSource_ItemUse_WithPotentialAmmo(player.HeldItem, ammoID);
					knockBack = player.GetWeaponKnockback(player.inventory[player.selectedItem], knockBack);
					bool spawnEnchantment = false;
					if (++player.GetModPlayer<ConfectionPlayer>().sweetToothCounter >= 3)
					{
						player.GetModPlayer<ConfectionPlayer>().sweetToothCounter = 0;
						spawnEnchantment = true;
					}
					float rotationAmount = player.inventory[player.selectedItem].shootSpeed * Projectile.scale;
					Vector2 position = playerPos;
					Vector2 bowPos = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY) - position;
					if (player.gravDir == -1f)
					{
						bowPos.Y = (float)(Main.screenHeight - Main.mouseY) + Main.screenPosition.Y - position.Y;
					}
					Vector2 bowRotation = Vector2.Normalize(bowPos);
					if (float.IsNaN(bowRotation.X) || float.IsNaN(bowRotation.Y))
					{
						bowRotation = -Vector2.UnitY;
					}
					bowRotation *= rotationAmount;
					if (bowRotation.X != Projectile.velocity.X || bowRotation.Y != Projectile.velocity.Y)
					{
						Projectile.netUpdate = true;
					}
					Projectile.velocity = bowRotation * 0.55f;
					for (int i = 0; i < arrowCount; i++)
					{
						Vector2 velcoity = Vector2.Normalize(Projectile.velocity) * shootSpeed;
						velcoity += Main.rand.NextVector2Square(0f - arrowSpawnArea, arrowSpawnArea);
						if (float.IsNaN(velcoity.X) || float.IsNaN(velcoity.Y))
						{
							velcoity = -Vector2.UnitY;
						}
						int projID = Projectile.NewProjectile(source, position, velcoity, shoot, damage, knockBack, Projectile.owner);
						Projectile proj = Main.projectile[projID];
						proj.noDropItem = true;
						if (spawnEnchantment)
						{
							proj.GetGlobalProjectile<CookieArrowEnchantment>().isEnchanted = true;
							proj.damage = (int)(proj.damage * 1.4f);
							proj.knockBack *= 2;
							proj.penetrate += 2;
						}
					}
				}
				else
				{
					Projectile.Kill();
				}
			}
			Projectile.position = player.RotatedRelativePoint(player.MountedCenter, reverseRotation: false, addGfxOffY: false) - Projectile.Size / 2f;
			Projectile.rotation = Projectile.velocity.ToRotation() + num;
			Projectile.spriteDirection = Projectile.direction;
			Projectile.timeLeft = 2;
			player.ChangeDir(Projectile.direction);
			player.heldProj = Projectile.whoAmI;
			player.SetDummyItemTime(itemTime);
			player.itemRotation = MathHelper.WrapAngle((float)Math.Atan2(Projectile.velocity.Y * (float)Projectile.direction, Projectile.velocity.X * (float)Projectile.direction) + additionalRotation);
			return false;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			SpriteEffects dir = (SpriteEffects)0;
			if (Projectile.spriteDirection == -1)
			{
				dir = (SpriteEffects)1;
			}
			Texture2D value154 = TextureAssets.Projectile[Projectile.type].Value;
			if (Main.player[Projectile.owner].gravDir == -1f)
			{
				int dir2 = (int)dir;
				dir = (SpriteEffects)(dir2 | 2);
			}
			int num254 = TextureAssets.Projectile[Projectile.type].Height() / Main.projFrames[Projectile.type];
			int y17 = num254 * Projectile.frame;
			Vector2 vector140 = (Projectile.position + new Vector2((float)Projectile.width, (float)Projectile.height) / 2f + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition).Floor();
			float num255 = 1f;
			if (Main.player[Projectile.owner].shroomiteStealth && Main.player[Projectile.owner].inventory[Main.player[Projectile.owner].selectedItem].DamageType == DamageClass.Ranged)
			{
				float num256 = Main.player[Projectile.owner].stealth;
				if ((double)num256 < 0.03)
				{
					num256 = 0.03f;
				}
				_ = (1f + num256 * 10f) / 11f;
				lightColor *= num256;
				num255 = num256;
			}
			if (Main.player[Projectile.owner].setVortex && Main.player[Projectile.owner].inventory[Main.player[Projectile.owner].selectedItem].DamageType == DamageClass.Ranged)
			{
				float num257 = Main.player[Projectile.owner].stealth;
				if ((double)num257 < 0.03)
				{
					num257 = 0.03f;
				}
				_ = (1f + num257 * 10f) / 11f;
				lightColor = lightColor.MultiplyRGBA(new Color(Vector4.Lerp(Vector4.One, new Vector4(0f, 0.12f, 0.16f, 0f), 1f - num257)));
				num255 = num257;
			}
			Main.EntitySpriteDraw(value154, vector140, (Rectangle?)new Rectangle(0, y17, value154.Width, num254), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2((float)value154.Width / 2f, (float)num254 / 2f), Projectile.scale, dir, 0f);
			return false;
		}
	}
}
