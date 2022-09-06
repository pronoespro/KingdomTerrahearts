using KingdomTerrahearts.Extra;
using KingdomTerrahearts.Items.Weapons.Bases;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons.Joke
{
    public class dreamRod:RodBase
    {

        public override void SetStaticDefaults()
        {
			base.SetStaticDefaults();
            DisplayName.SetDefault("Dream Rod");
            Tooltip.SetDefault("The power of the mystic" +
                "\nInner strength" +
                "\nA staff of power and ruin" +
                "\nDoes magic combos (magic missile->Meteor)");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            projectiles = new int[] { ProjectileID.MagicMissile, ProjectileID.MagicMissile,ProjectileID.MagicMissile, ProjectileID.Meteor2};
            projectilCombos = new int[] { 3,1,1,1};
            projectileAmmount = new int[] {1,2,3,4 };
            shootType = new projShootType[] { projShootType.normal,projShootType.enix,projShootType.circular,projShootType.rand};
            projectileDistanceWhenShot = new float[] {0,50,100,150 };
            damages = new int[] {10,7,5,10 };
            shootSpeeds = new float[] { 5f, 7f, 15f, 150f };

            Item.autoReuse = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.FallenStar)
            .AddTile(TileID.WorkBenches)
            .Register();
        }

    }
}
