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

namespace KingdomTerrahearts
{
    public class KingdomConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [DefaultValue(1f),Label("Screen shake strength"),Range(0f,5f)]
        public float screenShakeAmmount;

        [DefaultValue(true), Label("Play cutscenes")]
        public bool showCutscenes;

        [DefaultValue(true), Label("Keyblade Thrusting (gets you closer to enemies, but is risky)")]
        public bool keybladeThrusting;

        public override void OnChanged()
        {
            KingdomTerrahearts.screenShakeStrength = screenShakeAmmount;
            KingdomTerrahearts.canDoCutscenes = showCutscenes;
            KingdomTerrahearts.keybladeThrustingEnabled = keybladeThrusting;
        }

    }
}
