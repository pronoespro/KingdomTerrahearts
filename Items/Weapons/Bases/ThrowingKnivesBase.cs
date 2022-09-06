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
        public int maxCombo = 3;

        public int meleeDamage=1, thrownDamage=1;
        public int thrownManaUsage=2;
        public int meleeSpeed=30, thrownSpeed=10;
        public float spread = 0.25f;
        public float gravityScale = 1f;
        public int pierceAmmount = -1;
        public int projectileTimeLeft=0;
        public float projectileVelMult = 1f;
        public int manaSteal=3;
        public int lastUsedTime;

        public int tpProjectile = ProjectileID.None;
        public bool tpProjUsesProjTimeLeft = false;
        public bool tpProjIgnoresGround = false;
        public float tpProjVelMult = 1f;

        public Vector2 dashDir = new Vector2(0, 40);
        public int dashTime=12;
        public bool ignoreFloor = false;

        protected int curCombo;

        private int getInvencibilityAmmount;

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            base.SetStaticDefaults();
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            int manaRecoverAmmount = (player.statMana < player.statManaMax - manaSteal) ? manaSteal : player.statManaMax - player.statMana;
            if (manaRecoverAmmount > 0)
            {
                player.statMana += manaRecoverAmmount;
                player.ManaEffect(manaRecoverAmmount);
            }

            getInvencibilityAmmount = 3;

            base.OnHitNPC(player, target, damage, knockBack, crit);
        }

        public override void SetDefaults()
        {
            Item.width = Item.height = 50;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.useStyle = ItemUseStyleID.Swing;

            Item.noMelee = false;
            Item.channel = false;
            Item.noUseGraphic = false;
            Item.reuseDelay = 0;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                lastUsedTime = 0;
                if (player.altFunctionUse == 2)
                {
                    Item.damage = thrownDamage;
                    Item.useAnimation = Item.useTime = thrownSpeed;
                    Item.DamageType = DamageClass.Throwing;
                    Item.mana = thrownManaUsage;
                    Item.useStyle = ItemUseStyleID.Shoot;
                    Item.shoot = ModContent.ProjectileType<Projectiles.Weapons.LarxeneThrowingKnives_projectiles>();
                    Item.noMelee = true;
                }
                else
                {

                    Item.autoReuse = true;
                    if (getInvencibilityAmmount > 0)
                    {
                        getInvencibilityAmmount--;
                        player.GetModPlayer<SoraPlayer>().SetContactinvulnerability((int)(Item.useTime * 2));
                    }
                    Item.damage = meleeDamage;
                    Item.DamageType = DamageClass.Melee;
                    Item.mana = 0;
                    Item.shoot = ProjectileID.None;
                    Item.noMelee = false;
                    Item.useAnimation = Item.useTime = meleeSpeed;

                    GetComboAnimation(player);
                }
            }

            curCombo++;
            if (curCombo >= maxCombo)
            {
                curCombo = 0;
            }

            return true;
        }

        public override bool? UseItem(Player player)
        {
            lastUsedTime = 0;
            return true;
        }

        public void GetComboAnimation(Player player)
        {
            SoraPlayer sp = player.GetModPlayer<SoraPlayer>();
            switch (curCombo)
            {
                default:
                case 0:
                    Item.useStyle = ItemUseStyleID.Swing;
                    sp.GetCloserToEnemy(300, 10);
                    break;
                case 1:
                    Item.useStyle = ItemUseStyleID.Thrust;
                    sp.GetCloserToEnemy(300, 10);
                    break;
                case 2:
                    Item.useStyle = ItemUseStyleID.Thrust;

                    int target =sp.GetClosestEnemy(300);


                    if (target >=0)
                    {
                        Vector2 realDashDir = new Vector2(dashDir.X * player.direction, dashDir.Y);
                        if (CanTPToEnemy(target, player,realDashDir)) {
                            player.Center = Main.npc[target].Center - realDashDir * (dashTime / 2);
                            
                            sp.AttackMovement(realDashDir, dashTime, ignoreFloor);
                            ShootTPProjectile(player);

                            sp.AddInvulnerability(5);
                            sp.tpFallImmunity = dashTime;
                        }
                    }
                    break;
                case 3:
                    Item.useStyle = ItemUseStyleID.Thrust;
                    sp.GetCloserToEnemy(300, 10);
                    break;
            }

        }

        public void ShootTPProjectile(Player player)
        {
            if (tpProjectile > 0)
            {
                EntitySource_ItemUse source = new EntitySource_ItemUse(player, Item);
                int proj = Projectile.NewProjectile(source, player.Center, player.velocity*tpProjVelMult, tpProjectile, Item.damage, Item.knockBack, Item.playerIndexTheItemIsReservedFor);
                Main.projectile[proj].penetrate = pierceAmmount;
                Main.projectile[proj].timeLeft = (tpProjUsesProjTimeLeft)?projectileTimeLeft:Main.projectile[proj].timeLeft;
                Main.projectile[proj].tileCollide = !tpProjIgnoresGround;
                Main.projectile[proj].friendly = true;
                Main.projectile[proj].hostile = false;
            }
        }

        public bool CanTPToEnemy(int target, Player player,Vector2 dir)
        {
            if (ignoreFloor || 
            Collision.CanHitLine(Main.npc[target].Center - dir * (dashTime/2), player.width, player.height, Main.npc[target].Center, player.width / 2, player.height / 2))
            {
                return true;
            }
            return false;
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            player.fullRotation = player.itemAnimation/player.itemAnimationMax;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            for(int i = 0; i < knivesToThrow; i++)
            {
                float curPos = (i+(knivesToThrow%2==0?1:0.5f) - knivesToThrow/2f)/knivesToThrow*MathF.PI*spread;

                float rotatedVel = MathF.Atan2(MathHelp.Normalize(velocity).Y, MathHelp.Normalize(velocity).X) + curPos;

                int proj=Projectile.NewProjectile(source, position, new Vector2(MathF.Cos(rotatedVel), MathF.Sin(rotatedVel))*MathHelp.Magnitude(velocity), type, damage, knockback, Item.playerIndexTheItemIsReservedFor);
                Main.projectile[proj].penetrate = pierceAmmount;

                Projectiles.Weapons.LarxeneThrowingKnives_projectiles knive=(Projectiles.Weapons.LarxeneThrowingKnives_projectiles) Main.projectile[proj].ModProjectile;
                knive.textureToDraw = Texture;
                knive.gravityEffect = gravityScale;
                knive.projectileOnColision = collisionProjectile;
                knive.spawnProjTimeLeft = projectileTimeLeft;
                knive.spawnProjVelMult = projectileVelMult;

            }

            return false;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            return base.PreDrawInWorld(spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
        }

        public override void UpdateInventory(Player player)
        {
            if ((player.selectedItem >= 0 && player.selectedItem < player.inventory.Length
                && player.inventory[player.selectedItem] == Item)
                || player.HeldItem == Item)
            {
                lastUsedTime++;
                if (lastUsedTime > Item.useTime * 1.5f && curCombo > 0)
                {
                    curCombo = 0;
                    lastUsedTime = 0;
                    getInvencibilityAmmount = 3;
                }

            }
            else
            {
                curCombo = 0;
                lastUsedTime = 0;
                getInvencibilityAmmount = 3;
            }
            base.UpdateInventory(player);
        }

    }
}
