﻿using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Tiles.Trees
{
    public class SprinkleCactus : ModCactus
    {
        public override void SetStaticDefaults() //Add a IL edit that adds a map color to this
        {
            GrowsOnTileId = [ModContent.TileType<Creamsand>()];
        }

        public override Asset<Texture2D> GetTexture()
        {
            return ModContent.Request<Texture2D>("TheConfectionRebirth/Tiles/Trees/SprinkleCactus");
        }

        public override Asset<Texture2D> GetFruitTexture()
        {
            return ModContent.Request<Texture2D>("TheConfectionRebirth/Tiles/Trees/SprinkleCactusPear");
        }
    }
}
