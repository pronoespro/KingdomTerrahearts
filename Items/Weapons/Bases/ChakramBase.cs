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
using Terraria.DataStructures;
using Terraria.GameContent.Creative;

namespace KingdomTerrahearts.Items.Weapons.Org13
{
    public abstract class ChakramBase:ModItem
    {

        public int[] damages = new int[] { 1,1 };
        public int[] projectiles = new int[] { ProjectileID.EnchantedBoomerang,ProjectileID.IceBoomerang};
        public int maxChakrams=2;

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            base.SetStaticDefaults();
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            Item.shoot = (projectiles.Length > 1) ? (player.altFunctionUse == 2 ? projectiles[1] : projectiles[0]):Item.shoot;

            int projAmmount = 0;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                projAmmount += (Main.projectile[i].active && Main.projectile[i].type == Item.shoot) ? 1 : 0;
            }
            return projAmmount < maxChakrams && projAmmount<player.HeldItem.stack;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity + new Vector2(0, (player.altFunctionUse == 2) ? -5 : 5), type, damage, knockback, Item.playerIndexTheItemIsReservedFor, (player.altFunctionUse == 2) ? 100 : 0);
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }

    }
}
