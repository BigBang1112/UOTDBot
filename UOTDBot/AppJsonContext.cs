﻿using System.Text.Json.Serialization;
using UOTDBot.Models;

namespace UOTDBot;

[JsonSerializable(typeof(Dictionary<string, string>))]
[JsonSerializable(typeof(List<ulong>))]
[JsonSerializable(typeof(MapFeatures))]
internal sealed partial class AppJsonContext : JsonSerializerContext;
