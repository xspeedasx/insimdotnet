﻿using System;

namespace InSimDotNet.Packets {
    /// <summary>
    /// Represents the <see cref="IS_HLV"/> packets's HLVC property.
    /// </summary>
    [Flags]
    public enum HlvcFlags {
        /// <summary>
        /// Car drove off-track.
        /// </summary>
        Ground = 0,

        /// <summary>
        /// Car has hit a wall.
        /// </summary>
        Wall = 1,

        /// <summary>
        /// Car was speeding in pits.
        /// </summary>
        Speeding = 4,

        /// <summary>
        /// Car went out of bounds.
        /// </summary>
        OutOfBounds = 5,
    }
}
