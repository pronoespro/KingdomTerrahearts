using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items
{
    public class WielderSoul: AbilityBase
    {


        public float jumpHeight = 10;
        public int jumpCount = 1;

        public float glideTime = 120;
        public bool noFallDamage = false;

        public float dashSpeed = 7.5f;
        public int dashReaload = 360;
        public bool canDashMidair = false;

        public int invulnerabilityFrames = 1;
        public int reloadTime = 180;
        public bool autoHP = false;
        public int autoHPReload = 180 * 60;
        public int recoveredHp = 1;

        public int healAmmount = 30;
        public int invulnerability = 0;
        public int minManaCost = 40;


        public override void SetStaticDefaults()
        {
            abilityName = "Wielder Soul";
            DisplayName.SetDefault(abilityName + " level " + (level + 1).ToString());
            Tooltip.SetDefault("An ability that combines all previous ones" +
                "\nAllows you to jump again in midair, dash and glide" +
                "\nYou can use magic to heal and keep one hp after a fatal attack (has cooldown)" +
                "\nYou can run really fast and walk on water");
        }

        public override void UpdateEquip(Player player)
        {
            initLvl = 5;
            abilityName = "Max Mobility";
            SoraPlayer sp = player.GetModPlayer<SoraPlayer>();

            sp.hasSecondChance = true;
            sp.secondChanceInvulnerability = invulnerabilityFrames;
            sp.secondChanceReload = reloadTime;
            sp.hasAutoHP = autoHP;
            sp.autoHPRecover = recoveredHp;
            sp.autoHPReload = autoHPReload;

            sp.canDoubleJump = true;
            sp.doubleJumpHeight += jumpHeight;
            sp.doubleJumpQuantity += jumpCount;

            if (noFallDamage)
                player.noFallDmg = true;
            sp.canGlide = true;
            sp.glideTime += glideTime;
            sp.ChangeGlideFallSpeed(0.5f);

            sp.dashSpeed += dashSpeed;
            sp.canDash = true;
            sp.canDashMidAir = canDashMidair;
            sp.ChangeDashReload(dashReaload);
            base.UpdateEquip(player);

            sp.canCastHeal = true;
            sp.castHealAmount = healAmmount;
            sp.castHealInvulnerabilityTime = invulnerability;
            sp.castHealCost = minManaCost;

            player.maxRunSpeed += 10f;
            player.waterWalk = true;
        }

        public override void UpdateInventory(Player player)
        {
            abilityName = "Wielder soul";
            initLvl = 5;

            abilityTooltips = new string[]
            {
                "Glide lasts for "+ ((glideTime/60).ToString())+" seconds" +
                "\n"+((canDashMidair)?"Dash can be used in mid-air":"Dash cannot be used midair")
            };
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<MaxMobility>())
            .AddIngredient(ModContent.ItemType<Heal>())
            .AddIngredient(ModContent.ItemType<secondChance>())
            .AddTile(TileID.TinkerersWorkbench)
            .Register();

        }

        public override void RaiseLevel()
        {
            base.RaiseLevel();
            SoraPlayer sp = Main.player[Item.playerIndexTheItemIsReservedFor].GetModPlayer<SoraPlayer>();
            sp.RaiseSoulLevel(this);
        }

        public override void ResetLevelEffects()
        {
            base.ResetLevelEffects();
            glideTime = 120;

            jumpHeight = 7;
            jumpCount = 1;

            canDashMidair = false;
            dashSpeed = 7.5f;

            invulnerabilityFrames = 1;
            recoveredHp = 1;
            reloadTime = 180;
            autoHP = false;
            autoHPReload = 180 * 60;
            recoveredHp = 1;

            healAmmount = 10;
            invulnerability = 0;
            minManaCost = 40;
        }

    }
}
