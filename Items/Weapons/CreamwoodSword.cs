using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Weapons
{
    public class CreamwoodSword : ModItem
    {
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 1;
		}
		
        public override void SetDefaults()
        {
			Item.useStyle = 1;
			Item.useTurn = false;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.width = 24;
			Item.height = 28;
			Item.damage = 30;
			Item.knockBack = 7f;
			Item.UseSound = SoundID.Item1;
			Item.scale = 1f;
			Item.value = 100;
			Item.DamageType = DamageClass.Melee;
			Item.autoReuse = true;
		}
    }
}