using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using TheConfectionRebirth.Projectiles;

namespace TheConfectionRebirth.Items.Weapons
{
	public class GummyWormWhip : ModItem
	{
		private int uses;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gummy Worm-whip");
			SacrificeTotal = 1;
		}

		public override void SetDefaults()
		{
			Item.DamageType = DamageClass.SummonMeleeSpeed;
			Item.damage = 60;
			Item.knockBack = 2;
			Item.rare = ItemRarityID.Pink;

			Item.shoot = ModContent.ProjectileType<GummyWormWhipPro>();
			Item.shootSpeed = 4;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.UseSound = SoundID.Item152;
			Item.channel = false;
			Item.noMelee = true;
			Item.noUseGraphic = true;
		}

		public override bool MeleePrefix() => true;

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) => uses = (uses + 1) % 5;

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			int index = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
			(Main.projectile[index].ModProjectile as GummyWormWhipPro).Variation = uses - 1;
			if (Main.netMode == NetmodeID.Server)
				NetMessage.SendData(MessageID.SyncProjectile, number: index);
			return false;
		}

		public override void SaveData(TagCompound tag) => tag["uses"] = uses;

		public override void LoadData(TagCompound tag) => uses = tag.GetInt("uses");

		public override void NetSend(BinaryWriter writer) => writer.Write(uses);

		public override void NetReceive(BinaryReader reader) => uses = reader.ReadInt32();
	}
}
