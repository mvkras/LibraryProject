﻿using System;
using System.Security.Cryptography;
namespace Diplom.Krasnov__WindowsForms.Utils
{
    // https://stackoverflow.com/questions/4181198/how-to-hash-a-password/10402129#10402129 //на будущее оставил ссылку про это
    public static class Password
    {
        public static string GetHash(string password) //хэширование паролей пользователей
        {
            byte[] salt = new byte[16];
            new RNGCryptoServiceProvider().GetBytes(salt);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            return Convert.ToBase64String(hashBytes);
        }

        public static bool Verify(string password, string savedPasswordHash)
        {
            /* извлечение байтов */
            byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);
            /* извлечение переменной salt */
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            /* система хэширует пароль, введенный пользователем */
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);
            /* компиляция результатов */
            for (int i = 0; i < 20; i++)
                if (hashBytes[i + 16] != hash[i])
                    return false;
            return true;
        }
    }
}
