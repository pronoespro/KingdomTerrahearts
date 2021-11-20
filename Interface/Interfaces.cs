using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace KingdomTerrahearts.Interface
{
    public class Interfaces:ModSystem
    {

        public override void UpdateUI(GameTime gameTime)
        {
            KingdomTerrahearts.instance.UpdateUI(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            KingdomTerrahearts.instance.ModifyInterfaceLayers(layers);
        }
    }
}
