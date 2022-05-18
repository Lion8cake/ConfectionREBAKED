using Terraria;

namespace TheConfectionRebirth
{
    public static class ConfectionGlobalTile2
    {
        public static ConfectionPlayer Confection(this Player player)
        {
            return player.GetModPlayer<ConfectionPlayer>();
        }
    }
}