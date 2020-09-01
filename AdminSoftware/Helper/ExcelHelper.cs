using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

namespace AdminSoftware.Helper
{
    public class ExcelHelper
    {
        private static T ReadRowWithType<T>(ISheet sheet, int index) where T : new()
        {
            IRow row = sheet.GetRow(index);
            return ReadRowWithType<T>(sheet, row);
        }

        private static T ReadRowWithType<T>(ISheet sheet, IRow row) where T : new()
        {
            T objTemp = new T();
            var a = objTemp.GetType().Name;
            var typeT = typeof(T);
            for (int indexCell = 0; indexCell < typeT.GetProperties().Count(); indexCell++)
            {
                if (indexCell >= row.Cells.Count)
                {
                    break;
                }
                var item = typeT.GetProperties().ElementAt(indexCell);

                var proType = item.PropertyType.Name;

                var format = new XSSFWorkbook().CreateDataFormat();
                try
                {
                    var cell = row.GetCell(indexCell);
                    cell.CellStyle.DataFormat = format.GetFormat("");
                    //var valueStr = cell.ToString();

                    if (proType.Equals("Int64", StringComparison.OrdinalIgnoreCase) && cell.CellType == CellType.Numeric)
                    {
                        var value = cell.NumericCellValue;
                        item.SetValue(objTemp, Int64.Parse(value.ToString(CultureInfo.InvariantCulture)));
                    }
                    else if (proType.Equals("Int32", StringComparison.OrdinalIgnoreCase) &&
                             cell.CellType == CellType.Numeric)
                    {
                        var value = cell.NumericCellValue;
                        item.SetValue(objTemp, Int32.Parse(value.ToString(CultureInfo.InvariantCulture)));
                    }
                    else if (proType.Equals("String", StringComparison.OrdinalIgnoreCase))
                    {
                        item.SetValue(objTemp, cell.ToString());
                    }
                    else if (proType.Equals("Decimal", StringComparison.OrdinalIgnoreCase))
                    {
                        var value = cell.NumericCellValue;
                        item.SetValue(objTemp, Decimal.Parse(value.ToString(CultureInfo.InvariantCulture)));
                    }
                    else if (proType.Equals("DateTime", StringComparison.OrdinalIgnoreCase))
                    {
                        var value = cell.ToString();
                        item.SetValue(objTemp, Convert.ToDateTime(value.ToString()));
                    }
                }
                catch (Exception ex)
                {
                    ex.Data.Add("RowNum", row.RowNum);
                    ex.Data.Add("indexCell", indexCell);
                    ex.Data.Add("name", item.Name);
                    throw ex;
                }
            }

            return objTemp;
        }

        public static List<T> ReadExcelWithStream<T>(Stream file, bool freezeHeader = true) where T : new()
        {
            List<T> temp = new List<T>();
            var workbook = new XSSFWorkbook(file);
            int indexSheet = 0;
            while (indexSheet < workbook.NumberOfSheets)
            {
                var sheet = workbook.GetSheetAt(indexSheet);
                indexSheet++;

                int indexRow = freezeHeader ? 1 : 0;
                IRow row;
                while ((row = sheet.GetRow(indexRow)) != null)
                {
                    indexRow++;
                    temp.Add(ReadRowWithType<T>(sheet, row));
                }
            }
            return temp;
        }

        public static List<List<string>> ReadExcel(Stream file, bool freezeHeader = true)
        {
            List<List<string>> data = new List<List<string>>();
            var workbook = new XSSFWorkbook(file);
            int indexSheet = 0;
            while (indexSheet < workbook.NumberOfSheets)
            {
                var sheet = workbook.GetSheetAt(indexSheet);
                indexSheet++;

                //
                List<Dictionary<string, string>> dataTemp = new List<Dictionary<string, string>>();
                List<string> headerTemp = new List<string>();
                var rowHeaderTemp = sheet.GetRow(0);
                if (rowHeaderTemp != null)
                {
                    for (var indexCell = 0; indexCell < rowHeaderTemp.Cells.Count; indexCell++)
                    {
                        var cell = rowHeaderTemp.GetCell(indexCell, MissingCellPolicy.RETURN_NULL_AND_BLANK);
                        if (cell.CellType == CellType.String)
                        {
                            headerTemp.Add(cell.ToString());
                        }
                        else
                        {
                            headerTemp.Add("Col_" + indexCell);
                        }
                    }
                }
                //

                int indexRow = freezeHeader ? 1 : 0;
                IRow row;
                while ((row = sheet.GetRow(indexRow)) != null)
                {
                    List<string> dataRow = new List<string>();
                    data.Add(dataRow);
                    Dictionary<string, string> dataRowTemp = new Dictionary<string, string>();
                    dataTemp.Add(dataRowTemp);

                    indexRow++;
                    for (var indexCell = 0; indexCell < row.Cells.Count; indexCell++)
                    {
                        try
                        {
                            var cell = row.GetCell(indexCell, MissingCellPolicy.RETURN_NULL_AND_BLANK);
                            if (cell.CellType == CellType.Numeric)
                            {
                                dataRow.Add(cell.NumericCellValue.ToString(CultureInfo.InvariantCulture));
                                dataRowTemp.Add(freezeHeader ? headerTemp[indexCell] : "Col_" + indexCell,
                                    cell.NumericCellValue.ToString(CultureInfo.InvariantCulture));
                            }
                            else if (cell.CellType == CellType.String)
                            {
                                dataRow.Add(cell.ToString());
                                dataRowTemp.Add(freezeHeader ? headerTemp[indexCell] : "Col_" + indexCell,
                                    cell.ToString());
                            }
                            else if (cell.CellType == CellType.Boolean)
                            {
                                dataRow.Add(cell.BooleanCellValue.ToString());
                                dataRowTemp.Add(freezeHeader ? headerTemp[indexCell] : "Col_" + indexCell,
                                    cell.BooleanCellValue.ToString());
                            }
                            else if (cell.CellType == CellType.Blank)
                            {
                                dataRow.Add("");
                                dataRowTemp.Add(freezeHeader ? headerTemp[indexCell] : "Col_" + indexCell, "");
                            }
                        }
                        catch (Exception ex)
                        {
                            ex.Data.Add("indexRow", indexRow);
                            ex.Data.Add("indexCell", indexCell);
                            throw;
                        }
                    }
                }
            }
            return data;
        }

        public static List<Dictionary<string, string>> ReadExcelDictionary(Stream file, bool freezeHeader = true,
            List<string> columHeader = null, int rownumStart = 0, int maxColumn = 0, bool formatAllString = false)
        {
            List<Dictionary<string, string>> dataTemp = new List<Dictionary<string, string>>();
            var workbook = new XSSFWorkbook(file);
            int indexSheet = 0;
            while (indexSheet < workbook.NumberOfSheets)
            {
                var sheet = workbook.GetSheetAt(indexSheet);
                indexSheet++;

                //
                List<string> headerTemp = new List<string>();
                if (freezeHeader)
                {
                    var rowHeaderTemp = sheet.GetRow(0);

                    if (rowHeaderTemp != null)
                    {
                        // Add by CanTV: Check max column index
                        if (maxColumn == 0)
                            maxColumn = rowHeaderTemp.Cells.Count;
                        else
                        {
                            if (rowHeaderTemp.Cells.Count < maxColumn)
                                maxColumn = rowHeaderTemp.Cells.Count;
                        }

                        for (var indexCell = 0; indexCell < maxColumn; indexCell++)
                        {
                            var cell = rowHeaderTemp.GetCell(indexCell, MissingCellPolicy.RETURN_NULL_AND_BLANK);
                            if (cell.CellType == CellType.String)
                            {
                                headerTemp.Add(cell.ToString());
                            }
                            else
                            {
                                headerTemp.Add("Col_" + indexCell);
                            }
                        }
                    }
                }
                else
                {
                    // Get column default 
                    if (columHeader != null && columHeader.Any())
                        headerTemp = columHeader;
                    else
                    {
                        // Get column
                        for (int indexCell = 0; indexCell < maxColumn; indexCell++)
                        {
                            headerTemp.Add("Col_" + indexCell);
                        }
                    }
                }
                //

                int indexRow = freezeHeader ? 1 : (rownumStart);
                bool getNameHeaderFromList = headerTemp.Count > 0;

                IRow row;
                while ((row = sheet.GetRow(indexRow)) != null)
                {
                    Dictionary<string, string> dataRowTemp = new Dictionary<string, string>();
                    dataTemp.Add(dataRowTemp);

                    indexRow++;
                    for (var indexCell = 0;
                        indexCell < (row.Cells.Count < headerTemp.Count ? headerTemp.Count : row.Cells.Count);
                        indexCell++)
                    {
                        try
                        {
                            var cell = row.GetCell(indexCell, MissingCellPolicy.RETURN_NULL_AND_BLANK);

                            if (cell == null)
                            {
                                if ((indexCell + 1) > headerTemp.Count)
                                    dataRowTemp.Add("Col_" + indexCell, string.Empty);
                                else
                                    dataRowTemp.Add(getNameHeaderFromList ? headerTemp[indexCell] : "Col_" + indexCell,
                                        string.Empty);
                                continue;
                            }

                            if (formatAllString)
                            {
                                dataRowTemp.Add(getNameHeaderFromList ? headerTemp[indexCell] : "Col_" + indexCell,
                                    cell.ToString());
                            }
                            else
                            {
                                if (cell.CellType == CellType.Numeric)
                                {
                                    dataRowTemp.Add(getNameHeaderFromList ? headerTemp[indexCell] : "Col_" + indexCell,
                                        cell.NumericCellValue.ToString(CultureInfo.InvariantCulture));
                                }
                                else if (cell.CellType == CellType.String)
                                {
                                    dataRowTemp.Add(getNameHeaderFromList ? headerTemp[indexCell] : "Col_" + indexCell,
                                        cell.ToString());
                                }
                                else if (cell.CellType == CellType.Boolean)
                                {
                                    dataRowTemp.Add(getNameHeaderFromList ? headerTemp[indexCell] : "Col_" + indexCell,
                                        cell.BooleanCellValue.ToString());
                                }
                                else if (cell.CellType == CellType.Blank)
                                {
                                    dataRowTemp.Add(getNameHeaderFromList ? headerTemp[indexCell] : "Col_" + indexCell,
                                        "");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ex.Data.Add("indexRow", indexRow);
                            ex.Data.Add("indexCell", indexCell);
                            throw;
                        }
                    }
                }
            }
            return dataTemp;
        }
    }
}