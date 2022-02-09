using Galactica.Items;
using Galactica.NPCs.burningdemon;
using Galactica.NPCs.cosmicorb;
using Galactica.NPCs.galactica_i;
using Galactica.NPCs.worm;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Linq;

namespace Galactica
{
	public class heartmodestats : GlobalNPC
	{
		public override void NPCLoot(NPC npc)
		{
			if(world.heartmode && npc.boss){
				world.heartmode = false;
			}
		}
		public override bool InstancePerEntity => true;
		private int attackCounter;
		private bool start;
		private int timer;
		public override void SetDefaults(NPC npc){
			if(world.heartmode){
				if(npc.boss){
					npc.lifeMax += 999999;
					npc.defense = 10;
					npc.damage = 99;
				}
			}
		}
				
	}
}
