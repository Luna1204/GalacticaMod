using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Config.UI;
using Terraria.UI;

namespace Galactica
{
	public class GalacticaConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ClientSide;

		[Label("Darks Heart make npc friendly")]
		public bool FriendlyNPC;
		[Label("Enables / disables modded spore sac")]
		public bool SporeSac;
		[Label("Enables / disables modded gravity globe")]
		public bool GravityGlobe;
		[Label("Enables / disables projectile resize")]
		public bool Resize;
		[Label("Enables / disables extra proj from darks heart")]
		public bool ExtraProj;
		[Label("Enables / disables dropping of Soul Essence when being killed/hurt")]
		public bool Souls;
	}
}