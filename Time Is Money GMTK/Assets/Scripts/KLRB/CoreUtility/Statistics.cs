using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KLRB.Utility
{
    public static class Statistics
    {
        private class StatNode
        {
            public Dictionary<string, StatNode> Children = new();
            public Dictionary<string, Func<string>> Stats = new();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        static void Initialize()
        {
            root = new();
            collapsed = new();
        }

        private static StatNode root = new();
        private static Dictionary<string, bool> collapsed = new();

        public const string ShowAllLink  = "cmd_ShowAll";
        public const string HideAllLink  = "cmd_HideAll";

        private const string I1   = "<margin-left=1em>";
        private const string I2   = "<margin-left=2em>";
        private const string IEnd = "</margin>";

        public static void LogStat(string path, string name, Func<string> message, bool isCollapsed = true)
        {
            var parts = path.Split('/');
            var node  = root;

            string builtPath = "";
            foreach (var part in parts)
            {
                builtPath = builtPath == "" ? part : $"{builtPath}/{part}";
                if (!node.Children.ContainsKey(part))
                {
                    node.Children.Add(part, new StatNode());
                    if (!collapsed.ContainsKey(builtPath))
                        collapsed.Add(builtPath, isCollapsed);
                }
                node = node.Children[part];
            }

            node.Stats[name] = message;
        }

        public static void RemovePath(string path)
        {
            var parts  = path.Split('/');
            var parent = GetNode(string.Join("/", parts.Take(parts.Length - 1)));
            if (parent == null) return;

            string last = parts.Last();
            parent.Children.Remove(last);
        }

        public static void EvaluateLinkClick(string linkID)
        {
            switch (linkID)
            {
                case ShowAllLink: ShowAll(); return;
                case HideAllLink: HideAll(); return;
            }
            if (collapsed.ContainsKey(linkID))
                collapsed[linkID] = !collapsed[linkID];
        }

        public static void ShowAll() { foreach (var k in collapsed.Keys.ToList()) collapsed[k] = false; }
        public static void HideAll() { foreach (var k in collapsed.Keys.ToList()) collapsed[k] = true;  }
        public static bool IsCollapsed(string path) => collapsed.TryGetValue(path, out bool c) && c;

        public static string GetStatistics()
        {
            string str = "";
            str += $"<B><link={ShowAllLink}><u>Show All</u></link>   <link={HideAllLink}><u>Collapse All</u></link></B>\n\n";
            foreach (var child in root.Children)
                str += BuildNode(child.Key, child.Value, child.Key, 0);
            return str;
        }

        static string BuildNode(string label, StatNode node, string path, int depth)
        {
            string str   = "";
            string ind   = depth == 0 ? "" : I1;
            bool isCollapsed = IsCollapsed(path);
            string arrow = isCollapsed ? "►" : "▼";

            if (depth == 0)
            {
                str += $"<link={path}><B>{label}</B> {arrow}</link>\n";
                str += "-----------------\n";
            }
            else
            {
                str += $"{ind}<link={path}><b>{label}</b> {arrow}</link>{IEnd}\n \n";
            }

            if (!isCollapsed)
            {
               // string statInd = depth == 0 ? I1 : I2;
                string statInd = I1;

                foreach (var stat in node.Stats)
                    str += $"{statInd}{stat.Key}: <color=yellow><mspace=0.5em>{stat.Value.Invoke()}</mspace></color>{IEnd}\n";

                foreach (var child in node.Children)
                    str += BuildNode(child.Key, child.Value, $"{path}/{child.Key}", depth + 1);

                str += "\n";
            }

            return str;
        }

        static StatNode GetNode(string path)
        {
            if (path == "") return root;
            var node = root;
            foreach (var part in path.Split('/'))
            {
                if (!node.Children.TryGetValue(part, out node)) return null;
            }
            return node;
        }
    }
}