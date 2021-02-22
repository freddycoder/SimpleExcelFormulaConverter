# SimpleExcelFormulaConverter

## Goal

The goal of this project is to convert a subset of excel formula into a specific format.

## Example

Suppose you have a excel formula like this :
```
TEXT(TODAY(),"aaaammjj")&"120000+0000")
```

Using the class FormulaConverterTIPS.cs it will output this :
```
{!Date%yyyyMMdd%!}120000+0000
```

Many use case are show in this file : https://github.com/freddycoder/SimpleExcelFormulaConverter/blob/master/UnitTestProject1/ConverterTest.cs
