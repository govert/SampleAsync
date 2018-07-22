using System.Threading;

namespace SampleAsync.EndurLib.Analytics
{
    using Nito.AsyncEx;
    using System;
    using System.Reflection;
    using System.Threading.Tasks;

    public class RandClass
    {
        public double DoubleVal { get; set; }

        public RandClass(double val)
        {
            DoubleVal = val;
        }
    }

    public sealed class TradeFactory
    {
        private static TradeFactory _instance;

        private Random _randInstance;

        private TradeFactory() { }

        public static readonly AsyncLazy<TradeFactory> GetInstanceAsync =
            new AsyncLazy<TradeFactory>(async () =>
            {
                if (_instance == null)
                {
                   _instance = new TradeFactory();
                    await _instance.Initialize();

                }
                return _instance;
                });

        private Task Initialize()
        {
            return Task.Run(() =>
            {
                Thread.Sleep(5000);
                _randInstance = new Random();
            });
        }


        public async Task<RandClass> EuropeanMonthlyPowerOption(string optionType)
        {

            return await Task.Run(() =>
            {

                Thread.Sleep(5000);
                return new RandClass(_randInstance.Next());
            });

        }

        public async Task<object[,]> DisplayRandClassDetails(RandClass rc)
        {
            return await Task.Run(() =>
            {
                Thread.Sleep(5000);
                return new object[,] {{ "The double val is: " + rc.DoubleVal }};
            });
        }

    }
}
