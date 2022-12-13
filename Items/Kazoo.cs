using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items
{
    public class Kazoo : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 15;
            Item.useAnimation = 20;
            Item.useStyle = 2;
            Item.value = Item.buyPrice(gold: 1);
            Item.UseSound = new SoundStyle($"{nameof(TheConfectionRebirth)}/Sounds/Items/KazooSound")
            {
                Volume = 1f,
                PitchVariance = 0f,
                MaxInstances = 0,
            };
            Item.autoReuse = true;
        }

        public override void SetStaticDefaults()
        {
            SacrificeTotal = 1;
        }
    }
}