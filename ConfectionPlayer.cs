using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace TheConfectionRebirth
{
	public class ConfectionPlayer : ModPlayer
	{
		public bool cookiestPet;

		public override void ResetEffects()
		{
			cookiestPet = false;
		}

		public override void GetDyeTraderReward(List<int> rewardPool)
		{
			rewardPool.Add(ModContent.ItemType<Items.CandyCornDye>());
			rewardPool.Add(ModContent.ItemType<Items.FoaminWispDye>());
			rewardPool.Add(ModContent.ItemType<Items.GummyWispDye>());
			rewardPool.Add(ModContent.ItemType<Items.SwirllingChocolateDye>());
		}
	}
}
