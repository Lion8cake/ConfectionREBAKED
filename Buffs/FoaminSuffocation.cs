using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.Localization;

namespace TheConfectionRebirth.Buffs
{
    public class FoaminSuffocation : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
			BuffID.Sets.LongerExpertDebuff[Type] = true;
			BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
			player.cursed = true;
			player.sticky = true;
			player.GetModPlayer<ConfectionPlayer>().CandySuffocation = true;
			player.Hurt(PlayerDeathReason.ByCustomReason(Language.GetTextValue("Mods.TheConfectionRebirth.PlayerDeathReason.ChokedOutByRootbeer", player.name)), 5, 0);
        }
    }
}
