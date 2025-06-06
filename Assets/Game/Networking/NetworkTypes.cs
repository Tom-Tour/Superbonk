namespace Game.Networking
{
    public struct PlayerSlot
    {
        public readonly ulong ClientId;
        public readonly int LocalIndex;

        public PlayerSlot(ulong clientId, int localIndex)
        {
            ClientId = clientId;
            LocalIndex = localIndex;
        }
    }
}