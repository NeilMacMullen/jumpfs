﻿using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using jumpfs.Commands;

namespace jumpfs
{
    public static class UpgradeManager
    {
        public const string ReleaseSite = "https://github.com/NeilMacMullen/jumpfs";

        private
            static readonly Uri ChangeList =
                new("https://raw.githubusercontent.com/NeilMacMullen/jumpfs/master/doc/changelist.json");

        public static async Task<VersionInfo> GetLatestVersion()
        {
            try
            {
                using var client = new HttpClient();
                var raw = await client.GetStringAsync(ChangeList);
                var infos = JsonSerializer.Deserialize<VersionInfo[]>(raw);
                // ReSharper disable once AssignNullToNotNullAttribute
                return infos.OrderByDescending(i => i.Date).First();
            }
            catch
            {
                return VersionInfo.Default;
            }
        }

        public static void CheckAndWarnOfNewVersion(TextWriter writer, bool suppressUpToDate)
        {
            var t = GetLatestVersion();
            t.Wait();
            var latestVersion = t.Result;
            if (latestVersion.Supersedes(GitVersionInformation.SemVer))
                writer.WriteLine(@$"
  An Upgrade to jumpfs version {latestVersion.Version} is available.
  Please visit {ReleaseSite} for download.
");
            else if (!suppressUpToDate)

                writer.WriteLine(@"
  You are running the latest version."
                );
        }

        public static (int major, int minor, int patch) DecomposeVersion(string v)
        {
            try
            {
                var elements = v.Split(".")
                    .Select(int.Parse)
                    .ToArray();
                return (elements[0], elements[1], elements[2]);
            }
            catch
            {
                return (0, 0, 0);
            }
        }

        public static int CompareVersions(string a, string b)
        {
            var av = DecomposeVersion(a);
            var bv = DecomposeVersion(b);
            var d = av.major - bv.major;
            if (d != 0) return d;

            d = av.minor - bv.minor;
            if (d != 0) return d;
            d = av.patch - bv.patch;
            if (d != 0) return d;

            return 0;
        }

        public record VersionInfo
        {
            /// <summary>
            ///     The default value with sensible fields
            /// </summary>
            public static readonly VersionInfo Default = new();

            public string Version { get; init; } = "0.0.0";


            public DateTime Date { get; init; }
            public string Summary { get; init; } = string.Empty;

            public bool Supersedes(string currentVersion)
                => CompareVersions(Version, currentVersion) > 0;
        }
    }
}
