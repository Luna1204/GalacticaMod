using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ModLoader;
using Terraria.World.Generation;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Events;
using Terraria.ModLoader.IO;
using System;
using System.IO;
using System.Linq;
using Galactica.Items;
using Terraria.Graphics.Effects;
using Galactica.Items.StarlightWastes;
using Terraria.Utilities;
using Galactica.Items.FleshPit;

namespace Galactica
{
	public class world : ModWorld
	{
		public static bool overkillmode = false;
		public static bool ultramode;
		public static bool downedKnife;
		public static bool heartmode;
		public static bool flawless;
		public static bool downedCong = false;			
		public static float health;
		public static bool downedDemon = false;
		public static bool downedFracture = false;
		public static bool downedDeities;
		public static bool downedOrb = false;
		public static bool downedSlime = false;
		public static bool downedGalacticaI = false;
		public static bool downedWorm = false;
		public static bool downedIce = false;
		public static bool downedGalacticaII = false;
		public static bool downedDark = false;
		public static bool downedHarpy = false;
		public static int radTiles;
		public static int starTiles;
		public static int fleshTiles;
		public static bool BlueMoon;
		public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight) {
			// Because world generation is like layering several images ontop of each other, we need to do some steps between the original world generation steps.

			// The first step is an Ore. Most vanilla ores are generated in a step called "Shinies", so for maximum compatibility, we will also do this.
			// First, we find out which step "Shinies" is.
			int ShiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Shinies"));
			if (ShiniesIndex != -1) {
				// Next, we insert our step directly after the original "Shinies" step. 
				// ExampleModOres is a method seen below.
				tasks.Insert(ShiniesIndex + 1, new PassLegacy("Aqualite Ore", AqualiteOre));
			}
			int starIndex = tasks.FindIndex((GenPass genpass) => genpass.Name.Equals("Micro Biomes"));
			if (starIndex != -1)
			{
				tasks.Insert(starIndex + 1, new PassLegacy("Starlight Ocean", StarOcean));
			}
			
				int LivingTreesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Living Trees"));
			if (LivingTreesIndex != -1) {
				tasks.Insert(LivingTreesIndex + 1, new PassLegacy("Post Terrain", delegate (GenerationProgress progress) {
					// We can inline the world generation code like this, but if exceptions happen within this code 
					// the error messages are difficult to read, so making methods is better. This is called an anonymous method.
					progress.Message = "Never seen a crashed ship before?";
					MakeShips();
				}));
			}
			if (LivingTreesIndex != -1)
			{
				tasks.Insert(LivingTreesIndex + 1, new PassLegacy("Post Terrain", delegate (GenerationProgress progress) {
					// We can inline the world generation code like this, but if exceptions happen within this code 
					// the error messages are difficult to read, so making methods is better. This is called an anonymous method.
					FleshPit(progress);
				}));
			}
		}
		public override void ResetNearbyTileEffects() {
			starTiles = 0;
			fleshTiles = 0;
			radTiles = 0;
		}	

		public override void Initialize()
		{
			heartmode = false;
			downedDemon = false;
			downedKnife = false;
			downedFracture = false;
			downedDeities = false;
			downedOrb = false;
			downedSlime = false;
			downedCong = false;
			downedGalacticaI = false;
			downedWorm = false;
			downedIce = false;
			downedGalacticaII = false;
			downedDark = false;
			downedHarpy = false;
			overkillmode = false;
			ultramode = false;
		}
		public static Vector2 shippos;
		public override TagCompound Save()
		{
			List<Vector2> positions = new List<Vector2>();
			positions.Add(shippos);
			List<string> downed = new List<string>();
			if (downedDemon)
			{
				downed.Add("Demon");
			}
			if (downedKnife)
			{
				downed.Add("Knife");
			}
			if (downedFracture)
			{
				downed.Add("Fracture");
			}
			if (downedHarpy)
			{
				downed.Add("Harpy");
			}
			if (downedCong)
			{
				downed.Add("Cong");
			}
            if (downedDeities)
            {
				downed.Add("Deities");
            }
			if (downedOrb)
			{
				downed.Add("Orb");
			}
			if (downedSlime)
			{
				downed.Add("Slime");
			}
			if (downedGalacticaI)
			{
				downed.Add("GalacticaI");
			}
			if (downedWorm)
			{
				downed.Add("Worm");
			}
			if (downedIce)
			{
				downed.Add("Ice");
			}
			if (downedGalacticaII)
			{
				downed.Add("GalacticaII");
			}
			if (downedDark)
			{
				downed.Add("Dark");
			}
			if (overkillmode)
			{
				downed.Add("overkillmode");
			}
			if (ultramode)
			{
				downed.Add("ultramode");
			}
			return new TagCompound
			{
				{
					"downed",
					downed

				},
				{
					"positions",
					positions
				}
			};
		}
		public override void Load(TagCompound tag)
		{
			IList<string> list = tag.GetList<string>("downed");
			IList<Vector2> list2 = tag.GetList<Vector2>("positions");
			shippos = list2[0];
			downedHarpy = list.Contains("Harpy");
			downedKnife = list.Contains("Knife");
			downedFracture = list.Contains("Fracture");
			downedCong = list.Contains("Cong");
			downedOrb = list.Contains("Orb");
			downedDeities = list.Contains("Deities");
			downedGalacticaI = list.Contains("GalacticaI");
			downedGalacticaII = list.Contains("GalacticaII");
			downedIce = list.Contains("Ice");
			downedWorm = list.Contains("Worm");
			downedDark = list.Contains("Dark");
			overkillmode = list.Contains("overkillmode");
			ultramode = list.Contains("ultramode");
		}
		public static bool[] ValidTiles = TileID.Sets.Factory.CreateBoolSet(true, 21, 31, 26);

		public void FleshPit(GenerationProgress progress)
		{
			if (WorldGen.crimson)
			{
				UnifiedRandom genRand = WorldGen.genRand;
				progress.Message = Lang.gen[20].Value;
				int num510 = genRand.Next(320, Main.maxTilesX - 320);
				int num511 = num510 - genRand.Next(200) - 100;
				int num512 = num510 + genRand.Next(200) + 100;
				for (int i = 0; i < 15; i++)
				{
					ChasmRunner(num511 + (i * 30), (int)WorldGen.worldSurfaceLow, genRand.Next(150) + 150, makeOrb: false);

				}
			}
			
		}
		public static void ChasmRunner(int i, int j, int steps, bool makeOrb = false)
		{
			ushort fleshTile = (ushort)ModContent.TileType<LivingStone>();
			UnifiedRandom genRand = WorldGen.genRand;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			if (!makeOrb)
			{
				flag2 = true;
			}
			float num = steps;
			Vector2 vector = default(Vector2);
			vector.X = i;
			vector.Y = j;
			Vector2 vector2 = default(Vector2);
			vector2.X = (float)genRand.Next(-10, 11) * 0.1f;
			vector2.Y = (float)genRand.Next(11) * 0.2f + 0.5f;
			int num2 = 5;
			double num3 = genRand.Next(5) + 7;
			while (num3 > 0.0)
			{
				if (num > 0f)
				{
					num3 += (double)genRand.Next(3);
					num3 -= (double)genRand.Next(3);
					if (num3 < 7.0)
					{
						num3 = 7.0;
					}
					if (num3 > 20.0)
					{
						num3 = 20.0;
					}
					if (num == 1f && num3 < 10.0)
					{
						num3 = 10.0;
					}
				}
				else if ((double)vector.Y > Main.worldSurface + 45.0)
				{
					num3 -= (double)genRand.Next(4);
				}
				if ((double)vector.Y > Main.rockLayer && num > 0f)
				{
					num = 0f;
				}
				num -= 1f;
				int num4;
				int num5;
				int num6;
				int num7;
				if (num > (float)num2)
				{
					num4 = (int)((double)vector.X - num3 * 0.5);
					num5 = (int)((double)vector.X + num3 * 0.5);
					num6 = (int)((double)vector.Y - num3 * 0.5);
					num7 = (int)((double)vector.Y + num3 * 0.5);
					if (num4 < 0)
					{
						num4 = 0;
					}
					if (num5 > Main.maxTilesX - 1)
					{
						num5 = Main.maxTilesX - 1;
					}
					if (num6 < 0)
					{
						num6 = 0;
					}
					if (num7 > Main.maxTilesY)
					{
						num7 = Main.maxTilesY;
					}
					for (int k = num4; k < num5; k++)
					{
						for (int l = num6; l < num7; l++)
						{
							if ((double)(Math.Abs((float)k - vector.X) + Math.Abs((float)l - vector.Y)) < num3 * 0.5 * (1.0 + (double)genRand.Next(-10, 11) * 0.015) && Main.tile[k, l].type != 31 && Main.tile[k, l].type != TileID.Crimtane)
							{
								Main.tile[k, l].active(active: false);
							}
						}
					}
				}
				if (num <= 2f && (double)vector.Y < Main.worldSurface + 45.0)
				{
					num = 2f;
				}
				if (num <= 0f)
				{
					if (!flag2)
					{
						flag2 = true;
					}
					else if (!flag3)
					{
						flag3 = false;
						bool flag4 = false;
						int num8 = 0;
						while (!flag4)
						{
							int num9 = genRand.Next((int)vector.X - 25, (int)vector.X + 25);
							int num10 = genRand.Next((int)vector.Y - 50, (int)vector.Y);
							if (num9 < 5)
							{
								num9 = 5;
							}
							if (num9 > Main.maxTilesX - 5)
							{
								num9 = Main.maxTilesX - 5;
							}
							if (num10 < 5)
							{
								num10 = 5;
							}
							if (num10 > Main.maxTilesY - 5)
							{
								num10 = Main.maxTilesY - 5;
							}
							if ((double)num10 > Main.worldSurface)
							{
								WorldGen.Place3x2(num9, num10, TileID.Crimtane);
								if (Main.tile[num9, num10].type == TileID.Crimtane)
								{
									flag4 = true;
									continue;
								}
								num8++;
								if (num8 >= 10000)
								{
									flag4 = true;
								}
							}
							else
							{
								flag4 = true;
							}
						}
					}
				}
				vector += vector2;
				vector2.X += (float)genRand.Next(-10, 11) * 0.01f;
				if ((double)vector2.X > 0.3)
				{
					vector2.X = 0.3f;
				}
				if ((double)vector2.X < -0.3)
				{
					vector2.X = -0.3f;
				}
				num4 = (int)((double)vector.X - num3 * 1.1);
				num5 = (int)((double)vector.X + num3 * 1.1);
				num6 = (int)((double)vector.Y - num3 * 1.1);
				num7 = (int)((double)vector.Y + num3 * 1.1);
				if (num4 < 1)
				{
					num4 = 1;
				}
				if (num5 > Main.maxTilesX - 1)
				{
					num5 = Main.maxTilesX - 1;
				}
				if (num6 < 0)
				{
					num6 = 0;
				}
				if (num7 > Main.maxTilesY)
				{
					num7 = Main.maxTilesY;
				}
				for (int m = num4; m < num5; m++)
				{
					for (int n = num6; n < num7; n++)
					{
						if ((double)(Math.Abs((float)m - vector.X) + Math.Abs((float)n - vector.Y)) < num3 * 1.1 * (1.0 + (double)genRand.Next(-10, 11) * 0.015))
						{
							if (Main.tile[m, n].type != fleshTile && n > j + genRand.Next(3, 20))
							{
								Main.tile[m, n].active(active: true);
							}
							if (steps <= num2)
							{
								Main.tile[m, n].active(active: true);
							}
							if (Main.tile[m, n].type != 31)
							{
								Main.tile[m, n].type = fleshTile;
							}
						}
					}
				}
				for (int num11 = num4; num11 < num5; num11++)
				{
					for (int num12 = num6; num12 < num7; num12++)
					{
						if ((double)(Math.Abs((float)num11 - vector.X) + Math.Abs((float)num12 - vector.Y)) < num3 * 1.1 * (1.0 + (double)genRand.Next(-10, 11) * 0.015))
						{
							if (Main.tile[num11, num12].type != 31)
							{
								Main.tile[num11, num12].type = fleshTile;
							}
							if (steps <= num2)
							{
								Main.tile[num11, num12].active(active: true);
							}
							if (num12 > j + genRand.Next(3, 20))
							{
								Main.tile[num11, num12].wall = WallID.CrimstoneUnsafe;
							}
						}
					}
				}
			}
		}
		public void StarOcean(GenerationProgress generationProgress)
        {
			generationProgress.Message = "Starlight Wastes";
			int x = WorldGen.genRand.Next((int)(Main.maxTilesX / 1.01f), Main.maxTilesX);
			int y = (int)WorldGen.worldSurfaceLow; // WorldGen.worldSurfaceLow is actually the highest surface tile. In practice you might want to use WorldGen.rockLayer or other WorldGen values.
			int found = 1;
			for (int i2 = 0; i2 != found; i2 = i2 + 0)
			{
				int num3 = WorldGen.genRand.Next(0, Main.maxTilesX);
				int y3 = WorldGen.genRand.Next((int)WorldGen.rockLayer, Main.maxTilesY);
				if (Main.tile[num3, y3].type == TileID.Sand)
				{
					found = 0;
					WorldGen.TileRunner(num3, y3, (double)WorldGen.genRand.Next(150, 180), WorldGen.genRand.Next(150, 180), ModContent.TileType<StarlightSand>(), false, 0f, 0f, false, true);
				}
			}
		}
		public void AqualiteOre(GenerationProgress progress) {
			progress.Message = "Aqualite Ore";
			int x = WorldGen.genRand.Next(0, Main.maxTilesX);
			int y = WorldGen.genRand.Next((int)WorldGen.worldSurfaceLow, Main.maxTilesY); // WorldGen.worldSurfaceLow is actually the highest surface tile. In practice you might want to use WorldGen.rockLayer or other WorldGen values.
			for (int k = 0; k < (int)((Main.maxTilesX * Main.maxTilesY) * 2E-07); k++) {
				int found = 1;
				for(int i2 = 0; i2 != found; i2 = i2 + 0){
					int num3 = WorldGen.genRand.Next(0, Main.maxTilesX);
					int y3 = WorldGen.genRand.Next((int)WorldGen.rockLayer, Main.maxTilesY);
					if(Main.tile[num3, y3].type == TileID.Sand) {
						found = 0;
						WorldGen.TileRunner(num3, y3, (double)WorldGen.genRand.Next(5, 8), WorldGen.genRand.Next(5,8), ModContent.TileType<aqualiteoretile>(), false, 0f, 0f, false, true);
					}
					else{
						num3 = WorldGen.genRand.Next(0, Main.maxTilesX);
						y3 = WorldGen.genRand.Next((int)WorldGen.rockLayer, Main.maxTilesY);
					}
				}
			}
		}
		private void MakeShips() {
			float widthScale = Main.maxTilesX / 4200f;
			int numberToGenerate = WorldGen.genRand.Next(1, (int)(2f * widthScale));
			for (int k = 0; k < numberToGenerate; k++) {
				bool success = false;
				int attempts = 0;
				while (!success) {
					attempts++;
					if (attempts > 1000) {
						success = true;
						continue;
					}
					int i = WorldGen.genRand.Next(300, Main.maxTilesX - 300);
					if (i <= Main.maxTilesX / 2 - 50 || i >= Main.maxTilesX / 2 + 50) {
						int j = 0;
						while (!Main.tile[i, j].active() && (double)j < Main.worldSurface) {
							j++;
						}
						if ((Main.tile[i, j].type == TileID.Sand)) {
							j--;
							if (j > 150) {
								bool placementOK = true;
								for (int l = i - 4; l < i + 4; l++) {
									for (int m = j - 6; m < j + 20; m++) {
										if (Main.tile[l, m].active()) {
											int type = (int)Main.tile[l, m].type;
											if (!(type == TileID.Sand)) {
												placementOK = false;
											}
										}
									}
								}
								if (placementOK) {
									shippos = new Vector2(i, j);
									success = PlaceShip(i, j);
								}
							}
						}
					}
				}
			}
		}
		private readonly int[,] _shipshape = {
			{0,0,0,0,0,0,0,0,0,0,2,2,2,0,0,0 },
			{0,0,0,0,0,0,0,0,0,2,2,0,2,2,0,0 },
			{0,0,0,0,0,0,0,0,2,2,0,0,0,2,0,0 },
			{0,0,0,0,0,1,1,2,2,0,0,0,0,2,0,0 },
			{0,0,0,0,1,1,2,2,4,4,4,4,4,2,2,0 },
			{0,1,1,1,1,2,2,0,0,0,0,0,0,0,0,0 },
			{1,1,2,2,2,2,0,0,0,0,0,0,0,0,0,0 },
			{1,2,2,2,0,0,0,5,0,0,0,0,0,3,0,0 },
			{1,2,2,0,0,0,2,2,2,2,2,2,2,2,2,1 },
			{1,2,2,2,2,2,2,2,1,1,1,1,1,1,1,1 },
			{1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0 },
		};
		public bool PlaceShip(int i, int j) {
			if (Main.tile[i, j].active()) {
				return false;
			}
			if (j < 150) {
				return false;
			}
				for (int y = 0; y < _shipshape.GetLength(0); y++) {
				for (int x = 0; x < _shipshape.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if (WorldGen.InWorld(k, l, 30)) {
						Tile tile = Framing.GetTileSafely(k, l);
						switch (_shipshape[y, x]) {
							case 1:
								tile.type = 151;
								tile.active(true);
								break;
							case 2:
								tile.type = (ushort)ModContent.TileType<brokenplatingtile>();
								tile.active(true);
								break;
							case 3:
								tile.type = 10;
								tile.active(true);
								break;
							case 4:
								tile.type = 19;
								tile.active(true);
								break;
							case 5:
								tile.type = 21;
								tile.active(true);
								AddChestWithLoot(x, y, 21, 1u, 0);
								break;
							}
						
					}
				}
			}
			return true;
		}
		internal static Chest AddChestWithLoot(int i, int j, ushort type = 21, uint startingSlot = 1u, int tileStyle = 0)
		{
			int chestIndex = -1;
			while (j < Main.maxTilesY - 210)
			{
				if (!WorldGen.SolidTile(i, j))
				{
					j++;
					continue;
				}
				chestIndex = WorldGen.PlaceChest(i - 1, j - 1, type, notNearOtherChests: false, tileStyle);
				break;
			}
			if (chestIndex < 0)
			{
				return null;
			}
			Chest chest = Main.chest[chestIndex];
			PlaceLootInChest(ref chest, type, startingSlot);
			return chest;
		}
		internal static void PlaceLootInChest(ref Chest chest, ushort type, uint startingSlot)
		{
			PutItemInChest(ref chest, ModContent.ItemType<aqualitebar>(), 30, 80, condition: true, startingSlot);
		}
		static void PutItemInChest(ref Chest c, int id, int minQuantity, int maxQuantity, bool condition, uint startSlot)
		{
			if (condition)
			{
				c.item[startSlot].SetDefaults(id, noMatCheck: false);
				if (minQuantity > 0)
				{
					if (maxQuantity < minQuantity)
					{
						maxQuantity = minQuantity;
					}
					c.item[startSlot].stack = WorldGen.genRand.Next(minQuantity, maxQuantity + 1);
				}
				startSlot++;
			}
		}

		public override void TileCountsAvailable(int[] tileCounts) {
			// Here we count various tiles towards ZoneExample
			starTiles = tileCounts[ModContent.TileType<starlightmeteortile>()] + tileCounts[ModContent.TileType<starlightstonetile>()] + tileCounts[ModContent.TileType<StarlightSand>()];
			radTiles = tileCounts[ModContent.TileType<radiatedmaterialtile>()];
			fleshTiles = tileCounts[ModContent.TileType<LivingStone>()];
		}
		public bool attemptedToday;
		public override void PostUpdate() { 
			if (Main.netMode != NetmodeID.Server) // This all needs to happen client-side!
    		{
				if (Main.LocalPlayer.GetModPlayer<EXPlayer>().headeache)
				{
					Filters.Scene.Activate("BocDistort");
				}
				else
				{
					Filters.Scene.Deactivate("BocDistort");
				}
			}
			Texture2D BlueMoonT = ModContent.GetTexture("Galactica/ExtraTextures/BlueMoon");
			if(BlueMoon){
				int num23 = (int)(Main.time / 32400.0 * (double)(Main.screenWidth + BlueMoonT.Width * 2)) - BlueMoonT.Width;
				int num24 = 0;
				float num25 = 1f;
				float rotation2 = (float)(Main.time / 32400.0) * 2f - 7.3f;
				SpriteBatch spriteBatch = Main.spriteBatch;
				spriteBatch.Begin();
				spriteBatch.Draw(BlueMoonT, new Vector2(num23, num24 + Main.moonModY), new Microsoft.Xna.Framework.Rectangle(0, BlueMoonT.Width, BlueMoonT.Width, BlueMoonT.Width), Color.White, rotation2, new Vector2(BlueMoonT.Width / 2, BlueMoonT.Width / 2), num25, SpriteEffects.None, 0f);
				spriteBatch.End();
			}
			if(!BlueMoon && Main.rand.NextBool(10) && !Main.dayTime && !attemptedToday){  //&& Main.rand.NextBool(20)
				Main.NewText("The Blue Moon is rising", Color.LightBlue);
				BlueMoon = true;
			}
			if(!attemptedToday) attemptedToday = true;
			if(BlueMoon && Main.dayTime){
				Main.NewText("The Blue Moon dissapears", Color.LightBlue);
				BlueMoon = false;
				attemptedToday = false;
			}
    		}
	}
}
