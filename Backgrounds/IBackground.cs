using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace TheConfectionRebirth.Backgrounds
{
	internal interface IBackground
	{
		abstract Asset<Texture2D> GetFarTexture(int i);
		abstract Asset<Texture2D> GetCloseTexture(int i);
		abstract Asset<Texture2D> GetMidTexture(int i);
		abstract Asset<Texture2D> GetUltraFarTexture(int i);
	}
}
