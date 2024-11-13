using ClosedXML.Excel;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace CommonLibrary
{
    public class ExcelReader
    {
        public static IConfiguration Configuration { get; private set; }

        public static List<ExcelSheet> ToData(string filePath, ExcelConfiguration excelConfiguration)
        {

            bool hasHeader = true;
            int headerRow = 0;
            bool columnIndexGiven = false;
            List<FieldCellIndex> fieldCellIndexs = new List<FieldCellIndex>();
            ValidateAndSetConfiguration(excelConfiguration, ref hasHeader, ref headerRow, ref columnIndexGiven, ref fieldCellIndexs);

            List<ExcelSheet> excelSheets = new List<ExcelSheet>();

            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(filePath, false))
            {
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                WorksheetPart worksheetPart = null;
                //List to hold custom column names for mapping data to columns (index-free).
                foreach (Sheet sheet in workbookPart.Workbook.Descendants<Sheet>())
                {
                    ExcelSheet ExcelSheet = new ExcelSheet
                    {
                        SheetIndex = excelSheets.Count + 1,
                        SheetName = sheet.Name
                    };
                    worksheetPart = (WorksheetPart)(workbookPart.GetPartById(sheet.Id));

                    fieldCellIndexs = FillFieldColumn(headerRow, columnIndexGiven, fieldCellIndexs, spreadsheetDocument, worksheetPart);

                    bool hasValue;
                    IDictionary<string, object> expand = null;
                    Cell cell = null;
                    int actualCellCount;
                    foreach (var row in worksheetPart.Worksheet.Descendants<Row>().Skip(headerRow + 1))
                    {
                        hasValue = false;
                        expand = new ExpandoObject();
                        actualCellCount = row.ChildElements.Count;
                        foreach (var FieldCell in fieldCellIndexs)
                        {
                            cell = row.Elements<Cell>().Where(p => p.CellReference == (GetColumnPrefix(FieldCell.CellIndex) + row.RowIndex)).SingleOrDefault();

                            string cellValue = GetCellValue(spreadsheetDocument, cell);
                            expand.Add(FieldCell.FieldName, cellValue);

                            if (MyConvert.ToString(cellValue) != string.Empty)
                                hasValue = true;
                        }
                        if (hasValue)
                            ExcelSheet.Rows.Add(expand);
                    }

                    excelSheets.Add(ExcelSheet);
                }
            }

            return excelSheets;
        }

        private static List<FieldCellIndex> FillFieldColumn(int headerRow, bool columnIndexGiven, List<FieldCellIndex> fieldCellIndices, SpreadsheetDocument spreadsheetDocument, WorksheetPart worksheetPart)
        {
            if (!columnIndexGiven)
            {
                int cellIndex = 0;
                fieldCellIndices = new List<FieldCellIndex>();
                foreach (Cell headerCell in worksheetPart.Worksheet.Descendants<Row>().ElementAt(headerRow))
                {
                    string fieldName = Regex.Replace(GetCellValue(spreadsheetDocument, headerCell).Replace("(", "").Replace(")", ""), @"\s+", "");
                    fieldCellIndices.Add(new FieldCellIndex { CellIndex = cellIndex, FieldName = fieldName });
                    cellIndex++;
                }
            }

            return fieldCellIndices;
        }

        private static void ValidateAndSetConfiguration(ExcelConfiguration excelConfiguration, ref bool hasHeader, ref int headerRow, ref bool columnIndexGiven, ref List<FieldCellIndex> fieldCellIndices)
        {
            if (excelConfiguration != null)
            {
                hasHeader = excelConfiguration.HasHeader;
                headerRow = excelConfiguration.HeaderRow - 1;
                if (hasHeader && headerRow < 0)
                    throw new Exception("Header row should be greater than equal to 1 if has header is true.");

                fieldCellIndices = excelConfiguration.fieldCellIndices;
            }

            if (fieldCellIndices != null && fieldCellIndices.Count > 0)
                columnIndexGiven = true;
        }

        public static string ToXML(string filePath, ExcelConfiguration excelConfiguration)
        {
            bool hasHeader = true;
            int headerRow = 0;
            bool columnIndexGiven = false;
            List<FieldCellIndex> fieldCellIndices = new List<FieldCellIndex>();

            ValidateAndSetConfiguration(excelConfiguration, ref hasHeader, ref headerRow, ref columnIndexGiven, ref fieldCellIndices);

            XmlDocument xmlDocument = new XmlDocument();
            XmlNode docNode = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmlDocument.AppendChild(docNode);

            XmlNode xmlNodeSheets = xmlDocument.CreateElement("sheets");
            xmlDocument.AppendChild(xmlNodeSheets);

            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(filePath, false))
            {
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                WorksheetPart worksheetPart = null;
                int SheetIndex = 1;
                foreach (Sheet sheet in workbookPart.Workbook.Descendants<Sheet>())
                {
                    XmlNode xmlNode = xmlDocument.CreateElement("sheet");

                    XmlAttribute xmlAttribute = xmlDocument.CreateAttribute("id");
                    xmlAttribute.Value = SheetIndex.ToString();
                    xmlNode.Attributes.Append(xmlAttribute);

                    XmlAttribute xmlaSheetName = xmlDocument.CreateAttribute("name");
                    xmlaSheetName.Value = sheet.Name;
                    xmlNode.Attributes.Append(xmlaSheetName);

                    xmlNodeSheets.AppendChild(xmlNode);

                    worksheetPart = (WorksheetPart)(workbookPart.GetPartById(sheet.Id));

                    fieldCellIndices = FillFieldColumn(headerRow, columnIndexGiven, fieldCellIndices, spreadsheetDocument, worksheetPart);

                    bool hasValue;
                    IDictionary<string, object> expand = null;
                    Cell cell = null;
                    int actualCellCount;
                    foreach (var row in worksheetPart.Worksheet.Descendants<Row>().Skip(headerRow + 1))
                    {
                        hasValue = false;
                        expand = new ExpandoObject();
                        actualCellCount = row.ChildElements.Count;
                        foreach (var fieldCell in fieldCellIndices)
                        {
                            cell = row.Elements<Cell>().Where(p => p.CellReference == (GetColumnPrefix(fieldCell.CellIndex) + row.RowIndex)).SingleOrDefault();

                            string cellValue = GetCellValue(spreadsheetDocument, cell);
                            expand.Add(fieldCell.FieldName, cellValue);

                            if (MyConvert.ToString(cellValue) != string.Empty)
                                hasValue = true;
                        }
                        if (hasValue)
                        {
                            XmlNode xmlNodeRow = xmlDocument.CreateElement("row");
                            foreach (var item in expand.ToList())
                            {
                                XmlAttribute xmlAttributeCell = xmlDocument.CreateAttribute(item.Key);
                                xmlAttributeCell.Value = MyConvert.ToString(item.Value);
                                xmlNodeRow.Attributes.Append(xmlAttributeCell);
                            }
                            xmlNode.AppendChild(xmlNodeRow);
                        }
                    }
                    SheetIndex++;
                }
            }
            return xmlDocument.OuterXml;
        }

        private static string GetColumnPrefix(int cellIndex)
        {
            return ((CellIndexValue)cellIndex).ToString();
        }

        public enum CellIndexValue
        {
            A = 0,
            B,
            C,
            D,
            E,
            F,
            G,
            H,
            I,
            J,
            K,
            L,
            M,
            N,
            O,
            P,
            Q,
            R,
            S,
            T,
            U,
            V,
            W,
            X,
            Y,
            Z,
            AA,
            AB,
            AC,
            AD,
            AE,
            AF,
            AG,
            AH,
            AI,
            AJ,
            AK,
            AL,
            AM,
            AN,
            AO,
            AP,
            AQ,
            AR,
            AS,
            AT,
            AU,
            AV,
            AW,
            AX,
            AY,
            AZ,
            BA,
            BB,
            BC,
            BD,
            BE,
            BF,
            BG,
            BH,
            BI,
            BJ,
            BK,
            BL,
            BM,
            BN,
            BO,
            BP,
            BQ,
            BR,
            BS,
            BT,
            BU,
            BV,
            BW,
            BX,
            BY,
            BZ
        }

        private static string GetColumnAddress(string cellReference)
        {
            //Create a regular expression to get column address letters.
            Regex regex = new Regex("[A-Za-z]+");
            Match match = regex.Match(cellReference);
            return match.Value;
        }

        private static string GetCellValue(SpreadsheetDocument document, Cell cell)
        {
            if (cell == null) return null;
            string value = cell.InnerText;
            CellFormat cellFormat = document.WorkbookPart.WorkbookStylesPart.Stylesheet.CellFormats.ToList()[(int)cell.StyleIndex.Value] as CellFormat;
            int formatId = (int)cellFormat.NumberFormatId.Value;
            //add prefix with 0 value if postal code less then 5
            if (formatId == 164)
            {
                if (value.Length == 4)
                    value = "0" + value;
                if (value.Length == 3)
                    value = "00" + value;
            }

            //Process values particularly for those data types.
            // cell.StyleIndex
            if (cell.DataType != null)
            {
                switch (cell.DataType.Value)
                {
                    //Obtain values from shared string table.
                    case CellValues.SharedString:
                        var sstPart = document.WorkbookPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
                        value = sstPart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText;
                        break;

                    //Optional boolean conversion.
                    case CellValues.Boolean:
                        if (MyConvert.ToString(Configuration["AppSettings:BooleanToBit"]) != "Y")
                            value = value == "0" ? "FALSE" : "TRUE";
                        break;
                }
            }
            //Log.Write("\n" + value + " : " + formatId);
            if ((formatId >= 14 && formatId <= 22) || (formatId >= 45 && formatId <= 47))
            {
                try
                {
                    value = DateTime.FromOADate(double.Parse(value)).ToString("M/d/yyyy");
                    //Log.Write(" : " + value);
                }
                catch
                {
                }
            }
            return value;
        }

        public static string ToFormat(string filePath)
        {
            var workbook = new XLWorkbook(filePath);
            for (int sheetLoop = 1; sheetLoop <= workbook.Worksheets.Count; sheetLoop++)
            {
                var sheet = workbook.Worksheet(sheetLoop);

                Parallel.ForEach(sheet.Rows(), (row) =>
                {
                    int blankCount = 1;
                    for (int cellLoop = 1; cellLoop <= row.Cells().Count(); cellLoop++)
                    {
                        var cell = row.Cell(cellLoop);
                        string stringValue = string.Empty;
                        bool hasValue = cell.TryGetValue<string>(out stringValue);

                        if (stringValue.Length == 0)
                        {
                            blankCount++;
                            if (blankCount == 10)
                                break;
                        }

                        if (hasValue)
                        {
                            DateTime dateTimeValue;
                            bool hasDateTime = cell.TryGetValue<DateTime>(out dateTimeValue);
                            double doubleValue;
                            bool hasDouble = cell.TryGetValue<double>(out doubleValue);
                            int intValue;
                            bool hasInt = cell.TryGetValue<int>(out intValue);

                            if ((hasDateTime && stringValue.Contains("/") && stringValue.Contains(":")) || (hasDateTime && cell.Style.DateFormat.Format != "General"))
                            {
                                cell.Style.DateFormat.SetFormat("MM/dd/yyyy hh:mm AM/PM");
                                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                            }
                            else if (hasDateTime && stringValue.Contains("/") && !stringValue.Contains(":") || (hasDateTime && cell.Style.DateFormat.Format != "General"))
                            {
                                cell.Style.DateFormat.SetFormat("MM/dd/yyyy");
                                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                            }
                            else if (hasDouble && stringValue.Contains("."))
                            {
                                cell.Style.NumberFormat.SetNumberFormatId((int)XLPredefinedFormat.Number.Precision2);
                                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                            }
                            else if (hasInt)
                            {
                                cell.Style.NumberFormat.SetNumberFormatId((int)XLPredefinedFormat.Number.Integer);
                                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                            }
                        }
                    }
                });
            }
            workbook.SaveAs(filePath.Replace(".xlsx", "_f.xlsx"));
            return filePath.Replace(".xlsx", "_f.xlsx");
        }
    }

    public class ExcelSheet
    {
        public int SheetIndex { get; set; } = 0;
        public string SheetName { get; set; } = string.Empty;
        public List<dynamic> Rows = new List<dynamic>();
    }

    public class ExcelConfiguration
    {
        public bool HasHeader { get; set; } = true;
        public int HeaderRow { get; set; } = 0;
        public List<FieldCellIndex> fieldCellIndices = new List<FieldCellIndex>();
    }

    public class FieldCellIndex
    {
        public int CellIndex { get; set; } = 0;
        public string FieldName { get; set; } = string.Empty;
    }
}
