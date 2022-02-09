using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Galactica.Items;
using Galactica.Items.Weapons.Alchemy.celestial;
using Galactica.Items.Weapons.Throwing.Celestial;

namespace Galactica
{
	public class LunarDrops : GlobalNPC
	{
		public override void SetDefaults(NPC npc){
			List<int> all = new List<int>(){
				NPCID.StardustWormHead,
				NPCID.StardustCellBig,
				NPCID.StardustCellSmall,
				NPCID.StardustJellyfishBig,
				NPCID.StardustJellyfishSmall,
				NPCID.StardustSpiderBig,
				NPCID.StardustSpiderSmall,
				NPCID.StardustSoldier,
				NPCID.SolarCrawltipedeHead,
				NPCID.SolarDrakomire,
				NPCID.SolarDrakomireRider,
				NPCID.SolarSroller,
				NPCID.SolarCorite,
				NPCID.SolarSolenian,
				NPCID.NebulaBrain,
				NPCID.NebulaHeadcrab,
				NPCID.NebulaBeast,
				NPCID.NebulaSoldier,
				NPCID.VortexRifleman,
				NPCID.VortexHornetQueen,
				NPCID.VortexHornet,
				NPCID.VortexLarva,
				NPCID.VortexSoldier
	
			};
			if(all.Contains(npc.type)){
				npc.lifeMax = (int)(npc.lifeMax * 1.5f);
				npc.defense = (int)(npc.defense * 1.5f);
				npc.value = (int)(npc.value * 1.5f);
				npc.damage = (int)(npc.damage * 1.5f);
			}
		}
		public override void NPCLoot(NPC npc)
		{
			List<int> atom = new List<int>(){
				NPCID.StardustWormHead,
				NPCID.StardustCellBig,
				NPCID.StardustCellSmall,
				NPCID.StardustJellyfishBig,
				NPCID.StardustJellyfishSmall,
				NPCID.StardustSpiderBig,
				NPCID.StardustSpiderSmall,
				NPCID.StardustSoldier,
				NPCID.SolarCrawltipedeHead,
				NPCID.SolarDrakomire,
				NPCID.SolarDrakomireRider,
				NPCID.SolarSroller,
				NPCID.SolarCorite,
				NPCID.SolarSolenian,
				NPCID.NebulaBrain,
				NPCID.NebulaHeadcrab,
				NPCID.NebulaBeast,
				NPCID.NebulaSoldier,
				NPCID.VortexRifleman,
				NPCID.VortexHornetQueen,
				NPCID.VortexHornet,
				NPCID.VortexLarva,
				NPCID.VortexSoldier
	
			};
			List<int> stardust = new List<int>(){
				NPCID.StardustWormHead,
				NPCID.StardustCellBig,
				NPCID.StardustCellSmall,
				NPCID.StardustJellyfishBig,
				NPCID.StardustJellyfishSmall,
				NPCID.StardustSpiderBig,
				NPCID.StardustSpiderSmall,
				NPCID.StardustSoldier
	
			};
			List<int> pillars = new List<int>(){
				NPCID.LunarTowerSolar,
				NPCID.LunarTowerStardust,
				NPCID.LunarTowerVortex,
				NPCID.LunarTowerNebula
	
			};
			List<int> solar = new List<int>(){
				NPCID.SolarCrawltipedeHead,
				NPCID.SolarDrakomire,
				NPCID.SolarDrakomireRider,
				NPCID.SolarSroller,
				NPCID.SolarCorite,
				NPCID.SolarSolenian
	
			};
			List<int> nebula = new List<int>(){
				NPCID.NebulaBrain,
				NPCID.NebulaHeadcrab,
				NPCID.NebulaBeast,
				NPCID.NebulaSoldier
	
			};
			List<int> vortex = new List<int>(){
				NPCID.VortexRifleman,
				NPCID.VortexHornetQueen,
				NPCID.VortexHornet,
				NPCID.VortexLarva,
				NPCID.VortexSoldier
	
			};
			if(atom.Contains(npc.type)){
				if(Main.rand.NextBool()){
					Item.NewItem(npc.getRect(), ModContent.ItemType<atomfragment>(), Main.rand.Next(0, 2), false, 0, false, false);
				}
				if(Main.rand.NextBool()){
					Item.NewItem(npc.getRect(), ModContent.ItemType<SupernovaeFragment>(), Main.rand.Next(0, 2), false, 0, false, false);
				}
				if(NPC.downedMoonlord && Main.rand.NextBool(3)){
					Item.NewItem(npc.getRect(), ModContent.ItemType<SuperclusterFragment>(), Main.rand.Next(0, 2), false, 0, false, false);
				}
			}
			if(pillars.Contains(npc.type)){
				Item.NewItem(npc.getRect(), ModContent.ItemType<atomfragment>(), Main.rand.Next(14, 24), false, 0, false, false);
				Item.NewItem(npc.getRect(), ModContent.ItemType<SupernovaeFragment>(), Main.rand.Next(14, 24), false, 0, false, false);
			}
			if(stardust.Contains(npc.type)){
				if(Main.rand.NextBool()){
					Item.NewItem(npc.getRect(), ItemID.FragmentStardust, Main.rand.Next(0, 2), false, 0, false, false);
				}
			}
			if(solar.Contains(npc.type)){
				if(Main.rand.NextBool()){
					Item.NewItem(npc.getRect(), ItemID.FragmentSolar, Main.rand.Next(0, 2), false, 0, false, false);
				}
			}
			if(nebula.Contains(npc.type)){
				if(Main.rand.NextBool()){
					Item.NewItem(npc.getRect(), ItemID.FragmentNebula, Main.rand.Next(0, 2), false, 0, false, false);
				}
			}
			if(vortex.Contains(npc.type)){
				if(Main.rand.NextBool()){
					Item.NewItem(npc.getRect(), ItemID.FragmentVortex, Main.rand.Next(0, 2), false, 0, false, false);
				}
			}
		}

	}
}
