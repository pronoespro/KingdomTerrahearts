using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace KingdomTerrahearts.Interface
{
    public class DialogDisplay:UIState
    {

        SoraPlayer player;

        UIText dialogText;
        UIText dialogWhosTalkingText;

        public override void OnInitialize()
        {
            dialogText = new UIText(DialogSystem.GetConversation());
            dialogText.HAlign = 0.5f;
            dialogText.VAlign = 0.85f;
            Append(dialogText);

            dialogWhosTalkingText= new UIText(DialogSystem.GetWhosTalking(),textScale:1.5f);
            dialogWhosTalkingText.HAlign = 0.5f;
            dialogWhosTalkingText.VAlign = 0.8f;
            Append(dialogWhosTalkingText);
        }

        public override void Update(GameTime gameTime)
        {
            dialogText.SetText(DialogSystem.GetConversation());
            dialogWhosTalkingText.SetText(DialogSystem.GetWhosTalking());
            dialogWhosTalkingText.TextColor = DialogSystem.GetColorWhosTalking();
            DialogSystem.Update();
        }

        public void Destroy()
        {
            player = null;
        }

    }
}
