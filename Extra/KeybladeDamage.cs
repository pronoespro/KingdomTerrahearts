using System.Collections.Generic;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Extra
{
	public class KeybladeDamage : DamageClass
	{
		public override void SetStaticDefaults()
		{
			// Make weapons with this damage type have a tooltip of 'X example damage'.
			ClassName.SetDefault("Keyblade damage");
		}


        public override bool GetEffectInheritance(DamageClass damageClass)
		{
			return false;
		}

        public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
        {
			StatInheritanceData data = new StatInheritanceData(0.25f, 0.25f, 0.5f, 1f, 1f);

			if (damageClass == Generic)
				return StatInheritanceData.Full;


			if (damageClass == Melee)
				return data;

			return StatInheritanceData.None;
		}

    }
}
