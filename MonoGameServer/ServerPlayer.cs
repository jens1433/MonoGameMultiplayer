using Microsoft.Xna.Framework;
using MonoGameServer.Game;
using MonoGameServer.Packets;
using System;
using System.Collections.Generic;
using System.Net;

namespace MonoGameServer
{
    internal class ServerPlayer
    {
        protected IPEndPoint ipEndPoint;
        protected NetPlayer player;
        protected Dictionary<PacketType, long> lastPacketTimes = new Dictionary<PacketType, long>();

        public IPEndPoint IpEndPoint
        {
            get { return ipEndPoint; }
        }

        public NetPlayer Player
        {
            get { return player; }
        }

        public long GetLastPacketTime(PacketType type)
        {
            if (lastPacketTimes.ContainsKey(type))
            {
                return lastPacketTimes[type];
            }
            return 0;
        }

        public bool AddPacketTime(PacketType type, long time)
        {
            if (!lastPacketTimes.ContainsKey(type))
            {
                lastPacketTimes.Add(type, time);
                return true;
            }
            else
            {
                if (lastPacketTimes[type] <= time)
                {
                    lastPacketTimes[type] = time;
                    return true;
                }
            }
            return false;
        }

        public ServerPlayer(IPEndPoint address, NetPlayer player)
        {
            ipEndPoint = address;
            this.player = player;
        }
    }
}
