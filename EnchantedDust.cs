using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Galactica
{
	public class EnchantedDust : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.alpha = 0;
			dust.scale = 1.1f;
			dust.velocity *= -0.25f;
			dust.noGravity = true;
			dust.noLight = true;
			dust.frame = new Rectangle(0, Main.rand.Next(0, 27) * 16, 16, 16);
		}

		public override bool Update(Dust dust)
		{
			dust.position += dust.velocity;
			int oldAlpha = dust.alpha;
			dust.alpha = (int)(dust.alpha * 1.1);
			if (dust.alpha == oldAlpha)
			{
				dust.alpha++;
			}
			if (dust.alpha >= 255)
			{
				dust.alpha = 255;
				dust.active = false;
			}
			return false;
		}
	}
}