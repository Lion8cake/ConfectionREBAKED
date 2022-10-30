using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace TheConfectionRebirth.Tiles
{
    //Keep in mind this is my first time making an example of some of my own code so some of my explinations might sound weird
    public class ConfectionCampfire : ModTile
    {
        public int Timer;

        public override void SetStaticDefaults()
        {
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileWaterDeath[Type] = true;
            Main.tileFrameImportant[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;
            TileID.Sets.IgnoredByNpcStepUp[Type] = true;
            Main.tileLighted[Type] = true;

            DustType = ModContent.DustType<Dusts.ChipDust>();
            AdjTiles = new int[] { TileID.Campfire }; //This sets the tile to be a campfire so some things like dst seed rain put out is already done

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.addTile(Type);

            AnimationFrameHeight = 36;
            TileID.Sets.HasOutlines[Type] = true;
            ModTranslation name = CreateMapEntryName();
            AddMapEntry(new Color(254, 121, 2), name);
        }

        public override void NumDust(int x, int y, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            Player player = Main.LocalPlayer;
            if (Main.tile[i, j].TileFrameX < 52 && (int)Vector2.Distance(player.Center / 16f, new Vector2((float)i + 0.5f, (float)j + 0.5f)) <= 125)
            {
                player.AddBuff(BuffID.Campfire, 5); //Gives the player the campfire buff when around the campfire in a 125x125 area around the campfires centre
                Main.buffNoTimeDisplay[BuffID.Campfire] = true; //Stops the buff giving the time left when its given to the player
            }

            if (Main.tile[i, j].TileFrameX < 52 && (int)Vector2.Distance(player.Center / 16f, new Vector2((float)i + 0.5f, (float)j + 0.5f)) <= 3 && player.HeldItem.type == ItemID.MarshmallowonaStick)
            {
                Timer++; //A timer for a few seconds when holding a marshmellow on a stick over the campfire
                if (Timer > 2200)
                {
                    player.HeldItem.TurnToAir(); //Deletes the Marshmellow on a stick
                    player.QuickSpawnItem(Entity.GetSource_None(), ItemID.CookedMarshmallow, 1); //Gives the player a cooked marshmellow
                    Timer = 0; //Resets the timer
                }
            }

            if (Main.tile[i, j].TileFrameX < 52 && Main.rand.NextBool(5))
            {
                int num162 = Dust.NewDust(new Vector2(i * 16, j * 16), 0, -16, DustID.Smoke, 0, -2f, 128, default, 1f); //This spawns the smoke above the campfire, its not perfect but its better than not having any smoke at all
                Main.dust[num162].noGravity = true;
            }
        }

        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            frame = Main.tileFrame[TileID.Campfire];
            frameCounter = Main.tileFrameCounter[TileID.Campfire];  //Animates the campfire tile
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = ModContent.ItemType<Items.Placeable.ConfectionCampfire>(); 
        }

        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) => true;

        public override void KillMultiTile(int x, int y, int frameX, int frameY)
        {
            Item.NewItem(new EntitySource_TileBreak(x, y), x * 16, y * 16, 48, 32, ModContent.ItemType<Items.Placeable.ConfectionCampfire>());
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            if (Main.tile[i, j].TileFrameX < 52) //Only gives the on state (the sprites on the left side) light
            {
                r = 1.45f;
                g = 2.41f;
                b = 2.47f;
            }
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            //This code allows for the tile to have a glowmask while also having a outline
            //The animated tile in ExampleMod replace the regular texture and adds both the glowmask and regular texture which means adding a highlight is very difficult to do
            Tile tile = Main.tile[i, j];

            int frameYOffset = Main.tileFrame[Type] * AnimationFrameHeight;
            
            int xPos = Main.tile[i, j].TileFrameX;
            int yPos = Main.tile[i, j].TileFrameY + frameYOffset;

            Texture2D glowmask = ModContent.Request<Texture2D>("TheConfectionRebirth/Tiles/ConfectionCampfire_Glow").Value; 
            Vector2 zero = (Vector2)(Main.drawToScreen ? Vector2.Zero : new Vector2((float)Main.offScreenRange));
            Vector2 drawOffset = new Vector2((float)(i * 16) - Main.screenPosition.X, (float)(j * 16) - Main.screenPosition.Y) + zero;
            Main.spriteBatch.Draw(glowmask, drawOffset, (Rectangle?)new Rectangle(xPos, yPos, 18, 18), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }

        public override void HitWire(int i, int j)
        {
            ConfectionHitWire.HitWire(Type, i, j, 3, 2); //See ConfectionHitWire to see how hitwire works with the campfire and other tiles like water fountains
        }

        public override bool RightClick(int i, int j)
        {
            HitWire(i, j); //makes when you right click it redirects to HitWire for the rest of the code
            return true;
        }
    }
}
