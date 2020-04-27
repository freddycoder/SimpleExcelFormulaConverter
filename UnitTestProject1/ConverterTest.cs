﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using SimpleExcelFormulaConverter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1
{
    [TestClass]
    public class ConverterTest
    {
        private readonly IFormulaConverter _converter;

        public ConverterTest()
        {
            _converter = new FormulaConverterTIPS();
        }

        [TestMethod]
        public void ConvertDate()
        {
            var excelFormula = "TEXT(TODAY(),\"aaaammjj\")&\"120000+0000";

            var result = _converter.Convert(excelFormula);

            result.ShouldBe("{!Date%yyyyMMdd%!}120000+0000");
        }

        [TestMethod]
        public void ConvertDateWithMinusDaysSimpler()
        {
            var excelFormula = "TEXT(TODAY()-3,\"aaaammjj\")";

            var result = _converter.Convert(excelFormula);

            result.ShouldBe("{!Date%yyyyMMdd%-3j!}");
        }

        [TestMethod]
        public void ConvertDateWithMinusDays()
        {
            var excelFormula = "TEXT(TODAY()-3,\"aaaammjj\")&\"120000+0000";

            var result = _converter.Convert(excelFormula);

            result.ShouldBe("{!Date%yyyyMMdd%-3j!}120000+0000");
        }
    }
}