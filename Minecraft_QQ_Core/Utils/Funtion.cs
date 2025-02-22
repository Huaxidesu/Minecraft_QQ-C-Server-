﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Minecraft_QQ_Core.Robot;
using Newtonsoft.Json.Linq;
using OneBotSharp.Objs.Message;

namespace Minecraft_QQ_Core.Utils;

public static class Funtion
{
    public static string GetString(this List<string> list)
    {
        var str = new StringBuilder();
        str.Append('[');
        foreach (var item in list)
        {
            str.Append(item).Append(',');
        }
        if (list.Count > 0)
        {
            str.Remove(str.Length - 1, 1);
        }
        str.Append(']');

        return str.ToString();
    }
    public static string RemoveColorCodes(string text)
    {
        if (text.Contains('§') || text.Contains('&'))
        {
            var sb = new StringBuilder(text)
                .Replace("§0", string.Empty)
                .Replace("§1", string.Empty)
                .Replace("§2", string.Empty)
                .Replace("§3", string.Empty)
                .Replace("§4", string.Empty)
                .Replace("§5", string.Empty)
                .Replace("§6", string.Empty)
                .Replace("§7", string.Empty)
                .Replace("§8", string.Empty)
                .Replace("§9", string.Empty)
                .Replace("§a", string.Empty)
                .Replace("§b", string.Empty)
                .Replace("§c", string.Empty)
                .Replace("§d", string.Empty)
                .Replace("§e", string.Empty)
                .Replace("§f", string.Empty)
                .Replace("§r", string.Empty)
                .Replace("§k", string.Empty)
                .Replace("§n", string.Empty)
                .Replace("§m", string.Empty)
                .Replace("§l", string.Empty)
                .Replace("&0", string.Empty)
                .Replace("&1", string.Empty)
                .Replace("&2", string.Empty)
                .Replace("&3", string.Empty)
                .Replace("&4", string.Empty)
                .Replace("&5", string.Empty)
                .Replace("&6", string.Empty)
                .Replace("&7", string.Empty)
                .Replace("&8", string.Empty)
                .Replace("&9", string.Empty)
                .Replace("&a", string.Empty)
                .Replace("&b", string.Empty)
                .Replace("&c", string.Empty)
                .Replace("&d", string.Empty)
                .Replace("&e", string.Empty)
                .Replace("&f", string.Empty)
                .Replace("&r", string.Empty)
                .Replace("&k", string.Empty)
                .Replace("&n", string.Empty)
                .Replace("&m", string.Empty)
                .Replace("&l", string.Empty);
            return sb.ToString();
        }
        else
            return text;
    }
    public static string ReplaceFirst(this string value, string oldValue, string newValue)
    {
        if (string.IsNullOrWhiteSpace(oldValue))
            return value;

        int idx = value.IndexOf(oldValue);
        if (idx == -1)
            return value;
        value = value.Remove(idx, oldValue.Length);
        return value.Insert(idx, newValue);
    }
    public static string GetString(string a, string b, string? c = null)
    {
        int x = a.IndexOf(b) + b.Length;
        int y;
        if (c != null)
        {
            y = a.IndexOf(c, x);
            if (y == -1)
                return a;
            if (a[y - 1] == '"')
            {
                y = a.IndexOf(c, y + 1);
            }
            if (y - x <= 0)
                return a;
            else
                return a[x..y];
        }
        else
            return a[x..];
    }
    public static async Task<string?> GetRich(MsgBase a)
    {
        try
        {
            if (a is MsgJson json)
            {
                var obj = JObject.Parse(json.Data.Data!);
                var app = obj["app"]?.ToString();
                if (app == "com.tencent.qq.checkin")
                {
                    return obj["prompt"]?.ToString() + "-" + obj["meta"]?["checkInData"]?["desc"]?.ToString();
                }
                else if (app == "com.tencent.mannounce")
                {
                    return obj["prompt"]?.ToString();
                }
                else if (app == "com.tencent.structmsg")
                {
                    if (obj["view"]?.ToString() == "music")
                    {
                        return obj["prompt"]?.ToString() + "\n" + obj["meta"]?["music"]?["jumpUrl"]?.ToString();
                    }
                    return obj["prompt"]?.ToString() + "\n" + "链接：" + obj["meta"]?["news"]?["jumpUrl"]?.ToString();
                }
                else if (app == "com.tencent.troopsharecard")
                {
                    return obj["prompt"]?.ToString() + "：" + obj["meta"]?["contact"]?["nickname"]?.ToString()
                        + "\n" + obj["meta"]?["contact"]?["jumpUrl"]?.ToString();
                }
            }
            else if (a is MsgForward msg)
            {
                var text = await msg.GetMsg(RobotCore.Robot.Pipe);
                if (text == null)
                {
                    return null;
                }
                var list = text.Messages;
                var items = new StringBuilder();
                foreach (var item in list)
                {
                    items.AppendLine(item.ToString());
                }
                return "聊天记录：\n" + items.ToString()[..^1];
            }
        }
        catch (Exception e)
        {
            Logs.LogError(e);
        }
        return null;
    }
}
