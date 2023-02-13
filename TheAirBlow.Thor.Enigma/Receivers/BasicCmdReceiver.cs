﻿// Copyright © TheAirBlow 2022 <theairblow.help@gmail.com>
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.IO;
using TheAirBlow.Thor.Enigma.Exceptions;

namespace TheAirBlow.Thor.Enigma.Receivers;

/// <summary>
/// A basic command.
/// </summary>
public class BasicCmdReceiver : IReceiver
{
    /// <summary>
    /// Arguments received
    /// </summary>
    public readonly List<int> Arguments = new();
    
    /// <summary>
    /// Receive byte buffer
    /// </summary>
    /// <param name="buf">Byte Buffer</param>
    public void Receive(byte[] buf)
    {
        using var memory = new MemoryStream(buf);
        using var reader = new BinaryReader(memory);
        var value1 = reader.ReadInt32();
        if (value1 != _expectedValue) {
            File.WriteAllBytes("data.bin", buf);
            throw new UnexpectedValueException(
                $"Received 0x{value1:X4}, expected 0x{_expectedValue:X4}");
        }

        try {
            while (true)
                Arguments.Add(reader.ReadInt32());
        } catch { /* Ignore */ }
    }

    /// <summary>
    /// Expected value
    /// </summary>
    private int _expectedValue;

    /// <summary>
    /// Byte Ack
    /// </summary>
    /// <param name="packetType">Packet Type</param>
    public BasicCmdReceiver(int packetType)
        => _expectedValue = packetType;
}