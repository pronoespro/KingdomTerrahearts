using KingdomTerrahearts.Logic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace KingdomTerrahearts.Interface
{

    public class PartyUI:UIState
    {

        public SoraPlayer sora;
        UIPanel panel;
        UIPanel[] buttons=new UIPanel[30];

        UIItemSlot item;
        UIText slotsDisplay;

        public override void OnInitialize()
        {

            UIPanel GeneralPanel= new UIPanel();
            GeneralPanel.Width.Set(300, 0);
            GeneralPanel.Height.Set(325, 0);
            GeneralPanel.HAlign = 0.125f;
            GeneralPanel.VAlign = 0.9f;
            Append(GeneralPanel);

            UIText header = new UIText("Party menu:");
            header.HAlign = 0.5f; 
            header.Top.Set(15, 0);
            GeneralPanel.Append(header);

            panel = new UIPanel(); 
            panel.Width.Set(275, 0); 
            panel.Height.Set(250, 0); 
            panel.HAlign = 0; 
            panel.VAlign = 1f;
            GeneralPanel.Append(panel);

            slotsDisplay = new UIText("Slots left: 0");
            slotsDisplay.HAlign = 0.5f;
            slotsDisplay.Top.Set(45, 0);
            GeneralPanel.Append(slotsDisplay);

        }

        public override void Update(GameTime gameTime)
        {
            Player p = Main.player[Main.myPlayer];
            sora = p.GetModPlayer<SoraPlayer>();

            if (!Main.playerInventory)
            {
                KingdomTerrahearts.instance.SetPartyUI(false);
                return;
            }

            slotsDisplay.SetText("Slots left: "+PartyMemberLogic.GetPartySlotsLeft(p.name));

            //0:Guide
            if (NPC.AnyNPCs(NPCID.Guide))
            {
                if (buttons[0] == null)
                {
                    buttons[0] = new UIPanel();
                    buttons[0].Width.Set(50, 0);
                    buttons[0].Height.Set(50, 0);
                    buttons[0].HAlign = 0f;
                    buttons[0].Top.Set(25, 0);
                    buttons[0].OnClick += OnGuideClick;
                    panel.Append(buttons[0]);

                    UIText text = new UIText("Guide");
                    text.HAlign = text.VAlign = 0.5f;
                    buttons[0].Append(text);
                }
                else if(buttons[0]!= new UIPanel())
                {
                    buttons[0].BackgroundColor = (PartyMemberLogic.IsPartyMember(NPCID.Guide)>=0) ? Color.LightBlue : Color.Transparent;
                }
            }
            else
            {
                if (buttons[0] != null)
                {
                    buttons[0].Remove();
                    buttons[0] = null;
                }
            }

            //1:Merchant
            if (NPC.AnyNPCs(NPCID.Merchant))
            {
                if (buttons[1] == null)
                {
                    buttons[1] = new UIPanel();
                    buttons[1].Width.Set(50, 0);
                    buttons[1].Height.Set(50, 0);
                    buttons[1].HAlign = 0.25f;
                    buttons[1].Top.Set(25, 0);
                    buttons[1].OnClick += OnMerchantClick;
                    panel.Append(buttons[1]);

                    UIText text = new UIText("Merch");
                    text.HAlign = text.VAlign = 0.5f;
                    buttons[1].Append(text);
                }
                else if (buttons[1] != new UIPanel())
                {
                    buttons[1].BackgroundColor = (PartyMemberLogic.IsPartyMember(NPCID.Merchant) >= 0) ? Color.LightBlue : Color.Transparent;
                }
            }
            else
            {
                if (buttons[1] != null)
                {
                    buttons[1].Remove();
                    buttons[1] = null;
                }
            }

            //2:Nurse
            if (NPC.AnyNPCs(NPCID.Nurse))
            {
                if (buttons[2] == null)
                {
                    buttons[2] = new UIPanel();
                    buttons[2].Width.Set(50, 0);
                    buttons[2].Height.Set(50, 0);
                    buttons[2].HAlign = 0.5f;
                    buttons[2].Top.Set(25, 0);
                    buttons[2].OnClick += OnNurseClick;
                    panel.Append(buttons[2]);

                    UIText text = new UIText("Nurse");
                    text.HAlign = 0.5f;
                    text.VAlign = 0.5f;
                    buttons[2].Append(text);
                }
                else if (buttons[2] != new UIPanel())
                {
                    buttons[2].BackgroundColor = (PartyMemberLogic.IsPartyMember(NPCID.Nurse) >= 0) ? Color.LightBlue : Color.Transparent;
                }
            }
            else
            {
                if (buttons[2] != null)
                {
                    buttons[2].Remove();
                    buttons[2] = null;
                }
            }

            //3:Demolitionist
            if (NPC.AnyNPCs(NPCID.Demolitionist))
            {
                if (buttons[3] == null)
                {
                    buttons[3] = new UIPanel();
                    buttons[3].Width.Set(50, 0);
                    buttons[3].Height.Set(50, 0);
                    buttons[3].HAlign = 0.75f;
                    buttons[3].Top.Set(25, 0);
                    buttons[3].OnClick += OnBomberClick;
                    panel.Append(buttons[3]);

                    UIText text = new UIText("Bomb");
                    text.HAlign = 0.5f;
                    text.VAlign = 0.5f;
                    buttons[3].Append(text);
                }
                else if (buttons[3] != new UIPanel())
                {
                    buttons[3].BackgroundColor = (PartyMemberLogic.IsPartyMember(NPCID.Demolitionist) >= 0) ? Color.LightBlue : Color.Transparent;
                }
            }
            else
            {
                if (buttons[3] != null)
                {
                    buttons[3].Remove();
                    buttons[3] = null;
                }
            }
            
            //4:Dye trader
            if (NPC.AnyNPCs(NPCID.DyeTrader))
            {
                if (buttons[4] == null)
                {
                    buttons[4] = new UIPanel();
                    buttons[4].Width.Set(50, 0);
                    buttons[4].Height.Set(50, 0);
                    buttons[4].HAlign = 0f;
                    buttons[4].VAlign = 0.25f;
                    buttons[4].Top.Set(25, 0);
                    buttons[4].OnClick += OnDyeClick;
                    panel.Append(buttons[4]);

                    UIText text = new UIText("Dye");
                    text.HAlign = 0.5f;
                    text.VAlign = 0.5f;
                    buttons[4].Append(text);
                }
                else if (buttons[4] != new UIPanel())
                {
                    buttons[4].BackgroundColor = (PartyMemberLogic.IsPartyMember(NPCID.DyeTrader) >= 0) ? Color.LightBlue : Color.Transparent;
                }
            }
            else
            {
                if (buttons[4] != null)
                {
                    buttons[4].Remove();
                    buttons[4] = null;
                }
            }

            //5:Angler
            if (NPC.AnyNPCs(NPCID.Angler))
            {
                if (buttons[5] == null)
                {           
                    buttons[5] = new UIPanel();
                    buttons[5].Width.Set(50, 0);
                    buttons[5].Height.Set(50, 0);
                    buttons[5].HAlign = 0.25f;
                    buttons[5].VAlign = 0.25f;
                    buttons[5].Top.Set(25, 0);
                    buttons[5].OnClick += OnAnglerClick;
                    panel.Append(buttons[5]);

                    UIText text = new UIText("Fish");
                    text.HAlign = 0.5f;
                    text.VAlign = 0.5f;
                    buttons[5].Append(text);
                }
                else if (buttons[5] != new UIPanel())
                {
                    buttons[5].BackgroundColor = (PartyMemberLogic.IsPartyMember(NPCID.Angler) >= 0) ? Color.LightBlue : Color.Transparent;
                }
            }
            else
            {
                if (buttons[5] != null)
                {
                    buttons[5].Remove();
                    buttons[5] = null;
                }
            }

            //6:Dryad
            if (NPC.AnyNPCs(NPCID.Dryad))
            {
                if (buttons[6] == null)
                {
                    buttons[6] = new UIPanel();
                    buttons[6].Width.Set(50, 0);
                    buttons[6].Height.Set(50, 0);
                    buttons[6].HAlign = 0.5f;
                    buttons[6].VAlign = 0.25f;
                    buttons[6].Top.Set(25, 0);
                    buttons[6].OnClick += OnDryadClick;
                    panel.Append(buttons[6]);

                    UIText text = new UIText("Dryad");
                    text.HAlign = text.VAlign = 0.5f;
                    buttons[6].Append(text);
                }
                else if (buttons[6] != new UIPanel())
                {
                    buttons[6].BackgroundColor = (PartyMemberLogic.IsPartyMember(NPCID.Dryad) >= 0) ? Color.LightBlue : Color.Transparent;
                }
            }
            else
            {
                if (buttons[6] != null)
                {
                    buttons[6].Remove();
                    buttons[6] = null;
                }
            }

            //7:Painter
            if (NPC.AnyNPCs(NPCID.Painter))
            {
                if (buttons[7] == null)
                {
                    buttons[7] = new UIPanel();
                    buttons[7].Width.Set(50, 0);
                    buttons[7].Height.Set(50, 0);
                    buttons[7].HAlign = 0.75f;
                    buttons[7].VAlign = 0.25f;
                    buttons[7].Top.Set(25, 0);
                    buttons[7].OnClick += OnPainterClick;
                    panel.Append(buttons[7]);

                    UIText text = new UIText("Paint");
                    text.HAlign = text.VAlign = 0.5f;
                    buttons[7].Append(text);
                }
                else if (buttons[7] != new UIPanel())
                {
                    buttons[7].BackgroundColor = (PartyMemberLogic.IsPartyMember(NPCID.Painter) >= 0) ? Color.LightBlue : Color.Transparent;
                }
            }
            else
            {
                if (buttons[7] != null)
                {
                    buttons[7].Remove();
                    buttons[7] = null;
                }
            }

            //8:Arms Dealer
            if (NPC.AnyNPCs(NPCID.ArmsDealer))
            {
                if (buttons[8] == null)
                {
                    buttons[8] = new UIPanel();
                    buttons[8].Width.Set(50, 0);
                    buttons[8].Height.Set(50, 0);
                    buttons[8].HAlign = 0f;
                    buttons[8].VAlign = 0.5f;
                    buttons[8].Top.Set(25, 0);
                    buttons[8].OnClick += OnGunnerClick;
                    panel.Append(buttons[8]);

                    UIText text = new UIText("Guns");
                    text.HAlign = text.VAlign = 0.5f;
                    buttons[8].Append(text);
                }
                else if (buttons[8] != new UIPanel())
                {
                    buttons[8].BackgroundColor = (PartyMemberLogic.IsPartyMember(NPCID.ArmsDealer) >= 0) ? Color.LightBlue : Color.Transparent;
                }
            }
            else
            {
                if (buttons[8] != null)
                {
                    buttons[8].Remove();
                    buttons[8] = null;
                }
            }

            //9:Taberkeeper (DD2Bartender)
            if (NPC.AnyNPCs(NPCID.DD2Bartender))
            {
                if (buttons[9] == null)
                {
                    buttons[9] = new UIPanel();
                    buttons[9].Width.Set(50, 0);
                    buttons[9].Height.Set(50, 0);
                    buttons[9].HAlign = 0.25f;
                    buttons[9].VAlign = 0.5f;
                    buttons[9].Top.Set(25, 0);
                    buttons[9].OnClick += OnTabernClick;
                    panel.Append(buttons[9]);

                    UIText text = new UIText("Tabern");
                    text.HAlign = text.VAlign = 0.5f;
                    buttons[9].Append(text);
                }
                else if (buttons[9] != new UIPanel())
                {
                    buttons[9].BackgroundColor = (PartyMemberLogic.IsPartyMember(NPCID.DD2Bartender) >= 0) ? Color.LightBlue : Color.Transparent;
                }
            }
            else
            {
                if (buttons[9] != null)
                {           
                    buttons[9].Remove();
                    buttons[9] = null;
                }
            }

            //10:Stylist
            if (NPC.AnyNPCs(NPCID.Stylist))
            {
                if (buttons[10] == null)
                {
                    buttons[10] = new UIPanel();
                    buttons[10].Width.Set(50, 0);
                    buttons[10].Height.Set(50, 0);
                    buttons[10].HAlign = 0.5f;
                    buttons[10].VAlign = 0.5f;
                    buttons[10].Top.Set(25, 0);
                    buttons[10].OnClick += OnStylistClick;
                    panel.Append(buttons[10]);

                    UIText text = new UIText("Style");
                    text.HAlign = text.VAlign = 0.5f;
                    buttons[10].Append(text);
                }
                else if (buttons[10] != new UIPanel())
                {
                    buttons[10].BackgroundColor = (PartyMemberLogic.IsPartyMember(NPCID.Stylist) >= 0) ? Color.LightBlue : Color.Transparent;
                }
            }
            else
            {
                if (buttons[10] != null)
                {           
                    buttons[10].Remove();
                    buttons[10] = null;
                }
            }

            //11:Goblin tinkerer
            if (NPC.AnyNPCs(NPCID.GoblinTinkerer))
            {
                if (buttons[11] == null)
                {
                    buttons[11] = new UIPanel();
                    buttons[11].Width.Set(50, 0);
                    buttons[11].Height.Set(50, 0);
                    buttons[11].HAlign = 0.75f;
                    buttons[11].VAlign = 0.5f;
                    buttons[11].Top.Set(25, 0);
                    buttons[11].OnClick += OnGoblinClick;
                    panel.Append(buttons[11]);

                    UIText text = new UIText("Goblin");
                    text.HAlign = 0.5f;
                    text.VAlign = 0.5f;
                    buttons[11].Append(text);
                }
                else if (buttons[11] != new UIPanel())
                {
                    buttons[11].BackgroundColor = (PartyMemberLogic.IsPartyMember(NPCID.GoblinTinkerer) >= 0) ? Color.LightBlue : Color.Transparent;
                }
            }
            else
            {
                if (buttons[11] != null)
                {            
                    buttons[11].Remove();
                    buttons[11] = null;
                }
            }

            //12:Witch doctor
            if (NPC.AnyNPCs(NPCID.WitchDoctor))
            {
                if (buttons[12] == null)
                {
                    buttons[12] = new UIPanel();
                    buttons[12].Width.Set(50, 0);
                    buttons[12].Height.Set(50, 0);
                    buttons[12].HAlign = 0f;
                    buttons[12].VAlign = 0.75f;
                    buttons[12].Top.Set(25, 0);
                    buttons[12].OnClick += OnWitchClick;
                    panel.Append(buttons[12]);

                    UIText text = new UIText("Witch");
                    text.HAlign = 0.5f;
                    text.VAlign = 0.5f;
                    buttons[12].Append(text);
                }
                else if (buttons[12] != new UIPanel())
                {
                    buttons[12].BackgroundColor = (PartyMemberLogic.IsPartyMember(NPCID.WitchDoctor) >= 0) ? Color.LightBlue : Color.Transparent;
                }
            }
            else
            {
                if (buttons[12] != null)
                {            
                    buttons[12].Remove();
                    buttons[12] = null;
                }
            }

            //13:Clothier
            if (NPC.AnyNPCs(NPCID.Clothier))
            {
                if (buttons[13] == null)
                {            
                    buttons[13] = new UIPanel();
                    buttons[13].Width.Set(50, 0);
                    buttons[13].Height.Set(50, 0);
                    buttons[13].HAlign = 0.25f;
                    buttons[13].VAlign = 0.75f;
                    buttons[13].Top.Set(25, 0);
                    buttons[13].OnClick += OnClothierClick;
                    panel.Append(buttons[13]);

                    UIText text = new UIText("Clothier");
                    text.HAlign = 0.5f;
                    text.VAlign = 0.5f;
                    buttons[13].Append(text);
                }
                else if (buttons[13] != new UIPanel())
                {
                    buttons[13].BackgroundColor = (PartyMemberLogic.IsPartyMember(NPCID.Clothier) >= 0) ? Color.LightBlue : Color.Transparent;
                }
            }
            else
            {
                if (buttons[13] != null)
                { 
                    buttons[13].Remove();
                    buttons[13] = null;
                }
            }

            //14:Mechanic
            if (NPC.AnyNPCs(NPCID.Mechanic))
            {
                if (buttons[14] == null)
                {            
                    buttons[14] = new UIPanel();
                    buttons[14].Width.Set(50, 0);
                    buttons[14].Height.Set(50, 0);
                    buttons[14].HAlign = 0.5f;
                    buttons[14].VAlign = 0.75f;
                    buttons[14].Top.Set(25, 0);
                    buttons[14].OnClick += OnMechanicClick;
                    panel.Append(buttons[14]);

                    UIText text = new UIText("Mech");
                    text.HAlign = 0.5f;
                    text.VAlign = 0.5f;
                    buttons[14].Append(text);
                }
                else if (buttons[14] != new UIPanel())
                {
                    buttons[14].BackgroundColor = (PartyMemberLogic.IsPartyMember(NPCID.Mechanic) >= 0) ? Color.LightBlue : Color.Transparent;
                }
            }
            else
            {
                if (buttons[14] != null)
                {            
                    buttons[14].Remove();
                    buttons[14] = null;
                }
            }

            //15:Party Girl
            if (NPC.AnyNPCs(NPCID.PartyGirl))
            {
                if (buttons[15] == null)
                {            
                    buttons[15] = new UIPanel();
                    buttons[15].Width.Set(50, 0);
                    buttons[15].Height.Set(50, 0);
                    buttons[15].HAlign = 0.75f;
                    buttons[15].VAlign = 0.75f;
                    buttons[15].Top.Set(25, 0);
                    buttons[15].OnClick += OnPartyClick;
                    panel.Append(buttons[15]);

                    UIText text = new UIText("Party");
                    text.HAlign = 0.5f;
                    text.VAlign = 0.5f;
                    buttons[15].Append(text);
                }
                else if (buttons[15] != new UIPanel())
                {
                    buttons[15].BackgroundColor = (PartyMemberLogic.IsPartyMember(NPCID.PartyGirl) >= 0) ? Color.LightBlue : Color.Transparent;
                }
            }
            else
            {
                if (buttons[15] != null)
                {
                    buttons[15].Remove();
                    buttons[15] = null;
                }
            }


        }

        void OnGuideClick(UIMouseEvent evt, UIElement listeningElement)
        {
            Player p = Main.player[Main.myPlayer];
            sora = p.GetModPlayer<SoraPlayer>();
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && Main.npc[i].type == NPCID.Guide) sora.GetPartyMember(i);
            }
        }

        void OnMerchantClick(UIMouseEvent evt, UIElement listeningElement)
        {
            if (sora == null)
            {
                Player p = Main.player[Main.myPlayer];
                sora = p.GetModPlayer<SoraPlayer>();
            }
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && Main.npc[i].type == NPCID.Merchant) sora.GetPartyMember(i);
            }
        }

        void OnNurseClick(UIMouseEvent evt, UIElement listeningElement)
        {
            if (sora == null)
            {
                Player p = Main.player[Main.myPlayer];
                sora = p.GetModPlayer<SoraPlayer>();
            }
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && Main.npc[i].type == NPCID.Nurse) sora.GetPartyMember(i);
            }
        }

        void OnBomberClick(UIMouseEvent evt, UIElement listeningElement)
        {
            if (sora == null)
            {
                Player p = Main.player[Main.myPlayer];
                sora = p.GetModPlayer<SoraPlayer>();
            }
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && Main.npc[i].type == NPCID.Demolitionist) sora.GetPartyMember(i);
            }
        }

        void OnDyeClick(UIMouseEvent evt, UIElement listeningElement)
        {
            if (sora == null)
            {
                Player p = Main.player[Main.myPlayer];
                sora = p.GetModPlayer<SoraPlayer>();
            }
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && Main.npc[i].type == NPCID.DyeTrader) sora.GetPartyMember(i);
            }
        }

        void OnAnglerClick(UIMouseEvent evt, UIElement listeningElement)
        {
            if (sora == null)
            {
                Player p = Main.player[Main.myPlayer];
                sora = p.GetModPlayer<SoraPlayer>();
            }
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && Main.npc[i].type == NPCID.Angler) sora.GetPartyMember(i);
            }
        }

        void OnPainterClick(UIMouseEvent evt, UIElement listeningElement)
        {
            if (sora == null)
            {
                Player p = Main.player[Main.myPlayer];
                sora = p.GetModPlayer<SoraPlayer>();
            }
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && Main.npc[i].type == NPCID.Painter) sora.GetPartyMember(i);
            }
        }

        void OnDryadClick(UIMouseEvent evt, UIElement listeningElement)
        {
            if (sora == null)
            {
                Player p = Main.player[Main.myPlayer];
                sora = p.GetModPlayer<SoraPlayer>();
            }
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && Main.npc[i].type == NPCID.Dryad) sora.GetPartyMember(i);
            }
        }

        void OnGunnerClick(UIMouseEvent evt, UIElement listeningElement)
        {
            if (sora == null)
            {
                Player p = Main.player[Main.myPlayer];
                sora = p.GetModPlayer<SoraPlayer>();
            }
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && Main.npc[i].type == NPCID.ArmsDealer) sora.GetPartyMember(i);
            }
        }

        void OnTabernClick(UIMouseEvent evt, UIElement listeningElement)
        {
            if (sora == null)
            {
                Player p = Main.player[Main.myPlayer];
                sora = p.GetModPlayer<SoraPlayer>();
            }
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && Main.npc[i].type == NPCID.DD2Bartender) sora.GetPartyMember(i);
            }
        }

        void OnStylistClick(UIMouseEvent evt, UIElement listeningElement)
        {
            if (sora == null)
            {
                Player p = Main.player[Main.myPlayer];
                sora = p.GetModPlayer<SoraPlayer>();
            }
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && Main.npc[i].type == NPCID.Stylist) sora.GetPartyMember(i);
            }
        }

        void OnGoblinClick(UIMouseEvent evt, UIElement listeningElement)
        {
            if (sora == null)
            {
                Player p = Main.player[Main.myPlayer];
                sora = p.GetModPlayer<SoraPlayer>();
            }
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && Main.npc[i].type == NPCID.GoblinTinkerer) sora.GetPartyMember(i);
            }
        }

        void OnWitchClick(UIMouseEvent evt, UIElement listeningElement)
        {
            if (sora == null)
            {
                Player p = Main.player[Main.myPlayer];
                sora = p.GetModPlayer<SoraPlayer>();
            }
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && Main.npc[i].type == NPCID.WitchDoctor) sora.GetPartyMember(i);
            }
        }

        void OnClothierClick(UIMouseEvent evt, UIElement listeningElement)
        {
            if (sora == null)
            {
                Player p = Main.player[Main.myPlayer];
                sora = p.GetModPlayer<SoraPlayer>();
            }
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && Main.npc[i].type == NPCID.Clothier) sora.GetPartyMember(i);
            }
        }

        void OnMechanicClick(UIMouseEvent evt, UIElement listeningElement)
        {
            if (sora == null)
            {
                Player p = Main.player[Main.myPlayer];
                sora = p.GetModPlayer<SoraPlayer>();
            }
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && Main.npc[i].type == NPCID.Mechanic) sora.GetPartyMember(i);
            }
        }

        void OnPartyClick(UIMouseEvent evt, UIElement listeningElement)
        {
            if (sora == null)
            {
                Player p = Main.player[Main.myPlayer];
                sora = p.GetModPlayer<SoraPlayer>();
            }
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && Main.npc[i].type == NPCID.PartyGirl) sora.GetPartyMember(i);
            }
        }


        public void Destroy()
        {
            sora = null;
        }

    }
}
