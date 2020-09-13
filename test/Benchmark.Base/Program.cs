using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Running;

namespace Benchmark.Base
{
    public partial class Program
    {
        static void Main(string[] args)
        {
            var config = DefaultConfig.Instance
                .WithOptions(ConfigOptions.JoinSummary)
                .AddColumn(CategoriesColumn.Default)
                .AddDiagnoser(MemoryDiagnoser.Default)
                .WithOrderer(new DefaultOrderer(SummaryOrderPolicy.FastestToSlowest));
            _ = BenchmarkRunner.Run(typeof(Program).Assembly, config);
        }
    }
}
