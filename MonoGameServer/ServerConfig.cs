using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace MonoGameServer
{
    internal class ServerConfig
    {
        private Dictionary<string, object> config;

        public ServerConfig()
        {
            config = new Dictionary<string, object>();
        }

        public void SetValue<T>(string key, T value)
        {
            config[key] = value;
        }

        public T GetValue<T>(string key)
        {
            if(config.ContainsKey(key))
            {
                if(config[key] is T value)
                {
                    return value;
                }
            }
            return default;
        }
    }
}
