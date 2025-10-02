using System.ComponentModel;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace TheConfectionRebirth.ModSupport
{
	//[ExtendsFromMod("FargoSeeds")]
	public class FargoSeedConfectionConfiguration : ModConfig {

		public override bool Autoload(ref string name)
		{
			return ModLoader.HasMod("FargoSeeds");
		}

		public override ConfigScope Mode => ConfigScope.ServerSide;

		public static FargoSeedConfectionConfiguration Instance => ModContent.GetInstance<FargoSeedConfectionConfiguration>();

		[Header("DrunkSeedRequiresFargosbestofbothworldsmod")]
		[Label("[i:526][i:TheConfectionRebirth/CookieDough] Both Goods")]
		[DefaultValue(true)]
		public bool BothGoods;
	}
}
