using System.Collections.Generic;
using System.Diagnostics;

namespace EFBulkOp
{
    public class ExecutionTimer
    {
        private static readonly Dictionary<string, ExecutionTimer> Benchmarks = new Dictionary<string, ExecutionTimer>();
        public static ExecutionTimer For(string title)
        {
            if (!Benchmarks.TryGetValue(title, out var bm))
            {
                Benchmarks[title] = bm = new ExecutionTimer(title);
            }
            return bm;
        }

        private readonly string _title;
        private readonly Stopwatch _sw = new Stopwatch();
        private string _result;
        private long _lastCheckpoint;

        public ExecutionTimer(string title)
        {
            _title = title;
            _result = string.IsNullOrEmpty(title) ? "" : $"[{title}]";
        }
        public void Start()
        {
            _sw.Start();
        }

        public void CheckPoint(string checkDesc, string contextInfo = "")
        {
            var ms = _sw.ElapsedMilliseconds - _lastCheckpoint;
            _lastCheckpoint = _sw.ElapsedMilliseconds;
            var chkpmsg = $"{checkDesc}: {ms}ms  {(string.IsNullOrEmpty(contextInfo) ? "" : $"({contextInfo})")}; ";
            _result += chkpmsg;
            Debug.WriteLine(chkpmsg);
        }

        public string Stop(string finalCheck = "")
        {
            _sw.Stop();
            if (!string.IsNullOrEmpty(finalCheck)) CheckPoint(finalCheck);
            _result += $"TOTAL: {_sw.Elapsed:c}";
            Benchmarks.Remove(_title);
            return _result;
        }

    }
}
