using System;

namespace Game.Networking
{
    public struct PlayerSlot : IEquatable<PlayerSlot>
    {
        public readonly ulong ClientId;
        public readonly int LocalIndex;

        public PlayerSlot(ulong clientId, int localIndex)
        {
            ClientId = clientId;
            LocalIndex = localIndex;
        }

        public bool Equals(PlayerSlot other)
        {
            return ClientId == other.ClientId && LocalIndex == other.LocalIndex;
        }
        public override bool Equals(object obj)
        {
            return obj is PlayerSlot other && Equals(other);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(ClientId, LocalIndex);
        }
    }
}