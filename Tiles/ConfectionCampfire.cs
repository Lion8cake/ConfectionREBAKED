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
            AdjTiles = new int[] { TileID.Campfire };

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.addTile(Type);

            AnimationFrameHeight = 36;
            TileID.Sets.HasOutlines[Type] = true;
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Campfire");
            AddMapEntry(new Color(106, 65, 51), name);
        }

        public override void NumDust(int x, int y, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            Player player = Main.LocalPlayer;
            if (Main.tile[i, j].TileFrameY < 288 && (int)Vector2.Distance(player.Center / 16f, new Vector2((float)i, (float)j)) <= 125)
            {
                player.AddBuff(BuffID.Campfire, 60);
                Main.buffNoTimeDisplay[BuffID.Campfire] = true;
            }

            if (Main.tile[i, j].TileFrameY < 288 && (int)Vector2.Distance(player.Center / 16f, new Vector2((float)i + 0.5f, (float)j + 0.5f)) <= 3 && player.HeldItem.type == ItemID.MarshmallowonaStick)
            {
                Timer++;
                if (Main.rand.NextBool(5))
                {
                    Timer++;
                }
                if (Timer > 2200)
                {
                    player.HeldItem.TurnToAir();
                    player.QuickSpawnItem(Entity.GetSource_None(), ItemID.CookedMarshmallow, 1);
                    Timer = 0;
                }
            }

            if (Main.rand.NextBool(5))
            {
                int num162 = Dust.NewDust(new Vector2(i * 16, j * 16), 0, 0, DustID.Smoke, 0, -2f, 128, default, 1f);
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
            player.cursorItemIconID = ModContent.ItemType<Items.Placeable.ConfectionCampfire>();
        }

        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) => true;

        public override void KillMultiTile(int x, int y, int frameX, int frameY)
        {
            Item.NewItem(new EntitySource_TileBreak(x, y), x * 16, y * 16, 48, 32, ModContent.ItemType<Items.Placeable.ConfectionCampfire>());
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 1.45f;
            g = 2.41f;
            b = 2.47f;
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];
            Texture2D texture = ModContent.Request<Texture2D>("TheConfectionRebirth/Tiles/ConfectionCampfire").Value;
            Texture2D glowTexture = ModContent.Request<Texture2D>("TheConfectionRebirth/Tiles/ConfectionCampfire_Glow").Value;

            Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);

            int height = tile.TileFrameY % AnimationFrameHeight == 36 ? 16 : 16;

            int frameYOffset = Main.tileFrame[Type] * AnimationFrameHeight;

            spriteBatch.Draw(
                texture,
                new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero,
                new Rectangle(tile.TileFrameX, tile.TileFrameY + frameYOffset, 16, height),
                Lighting.GetColor(i, j), 0f, default, 1f, SpriteEffects.None, 0f);

            spriteBatch.Draw(
                glowTexture,
                new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero,
                new Rectangle(tile.TileFrameX, tile.TileFrameY + frameYOffset, 16, height),
                Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            return false;
        }

        public override void HitWire(int i, int j)
        {
            int x = i - Main.tile[i, j].TileFrameX / 18 % 3;
            int y = j - Main.tile[i, j].TileFrameY / 18 % 2;
            for (int l = x; l < x + 3; l++)
            {
                for (int m = y; m < y + 2; m++)
                {
                    if (Main.tile[l, m].HasTile && Main.tile[l, m].TileType == Type)
                    {
                        if (Main.tile[l, m].TileFrameY < 36)
                        {
                            Main.tile[l, m].TileFrameY += 36;
                        }
                        else
                        {
                            Main.tile[l, m].TileFrameY -= 36;
                        }
                    }
                }
            }
            if (Wiring.running)
            {
                Wiring.SkipWire(x, y);
                Wiring.SkipWire(x, y + 1);
                Wiring.SkipWire(x, y + 2);
                Wiring.SkipWire(x + 1, y);
                Wiring.SkipWire(x + 1, y + 1);
                Wiring.SkipWire(x + 1, y + 2);
            }
            NetMessage.SendTileSquare(-1, x, y + 1, 3);
        }

        public override bool RightClick(int i, int j)
        {
            HitWire(i, j);
            return true;
        }
    }
}
