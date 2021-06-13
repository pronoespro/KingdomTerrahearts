using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace KingdomTerrahearts.Interface
{
    public class CommandMenu : UIState
    {

        public SoraPlayer sora;
        UIPanel comandPanel;
        UIPanel[] commands;
        UIText[] commandText;
        UIPanel[] commandBackground;

        UIPanel[] reactionPanels;
        UIText reacionActiveText;

        float panelSize = 180;
        Color commandColor = new Color(0, 0, 0, 205);

        public override void OnInitialize()
        {

            comandPanel = new UIPanel();
            comandPanel.Width.Set(150, 0);
            comandPanel.Height.Set(panelSize+50, 0);
            comandPanel.HAlign = 0.025f;
            comandPanel.VAlign = 0.95f;
            comandPanel.BackgroundColor = new Color(0, 0, 0, 0);
            comandPanel.BorderColor = new Color(0, 0, 0, 0);
            Append(comandPanel);

            UIPanel CommandTitlePanel = new UIPanel();
            CommandTitlePanel.Width.Set(200,0);
            CommandTitlePanel.Height.Set(panelSize+150,0);
            CommandTitlePanel.Top.Set(-5, 0); 
            CommandTitlePanel.HAlign = 0;
            CommandTitlePanel.BackgroundColor = commandColor;
            comandPanel.Append(CommandTitlePanel);

            UIText header = new UIText("COMMAND");
            header.TextColor = Color.LightBlue;
            header.HAlign = 0f;
            CommandTitlePanel.Append(header);

            commands = new UIPanel[4];
            commandText = new UIText[commands.Length];
            commandBackground = new UIPanel[commands.Length];
            reactionPanels = new UIPanel[3];
        }

        public override void Update(GameTime gameTime)
        {
            if (sora == null) sora = Main.player[Main.myPlayer].GetModPlayer<SoraPlayer>();

            //First reaction arrow
            if (reactionPanels[0] == null)
            {
                reactionPanels[0] = new UIPanel();
                reactionPanels[0].Width.Set(30, 0);
                reactionPanels[0].Height.Set(30, 0);
                reactionPanels[0].HAlign = 0.04f;
                reactionPanels[0].VAlign = 0.68f;
                Append(reactionPanels[0]);
            }
            reactionPanels[0].BackgroundColor = (CommandLogic.instance.curHitAmmount >= CommandLogic.instance.hitsToReaction / 3) ? Color.White : new Color(0, 0, 0, 0);


            if (reactionPanels[1] == null)
            {
                reactionPanels[1] = new UIPanel();
                reactionPanels[1].Width.Set(30, 0);
                reactionPanels[1].Height.Set(30, 0);
                reactionPanels[1].HAlign = (0.025f + 0.1f) / 2;
                reactionPanels[1].VAlign = 0.68f;
                Append(reactionPanels[1]);
            }
            reactionPanels[1].BackgroundColor = (CommandLogic.instance.curHitAmmount >= CommandLogic.instance.hitsToReaction / 3 * 2) ? Color.White : new Color(0, 0, 0, 0);


            if (reactionPanels[2] == null)
            {
                reactionPanels[2] = new UIPanel();
                reactionPanels[2].Width.Set(30, 0);
                reactionPanels[2].Height.Set(30, 0);
                reactionPanels[2].HAlign = 0.1f - 0.015f;
                reactionPanels[2].VAlign = 0.68f;

                reacionActiveText = new UIText("DRIVE");
                reacionActiveText.HAlign = 0.055f;
                reacionActiveText.VAlign = 0.6775f;
                reacionActiveText.TextColor = Color.LightGreen;
            }
            reactionPanels[2].BackgroundColor = (CommandLogic.instance.curHitAmmount >= CommandLogic.instance.hitsToReaction) ? Color.White : new Color(0, 0, 0, 0);
            Append(reactionPanels[2]);

            reacionActiveText.SetText((CommandLogic.instance.reactionActive)?"DRIVE":"");
            Append(reacionActiveText);


            if (commands[0] == null)
            {
                commands[0] = new UIPanel();
                commands[0].Width.Set(150, 0);
                commands[0].Height.Set(panelSize / 5, 0);
                commands[0].Top.Set(0, 0);
                commands[0].VAlign = 0.2f;
                comandPanel.Append(commands[0]);

                commandBackground[0] = new UIPanel();
                commandBackground[0].Width.Set(110, 0);
                commandBackground[0].Height.Set(panelSize / 5 - 3, 0);
                commandBackground[0].BackgroundColor = commandColor;
                commandBackground[0].HAlign = 1f;
                commandBackground[0].VAlign = 0.2f;
                commandBackground[0].Top.Set(0, 0);
                comandPanel.Append(commandBackground[0]);

                commandText[0] = new UIText("Attack");
                commandText[0].HAlign = 0f;
                commandText[0].VAlign = 0.5f;
                commandText[0].TextColor = Color.White;
                commandBackground[0].Append(commandText[0]);
            }
            else
            {
                commandBackground[0].BackgroundColor = (CommandLogic.instance.selectedCommand == 0) ? Color.Blue : commandColor;
                //comandPanel.Append(commandBackground[0]);
                commandText[0].TextColor = (CommandLogic.instance.CanUseCommand(0)) ? Color.White : Color.Gray;
                //commandBackground[0].Append(commandText[0]);
            }

            if (commands[1] == null)
            {
                commands[1] = new UIPanel();
                commands[1].Width.Set(150, 0);
                commands[1].Height.Set(panelSize / 5, 0);
                commands[1].Top.Set(25, 0);
                commands[1].VAlign = 0.3f;
                comandPanel.Append(commands[1]);

                commandBackground[1] = new UIPanel();
                commandBackground[1].Width.Set(110, 0);
                commandBackground[1].Height.Set(panelSize / 5 - 3, 0);
                commandBackground[1].BackgroundColor = commandColor;
                commandBackground[1].HAlign = 1f;
                commandBackground[1].VAlign = 0.3f;
                commandBackground[1].Top.Set(26, 0);
                comandPanel.Append(commandBackground[1]);

                commandText[1] = new UIText("Magic");
                commandText[1].HAlign = 0f;
                commandText[1].VAlign = 0.5f;
                commandText[1].TextColor = Color.White;
                commandBackground[1].Append(commandText[1]);
            }
            else
            {
                commandBackground[1].BackgroundColor = (CommandLogic.instance.selectedCommand == 1 && sora.player.statMana >= sora.lastHeldItem.mana) ? Color.Blue : commandColor;
                //comandPanel.Append(commandBackground[1]);
                commandText[1].TextColor = (CommandLogic.instance.CanUseCommand(1)) ? Color.White : Color.Gray;
                //commandBackground[1].Append(commandText[1]);
            }

            if (commands[2] == null)
            {
                commands[2] = new UIPanel();
                commands[2].Width.Set(150, 0);
                commands[2].Height.Set(panelSize / 5, 0);
                commands[2].Top.Set(50, 0);
                commands[2].VAlign = 0.4f;
                comandPanel.Append(commands[2]);

                commandBackground[2] = new UIPanel();
                commandBackground[2].Width.Set(110, 0);
                commandBackground[2].Height.Set(panelSize / 5 - 3, 0);
                commandBackground[2].BackgroundColor = commandColor;
                commandBackground[2].HAlign = 1f;
                commandBackground[2].VAlign = 0.4f;
                commandBackground[2].Top.Set(51, 0);
                comandPanel.Append(commandBackground[2]);

                commandText[2] = new UIText("Drive");
                commandText[2].HAlign = 0f;
                commandText[2].VAlign = 0.5f;
                commandText[2].TextColor = Color.White;
                commandBackground[2].Append(commandText[2]);
            }
            else
            {
                commandBackground[2].BackgroundColor = (CommandLogic.instance.selectedCommand == 2 && CommandLogic.instance.reactionActive) ? Color.Blue : commandColor;
                //comandPanel.Append(commandBackground[2]);
                commandText[2].TextColor = (CommandLogic.instance.CanUseCommand(2)) ? Color.White : Color.Gray;
                //commandBackground[2].Append(commandText[2]);
            }

            if (commands[3] == null)
            {
                commands[3] = new UIPanel();
                commands[3].Width.Set(150, 0);
                commands[3].Height.Set(panelSize / 5, 0);
                commands[3].Top.Set(75, 0);
                commands[3].VAlign = 0.5f;
                comandPanel.Append(commands[3]);

                commandBackground[3] = new UIPanel();
                commandBackground[3].Width.Set(110, 0);
                commandBackground[3].Height.Set(panelSize / 5 - 3, 0);
                commandBackground[3].BackgroundColor = commandColor;
                commandBackground[3].HAlign = 1f;
                commandBackground[3].VAlign = 0.5f;
                commandBackground[3].Top.Set(76, 0);
                comandPanel.Append(commandBackground[3]);

                commandText[3] = new UIText("Summons");
                commandText[3].HAlign = 0f;
                commandText[3].VAlign = 0.5f;
                commandText[3].TextColor = Color.White;
                commandBackground[3].Append(commandText[3]);
            }
            else
            {
                commandBackground[3].BackgroundColor = (CommandLogic.instance.selectedCommand == 3 && sora.player.statMana >= sora.player.statManaMax) ? Color.Blue : commandColor;
                //comandPanel.Append(commandBackground[3]);
                commandText[3].TextColor = (CommandLogic.instance.CanUseCommand(3)) ? Color.White : Color.Gray;
                //commandBackground[3].Append(commandText[3]);
            }
        }

        public void Destroy()
        {
            sora = null;
        }

    }
}
