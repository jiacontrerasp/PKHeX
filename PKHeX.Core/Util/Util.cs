﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace PKHeX.Core
{
    public static partial class Util
    {
        /// <summary>
        /// Parses the string into an <see cref="int"/>, skipping all characters except for valid digits.
        /// </summary>
        /// <param name="value">String to parse</param>
        /// <returns>Parsed value</returns>
        public static int ToInt32(string value)
        {
            int result = 0;
            if (string.IsNullOrEmpty(value))
                return result;

            bool negative = false;
            foreach (var c in value)
            {
                if (IsNum(c))
                {
                    result *= 10;
                    result += c;
                    result -= '0';
                }
                else if (c == '-' && result == 0)
                {
                    negative = true;
                }
            }
            return negative ? -result : result;
        }

        /// <summary>
        /// Parses the string into a <see cref="uint"/>, skipping all characters except for valid digits.
        /// </summary>
        /// <param name="value">String to parse</param>
        /// <returns>Parsed value</returns>
        public static uint ToUInt32(string value)
        {
            uint result = 0;
            if (string.IsNullOrEmpty(value))
                return result;

            foreach (var c in value)
            {
                if (!IsNum(c))
                    continue;
                result *= 10;
                result += (uint)(c - '0');
            }
            return result;
        }

        /// <summary>
        /// Parses the hex string into a <see cref="uint"/>, skipping all characters except for valid digits.
        /// </summary>
        /// <param name="value">Hex String to parse</param>
        /// <returns>Parsed value</returns>
        public static uint GetHexValue(string value)
        {
            uint result = 0;
            if (string.IsNullOrEmpty(value))
                return result;

            foreach (var c in value)
            {
                if (IsNum(c))
                {
                    result <<= 4;
                    result += (uint)(c - '0');
                }
                else if (IsHexUpper(c))
                {
                    result <<= 4;
                    result += (uint)(c - 'A' + 10);
                }
                else if (IsHexLower(c))
                {
                    result <<= 4;
                    result += (uint)(c - 'a' + 10);
                }
            }
            return result;
        }

        /// <summary>
        /// Parses the hex string into a <see cref="ulong"/>, skipping all characters except for valid digits.
        /// </summary>
        /// <param name="value">Hex String to parse</param>
        /// <returns>Parsed value</returns>
        public static ulong GetHexValue64(string value)
        {
            ulong result = 0;
            if (string.IsNullOrEmpty(value))
                return result;

            foreach (var c in value)
            {
                if (IsNum(c))
                {
                    result <<= 4;
                    result += (uint)(c - '0');
                }
                else if (IsHexUpper(c))
                {
                    result <<= 4;
                    result += (uint)(c - 'A' + 10);
                }
                else if (IsHexLower(c))
                {
                    result <<= 4;
                    result += (uint)(c - 'a' + 10);
                }
            }
            return result;
        }

        public static byte[] GetBytesFromHexString(string seed)
        {
            byte[] result = new byte[seed.Length / 2];
            for (int i = 0; i < result.Length; i++)
            {
                var slice = seed.Substring(i * 2, 2);
                result[i] = Convert.ToByte(slice, 16);
            }
            Array.Reverse(result);
            return result;
        }

        public static string GetHexStringFromBytes(byte[] data, int offset, int length)
        {
            data = data.Slice(offset, length);
            var sb = new StringBuilder(data.Length * 2);
            for (int i = data.Length - 1; i >= 0; i--)
                sb.AppendFormat("{0:x2}", data[i]);
            return sb.ToString();
        }

        private static bool IsNum(char c) => (uint)(c - '0') <= 9;
        private static bool IsHexUpper(char c) => (uint)(c - 'A') <= 5;
        private static bool IsHexLower(char c) => (uint)(c - 'a') <= 5;
        private static bool IsHex(char c) => IsNum(c) || IsHexUpper(c) || IsHexLower(c);
        private static string TitleCase(string word) => char.ToUpper(word[0]) + word.Substring(1, word.Length - 1).ToLower();

        /// <summary>
        /// Filters the string down to only valid hex characters, returning a new string.
        /// </summary>
        /// <param name="str">Input string to filter</param>
        public static string GetOnlyHex(string str) => string.IsNullOrWhiteSpace(str) ? string.Empty : string.Concat(str.Where(IsHex));

        /// <summary>
        /// Returns a new string with each word converted to its appropriate title case.
        /// </summary>
        /// <param name="str">Input string to modify</param>
        public static string ToTitleCase(string str) => string.IsNullOrWhiteSpace(str) ? string.Empty : string.Join(" ", str.Split(' ').Select(TitleCase));

        /// <summary>
        /// Trims a string at the first instance of a 0x0000 terminator.
        /// </summary>
        /// <param name="input">String to trim.</param>
        /// <returns>Trimmed string.</returns>
        public static string TrimFromZero(string input) => TrimFromFirst(input, '\0');

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string TrimFromFirst(string input, char c)
        {
            int index = input.IndexOf(c);
            return index < 0 ? input : input.Substring(0, index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TrimFromFirst(StringBuilder input, char c)
        {
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] != c)
                    continue;
                input.Remove(i, input.Length - i);
                return;
            }
        }

        public static Dictionary<string, int>[] GetMultiDictionary(IReadOnlyList<IReadOnlyList<string>> nameArray)
        {
            var result = new Dictionary<string, int>[nameArray.Count];
            for (int i = 0; i < result.Length; i++)
            {
                var dict = result[i] = new Dictionary<string, int>();
                var names = nameArray[i];
                for (int j = 0; j < names.Count; j++)
                    dict.Add(names[j], j);
            }
            return result;
        }
    }
}
