using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Mounts
{
    public abstract class FlyingMountBase:ModMount
    {

        public float mountVel=5;
        public float rotateVel = 25;

        Vector2 mountDir;
        Vector2 curDir;
        Vector2 desDir;

        public override void UpdateEffects(Player player)
        {

            mountDir = new Vector2();
            
            mountDir.Y += (player.controlDown) ? 1 : 0;
            mountDir.Y += (player.controlDown) ? 1 : 0;
            mountDir.Y -= (player.controlUp) ? 1:0;
            mountDir.X += (player.controlRight) ? 1 : 0;  
            mountDir.X -= (player.controlLeft) ? 1 : 0;

            desDir = (MathHelp.Magnitude(mountDir) > 0) ? mountDir : curDir;

            curDir = MathHelp.Normalize(Vector2.Lerp(curDir, desDir, rotateVel / 100));

            player.velocity = curDir * mountVel;

            if(Math.Abs(curDir.X)>0.5f)
                player.direction = (int)curDir.X;

        }

        public abstract void SpecialEffects();

    }
}
