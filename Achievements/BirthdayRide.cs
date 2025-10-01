using Terraria.Achievements;
using Terraria.GameContent.Achievements;
using Terraria.ModLoader;
using TheConfectionRebirth.Items.Weapons;

namespace TheConfectionRebirth.Achievements
{
	public class BirthdayRide : ModAchievement
	{
		public CustomFlagCondition BirthdaySuitRollerCookieRide { get; private set; }

		public override void SetStaticDefaults()
		{
			Achievement.SetCategory(AchievementCategory.Challenger);

			BirthdaySuitRollerCookieRide = AddCondition();
		}

		public override Position GetDefaultPosition() => new After("RAINBOWS_AND_UNICORNS");
	}
}
