using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

public class Conversation
{
    public string whosTalking;
    public string dialog;
    public int talkTime;
    public Color dialogColor;

    public Conversation(string dialog, Color dialogColor, int talkTime=60,string whosTalking="")
    {
        this.dialog = dialog;
        this.talkTime = talkTime;
        this.whosTalking = whosTalking;
        this.dialogColor = dialogColor;
    }
}

namespace KingdomTerrahearts.Interface
{
    public class DialogSystem
    {

        public static DialogSystem instance;

        public static Conversation[] conversations=new Conversation[0];

        public static void OnInitialize()
        {
            instance = new DialogSystem();
        }

        public static void OnUnload()
        {
            instance = null;
        }

        public static void Update()
        {
            if (conversations.Length > 0)
            {
                conversations[0].talkTime--;
                if (conversations[0].talkTime <= 0)
                    RemoveConversations(1);
            }
        }

        public static string GetWhosTalking()
        {
            if (conversations.Length<=0)
                return "";
            return conversations[0].whosTalking;
        }

        public static Color GetColorWhosTalking()
        {
            if (conversations.Length <= 0)
                return Color.White;
            return conversations[0].dialogColor;
        }

        public static string GetConversation()
        {
            if (conversations.Length <= 0)
                return "";
            return conversations[0].dialog;
        }

        public static void AddConversation(Conversation[] conv)
        {
            if (conversations.Length>0 && conversations[conversations.Length - 1] == conv[conv.Length - 1])
                return;
            Conversation[] newConv = conversations;
            conversations = new Conversation[newConv.Length + conv.Length];
            for(int i = 0; i < conversations.Length; i++)
            {
                if (i < newConv.Length)
                    conversations[i] = newConv[i];
                else
                    conversations[i] = conv[i-newConv.Length];
            }
        }
        public static void AddConversation(Conversation conv)
        {
            if (conversations.Length > 0 && conversations[conversations.Length - 1] == conv) return;

            Conversation[] newConv = conversations;
            conversations = new Conversation[newConv.Length + 1];
            for (int i = 0; i < conversations.Length; i++)
            {
                if (i < newConv.Length)
                    conversations[i] = newConv[i];
                else
                    conversations[i] = conv;
            }
        }

        public static void RemoveConversations(int quantity)
        {
            Conversation[] newConv = conversations;
            conversations = new Conversation[newConv.Length -quantity];
            for (int i = 0; i < conversations.Length; i++)
            {
                conversations[i] = newConv[i+quantity];
            }
        }

    }
}
