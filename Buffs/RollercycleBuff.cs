using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Buffs
{
	public class RollercycleBuff : ModBuff
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Rollercycle Mount");
			Description.SetDefault("'What a sweet ride!'");
			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex) {
			player.mount.SetMount(ModContent.MountType<Mounts.Rollercycle>(), player);
			player.buffTime[buffIndex] = 10;
		}
	}
}
