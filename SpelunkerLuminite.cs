using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace Galactica
{
	internal sealed class SpelunkerLuminite : GlobalTile
	{
		public override void SetDefaults(){
			Main.tileSpelunker[TileID.LunarOre] = true;
		}
		public override void PostDraw(int i, int j, int type, SpriteBatch spriteBatch)
		{
			if(Main.tile[i, j].type == mod.TileType("DeepRockTile") && !NPC.AnyNPCs(mod.NPCType("awakecat")) && !NPC.AnyNPCs(mod.NPCType("phase2"))){
				WorldGen.KillTile(i, j);
			}
		}
	}
}