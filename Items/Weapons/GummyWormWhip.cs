using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
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
			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults() {
			Item.DefaultToWhip(ModContent.ProjectileType<Projectiles.GummyWormWhipPro>(), 48, 2, 4);

			Item.shootSpeed = 4;
			Item.SetShopValues(ItemRarityColor.Pink5, Item.sellPrice(0, 2));
		}


		public override bool MeleePrefix() => true;

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) => uses = (uses + 1) % 5;

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			int index = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
			(Main.projectile[index].ModProjectile as GummyWormWhipPro).Variation = uses;
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
