using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Weapons.Minions.GummyFish
{
	public class GummyStaff : MinionWeaponBaseClass<GummyFishSummonBuff, GummyFishSummonProj>
	{
		public override int Damage => 23;
		public override int UseTime => 36;
        public override float Knockback => 3;
        public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 28;
			Item.value = Item.buyPrice(0, 20, 0, 0);
			Item.rare = ItemRarityID.Pink;
			Item.UseSound = SoundID.Item44;
			base.SetDefaults();
		}
	}
}
