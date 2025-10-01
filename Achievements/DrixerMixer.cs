using Terraria.Achievements;
using Terraria.ModLoader;
using TheConfectionRebirth.Items.Weapons;

namespace TheConfectionRebirth.Achievements
{
	public class DrixerMixer : ModAchievement
	{
		public override void SetStaticDefaults()
		{
			Achievement.SetCategory(AchievementCategory.Collector);

			AddItemCraftCondition(new int[] { ModContent.ItemType<Drixer>(), ModContent.ItemType<Pix>() });
		}

		public override Position GetDefaultPosition() => new After("DRAX_ATTAX");
	}
}
