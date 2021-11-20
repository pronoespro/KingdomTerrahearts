using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace KingdomTerrahearts
{
    public class CommandLogic
    {

        public static CommandLogic instance;

        //Selected command stuff
        internal int selectedCommand;
        internal int maxCommand=3;

        public static SoraPlayer sora;

        //Reaction Command stuff
        internal int hitsToReaction=15;
        internal int curHitAmmount=0;
        internal Item reactionItem;
        internal bool reactionActive;

        public static void Initialize()
        {
            instance = new CommandLogic();
        }

        public static void Unload()
        {
            instance = null;
            sora = null;
        }

        public void Update()
        {
            sora = Main.player[Main.myPlayer].GetModPlayer<SoraPlayer>();
        }

        public void UseReaction()
        {
            if (sora.Player.HeldItem != null && reactionItem.active)
            {
                curHitAmmount = 0;
                reactionItem = new Item();
                reactionActive = false;
            }
        }

        public void HitAttack()
        {
            if (!reactionActive)
            {
                curHitAmmount++;
                if (curHitAmmount >= hitsToReaction)
                {
                    reactionActive = true;
                    reactionItem = sora.Player.HeldItem;
                }
            }
        }

        public void ChangeCommand(int command)
        {
            selectedCommand = Math.Min(command, maxCommand);
            while (!CanUseCommand(selectedCommand))
            {
                selectedCommand++;
                if (selectedCommand >= maxCommand) 
                    selectedCommand = 0;
            }
        }

        public bool CanUseCommand(int curCommand)
        {
            Player p = Main.player[Main.myPlayer];
            if (p != null)
            {
                switch (curCommand)
                {
                    case 0:
                    default:
                        return true;
                    case 1:
                        return p.HeldItem.mana < p.statMana;
                    case 2:
                        return reactionActive;
                    case 3:
                        return p.statMana >= p.statManaMax;
                }
            }
            return false;
        }

        public void MoveCommandCursor(bool up=false)
        {
            if (!Main.playerInventory)
            {
                selectedCommand += (up) ? -1 : 1;
                selectedCommand = (selectedCommand < 0) ? maxCommand : (selectedCommand > maxCommand) ? 0 : selectedCommand;

                while (!CanUseCommand(selectedCommand))
                {
                    selectedCommand++;
                    if (selectedCommand > maxCommand)
                        selectedCommand = 0;
                }
            }
        }

    }
}
