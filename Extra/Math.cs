using Microsoft.Xna.Framework;
using System;

namespace KingdomTerrahearts.Extra
{
    public static class MathHelp
    {
        public static float Magnitude(Vector2 vector)
        {
            return Math.Abs(vector.X) + Math.Abs(vector.Y)/2;
        }

        public static Vector2 Normalize(Vector2 vector)
        {
            return vector/Magnitude(vector);
        }

        public static float Sign(float number)
        {
            if (number == 0) return 0;
            return number / Math.Abs(number);
        }
    }
}
