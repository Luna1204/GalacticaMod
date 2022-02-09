using Terraria.UI;
using Microsoft.Xna.Framework;
using Terraria.GameContent.UI;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.GameContent.UI.States;
using Terraria;
using System;

namespace Galactica
{
    internal class SkillsPannel : UIState
    {
        public static bool visible;
        public DragableUIPanel panel;
	private UIText text;
	public float percentdamage;
	public override void Update(GameTime gameTime)
	{
    		var player = Main.LocalPlayer; //Get Player
		percentdamage = (player.GetModPlayer<StatsPlayer>().DMGXP / (float)((player.GetModPlayer<StatsPlayer>().DMGSkill * 350) + (player.GetModPlayer<StatsPlayer>().DMGSkill * 3 * player.GetModPlayer<StatsPlayer>().DMGSkill))) * 100;
    		text.SetText("test" + player.GetModPlayer<StatsPlayer>().DMGSkill + "\n" + percentdamage + "%"); //Set Life
    		base.Update(gameTime);
	}
        public override void OnInitialize()
        {
            // if you set this to true, it will show up in game
            visible = false;

            panel = new DragableUIPanel(); //initialize the panel
            // ignore these extra 0s
            panel.Left.Set(800, 0); //this makes the distance between the left of the screen and the left of the panel 500 pixels (somewhere by the middle)
            panel.Top.Set(100, 0); //this is the distance between the top of the screen and the top of the panel
            panel.Width.Set(500, 0);
            panel.Height.Set(100, 0);

            Append(panel); //appends the panel to the UIState
            text = new UIText("test"); //text to show current hp or mana
            text.Width.Set(100f, 0f);
    	    text.Height.Set(100f, 0f);
            text.Top.Set(100f / 2 - text.MinHeight.Pixels / 2, 0f);
        }
    }
}