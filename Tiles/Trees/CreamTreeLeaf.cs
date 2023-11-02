using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;

namespace TheConfectionRebirth.Tiles.Trees
{
	public class CreamTreeLeaf : ModGore
	{
		public override string Texture => "TheConfectionRebirth/Tiles/Trees/CreamTree_Leaf";

		public override void SetStaticDefaults() {
			ChildSafety.SafeGore[Type] = true; 
			GoreID.Sets.SpecialAI[Type] = 3;
			GoreID.Sets.PaintedFallingLeaf[Type] = true;
		}
	}
}
