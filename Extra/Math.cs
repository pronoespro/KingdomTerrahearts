using Microsoft.Xna.Framework;
using System;

namespace KingdomTerrahearts.Extra
{
    public static class MathHelp
    {
        public static float Magnitude(Vector2 vector)
        {
            return (Math.Abs(vector.X) + Math.Abs(vector.Y))/2;
        }

        public static Vector2 Normalize(Vector2 vector)
        {
            Vector2 ret = vector / Magnitude(vector);
            return (Magnitude(ret)==0)? new Vector2(1,0) : ret;
        }

        public static float Sign(float number)
        {
            if (number == 0) return 0;
            return number / Math.Abs(number);
        }

		public static bool IsInBounds(Vector2 a,Vector2 boundMin,Vector2 boundMax)
        {
			return a.X > boundMin.X && a.X < boundMax.X && a.Y > boundMin.Y && a.Y < boundMax.Y;
        }

        public static float QuaternionToDegree(double q)
        {
            return (float)Math.Pow(q, 3)/2f;
        }

        public static float DegreeToQuat(double d)
        {
            return (float)Math.Sqrt(d* 2f);
        }

		public static float Lerp(float min, float max,float t)
        {
			float result = max - min;
			result *= t;
			result += min;
			return result;
        }

    }


	//Angles


	public struct Angle
	{
		public float Value;

		public Angle(float angle)
		{
			Value = angle;
			float remainder = Value % (2f * (float)Math.PI);
			float rotations = Value - remainder;
			Value -= rotations;
			if (Value < 0f)
			{
				Value += 2f * (float)Math.PI;
			}
		}

		public static Angle operator +(Angle a1, Angle a2)
			=> new Angle(a1.Value + a2.Value);

		public static Angle operator -(Angle a1, Angle a2)
			=> new Angle(a1.Value - a2.Value);

		public Angle Opposite()
			=> new Angle(Value + (float)Math.PI);

		public bool ClockwiseFrom(Angle other)
		{
			if (other.Value >= (float)Math.PI)
			{
				return Value < other.Value && Value >= other.Opposite().Value;
			}
			return Value < other.Value || Value >= other.Opposite().Value;
		}

		public bool Between(Angle cLimit, Angle ccLimit)
		{
			if (cLimit.Value < ccLimit.Value)
			{
				return Value >= cLimit.Value && Value <= ccLimit.Value;
			}
			return Value >= cLimit.Value || Value <= ccLimit.Value;
		}
	}

}