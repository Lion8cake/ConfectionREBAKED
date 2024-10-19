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
	}
}
