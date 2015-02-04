﻿using System;

namespace SunLine.Community.Entities.Exceptions
{
    public class BitlyConfigurationNotFoundException : Exception
    {
        public BitlyConfigurationNotFoundException() 
            : base("You have to configure Bit.ly username and password in Web.config")
        {
        }
    }
}

