﻿using blockchain_dotnet_core.API.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace blockchain_dotnet_core.Tests.Utils
{
    [TestClass]
    public class TimestampUtilsTests
    {
        [TestMethod]
        public void GenerateTimestamp()
        {
            var result = TimestampUtils.GenerateTimestamp();

            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        public void GenerateTimestampIsAccurate()
        {
            var result = TimestampUtils.GenerateTimestamp();

            var expectedResult = DateTime.Now;

            var startTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var currentTime = startTime.AddMilliseconds(result).ToLocalTime();

            double totalDifference = Math.Abs((currentTime - expectedResult).TotalMilliseconds);

            var variation = TimeSpan.FromMilliseconds(1);

            Assert.IsNotNull(currentTime);
            Assert.IsTrue(totalDifference <= variation.Milliseconds);
        }
    }
}