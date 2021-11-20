using KingdomTerrahearts.Items.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace KingdomTerrahearts.Interface
{
    internal class UIItemSlot : UIElement
    {
        internal Item Item;
        private readonly int _context;
        private readonly float _scale;
        internal Func<Item, bool> ValidItemFunc;

        public UIItemSlot(int context = ItemSlot.Context.BankItem, float scale = 1f)
        {
            _context = context;
            _scale = scale;
            Item = new Item();
            Item.SetDefaults(0);

            Width.Set(Terraria.GameContent.TextureAssets.InventoryBack.Width() * scale, 0f);
            Height.Set(Terraria.GameContent.TextureAssets.InventoryBack.Height() * scale, 0f);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            float oldScale = Main.inventoryScale;
            Main.inventoryScale = _scale;
            Rectangle rectangle = GetDimensions().ToRectangle();

            if (ContainsPoint(Main.MouseScreen) && !PlayerInput.IgnoreMouseInterface)
            {
                Main.LocalPlayer.mouseInterface = true;
                if (ValidItemFunc == null || ValidItemFunc(Main.mouseItem))
                {
                    // Handle handles all the click and hover actions based on the context.
                    ItemSlot.Handle(ref Item, _context);
                }
            }
            // Draw draws the slot itself and Item. Depending on context, the color will change, as will drawing other things like stack counts.
            ItemSlot.Draw(spriteBatch, ref Item, _context, rectangle.TopLeft());
            Main.inventoryScale = oldScale;
        }
    }



    public class KeybladeLeveling : UIState
    {

        UIItemSlot item;
        UIText text;


        const int slotX = 50;
        const int slotY = 270;

        public override void OnInitialize()
        {

            UIPanel panel = new UIPanel();
            panel.Width.Set(150, 0);
            panel.Height.Set(150, 0);
            panel.HAlign = 0.5f;
            panel.VAlign = 0.5f;
            Append(panel);

            item = new UIItemSlot();
            item.HAlign = item.VAlign = 0.5f;
            Append(item);


        }

        public override void Update(GameTime gameTime)
        {

            if (Main.LocalPlayer.talkNPC!= -1)
            {
                KingdomTerrahearts.instance.HideLevelUpUI();
            }

            if (item != null) {
                KeybladeBase keyblade = (KeybladeBase)item.Item.ModItem;

                if (keyblade==null)
                {
                    // QuickSpawnClonedItem will preserve mod data of the item. QuickSpawnItem will just spawn a fresh version of the item, losing the prefix.
                    Main.LocalPlayer.QuickSpawnClonedItem(item.Item, item.Item.stack);
                    // Now that we've spawned the item back onto the player, we reset the item by turning it into air.
                    item.Item.TurnToAir();
                }
            }
        }

        private bool tickPlayed;

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {

            Main.hidePlayerCraftingMenu = true;

            KeybladeBase keyblade = (KeybladeBase)item.Item.ModItem;
            if (keyblade != null)
            {


                if (text == null)
                {
                    text = new UIText("Keyblade level: " + (keyblade.keyLevel).ToString());
                    text.HAlign = 0.56f;
                    text.VAlign = 0.54f;
                    Append(text);
                }
                else
                {
                    text.SetText("Keyblade level: " + (keyblade.keyLevel).ToString());
                }


                bool hoveringOnLevelButton = Main.mouseX > slotX && Main.mouseX < slotX && Main.mouseY > slotY && Main.mouseY > slotY && !PlayerInput.IgnoreMouseInterface;

                if (hoveringOnLevelButton)
                {
                    if (!tickPlayed)
                    {
                        SoundEngine.PlaySound(SoundID.MenuTick, -1, -1, 1, 1f, 0f);
                    }
                    tickPlayed = true;


                    if (Main.mouseLeftRelease && Main.mouseLeft)
                    {

                        if (keyblade.keyLevel < 10000)
                        {
                            keyblade.keyLevel++;
                            SoundEngine.PlaySound(SoundID.Item37, -1, -1);
                        }
                    }
                }





            }
            else
            {
                if (text != null)
                {
                    text.Deactivate();
                    text.Remove();
                    text = null;
                }
            }
        }

        public override void OnDeactivate()
        {
            if (!item.Item.IsAir)
            {
                // QuickSpawnClonedItem will preserve mod data of the item. QuickSpawnItem will just spawn a fresh version of the item, losing the prefix.
                Main.LocalPlayer.QuickSpawnClonedItem(item.Item, item.Item.stack);
                // Now that we've spawned the item back onto the player, we reset the item by turning it into air.
                item.Item.TurnToAir();
            }
        }

    }
}
