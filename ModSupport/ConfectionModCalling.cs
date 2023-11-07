using Terraria.ModLoader;
using Terraria.Achievements;

namespace TheConfectionRebirth
{
    public class ConfectionModCalling : ModSystem
    {
		public static readonly Mod? Achievements = ModLoader.TryGetMod("TMLAchievements", out Mod obtainedMod) ? obtainedMod : null;


		/// <summary>
		/// Checks if fargos best of both worlds is enabled
		/// </summary>
		public static bool FargoBoBW = ModLoader.HasMod("FargoSeeds") && ModContent.GetInstance<ModSupport.FargoSeedConfectionConfiguration>().BothGoods;

		public override void PostSetupContent()
		{
            if (Achievements == null)
            {
                return;
            }

            Achievements.Call("AddAchievement", ModContent.GetInstance<TheConfectionRebirth>(), "BirthdayRide", AchievementCategory.Challenger, "TheConfectionRebirth/Assets/BirthdayRide", null, false, false, 8f, new string[] { "Event_BirthdaySuitRollerCookieRide" });
            Achievements.Call("AddAchievement", ModContent.GetInstance<TheConfectionRebirth>(), "DrixerMixer", AchievementCategory.Collector, "TheConfectionRebirth/Assets/DrixerMixer", null, false, false, 8f, new string[] { "Craft_" + ModContent.ItemType<Items.Weapons.Drixer>() });
			Achievements.Call("AddAchievement", ModContent.GetInstance<TheConfectionRebirth>(), "TheBeamOfCream", AchievementCategory.Collector, "TheConfectionRebirth/Assets/TheBeamOfCream", null, false, false, 8f, new string[] { "Craft_" + ModContent.ItemType<Items.Weapons.CreamBeam>() });
		}
	}
}