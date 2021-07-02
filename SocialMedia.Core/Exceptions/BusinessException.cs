﻿using System;
using System.Runtime.Serialization;

namespace SocialMedia.Core.Exceptions
{
    public class BusinessException : Exception
    {
        public BusinessException() { }

        public BusinessException(string message) : base(message) { }
    }
}
