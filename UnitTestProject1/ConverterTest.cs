using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        public void EmptyString()
        {
            _converter.Convert("")
                .ShouldBe("");
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

        [TestMethod]
        public void Concatenate()
        {
            _converter.Convert("CONCATENATE(\"/\", TEXT(TODAY() - 5, \"AAAAMMJJ\"))")
                .ShouldBe("/{!Date%yyyyMMdd%-5j!}");
        }

        [TestMethod]
        public void MoreComplexeCase()
        {
            _converter.Convert("TEXT(TODAY()-30,\"aaaammjj\")&\"/\"&TEXT(TODAY(),\"aaaammjj\")")
                .ShouldBe("{!Date%yyyyMMdd%-30j!}/{!Date%yyyyMMdd%!}");
        }

        [TestMethod]
        public void MoreComplexeCase2()
        {
            _converter.Convert("TEXT(TODAY()-30,\"aaaammjj\")&\"/\"&TEXT(TODAY()+30,\"aaaammjj\")")
                .ShouldBe("{!Date%yyyyMMdd%-30j!}/{!Date%yyyyMMdd%+30j!}");
        }

        [TestMethod]
        public void ManyConcat()
        {
            _converter.Convert("\"/\"&\"/\"&\"/\"&\"/\"&\"/\"")
                .ShouldBe("/////");
        }

        [TestMethod]
        public void TextToday()
        {
            _converter.Convert("TEXT(TODAY())")
                .ShouldBe("{!Date!}");
        }

        [TestMethod]
        public void MoisDecaler()
        {
            _converter.Convert("TEXT(EDATE(TODAY()+3,-36),\"aaaammjj\")&\"050000+0000\"")
                .ShouldBe("{!Date%yyyyMMdd%+3j-36M!}050000+0000");
        }
    }
}
