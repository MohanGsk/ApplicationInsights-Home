using System;

namespace PingMesh
{
    public class PingTarget : IEquatable<PingTarget>
    {
        public string Uri;
        public int Port;

        public override int GetHashCode()
        {
            return Port.GetHashCode() ^ Uri.GetHashCode();
        }

        public bool Equals(PingTarget other)
        {
            if (other == null)
                return false;

            return this.GetHashCode() == other.GetHashCode();
        }
    }
}

