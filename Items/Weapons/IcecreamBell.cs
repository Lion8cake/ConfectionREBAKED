using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Projectiles;

namespace TheConfectionRebirth.Items.Weapons
{
    public class IcecreamBell : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 38;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 4;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = 1;
            Item.noMelee = true;
            Item.knockBack = 5;
            Item.value = 200000;
            Item.rare = ItemRarityID.Pink;
			Item.UseSound = null;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<IcecreamBall>();
            Item.shootSpeed = 7.5f;
        }

		public override Vector2? HoldoutOffset() => new Vector2(-4f, 4f);

		public override void UseAnimation(Player player)
		{
			Vector2 vector2 = new Vector2(player.position.X + (float)player.width * 0.5f, player.position.Y + (float)player.height * 0.5f);
			float num5 = (float)Main.mouseX + Main.screenPosition.X - vector2.X;
			float num6 = (float)Main.mouseY + Main.screenPosition.Y - vector2.Y;
			float num7 = (float)Math.Sqrt(num5 * num5 + num6 * num6);
			float num8 = (float)Main.screenHeight / Main.GameViewMatrix.Zoom.Y;
			num7 /= num8 / 2f;
			if (num7 > 1f)
			{
				num7 = 1f;
			}
			num7 = num7 * 2f - 1f;
			if (num7 < -1f)
			{
				num7 = -1f;
			}
			if (num7 > 1f)
			{
				num7 = 1f;
			}
			num7 = (float)Math.Round(num7 * (float)Player.musicNotes);
			num7 = (Main.musicPitch = num7 / (float)Player.musicNotes);
			SoundEngine.PlaySound(SoundID.Item35 with
			{
				Pitch = num7,
			}, player.position);
			NetMessage.SendData(MessageID.InstrumentSound, -1, -1, null, player.whoAmI, num7);
		}
	}
}