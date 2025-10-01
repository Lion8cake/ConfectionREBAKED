using Terraria.Achievements;
using Terraria.ModLoader;
using TheConfectionRebirth.Items.Weapons;

namespace TheConfectionRebirth.Achievements
{
	public class TheBeamOfCream : ModAchievement
	{
		public override void SetStaticDefaults()
		{
			Achievement.SetCategory(AchievementCategory.Collector);

			AddItemPickupCondition(ModContent.ItemType<CreamBeam>());
		}

		public override Position GetDefaultPosition() => new After("PRISMANCER");
	}
}
