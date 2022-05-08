using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace TheConfectionRebirth.NPCs
{
    public class NpcDrops : GlobalNPC
    {
        public override void OnKill(NPC npc)
        {
 
            /*if (npc.type == Mod.Find<ModNPC>("Rollercookie").Type)
            {
                if (Main.rand.Next(0) == 0)
                {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Mod.Find<ModItem>("CookieDough").Type, Main.rand.Next(1, 2));
                }
				
                if (Main.rand.Next(100) == 0)
                {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Mod.Find<ModItem>("ChocolateChunk").Type, 1);
                }
				
                if (Main.rand.Next(20) == 0)
                {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Mod.Find<ModItem>("CookieMask").Type, 1);
                }
				
				if (Main.rand.Next(20) == 0)
                {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Mod.Find<ModItem>("CookieShirt").Type, 1);
                }
				
				if (Main.rand.Next(20) == 0)
                {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Mod.Find<ModItem>("CookiePants").Type, 1);
                }
			}	
			if (npc.type == Mod.Find<ModNPC>("Rollercookie_2").Type)
            {
                if (Main.rand.Next(0) == 0)
                {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Mod.Find<ModItem>("CookieDough").Type, Main.rand.Next(1, 2));
                }
				
                if (Main.rand.Next(100) == 0)
                {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Mod.Find<ModItem>("ChocolateChunk").Type, 1);
                }
				
                if (Main.rand.Next(20) == 0)
                {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Mod.Find<ModItem>("CookieMask").Type, 1);
                }
				
				if (Main.rand.Next(20) == 0)
                {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Mod.Find<ModItem>("CookieShirt").Type, 1);
                }
				
				if (Main.rand.Next(20) == 0)
                {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Mod.Find<ModItem>("CookiePants").Type, 1);
                }
			}
			if (npc.type == Mod.Find<ModNPC>("BirthdayCookie").Type)
            {
                if (Main.rand.Next(0) == 0)
                {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Mod.Find<ModItem>("CookieDough").Type, Main.rand.Next(1, 2));
                }
				
                if (Main.rand.Next(100) == 0)
                {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Mod.Find<ModItem>("ChocolateChunk").Type, 1);
                }
				
                if (Main.rand.Next(0) == 0)
                {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Mod.Find<ModItem>("TopCake").Type, 1);
                }
				
				if (Main.rand.Next(0) == 0)
                {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Mod.Find<ModItem>("BirthdaySuit").Type, 1);
                }
				
				if (Main.rand.Next(0) == 0)
                {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Mod.Find<ModItem>("RightTrousers").Type, 1);
                }
			}
		    if (npc.type == Mod.Find<ModNPC>("ParfaitSlime").Type)
            {
                if (Main.rand.Next(0) == 0)
                {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Gel, Main.rand.Next(1, 3));
                }
			}	
			if (npc.type == Mod.Find<ModNPC>("ParfaitSlime_2").Type)
            {
                if (Main.rand.Next(0) == 0)
                {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Gel, Main.rand.Next(1, 3));
                }
			}
			if (npc.type == Mod.Find<ModNPC>("CrazyCone").Type)
            {
                if (Main.rand.Next(100) == 0)
                {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Nazar, Main.rand.Next(1));
                }
			}
		    if (npc.type == Mod.Find<ModNPC>("SweetGummy").Type)
            {
                if (Main.rand.Next(10) == 0)
                {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Mod.Find<ModItem>("CreamPuff").Type, 1);
                }
				
                if (Main.rand.Next(100) == 0)
                {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.TrifoldMap, 1);
                }
				
				if (Main.rand.Next(95) == 0)
                {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Mod.Find<ModItem>("GummyMask").Type, 1);
                }
				
				if (Main.rand.Next(95) == 0)
                {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Mod.Find<ModItem>("GummyShirt").Type, 1);
                }
				
				if (Main.rand.Next(95) == 0)
                {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Mod.Find<ModItem>("GummyPants").Type, 1);
                }
			}
			if (npc.type == Mod.Find<ModNPC>("SweetGummy_1").Type)
            {
                if (Main.rand.Next(10) == 0)
                {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Mod.Find<ModItem>("CreamPuff").Type, 1);
                }
				
                if (Main.rand.Next(100) == 0)
                {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.TrifoldMap, 1);
                }
				
				if (Main.rand.Next(95) == 0)
                {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Mod.Find<ModItem>("GummyMask").Type, 1);
                }
				
				if (Main.rand.Next(95) == 0)
                {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Mod.Find<ModItem>("GummyShirt").Type, 1);
                }
				
				if (Main.rand.Next(95) == 0)
                {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Mod.Find<ModItem>("GummyPants").Type, 1);
                }
			}
			if (npc.type == Mod.Find<ModNPC>("SweetGummy_2").Type)
            {
                if (Main.rand.Next(10) == 0)
                {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Mod.Find<ModItem>("CreamPuff").Type, 1);
                }
				
                if (Main.rand.Next(100) == 0)
                {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.TrifoldMap, 1);
                }
				
				if (Main.rand.Next(95) == 0)
                {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Mod.Find<ModItem>("GummyMask").Type, 1);
                }
				
				if (Main.rand.Next(95) == 0)
                {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Mod.Find<ModItem>("GummyShirt").Type, 1);
                }
				
				if (Main.rand.Next(95) == 0)
                {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Mod.Find<ModItem>("GummyPants").Type, 1);
                }
			}
			if (npc.type == Mod.Find<ModNPC>("SweetGummy_3").Type)
            {
                if (Main.rand.Next(10) == 0)
                {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Mod.Find<ModItem>("CreamPuff").Type, 1);
                }
				
                if (Main.rand.Next(100) == 0)
                {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.TrifoldMap, 1);
                }
				
				if (Main.rand.Next(95) == 0)
                {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Mod.Find<ModItem>("GummyMask").Type, 1);
                }
				
				if (Main.rand.Next(95) == 0)
                {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Mod.Find<ModItem>("GummyShirt").Type, 1);
                }
				
				if (Main.rand.Next(95) == 0)
                {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Mod.Find<ModItem>("GummyPants").Type, 1);
                }
			}
			if (npc.type == Mod.Find<ModNPC>("WildWilly").Type)
            {
                if (Main.rand.Next(10) == 0)
                {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Mod.Find<ModItem>("CcretTicket").Type, 1);
                }
				
				if (Main.rand.Next(0) == 0)
                {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Mod.Find<ModItem>("WonkyHat").Type, 1);
                }
				
				if (Main.rand.Next(0) == 0)
                {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Mod.Find<ModItem>("WonkyCoat").Type, 1);
                }
				
				if (Main.rand.Next(0) == 0)
                {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Mod.Find<ModItem>("WonkyTrousers").Type, 1);
                }
			}
			if (npc.type == Mod.Find<ModNPC>("FoaminFloat").Type)
            {
                if (Main.rand.Next(100) == 0)
                {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Mod.Find<ModItem>("CcretTicket").Type, 1);
                }
			}
			if (npc.type == Mod.Find<ModNPC>("SherbetSlime").Type)
            {
                if (Main.rand.Next(0) == 0)
                {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Mod.Find<ModItem>("SherbetBricks").Type, 45);
                }
			}
			if (npc.type == Mod.Find<ModNPC>("StripedPigron").Type)
            {
                if (Main.rand.Next(3) == 0)
                {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Bacon, 1);
                }
			}
			//if (npc.type == Mod.Find<ModNPC>("MeetyMummy").Type)
            //{
                //if (Main.rand.Next(5) == 0)
                //{
                //Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Mod.Find<ModItem>("SoulofSpite").Type, 1);
                //}
				//if (Main.rand.Next(10) == 0)
                //{
                //Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Mod.Find<ModItem>("CanofMeat").Type, 1);
                //}
				//if (Main.rand.Next(95) == 0)
                //{
                //Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.MummyMask, 1);
                //}
				//if (Main.rand.Next(95) == 0)
                //{
                //Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.MummyShirt, 1);
                //}
				//if (Main.rand.Next(95) == 0)
                //{
                //Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.MummyPants, 1);
                //}
				//if (Main.rand.Next(100) == 0)
                //{
                //Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.TrifoldMap, 1);
                //}
				//if (Main.rand.Next(100) == 0)
                //{
                //Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Megaphone, 1);
                //}
			//}
			if (npc.type == Mod.Find<ModNPC>("BigMimicConfection").Type) //Thanks to Neobind for making the mimic texture. Love ur work X3
            {
                if (Main.rand.Next(4) == 0)
                {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Mod.Find<ModItem>("CookieCrumbler").Type, 1);
                }
                if (Main.rand.Next(4) == 0)
                {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Mod.Find<ModItem>("SweetTooth").Type, 1);
                }
                if (Main.rand.Next(4) == 0)
                {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Mod.Find<ModItem>("SweetHook").Type, 1);
                }
				if (Main.rand.Next(4) == 0)
                {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Mod.Find<ModItem>("CreamSpray").Type, 1);
                }
                if (Main.rand.Next(0) == 0)
                {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.GreaterHealingPotion, 10);
                }
                if (Main.rand.Next(0) == 0)
                {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.GreaterManaPotion, 10);
                }
			}
			if (npc.type == Mod.Find<ModNPC>("Sprinkling").Type)
            {
                if (Main.rand.Next(2) == 0)
                {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Mod.Find<ModItem>("Sprinkles").Type, 1);
                }
			}
			if (npc.type == NPCID.Gastropod)
            { 
			    if (Main.rand.Next(5) == 0)
                {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Mod.Find<ModItem>("ShellBlock").Type, 15);
                }
			}
			if (npc.type == Mod.Find<ModNPC>("Iscreamer").Type)
            {
                if (Main.rand.Next(100) == 0)
                {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Mod.Find<ModItem>("BearClaws").Type, 1);
                }
				if (Main.rand.Next(500) == 0)
                {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Mod.Find<ModItem>("DimensionSplit").Type, 1);
                }
			}
			if (npc.type == Mod.Find<ModNPC>("CreamsandWitchPhase2").Type)
			{
				if (Main.rand.Next(5) == 0)
				{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Mod.Find<ModItem>("CreamHat").Type, 1);
				}
				if (Main.rand.Next(5) == 0)
				{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Mod.Find<ModItem>("CookieCorset").Type, 1);
				}
				if (Main.rand.Next(5) == 0)
				{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Mod.Find<ModItem>("CakeDress").Type, 1);
				}
				if (Main.rand.Next(1) == 0)
				{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Mod.Find<ModItem>("Creamsand").Type, Main.rand.Next(30, 50));
				}
				if (Main.rand.Next(10) == 0)
				{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Mod.Find<ModItem>("PixieStick").Type, 1);
				}
				if (Main.rand.Next(10) == 0)
				{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Mod.Find<ModItem>("CreamySandwhich").Type, 1);
				}
			}
			if (npc.type == Mod.Find<ModNPC>("TheUnfirm").Type)
			{
				if (Main.rand.Next(33) == 0)
				{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Mod.Find<ModItem>("AdmiralHat").Type, 1);
				}
                if (Main.rand.Next(0) == 0)
                {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Marshmallow, Main.rand.Next(20, 30));
                }
			}*/
		}
	 }
}
