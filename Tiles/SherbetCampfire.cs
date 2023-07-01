using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace TheConfectionRebirth.Tiles
{
    //Look in ConfectionCampfire.cs in TheConfectionRebirth/Tiles for documentation
    public class SherbetCampfire : ModTile
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
            AdjTiles = new int[] { TileID.Campfire };

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.addTile(Type);

            AnimationFrameHeight = 36;
            TileID.Sets.HasOutlines[Type] = true;
            LocalizedText name = CreateMapEntryName();
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
                player.AddBuff(BuffID.Campfire, 5);
                Main.buffNoTimeDisplay[BuffID.Campfire] = true;
            }

            if (Main.tile[i, j].TileFrameX < 52 && (int)Vector2.Distance(player.Center / 16f, new Vector2((float)i + 0.5f, (float)j + 0.5f)) <= 3 && player.HeldItem.type == ItemID.MarshmallowonaStick)
            {
                Timer++;
                if (Timer > 2200)
                {
                    player.HeldItem.TurnToAir();
                    player.QuickSpawnItem(Entity.GetSource_None(), ItemID.CookedMarshmallow, 1);
                    Timer = 0;
                }
            }

            if (Main.tile[i, j].TileFrameX < 52 && Main.rand.NextBool(5))
            {
                int num162 = Dust.NewDust(new Vector2(i * 16, j * 16), 0, -16, DustID.Smoke, 0, -2f, 128, default, 1f);
                Main.dust[num162].noGravity = true;
            }
        }

        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            frame = Main.tileFrame[TileID.Campfire];
            frameCounter = Main.tileFrameCounter[TileID.Campfire];
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = ModContent.ItemType<Items.Placeable.SherbetCampfire>(); 
        }

        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) => true;

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            if (Main.tile[i, j].TileFrameX < 52)
            {
                r = 2f;
                g = 0.5f;
                b = 0.5f;
            }
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];

            int frameYOffset = Main.tileFrame[Type] * AnimationFrameHeight;
            
            int xPos = Main.tile[i, j].TileFrameX;
            int yPos = Main.tile[i, j].TileFrameY + frameYOffset;

            Texture2D glowmask = ModContent.Request<Texture2D>("TheConfectionRebirth/Tiles/SherbetCampfire_Glow").Value; 
            Vector2 zero = (Vector2)(Main.drawToScreen ? Vector2.Zero : new Vector2((float)Main.offScreenRange));
            Vector2 drawOffset = new Vector2((float)(i * 16) - Main.screenPosition.X, (float)(j * 16) - Main.screenPosition.Y) + zero;
            Main.spriteBatch.Draw(glowmask, drawOffset, (Rectangle?)new Rectangle(xPos, yPos, 18, 18), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }

        public override void HitWire(int i, int j)
        {
            ConfectionHitWire.HitWire(Type, i, j, 3, 2);
        }

        public override bool RightClick(int i, int j)
        {
            HitWire(i, j);
            return true;
        }

		public override bool CanDrop(int i, int j) {
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 48, 32, ModContent.ItemType<Items.Placeable.SherbetCampfire>());
			return false;
		}
	}
}
