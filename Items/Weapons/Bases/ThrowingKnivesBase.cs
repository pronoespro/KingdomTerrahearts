using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons.Bases
{
    public abstract class ThrowingKnivesBase:ModItem
    {

        public int knivesToThrow = 3;
        public int collisionProjectile=ProjectileID.None;
        public int maxCombo = 1;

        public int meleeDamage=1, thrownDamage=1;
        public int thrownManaUsage=2;
        public int meleeSpeed=30, thrownSpeed=10;
        public float spread = 0.25f;

        protected int curCombo;

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.width = Item.height = 50;
            Item.DamageType = DamageClass.Throwing;
            Item.holdStyle = ItemHoldStyleID.HoldFront;
            Item.useStyle = ItemUseStyleID.Swing;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {

                if (player.altFunctionUse == 2)
                {
                    Item.damage = meleeDamage;
                    Item.DamageType = DamageClass.Throwing;
                    Item.mana = thrownManaUsage;
                    Item.useStyle = ItemUseStyleID.Shoot;
                    Item.shoot = ProjectileID.ThrowingKnife;
                    Item.noMelee = true;
                    Item.useAnimation = Item.useTime = thrownSpeed;
                }
                else
                {

                    SoraPlayer sp = player.GetModPlayer<SoraPlayer>();
                    sp.GetCloserToEnemy(300, 10);

                    Item.damage = thrownDamage;
                    Item.DamageType = DamageClass.Melee;
                    Item.mana = 0;
                    GetComboAnimation();
                    Item.shoot = ProjectileID.None;
                    Item.noMelee = false;
                    Item.useAnimation = Item.useTime = meleeSpeed;
                }
            }


            return base.CanUseItem(player);
        }

        public void GetComboAnimation()
        {
            curCombo = (curCombo + 1) % maxCombo;
            switch (curCombo)
            {
                default:
                case 0:
                    Item.useStyle = ItemUseStyleID.Swing;
                    break;
                case 1:
                    Item.useStyle = ItemUseStyleID.Rapier;
                    break;
            }
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            player.fullRotation = player.itemAnimation/player.itemAnimationMax;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            for(int i = 0; i < knivesToThrow; i++)
            {
                float curPos = (float)((i+1 - knivesToThrow/2f)/knivesToThrow*MathF.PI*spread);

                float rotatedVel = MathF.Atan2(MathHelp.Normalize(velocity).Y, MathHelp.Normalize(velocity).X) + curPos;

                Projectile.NewProjectile(source, position, new Vector2(MathF.Cos(rotatedVel), MathF.Sin(rotatedVel))*MathHelp.Magnitude(velocity), type, damage, knockback, Item.playerIndexTheItemIsReservedFor);

            }

            return false;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            return base.PreDrawInWorld(spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
        }

    }
}
