using KingdomTerrahearts.Items.Weapons.Bases;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons.Org13.Larxene
{
    public class Knives_Carmin : ThrowingKnivesBase
    {

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Carmin");
            Tooltip.SetDefault("Tempestuous fighting is the way");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.autoReuse = true;
            Item.damage = 35;
            Item.useTime = Item.useAnimation = 25;
            Item.knockBack = 0.1f;
            Item.maxStack = 1;
            Item.rare = ItemRarityID.Yellow;
            Item.scale = 0.5f;
            Item.shootSpeed = 25;
            Item.UseSound = SoundID.Item19;
            Item.value = 2000;

            knivesToThrow = 1;
            thrownManaUsage = 20;
            meleeDamage = 24;
            thrownDamage = 24;
            meleeSpeed = 20;
            thrownSpeed = 10;
            spread = 0f;
            maxCombo = 3;

            gravityScale = 1.5f;
            pierceAmmount =-1;
            collisionProjectile = ProjectileID.SandnadoFriendly;
            projectileTimeLeft = 60;
            projectileVelMult = 0f;
            manaSteal = 10;

            tpProjectile = ProjectileID.SandnadoFriendly;
            tpProjUsesProjTimeLeft= true;
            tpProjIgnoresGround = true;
            tpProjVelMult = 0f;

            dashDir = new Vector2(0, -20);
            dashTime = 20;
            ignoreFloor = true;

        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Knives_Trancheuse>())
                .AddIngredient(ModContent.ItemType<Materials.blazingShard>(), 10)
                .AddIngredient(ItemID.SandBlock, 10)
                .AddIngredient(ItemID.SandstorminaBottle)
                .AddTile(TileID.Anvils).Register();
        }

    }
}
