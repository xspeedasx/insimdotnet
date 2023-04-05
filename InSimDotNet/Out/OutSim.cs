using System;

namespace InSimDotNet.Out {
    /// <summary>
    /// Class to manage a OutSim connection with LFS.
    /// </summary>
    public class OutSim : OutClient {
        /// <summary>
        /// If set to > 0, subscribe to <see cref="PacketReceived2"/> to receive <see cref="OutSimPack2"/>.
        /// </summary>
        public OutSimOptions OutSimOptions { get; set; }

        /// <summary>
        /// Occurs when a OutSim packet is received.
        /// </summary>
        public event EventHandler<OutSimEventArgs> PacketReceived;
        
        /// <summary>
        /// Occurs when a OutSim packet 2 is received.
        /// </summary>
        public event EventHandler<OutSimEventArgs2> PacketReceived2;

        /// <summary>
        /// If options set to > 0, subscribe to <see cref="PacketReceived2"/> to receive <see cref="OutSimPack2"/>.
        /// </summary>
        public OutSim WithOutSimOpts(OutSimOptions options)
        {
            OutSimOptions = options;
            return this;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="OutClient"/> class.
        /// </summary>
        public OutSim()
            : this(TimeSpan.Zero) { }

        /// <summary>
        /// Creates a new instance of the <see cref="OutSim"/> class with the specified timeout.
        /// </summary>
        /// <param name="timeout">The timeout period in milliseconds.</param>
        public OutSim(int timeout)
            : this(TimeSpan.FromMilliseconds(timeout)) { }

        /// <summary>
        /// Creates a new instance of the <see cref="OutSim"/> class with the specified timeout.
        /// </summary>
        /// <param name="timeout">The timeout period</param>
        public OutSim(TimeSpan timeout)
            : base(timeout) { }

        /// <summary>
        /// Called when packet data is received. Override to implement handling for specific packets.
        /// </summary>
        /// <param name="buffer">The packet data.</param>
        protected override void HandlePacket(byte[] buffer) {
            if (buffer == null) {
                throw new ArgumentNullException(nameof(buffer));
            }

            if (OutSimOptions > 0)
            {
                OnPacketReceived2(new OutSimEventArgs2(new OutSimPack2(buffer, OutSimOptions)));
            }

            if (buffer.Length is OutSimPack.MinSize or OutSimPack.MaxSize)
            {
                OnPacketReceived(new OutSimEventArgs(new OutSimPack(buffer)));
            }
        }

        /// <summary>
        /// Raises the PacketReceived event.
        /// </summary>
        /// <param name="e">The <see cref="OutSimEventArgs"/> object containing the event data.</param>
        protected virtual void OnPacketReceived(OutSimEventArgs e) {
            if (PacketReceived is { } temp) {
                temp(this, e);
            }
        }

        /// <summary>
        /// Raises the PacketReceived event.
        /// </summary>
        /// <param name="e">The <see cref="OutSimEventArgs"/> object containing the event data.</param>
        protected virtual void OnPacketReceived2(OutSimEventArgs2 e) {
            if (PacketReceived2 is { } temp) {
                temp(this, e);
            }
        }
    }
}
