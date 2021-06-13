using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using KingdomTerrahearts.Extra;
using Terraria.ModLoader.IO;
using System;

namespace KingdomTerrahearts.Items.Weapons.Org13
{
    public abstract class BaseChakram:ModItem
    {

        public int[] damages = new int[] { 1,1 };
        public int[] projectiles = new int[] { ProjectileID.EnchantedBoomerang,ProjectileID.IceBoomerang};
        public int maxChakrams=2;

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            item.shoot = (projectiles.Length > 1) ? (player.altFunctionUse == 2 ? projectiles[1] : projectiles[0]):item.shoot;

            int projAmmount = 0;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                projAmmount += (Main.projectile[i].active && Main.projectile[i].type == item.shoot) ? 1 : 0;
            }
            return projAmmount < maxChakrams;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile.NewProjectile(position, new Vector2(speedX, speedY+((player.altFunctionUse==2)?-5:5)), type, damage, knockBack, item.owner,(player.altFunctionUse==2)?100:0);
            return false;
        }

    }
}
