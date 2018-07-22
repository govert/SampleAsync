using System.Threading.Tasks;

namespace SampleAsync.Pricers
{
    using EndurLib.Analytics;
    using ExcelDna.Integration;
    using System;
    using Utils;

    public class EndurOptions
    {
        //private static ObjectHandler _ObjectHandler = new ObjectHandler();

        [ExcelFunction("Returns a handle to an  Endur option in memory (not booked)")]
        public static object SampleOption(
    [ExcelArgument("is whether the instrument is a call (c) or a put (p).",
                Name = "call_put_flag")] string callPutFlag)
        {

            if (ExcelDnaUtil.IsInFunctionWizard())
            {
                return ExcelError.ExcelErrorRef;
            }

            callPutFlag = callPutFlag == "p" ? "put" : "call";


            string optName = callPutFlag;


            var ret = GlobalCache.CreateHandle(nameof(SampleOption), new object[] { optName },
                (objectType, parameters) =>
                {

                    try
                    {

                        var instance = TradeFactory.GetInstanceAsync.Task.Result;
                        var option = instance.EuropeanMonthlyPowerOption(optName).Result;
                        return option;
                    }
                    catch (Exception e)
                    {
                        return new object[,] { { "!Exception! : " + e.Message } };
                    }

                });

            if (Equals(ret, ExcelError.ExcelErrorNA))
            {
                return new object[,] { { "! WAIT !" } };
            }

            return ret;

        }


        [ExcelFunction("Returns a handle to an  Endur option in memory (not booked)")]
        public static object SampleOptionAsync(
    [ExcelArgument("is whether the instrument is a call (c) or a put (p).",
                Name = "call_put_flag")] string callPutFlag)
        {

            if (ExcelDnaUtil.IsInFunctionWizard())
            {
                return ExcelError.ExcelErrorRef;
            }

            callPutFlag = callPutFlag == "p" ? "put" : "call";


            string optName = callPutFlag;


            var ret = GlobalCache.CreateHandleAsync(nameof(SampleOption), new object[] { optName },
                async (objectType, parameters) =>
                {

                    try
                    {

                        var instance = await TradeFactory.GetInstanceAsync;
                        var option = await instance.EuropeanMonthlyPowerOption(optName);
                        return option;
                    }
                    catch (Exception e)
                    {
                        return new object[,] { { "!Exception! : " + e.Message } };
                    }

                });

            if (Equals(ret, ExcelError.ExcelErrorNA))
            {
                return new object[,] { { "! WAIT !" } };
            }

            return ret;

        }

        [ExcelFunction("Get basic details for option")]
        public static object[,] DisplaySampleOptionDetails([ExcelArgument("the option handle", Name = "option handle")] string optHandle)
        {

            if (ExcelDnaUtil.IsInFunctionWizard())
            {
                return new object[,] { { ExcelError.ExcelErrorRef } };
            }

            RandClass value;
            if (GlobalCache.TryGetObject(optHandle, out value))
            {

                try
                {
                    RandClass obj = (RandClass)value;
                    return TradeFactory.GetInstanceAsync.Task.Result.DisplayRandClassDetails(obj).Result;
                }
                catch (Exception e)
                {
                    return new object[,] { { e.Message } };
                }

            }

            object[,] ret = { { "!Invalid Handle!" } };
            return ret;
        }

    }
}
