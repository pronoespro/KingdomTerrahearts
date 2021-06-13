using KingdomTerrahearts.Items.Weapons;
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

            Width.Set(Main.inventoryBack9Texture.Width * scale, 0f);
            Height.Set(Main.inventoryBack9Texture.Height * scale, 0f);
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

        public override void OnInitialize()
        {
            item = new UIItemSlot();
            item.HAlign = item.VAlign = 0.5f;
            Append(item);

            UIImageButton itemResetButton = new UIImageButton(ModContent.GetTexture("Terraria/UI/ButtonFavoriteActive"));
            itemResetButton.Width.Set(100, 0);
            itemResetButton.Height.Set(100, 0);
            itemResetButton.HAlign = 0.56f;
            itemResetButton.VAlign = 0.54f;
            itemResetButton.OnClick += new MouseEvent(LevelUpKeyblade);
            Append(itemResetButton);

        }

        public override void Update(GameTime gameTime)
        {
            Keyblade keyblade = (Keyblade)item.Item.modItem;
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

        void LevelUpKeyblade(UIMouseEvent evt, UIElement listeningElement)
        {
            ModItem keybladeItem=item.Item.modItem;

            if (keybladeItem.GetType().Name.Contains("Keyblade_"))
            {
                Keyblade keyblade = keybladeItem as Keyblade;
                if (keyblade.keyLevel < 10)
                    keyblade.keyLevel++;
            }
        }

        void Destroy()
        {
            item = null;
        }

    }
}
