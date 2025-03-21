﻿using static Wazzifni.Enums.Enum;

namespace Wazzifni.Models.TokenAuth
{
    public class AuthenticateResultModel
    {
        public string AccessToken { get; set; }

        public string EncryptedAccessToken { get; set; }

        public int ExpireInSeconds { get; set; }

        public long UserId { get; set; }


        public UserType UserType { get; set; }

    }
}
