using System;

namespace InSimDotNet.Out;

/// <summary>
///     Represents the OutSim packet structure.
/// </summary>
public class OutSimPack2
{
    /// <summary>
    ///     Header of packet. Should be 'LFST' if OSOpts contains OSO_HEADER flag.
    /// </summary>
    public string Header { get; set; } = "";

    /// <summary>
    ///     OutSim ID from cfg.txt.
    /// </summary>
    public int ID { get; set; }

    /// <summary>
    ///     Time in milliseconds (to check order).
    /// </summary>
    public uint Time { get; set; }

    /// <summary>
    ///     Vehicle position and velocity part.
    /// </summary>
    public OutSimMain OSMain { get; set; }

    /// <summary>
    ///     Vehicle inputs part.
    /// </summary>
    public OutSimInputs OSInputs { get; set; }

    /// <summary>
    ///     0=R, 1=N, 2=first gear.
    /// </summary>
    public byte Gear { get; set; }

    /// <summary>
    ///     Spare.
    /// </summary>
    public byte Sp1 { get; set; }

    /// <summary>
    ///     Spare.
    /// </summary>
    public byte Sp2 { get; set; }

    /// <summary>
    ///     Spare.
    /// </summary>
    public byte Sp3 { get; set; }

    /// <summary>
    ///     Radians/s.
    /// </summary>
    public float EngineAngVel { get; set; }

    /// <summary>
    ///     Nm: output torque for throttle 1.0.
    /// </summary>
    public float MaxTorqueAtVel { get; set; }

    /// <summary>
    ///     M - travelled by car.
    /// </summary>
    public float CurrentLapDist { get; set; }

    /// <summary>
    ///     M - track ruler measurement.
    /// </summary>
    public float IndexedDistance { get; set; }

    /// <summary>
    ///     Wheels data.
    /// </summary>
    public OutSimWheel[] OSWheels { get; set; }

    /// <summary>
    ///     Nm: steering torque on front wheels (proportional to force feedback).
    /// </summary>
    public float SteerTorque { get; set; }

    /// <summary>
    ///     Spare.
    /// </summary>
    public float Spare { get; set; }

    /// <summary>
    ///     Constructs an instance of the OutSimPack2 with the specified OutSim options.
    /// </summary>
    /// <param name="outSimOptions">The OutSim options to use.</param>
    public OutSimPack2(byte[] buffer, OutSimOptions outSimOptions)
    {
        if (buffer == null)
        {
            throw new ArgumentNullException("buffer");
        }

        var reader = new PacketReader(buffer);
        if ((outSimOptions & OutSimOptions.OSO_HEADER) == OutSimOptions.OSO_HEADER)
        {
            Header = reader.ReadString(4);
        }

        if ((outSimOptions & OutSimOptions.OSO_ID) == OutSimOptions.OSO_ID)
        {
            ID = reader.ReadInt32();
        }

        if ((outSimOptions & OutSimOptions.OSO_TIME) == OutSimOptions.OSO_TIME)
        {
            Time = reader.ReadUInt32();
        }

        if ((outSimOptions & OutSimOptions.OSO_MAIN) == OutSimOptions.OSO_MAIN)
        {
            OSMain = new OutSimMain(reader);
        }

        if ((outSimOptions & OutSimOptions.OSO_INPUTS) == OutSimOptions.OSO_INPUTS)
        {
            OSInputs = new OutSimInputs(reader);
        }

        if ((outSimOptions & OutSimOptions.OSO_DRIVE) == OutSimOptions.OSO_DRIVE)
        {
            Gear = reader.ReadByte();
            Sp1 = reader.ReadByte();
            Sp2 = reader.ReadByte();
            Sp3 = reader.ReadByte();
            EngineAngVel = reader.ReadSingle();
            MaxTorqueAtVel = reader.ReadSingle();
        }

        if ((outSimOptions & OutSimOptions.OSO_DISTANCE) == OutSimOptions.OSO_DISTANCE)
        {
            CurrentLapDist = reader.ReadSingle();
            IndexedDistance = reader.ReadSingle();
        }

        if ((outSimOptions & OutSimOptions.OSO_WHEELS) == OutSimOptions.OSO_WHEELS)
        {
            OSWheels = new[]
            {
                new OutSimWheel(reader),
                new OutSimWheel(reader),
                new OutSimWheel(reader),
                new OutSimWheel(reader)
            };
        }

        if ((outSimOptions & OutSimOptions.OSO_EXTRA_1) == OutSimOptions.OSO_EXTRA_1)
        {
            SteerTorque = reader.ReadSingle();
            Spare = reader.ReadSingle();
        }
    }
}

/// <summary>
///     Vehicle position and velocity part.
/// </summary>
public class OutSimMain
{
    /// <summary>
    ///     3 floats, angular velocity vector
    /// </summary>
    public Vector AngVel { get; set; }

    /// <summary>
    ///     anticlockwise from above (Z)
    /// </summary>
    public float Heading { get; set; }

    /// <summary>
    ///     anticlockwise from right (X)
    /// </summary>
    public float Pitch { get; set; }

    /// <summary>
    ///     anticlockwise from front (Y)
    /// </summary>
    public float Roll { get; set; }

    /// <summary>
    ///     3 floats X, Y, Z
    /// </summary>
    public Vector Accel { get; set; }

    /// <summary>
    ///     3 floats X, Y, Z
    /// </summary>
    public Vector Vel { get; set; }

    /// <summary>
    ///     3 ints   X, Y, Z (1m = 65536)
    /// </summary>
    public Vec Pos { get; set; }

    public OutSimMain(PacketReader reader)
    {
        AngVel = reader.ReadVector();
        Heading = reader.ReadSingle();
        Pitch = reader.ReadSingle();
        Roll = reader.ReadSingle();
        Accel = reader.ReadVector();
        Vel = reader.ReadVector();
        Pos = reader.ReadVec();
    }
}

/// <summary>
///     Vehicle inputs part.
/// </summary>
public class OutSimInputs
{
    /// <summary> 0 to 1 </summary>
    public float Throttle { get; set; }

    /// <summary> 0 to 1 </summary>
    public float Brake { get; set; }

    /// <summary> radians </summary>
    public float InputSteer { get; set; }

    /// <summary> 0 to 1 </summary>
    public float Clutch { get; set; }

    /// <summary> 0 to 1 </summary>
    public float Handbrake { get; set; }

    public OutSimInputs(PacketReader reader)
    {
        Throttle = reader.ReadSingle();
        Brake = reader.ReadSingle();
        InputSteer = reader.ReadSingle();
        Clutch = reader.ReadSingle();
        Handbrake = reader.ReadSingle();
    }
}

/// <summary>
///     Struct representing wheel data
/// </summary>
public class OutSimWheel
{
    /// <summary>
    ///     Compression from unloaded
    /// </summary>
    public float SuspDeflect { get; set; }

    /// <summary>
    ///     Including Ackermann and toe
    /// </summary>
    public float Steer { get; set; }

    /// <summary>
    ///     Force right
    /// </summary>
    public float XForce { get; set; }

    /// <summary>
    ///     Force forward
    /// </summary>
    public float YForce { get; set; }

    /// <summary>
    ///     Perpendicular to surface
    /// </summary>
    public float VerticalLoad { get; set; }

    /// <summary>
    ///     Radians/s
    /// </summary>
    public float AngVel { get; set; }

    /// <summary>
    ///     Radians a-c viewed from rear
    /// </summary>
    public float LeanRelToRoad { get; set; }

    /// <summary>
    ///     Degrees C
    /// </summary>
    public byte AirTemp { get; set; }

    /// <summary>
    ///     (0 to 255 - see below)
    /// </summary>
    public byte SlipFraction { get; set; }

    /// <summary>
    ///     Touching ground
    /// </summary>
    public byte Touching { get; set; }

    /// <summary>
    ///     Spare
    /// </summary>
    public byte Sp3 { get; set; }

    /// <summary>
    ///     Slip ratio
    /// </summary>
    public float SlipRatio { get; set; }

    /// <summary>
    ///     Tangent of slip angle
    /// </summary>
    public float TanSlipAngle { get; set; }

    public OutSimWheel(PacketReader reader)
    {
        SuspDeflect = reader.ReadSingle();
        Steer = reader.ReadSingle();
        XForce = reader.ReadSingle();
        YForce = reader.ReadSingle();
        VerticalLoad = reader.ReadSingle();
        AngVel = reader.ReadSingle();
        LeanRelToRoad = reader.ReadSingle();
        AirTemp = reader.ReadByte();
        SlipFraction = reader.ReadByte();
        Touching = reader.ReadByte();
        Sp3 = reader.ReadByte();
        SlipRatio = reader.ReadSingle();
        TanSlipAngle = reader.ReadSingle();
    }
}