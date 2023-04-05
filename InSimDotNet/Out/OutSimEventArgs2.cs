using System;

namespace InSimDotNet.Out {
    /// <summary>
    /// Provides data for the OutSim PacketReceived event.
    /// </summary>
    public class OutSimEventArgs2 : EventArgs {
        /// <summary>
        /// Gets the OutSim packet.
        /// </summary>
        public OutSimPack2 Packet { get; private set; }

        /// <summary>
        /// Creates a new OutSimEventArgs object.
        /// </summary>
        /// <param name="packet">The OutSim packet.</param>
        public OutSimEventArgs2(OutSimPack2 packet) {
            Packet = packet;
        }
    }
}
