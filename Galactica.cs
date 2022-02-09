using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Events;
using Terraria.GameContent.Generation;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.World.Generation;
using System;
using System.Runtime;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;
using System.Windows;
using Galactica.Items;
using Galactica.Items.Weapons;
using Galactica.Items.Weapons.Alchemy;
using Galactica.NPCs.cosmicorb;
using Galactica.NPCs.CosmicGuardians;
using Galactica.NPCs.galactica_i;
using Galactica.NPCs.worm;
using Galactica.NPCs.iceking;
using Galactica.NPCs.burningdemon;
using Galactica.NPCs.galactica_ii;
using Galactica.NPCs.dark;
using Galactica.NPCs.conglomerate;
using Galactica.NPCs.harpyking;
using Galactica.NPCs.Fracture;
using Galactica.NPCs.HauntedKnife;
using Microsoft.Win32;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Graphics;
using ReLogic.Localization.IME;
using ReLogic.Utilities;
using Terraria;
using Terraria.Achievements;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.Cinematics;
using Terraria.DataStructures;
using Terraria.Enums;
using Galactica.Items.Weapons.Alchemy.celestial;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Galactica.NPCs.Slimer;

namespace Galactica
{
	public class Galactica : Mod
	{
        	internal SkillsPannel skillsPannel;
		public static ModHotKey Teleport;
		public static Mod acc;
		public static bool accLoaded;
       		public UserInterface skillsInterface;
		public static float Clamp(float orig, float min, float max)
        {
			if (orig > max) return max;
			if (orig < min) return min;
			return orig;
        }
		public static Vector2 Slerp(Vector2 start, Vector2 end, float percent)
		{
			// Dot product - the cosine of the angle between 2 vectors.
			float dot = Vector2.Dot(start, end);
			// Clamp it to be in the range of Acos()
			// This may be unnecessary, but floating point
			// precision can be a fickle mistress.
			Clamp(dot, -1.0f, 1.0f);
			// Acos(dot) returns the angle between start and end,
			// And multiplying that by percent returns the angle between
			// start and the final result.
			float theta = (float)Math.Acos(dot) * percent;
			Vector2 RelativeVec = end - start * dot;
			RelativeVec.Normalize();
			// Orthonormal basis
			// The final result.
			return ((start * (float)Math.Cos(theta)) + (RelativeVec * (float)Math.Sin(theta)));
		}
		public override void Unload(){
			Texture2D[] oldMoonTexture = new Texture2D[3]{ ModContent.GetTexture("Terraria/Moon_0"), ModContent.GetTexture("Terraria/Moon_1"), ModContent.GetTexture("Terraria/Moon_2")};
			Main.moonTexture = oldMoonTexture;
			Teleport = null;
			EXPlayer.dHeartText = null;
			EXPlayer.cHeartText = null;
			EXPlayer.gHeartText = null;
			EXPlayer.crHeartText = null;
			EXPlayer.glHeartText = null;
		}
		public static uint RoundToN(uint inp, uint n)
        {
			return (uint)Math.Ceiling(inp * 1.0f / n) * n;
        }
		public override void ModifySunLightColor(ref Color tileColor, ref Color backgroundColor) {
			if(!world.BlueMoon) return;
			backgroundColor = new Color(15, 33, 63);
			tileColor = new Color(0, 99, 255);
		}
		internal UserInterface NarratorUserInterface;
		internal UserInterface AlchemyUserInterface;
		internal UserInterface SorceryUserInterface;
		public static Color ColorAverageCauseLerpDoesntWorkAAA(Color c, Color c2)
        {
			Color NewColor = new Color((int)Math.Sqrt((c.R ^ 2 + c2.R ^ 2) / 2), (int)Math.Sqrt((c.G ^ 2 + c2.G ^ 2) / 2), (int)Math.Sqrt((c.B ^ 2 + c2.B ^ 2) / 2));
			return NewColor; 
        }
		public static Color ColorAdd(Color col1, Color col2){
			float r = col1.R + col2.R;
			float g = col1.G + col2.G;
			float b = col1.B + col2.B;
			if (col1.R + col2.R > 255) r = col1.R + col2.R - 255;
			if (col1.G + col2.G > 255) g = col1.G + col2.G - 255;
			if (col1.B + col2.B > 255) b = col1.B + col2.B - 255;
			return new Color(r, g, b);
		}
		public static string ColorText(List<Color> col, string text)
		{
			string returntxt = "";
			List<char> texts = text.ToList();
			for(int i = 0; i < texts.Count; i++){
				returntxt += "[c/" + col[i].R.ToString("X2") + col[i].G.ToString("X2") + col[i].B.ToString("X2") + ":" + texts[i] + "]";
			}
			return returntxt;
		}
		public override void AddRecipeGroups()
		{
			RecipeGroup group = new RecipeGroup(() => "Any Copper Bar", new int[]
			{
				ItemID.CopperBar,
				ItemID.TinBar
			});
			RecipeGroup.RegisterGroup("Galactica:anyCopper", group);
			RecipeGroup group1 = new RecipeGroup(() => "Any Iron Bar", new int[]
			{
				ItemID.IronBar,
				ItemID.LeadBar
			});
			RecipeGroup.RegisterGroup("Galactica:anyIron", group1);
			RecipeGroup group2 = new RecipeGroup(() => "Any Gold Bar", new int[]
			{
				ItemID.GoldBar,
				ItemID.PlatinumBar
			});
			RecipeGroup.RegisterGroup("Galactica:anyGold", group2);
			RecipeGroup group3 = new RecipeGroup(() => "Any Evil Bar", new int[]
			{
				ItemID.DemoniteBar,
				ItemID.CrimtaneBar
			});
			RecipeGroup.RegisterGroup("Galactica:anyEvil", group3);
			RecipeGroup group4 = new RecipeGroup(() => "Any Hardmode t1 Bar", new int[]
			{
				ItemID.CobaltBar,
				ItemID.PalladiumBar
			});
			RecipeGroup.RegisterGroup("Galactica:anyCobalt", group4);
			RecipeGroup group6 = new RecipeGroup(() => "Any Hardmode t2 Bar", new int[]
			{
				ItemID.MythrilBar,
				ItemID.OrichalcumBar
			});
			RecipeGroup.RegisterGroup("Galactica:anyMythril", group6);
			RecipeGroup group5 = new RecipeGroup(() => "Any Hardmode t3 Bar", new int[]
			{
				ItemID.TitaniumBar,
				ItemID.AdamantiteBar
			});
			RecipeGroup.RegisterGroup("Galactica:anyTitanium", group5);
			RecipeGroup group7 = new RecipeGroup(() => "Any Evil Material", new int[]
			{
				ItemID.ShadowScale,
				ItemID.TissueSample
			});
			RecipeGroup.RegisterGroup("Galactica:anyScale", group7);
		}
		public override void Load()
		{

			if(Main.netMode != NetmodeID.Server)
            {
				Ref<Effect> bocDistort = new Ref<Effect>(GetEffect("Effects/BocDistort"));
				Filters.Scene["BocDistort"] = new Filter(new ScreenShaderData(bocDistort, "ExampleEffectScreen"), EffectPriority.High);
			}
			Dictionary<int, string> nTexts = new Dictionary<int, string>();
			nTexts.Add(this.ItemType("eee"), "");
			nTexts.Add(this.ItemType("starlighthazmatbody"), "It is an ancient set crafted to combat against the starlight sickness");
			nTexts.Add(this.ItemType("starlighthazmathead"), "It is an ancient set crafted to combat against the starlight sickness");
			nTexts.Add(this.ItemType("starlighthazmatlegs"), "It is an ancient set crafted to combat against the starlight sickness");
			nTexts.Add(this.ItemType("terrabow"), "The origional owner of these heroic weapons was dark, You are just some copycat");
			nTexts.Add(this.ItemType("terradisk"), "The origional owner of these heroic weapons was dark, You are just some copycat");
			nTexts.Add(this.ItemType("terraflamethrower"), "The origional owner of these heroic weapons was dark, You are just some copycat");
			nTexts.Add(this.ItemType("terraknives"), "The origional owner of these heroic weapons was dark, You are just some copycat");
			nTexts.Add(this.ItemType("terramutilator"), "The origional owner of these heroic weapons was dark, You are just some copycat");
			nTexts.Add(this.ItemType("TerraTurret"), "The origional owner of these heroic weapons was dark, You are just some copycat");
			nTexts.Add(ItemID.TerraBlade, "The origional owner of these heroic weapons was dark, You are just some copycat");
			nTexts.Add(this.ItemType("terrastaff"), "The origional owner of these heroic weapons was dark, You are just some copycat");
			nTexts.Add(this.ItemType("terrasteel"), "The origional owner of these heroic weapons was dark, You are just some copycat");
			nTexts.Add(this.ItemType("terrayoyo"), "The origional owner of these heroic weapons was dark, You are just some copycat");
			nTexts.Add(this.ItemType("terratome"), "The origional owner of these heroic weapons was dark, You are just some copycat");
			nTexts.Add(this.ItemType("terraflask"), "The origional owner of these heroic weapons was dark, You are just some copycat");
			nTexts.Add(this.ItemType("TerraKnife"), "The origional owner of these heroic weapons was dark, You are just some copycat");
			nTexts.Add(this.ItemType("zenithblaster"), "One of your own, A creation unlike anything from this world before, Maybe combining other weapons will bring different results");
			nTexts.Add(this.ItemType("zenithbow"), "One of your own, A creation unlike anything from this world before, Maybe combining other weapons will bring different results");
			nTexts.Add(this.ItemType("zenith2"), "One of your own, A creation unlike anything from this world before, Maybe combining other weapons will bring different results");
			nTexts.Add(this.ItemType("zenithstaff"), "One of your own, A creation unlike anything from this world before, Maybe combining other weapons will bring different results");
			nTexts.Add(this.ItemType("zenithtome"), "One of your own, A creation unlike anything from this world before, Maybe combining other weapons will bring different results");
			nTexts.Add(this.ItemType("zenithyoyo"), "One of your own, A creation unlike anything from this world before, Maybe combining other weapons will bring different results");
			nTexts.Add(this.ItemType("zenithflask"), "One of your own, A creation unlike anything from this world before, Maybe combining other weapons will bring different results");
			nTexts.Add(this.ItemType("Zenith6"), "One of your own, A creation unlike anything from this world before, Maybe combining other weapons will bring different results");
			nTexts.Add(this.ItemType("soulfragment"), "A fragment of an elders soul, Creation or Chaos, who knows, either way it holds unimaginable power");
			nTexts.Add(this.ItemType("ConcentratedDarkMatter"), "A fuel used by the ancient civilisation, It is highly reactive and can cause mass distruction in the wrong hands");
			nTexts.Add(this.ItemType("Omega"), "A prototype of the Aleph Null, it is a weaker version crafted out of the starlight worms essence");
			nTexts.Add(this.ItemType("aleph0"), "A real hero blade, Used by dark to banish her enemies before the elders stopped her, Its as dense as neutronium");
			nTexts.Add(this.ItemType("UltimatumFragment"), "By either luck or skill the strongest creatures of each stage have gathered the shattered fragments of one of the elders struck down by all the others");
			nTexts.Add(this.ItemType("CoreofTheUniverse"), "It seems like you have come across her magnum opus, a truly divine level of power rests within this crystal, it was created as a way to store power from the elder gods incase of an emergency so someone else could take over, i guess youre the one to finally finish the job that dark had started");
			nTexts.Add(this.ItemType("FinalDawn"), "One of your finest Creations, Dark would love this if she wasnt so mad at you, Maybe it can be used against the Elder Gods");
			nTexts.Add(this.ItemType("Electromagnetism"), "One of your finest Creations, Dark would love this if she wasnt so mad at you, Maybe it can be used against the Elder Gods");
			nTexts.Add(this.ItemType("Heavenpeircer"), "One of your finest Creations, Dark would love this if she wasnt so mad at you, Maybe it can be used against the Elder Gods");
			nTexts.Add(this.ItemType("AdAstra"), "One of your finest Creations, Dark would love this if she wasnt so mad at you, Maybe it can be used against the Elder Gods");
			nTexts.Add(this.ItemType("MetallicPlating"), "A sheet of concentrated dark metal, It might beable to hold pure concentrated dark matter");
			ModGlobalItem.NarratorTexts = nTexts;
			NarratorUserInterface = new UserInterface();
			AlchemyUserInterface = new UserInterface();
			SorceryUserInterface = new UserInterface();
			acc = ModLoader.GetMod("UpgradedAccessories");
			accLoaded = acc != null;
			Teleport = RegisterHotKey("Teleport Home", "P"); // See https://docs.microsoft.com/en-us/previous-versions/windows/xna/bb197781(v=xnagamestudio.41) for special keys
			if (!Main.dedServ)
			{
				skillsPannel = new SkillsPannel();
				skillsPannel.Initialize();
				skillsInterface = new UserInterface();
				skillsInterface.SetState(skillsPannel);
			}
			//Every thing that text is equal to and in this list automatically starts with "Galactica: "
			// e.g "Galactica: Now in space" or "Galactica: Happy Birthday Luna!"
			//Except Deus Ex which turns into "Deus Ex: Galactica" in reference to the item
			List<string> _title = new List<string>{
				"Now in space",
				"New and improved",
				"Too many worms",
				"Inter-Galactic(a)",
				"To the moon!",
				"string text = _title[Main.rand.Next(0, _title.Count)]",
				"Galactica: Galactica: Galactica:",
				"The Rebirth",
				"Not 2.0 (yet)",
				"Poggers Edition",
				"#BF00FF",
				"Deus Ex",
				"Now slower than the speed of light",
				"The superior content mod (probably not idk)",
				"2 Return of Galacticus",
				"This isnt even my final form",
				":acitcalaG",
				"Now with less souls",
				"Kills 99% of space worms (no it doesnt)",
				"The Ascension",
				"More holy than before",
				"Lol",
				"Rawr XD",
				"Galacticool"
			};
			DateTime now = DateTime.Now;
			string text = _title[Main.rand.Next(0, _title.Count)];
			if (now.Day == 4 && now.Month == 12) text = "Happy Birthday, Luna!";
			if (now.Day == 23 && now.Month == 9) text = "Happy Birthday, Chris!";
			if (now.Day == 24 && now.Month == 8) text = "Happy Birthday, Rowing!";
			try {
				GameWindow window = Main.instance.Window;
				GameWindow gameWindow = (GameWindow)typeof(Game).GetProperty("Window", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).GetValue(Main.instance);
				GameWindow gameWindow2 = window ?? gameWindow;
				if (gameWindow2 != null)
				{
					gameWindow2.Title = text == "Deus Ex" ? "Deus Ex: Galactica" : $"Galactica: {text}";
				}
			}
            catch
            {
				Main.NewText("Something went wrong", Color.White);
            }
			}

        	public override void UpdateUI(GameTime gameTime) {
			NarratorUserInterface?.Update(gameTime);
			AlchemyUserInterface?.Update(gameTime);
			SorceryUserInterface?.Update(gameTime);
		}

        	public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        	{
				int inventoryIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
				if (inventoryIndex != -1) {
				layers.Insert(inventoryIndex, new LegacyGameInterfaceLayer(
					"Galactica: Narrator UI",
					delegate {
						// If the current UIState of the UserInterface is null, nothing will draw. We don't need to track a separate .visible value.
						NarratorUserInterface.Draw(Main.spriteBatch, new GameTime());
						return true;
					},
					InterfaceScaleType.UI)
				);
				layers.Insert(inventoryIndex, new LegacyGameInterfaceLayer(
					"Galactica: Alchemy UI",
					delegate {
						// If the current UIState of the UserInterface is null, nothing will draw. We don't need to track a separate .visible value.
						AlchemyUserInterface.Draw(Main.spriteBatch, new GameTime());
						return true;
					},
					InterfaceScaleType.UI)
				);
				layers.Insert(inventoryIndex, new LegacyGameInterfaceLayer(
					"Galactica: Enchanting UI",
					delegate {
						// If the current UIState of the UserInterface is null, nothing will draw. We don't need to track a separate .visible value.
						SorceryUserInterface.Draw(Main.spriteBatch, new GameTime());
						return true;
					},
					InterfaceScaleType.UI)
				);
			}
        	}

        	private bool DrawSkillsPannel()
        	{
            // it will only draw if the player is not on the main menu
            		if (!Main.gameMenu && SkillsPannel.visible)
            		{
                		skillsInterface.Draw(Main.spriteBatch, new GameTime());
            		}
            		return true;
        	}
		public bool Overkiller;
		private void RecipeBrowser_AddToCategory(string name, string category, string texture, Predicate<Item> predicate)
		{
			Mod recipeBrowser = ModLoader.GetMod("RecipeBrowser");
			if (recipeBrowser != null && !Main.dedServ)
			{
				recipeBrowser.Call(new object[5]
				{
					"AddItemCategory",
					name,
					category,
					((Mod)this).GetTexture(texture),
					predicate
				});
			}
		}

		public override void PostSetupContent() {
			Item item2 = new Item();
			AlchemyWeapon.addWeapon(item2);
			Mod RecipeBrowser = ModLoader.GetMod("RecipeBrowser");
			if (RecipeBrowser != null) {
				RecipeBrowser_AddToCategory("Alchemy", "Weapons", "Items/Weapons/Alchemy/alchemyemblem", (Item item) => ModGlobalItem.alchemyWeapons.Contains(item.type));
				RecipeBrowser_AddToCategory("Galactica Throwing", "Weapons", "Items/Weapons/Throwing/CopperKunai", (Item item) => ModGlobalItem.throwingWeapons.Contains(item.type));
			}

			Mod bossChecklist = ModLoader.GetMod("BossChecklist");
			if (bossChecklist != null) {
				bossChecklist.Call(
					"AddBoss",
					0.1f,
					ModContent.NPCType<harpyking>(),
					this, // Mod
					"The Harpy King",
					(Func<bool>)(() => world.downedHarpy),
					ModContent.ItemType<mysticalfeather>(),
					new List<int> { },
					new List<int> {ModContent.ItemType<extremeharpyfeather>()},
					"Spawned using a [i:" + ModContent.ItemType<mysticalfeather>() + "] anywhere",
					"The Harpy King has executed all players"
				);
				bossChecklist.Call(
					"AddBoss",
					5.9f,
					ModContent.NPCType<HauntedKnife>(),
					this, // Mod
					"Haunted Blade",
					(Func<bool>)(() => world.downedKnife),
					ModContent.ItemType<ancientflames>(),
					new List<int> { },
					new List<int> {ModContent.ItemType<BladePiece>()},
					"Spawned using an [i:" + ModContent.ItemType<ancientflames>() + "] in hell",
					"The Haunted blade leaves to reap another soul"
				);
				bossChecklist.Call(
					"AddBoss",
					6.1f,
					ModContent.NPCType<burningdemon>(),
					this, // Mod
					"The Burning Demon",
					(Func<bool>)(() => world.downedDemon),
					ModContent.ItemType<ancientflames>(),
					new List<int> { },
					new List<int> {ModContent.ItemType<fireblast>(), ModContent.ItemType<sunfury>(), ModContent.ItemType<reddevil>(), ModContent.ItemType<burningore>()},
					"Spawned using an [i:" + ModContent.ItemType<ancientflames>() + "] in hell",
					"The Burning demon has scorched all players"
				);
				bossChecklist.Call(
					"AddBoss",
					8f,
					ModContent.NPCType<theconglomerate>(),
					this, // Mod
					"The Conglomerate",
					(Func<bool>)(() => world.downedCong),
					ModContent.ItemType<miniaturereactor>(),
					new List<int> { },
					new List<int> {ModContent.ItemType<purifieduranium>()},
					"Spawned using  [i:" + ModContent.ItemType<miniaturereactor>() + "] anywhere",
					"You have been radiated"
				);
				bossChecklist.Call(
					"AddBoss",
					9f,
					ModContent.NPCType<FracturedSoul>(),
					this, // Mod
					"The Fractured Soul",
					(Func<bool>)(() => world.downedFracture),
					ModContent.ItemType<SoulFracture>(),
					new List<int> { },
					new List<int> { },
					"Spawned using  [i:" + ModContent.ItemType<SoulFracture>() + "] anywhere",
					"You hear crying and shouting from away"
				);
				bossChecklist.Call(
					"AddBoss",
					11f,
					ModContent.NPCType<CoagulatedBlood>(),
					this, // Mod
					"The Evil Deities",
					(Func<bool>)(() => world.downedDeities),
					ModContent.ItemType<InfestedHeart>(),
					new List<int> { },
					new List<int> { },
					"Spawned using  [i:" + ModContent.ItemType<InfestedHeart>() + "] anywhere",
					"You hear the beating being to fade"
				);
				bossChecklist.Call(
					"AddBoss",
					15f,
					ModContent.NPCType<cosmicorb>(),
					this, // Mod
					"Cosmic Energy Field",
					(Func<bool>)(() => world.downedOrb),
					ModContent.ItemType<enchantedstar>(),
					new List<int> { },
					new List<int> {3456, 3457, 3458, 3459, ModContent.ItemType<atomfragment>(), ModContent.ItemType<universalessence>()},
					"Spawned using an [i:" + ModContent.ItemType<enchantedstar>() + "] at night",
					"Cosmic Energy Field flees"
				);
				bossChecklist.Call(
					"AddBoss",
					16f,
					new List<int> { ModContent.NPCType<DevilsSerpentHead>(), ModContent.NPCType<SoulSuckerHead>(), ModContent.NPCType<SkyEaterHead>() },
					this, // Mod
					"Cosmic Guardians",
					(Func<bool>)(() => world.downedSlime),
					ModContent.ItemType<cosmicjelly>(),
					new List<int> { },
					new List<int> {ModContent.ItemType<electrolizedgel>()},
					"Spawned using a [i:" + ModContent.ItemType<cosmicjelly>() + "] anywhere",
					"The Cosmic Guardians flees"
				);
				bossChecklist.Call(
					"AddBoss",
					17f,
					ModContent.NPCType<galactica_i>(),
					this, // Mod
					"The Observer",
					(Func<bool>)(() => world.downedGalacticaI),
					ModContent.ItemType<galaxiteshard>(),
					new List<int> { },
					new List<int> {ModContent.ItemType<galaxitebar>()},
					"Spawned using a [i:" + ModContent.ItemType<galaxiteshard>() + "] at night",
					"The Observer flees"
				);
				bossChecklist.Call(
					"AddBoss",
					18f,
					ModContent.NPCType<StarlightHead>(),
					this, // Mod
					"Starlight Worm",
					(Func<bool>)(() => world.downedWorm),
					ModContent.ItemType<starlightbeacon>(),
					new List<int> { },
					new List<int> {ModContent.ItemType<starlightfragment>()},
					"Spawned using a [i:" + ModContent.ItemType<starlightbeacon>() + "] preferably at night",
					"The Starlight Worm flees"
				);
				bossChecklist.Call(
					"AddBoss",
					19f,
					ModContent.NPCType<iceking>(),
					this, // Mod
					"Ice King",
					(Func<bool>)(() => world.downedIce),
					ModContent.ItemType<icechunk>(),
					new List<int> { },
					new List<int> {ModContent.ItemType<aurealisore>()},
					"Spawned using an [i:" + ModContent.ItemType<icechunk>() + "] in the ice biome",
					"The Ice King flees"
				);
				bossChecklist.Call(
					"AddBoss",
					20f,
					ModContent.NPCType<galactica_ii>(),
					this, // Mod
					"Galactica II",
					(Func<bool>)(() => world.downedGalacticaII),
					ModContent.ItemType<galacticbeacon>(),
					new List<int> { },
					new List<int> {ModContent.ItemType<galacticshard>()},
					"Spawned using a [i:" + ModContent.ItemType<galacticbeacon>() + "] at night",
					"Galactica II flees"
				);
				bossChecklist.Call(
					"AddBoss",
					21f,
					ModContent.NPCType<Luna>(),
					this, // Mod
					"???",
					(Func<bool>)(() => world.downedDark),
					ModContent.ItemType<catears>(),
					new List<int> { },
					new List<int> { },
					"Spawned using a ??? anywhere",
					"Dark flees"
				);
				// Additional bosses here			
			}
		}

    }
}
