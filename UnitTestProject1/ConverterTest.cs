using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using SimpleExcelFormulaConverter;

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
                .ShouldBe("{!Date%yyyyMMdd%+3j-36mois!}050000+0000");
        }

        [TestMethod]
        public void MoisDecaler2()
        {
            _converter.Convert("TEXT(EDATE(TODAY(),+12),\"aaaammjj\")&\"050000+0000\"")
                .ShouldBe("{!Date%yyyyMMdd%+12mois!}050000+0000");
        }

        [TestMethod]
        public void MoisDecaler3()
        {
            _converter.Convert("TEXT(EDATE(TODAY(),12),\"aaaammjj\")&\"050000+0000\"")
                .ShouldBe("{!Date%yyyyMMdd%+12mois!}050000+0000");
        }

        [TestMethod]
        public void OperationMathematiqueSurLesJours()
        {
            _converter.Convert("TEXT(TODAY()-30+1,\"aaaammjj\")&\"050000+0000\"")
                .ShouldBe("{!Date%yyyyMMdd%-29j!}050000+0000");
        }

        [TestMethod]
        public void OperationMathematiqueSurLesJours2()
        {
            _converter.Convert("TEXT(TODAY()+30+1,\"aaaammjj\")&\"050000+0000\"")
                .ShouldBe("{!Date%yyyyMMdd%+31j!}050000+0000");
        }

        [TestMethod]
        public void OperationMathematiqueSurLesJours3()
        {
            _converter.Convert("TEXT(TODAY()-30+1, \"aaaammjj\")")
                .ShouldBe("{!Date%yyyyMMdd%-29j!}");
        }

        [TestMethod]
        public void OperationMathematiqueSurLesJours4()
        {
            _converter.Convert("TODAY()-30+1")
                .ShouldBe("Date-29j");
        }

        [TestMethod]
        public void OperationMathematiqueSurLesJours5()
        {
            _converter.Convert("TEXT(TODAY()-30+1)")
                .ShouldBe("{!Date-29j!}");
        }

        [TestMethod]
        public void OperationMathematiqueSurLesJours6()
        {
            _converter.Convert("TEXT(TODAY()+30+1)")
                .ShouldBe("{!Date+31j!}");
        }

        [TestMethod]
        public void MoisDecalerEtOperationSurJour()
        {
            _converter.Convert("TEXT(EDATE(TODAY()-50+30,12+12),\"aaaammjj\")&\"050000+0000\"")
                .ShouldBe("{!Date%yyyyMMdd%-20j+24mois!}050000+0000");
        }

        [TestMethod]
        public void MoisDecalerEtOperationSurJour2()
        {
            _converter.Convert("TEXT(EDATE(TODAY(),12)-3,\"aaaammjj\")&\"050000+0000\"")
                .ShouldBe("{!Date%yyyyMMdd%+12mois-3j!}050000+0000");
        }
    }
}
