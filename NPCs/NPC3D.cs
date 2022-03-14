using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.NPCs
{
    public abstract class NPC3D:ModNPC
    {

        public Vector2 camOffseet;
        public float depth;

        public Vector2 Get3DOffsetWithPosition()
        {
            return (Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) - NPC.position)*depth;
        }

        public Vector2 Get3DOffsetWithCenter()
        {
            return (Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) - NPC.Center)*depth;
        }

    }
}
