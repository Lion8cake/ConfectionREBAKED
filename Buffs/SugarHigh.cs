using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Buffs
{
    public class SugarHigh : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sugar High!");
            Description.SetDefault("Decreased Defense");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense -= 5;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
                npc.defense -= 1;
        }
    }
}
