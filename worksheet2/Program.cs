using System;
using System.Text;

namespace comp101_worksheet2
{
    public class Program
    {
        public static void Main(string[] args)
        {

        }

        /* CONVERSION LOGIC BELOW */

        /// <summary>
        /// Convert one 'ascii' character to hexadecimal 
        /// </summary>
        /// <param name="myValue">The value to convert</param>
        /// <returns>A value between 0-15 if valid, else -1</returns>
        public static int HexDigit(char myValue)
        {
            if ('0' <= myValue && myValue <= '9')
            {
                return myValue - '0';  // Convert '0'-'9' to 0-9
            }
            else if ('A' <= myValue && myValue <= 'F')
            {
                return (myValue - 'A') + 10;  // Convert 'A'-'F' to 10-15
            }
            else if ('a' <= myValue && myValue <= 'f')
            {
                return (myValue - 'a') + 10;  // Handle lowercase 'a'-'f'
            }

            return -1;  // Return -1 for invalid characters
        }

        /// <summary>
        /// Convert a provided integer to hexadecimal.
        /// </summary>
        /// <param name="value">the integer to convert</param>
        /// <returns>The hexadecimal value, or "ERROR" if invalid</returns>
        public static string ConvertToHex(int value)
        {
            // Special case: Negative
            if (value < 0)
            {
                return "ERROR";
            }
            // Special case: Zero
            if (value == 0)
            {
                return "0x0";
            }

            // Convert decimal to hexadecimal
            string hexValue = "";
            while (value > 0)
            {
                int remainder = value % 16;
                char hexChar = (char)(remainder < 10 ? ('0' + remainder) : ('A' + (remainder - 10)));
                hexValue = hexChar + hexValue;  // Prepend the hex character to the result string
                value /= 16;
            }

            return "0x" + hexValue;  // Return the complete hexadecimal string with '0x' prefix
        }

        /// <summary>
        /// Convert a provided hexadecimal string to an integer.
        /// </summary>
        /// <param name="hexString">The hexadecimal value as a string</param>
        /// <returns>The corresponding (base 10) integer, or -1 on error</returns>
        public static int ConvertToInt(string hexString)
        {
            // Ensure the string starts with "0x" prefix
            if (string.IsNullOrEmpty(hexString) || !hexString.StartsWith("0x"))
            {
                return -1;
            }

            // Remove the "0x" prefix
            hexString = hexString.Substring(2);

            int result = 0;
            foreach (char c in hexString)
            {
                int digit = HexDigit(c);
                if (digit == -1)
                {
                    return -1;  // Return -1 if an invalid character is found
                }

                result = result * 16 + digit;  // Convert hex to decimal
            }

            return result;  // Return the integer value of the hex string
        }

        /// <summary>
        /// Convert a 'middle endian' hex value back to its integer version.
        /// </summary>
        /// <param name="hexString">The 'middle endian' hexadecimal value</param>
        /// <returns>The corresponding (base 10) integer, or -1 on error</returns>
        public static int ChallengeMiddleEndianHex(string hexString)
        {
            // Ensure the string starts with "0x" prefix
            if (string.IsNullOrEmpty(hexString) || !hexString.StartsWith("0x"))
            {
                return -1;
            }

            // Remove the "0x" prefix
            hexString = hexString.Substring(2);

            // If the length is not even, return an error
            if (hexString.Length % 4 != 0)
            {
                return -1;
            }

            // Reverse the byte order in the string
            StringBuilder reversed = new StringBuilder();
            for (int i = hexString.Length - 2; i >= 0; i -= 2)
            {
                reversed.Append(hexString[i]);
                reversed.Append(hexString[i + 1]);
            }

            // Now convert the reversed string back to an integer
            return ConvertToInt("0x" + reversed.ToString());
        }

        /* VECTOR LOGIC BELOW */

        public static float[] addVectors(float[] vec1, float[] vec2)
        {
            if (vec1.Length != vec2.Length) return new float[0];
            float[] result = new float[vec1.Length];
            for (int i = 0; i < vec1.Length; i++)
            {
                result[i] = vec1[i] + vec2[i];
            }
            return result;
        }

        public static float[] subVectors(float[] vec1, float[] vec2)
        {
            if (vec1.Length != vec2.Length) return new float[0];
            float[] result = new float[vec1.Length];
            for (int i = 0; i < vec1.Length; i++)
            {
                result[i] = vec1[i] - vec2[i];
            }
            return result;
        }

        public static float lengthVector(float[] vec)
        {
            if (vec.Length == 0) return 0.0F;
            float sum = 0;
            foreach (float component in vec)
            {
                sum += component * component;
            }
            return MathF.Sqrt(sum);
        }

        public static float vectorDistance(float[] vec1, float[] vec2)
        {
            if (vec1.Length != vec2.Length) return 0.0F;
            float sum = 0;
            for (int i = 0; i < vec1.Length; i++)
            {
                sum += (vec1[i] - vec2[i]) * (vec1[i] - vec2[i]);
            }
            return MathF.Sqrt(sum);
        }
    }
}

