﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Web;

namespace DigiGuard
{
    /// <summary>
    /// Salted password hashing with PBKDF2-SHA1.
    /// </summary>
    public class StringCipher
    {
        // The following constants may be changed without breaking existing hashes.
        public const int SaltByteSize = 24;
        public const int HashByteSize = 24;
        public const int Pbkdf2Iterations = 1000;

        public const int IterationIndex = 0;
        public const int SaltIndex = 1;
        public const int Pbkdf2Index = 2;

        /// <summary>
        /// Creates a salted PBKDF2 hash of the password.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <returns>The hash of the password.</returns>
        public static string CreateHash(string password)
        {
            // Generate a random salt
            RNGCryptoServiceProvider csprng = new RNGCryptoServiceProvider();
            byte[] salt = new byte[SaltByteSize];
            csprng.GetBytes(salt);

            // Hash the password and encode the parameters
            byte[] hash = PBKDF2(password, salt, Pbkdf2Iterations, HashByteSize);
            return Convert.ToBase64String(salt) + ":" +
                Convert.ToBase64String(hash);
        }

        /// <summary>
        /// Validates a password given a hash of the correct one.
        /// </summary>
        /// <param name="password">The password to check.</param>
        /// <param name="hash">A hash of the correct password.</param>
        /// <param name="salt">The salt used to hash the original password</param>
        /// <returns>True if the password is correct. False otherwise.</returns>
        public static bool ValidatePassword(string password, string hash, string salt)
        {
            // Extract the parameters from the hash
            byte[] saltBytes = Convert.FromBase64String(salt);
            byte[] hashBytes = Convert.FromBase64String(hash);
            byte[] testHash = PBKDF2(password, saltBytes, Pbkdf2Iterations, HashByteSize);
            return SlowEquals(hashBytes, testHash);
        }

        /// <summary>
        /// Compares two byte arrays in length-constant time. This comparison
        /// method is used so that password hashes cannot be extracted from
        /// on-line systems using a timing attack and then attacked off-line.
        /// </summary>
        /// <param name="a">The first byte array.</param>
        /// <param name="b">The second byte array.</param>
        /// <returns>True if both byte arrays are equal. False otherwise.</returns>
        private static bool SlowEquals(byte[] a, byte[] b)
        {
            uint diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
                diff |= (uint)(a[i] ^ b[i]);
            return diff == 0;
        }

        /// <summary>
        /// Computes the PBKDF2-SHA1 hash of a password.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <param name="salt">The salt.</param>
        /// <param name="iterations">The PBKDF2 iteration count.</param>
        /// <param name="outputBytes">The length of the hash to generate, in bytes.</param>
        /// <returns>A hash of the password.</returns>
        private static byte[] PBKDF2(string password, byte[] salt, int iterations, int outputBytes)
        {
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt);
            pbkdf2.IterationCount = iterations;
            return pbkdf2.GetBytes(outputBytes);
        }
    }
}
