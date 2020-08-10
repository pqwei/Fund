using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Common
{
    //NPOI方式
    //NPOI 是 POI 项目的 .NET 版本。POI是一个开源的Java读写Excel、WORD等微软OLE2组件文档的项目。使用 NPOI 你就可以在没有安装 Office 或者相应环境的机器上对 WORD/EXCEL 文档进行读写。
    //优点：读取Excel速度较快，读取方式操作灵活性
    //缺点：需要下载相应的插件并添加到系统引用当中。
    public static class NPOIHelper
    {
        /// <summary>
        /// Excel转换成DataTable（.xls）
        /// </summary>
        /// <param name="filePath">Excel文件路径</param>
        /// <returns></returns>
        public static DataTable ExcelToDataTable(string filePath, int sheetAt)
        {
            var dt = new DataTable();
            using (var file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                var hssfworkbook = new HSSFWorkbook(file);
                var sheet = hssfworkbook.GetSheetAt(sheetAt);
                for (var j = 0; j < 5; j++)
                {
                    dt.Columns.Add(Convert.ToChar(((int)'A') + j).ToString());
                }
                var rows = sheet.GetRowEnumerator();
                while (rows.MoveNext())
                {
                    var row = (HSSFRow)rows.Current;
                    var dr = dt.NewRow();
                    for (var i = 0; i < row.LastCellNum; i++)
                    {
                        var cell = row.GetCell(i);
                        if (cell == null)
                        {
                            dr[i] = null;
                        }
                        else
                        {
                            switch (cell.CellType)
                            {
                                case CellType.Blank:
                                    dr[i] = "[null]";
                                    break;
                                case CellType.Boolean:
                                    dr[i] = cell.BooleanCellValue;
                                    break;
                                case CellType.Numeric:
                                    dr[i] = cell.ToString();
                                    break;
                                case CellType.String:
                                    dr[i] = cell.StringCellValue;
                                    break;
                                case CellType.Error:
                                    dr[i] = cell.ErrorCellValue;
                                    break;
                                case CellType.Formula:
                                    try
                                    {
                                        dr[i] = cell.NumericCellValue;
                                    }
                                    catch
                                    {
                                        dr[i] = cell.StringCellValue;
                                    }
                                    break;
                                default:
                                    dr[i] = "=" + cell.CellFormula;
                                    break;
                            }
                        }
                    }
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }

        /// <summary>
        /// Excel转换成DataSet（.xlsx/.xls）
        /// </summary>
        /// <param name="filePath">Excel文件路径</param>
        /// <param name="strMsg"></param>
        /// <returns></returns>
        public static DataSet ExcelToDataSet(string filePath, out string strMsg)
        {
            strMsg = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            string fileType = Path.GetExtension(filePath).ToLower();
            string fileName = Path.GetFileName(filePath).ToLower();
            try
            {
                ISheet sheet = null;
                int sheetNumber = 0;
                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                if (fileType == ".xlsx")
                {
                    // 2007版本
                    XSSFWorkbook workbook = new XSSFWorkbook(fs);
                    sheetNumber = workbook.NumberOfSheets;
                    for (int i = 0; i < sheetNumber; i++)
                    {
                        string sheetName = workbook.GetSheetName(i);
                        sheet = workbook.GetSheet(sheetName);
                        if (sheet != null)
                        {
                            dt = GetSheetDataTable(sheet, out strMsg);
                            if (dt != null)
                            {
                                dt.TableName = sheetName.Trim();
                                ds.Tables.Add(dt);
                            }
                            else
                            {
                                //MessageBox.Show("Sheet数据获取失败，原因：" + strMsg);
                            }
                        }
                    }
                }
                else if (fileType == ".xls")
                {
                    // 2003版本
                    HSSFWorkbook workbook = new HSSFWorkbook(fs);
                    sheetNumber = workbook.NumberOfSheets;
                    for (int i = 0; i < sheetNumber; i++)
                    {
                        string sheetName = workbook.GetSheetName(i);
                        sheet = workbook.GetSheet(sheetName);
                        if (sheet != null)
                        {
                            dt = GetSheetDataTable(sheet, out strMsg);
                            if (dt != null)
                            {
                                dt.TableName = sheetName.Trim();
                                ds.Tables.Add(dt);
                            }
                            else
                            {
                                //MessageBox.Show("Sheet数据获取失败，原因：" + strMsg);
                            }
                        }
                    }
                }
                return ds;
            }
            catch (Exception ex)
            {
                strMsg = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// 获取sheet表对应的DataTable
        /// </summary>
        /// <param name="sheet">Excel工作表</param>
        /// <param name="strMsg"></param>
        /// <returns></returns>
        public static DataTable GetSheetDataTable(ISheet sheet, out string strMsg)
        {
            strMsg = "";
            DataTable dt = new DataTable();
            string sheetName = sheet.SheetName;
            int startIndex = 0;// sheet.FirstRowNum;
            int lastIndex = sheet.LastRowNum;
            //最大列数
            int cellCount = 0;
            IRow maxRow = sheet.GetRow(0);
            for (int i = startIndex; i <= lastIndex; i++)
            {
                IRow row = sheet.GetRow(i);
                if (row != null && cellCount < row.LastCellNum)
                {
                    cellCount = row.LastCellNum;
                    maxRow = row;
                }
            }
            //列名设置
            try
            {
                for (int i = 0; i < maxRow.LastCellNum; i++)//maxRow.FirstCellNum
                {
                    dt.Columns.Add(Convert.ToChar(((int)'A') + i).ToString());
                    //DataColumn column = new DataColumn("Column" + (i + 1).ToString());
                    //dt.Columns.Add(column);
                }
            }
            catch
            {
                strMsg = "工作表" + sheetName + "中无数据";
                return null;
            }
            //数据填充
            for (int i = startIndex; i <= lastIndex; i++)
            {
                IRow row = sheet.GetRow(i);
                DataRow drNew = dt.NewRow();
                if (row != null)
                {
                    for (int j = row.FirstCellNum; j < row.LastCellNum; ++j)
                    {
                        if (row.GetCell(j) != null)
                        {
                            ICell cell = row.GetCell(j);
                            switch (cell.CellType)
                            {
                                case CellType.Blank:
                                    drNew[j] = "";
                                    break;
                                case CellType.Numeric:
                                    short format = cell.CellStyle.DataFormat;
                                    //对时间格式（2015.12.5、2015/12/5、2015-12-5等）的处理 
                                    if (format == 14 || format == 31 || format == 57 || format == 58 || format == 176)
                                        drNew[j] = cell.DateCellValue;
                                    else
                                        drNew[j] = cell.NumericCellValue;
                                    if (cell.CellStyle.DataFormat == 177 || cell.CellStyle.DataFormat == 178 || cell.CellStyle.DataFormat == 188)
                                        drNew[j] = cell.NumericCellValue.ToString("#0.00");
                                    break;
                                case CellType.String:
                                    drNew[j] = cell.StringCellValue;
                                    break;
                                case CellType.Formula:
                                    try
                                    {
                                        drNew[j] = cell.NumericCellValue;
                                        if (cell.CellStyle.DataFormat == 177 || cell.CellStyle.DataFormat == 178 || cell.CellStyle.DataFormat == 188)
                                            drNew[j] = cell.NumericCellValue.ToString("#0.00");
                                    }
                                    catch
                                    {
                                        try
                                        {
                                            drNew[j] = cell.StringCellValue;
                                        }
                                        catch { }
                                    }
                                    break;
                                case CellType.Error:
                                    break;
                                default:
                                    drNew[j] = cell.StringCellValue;
                                    break;
                            }
                        }
                    }
                }
                dt.Rows.Add(drNew);
            }
            return dt;
        }

        /// <summary>
        /// DataTable转成List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> ToDataList<T>(this DataTable dt)
        {
            var list = new List<T>();
            var plist = new List<PropertyInfo>(typeof(T).GetProperties());
            foreach (DataRow item in dt.Rows)
            {
                T s = Activator.CreateInstance<T>();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    PropertyInfo info = plist.Find(p => p.Name == dt.Columns[i].ColumnName);
                    if (info != null)
                    {
                        try
                        {
                            if (!Convert.IsDBNull(item[i]))
                            {
                                object v = null;
                                if (info.PropertyType.ToString().Contains("System.Nullable"))
                                {
                                    v = Convert.ChangeType(item[i], Nullable.GetUnderlyingType(info.PropertyType));
                                }
                                else
                                {
                                    v = Convert.ChangeType(item[i], info.PropertyType);
                                }
                                info.SetValue(s, v, null);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("字段[" + info.Name + "]转换出错," + ex.Message);
                        }
                    }
                }
                list.Add(s);
            }
            return list;
        }

        /// <summary>
        /// DataTable转成Dto
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static T ToDataDto<T>(this DataTable dt)
        {
            T s = Activator.CreateInstance<T>();
            if (dt == null || dt.Rows.Count == 0)
            {
                return s;
            }
            var plist = new List<PropertyInfo>(typeof(T).GetProperties());
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                PropertyInfo info = plist.Find(p => p.Name == dt.Columns[i].ColumnName);
                if (info != null)
                {
                    try
                    {
                        if (!Convert.IsDBNull(dt.Rows[0][i]))
                        {
                            object v = null;
                            if (info.PropertyType.ToString().Contains("System.Nullable"))
                            {
                                v = Convert.ChangeType(dt.Rows[0][i], Nullable.GetUnderlyingType(info.PropertyType));
                            }
                            else
                            {
                                v = Convert.ChangeType(dt.Rows[0][i], info.PropertyType);
                            }
                            info.SetValue(s, v, null);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("字段[" + info.Name + "]转换出错," + ex.Message);
                    }
                }
            }
            return s;
        }

        /// <summary>
        /// 将实体集合转换为DataTable
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entities">实体集合</param>
        public static DataTable ToDataTable<T>(List<T> entities)
        {
            var result = CreateTable<T>();
            FillData(result, entities);
            return result;
        }

        /// <summary>
        /// 创建表
        /// </summary>
        private static DataTable CreateTable<T>()
        {
            var result = new DataTable();
            var type = typeof(T);
            foreach (var property in type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
            {
                var propertyType = property.PropertyType;
                if ((propertyType.IsGenericType) && (propertyType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                    propertyType = propertyType.GetGenericArguments()[0];
                result.Columns.Add(property.Name, propertyType);
            }
            return result;
        }

        /// <summary>
        /// 填充数据
        /// </summary>
        private static void FillData<T>(DataTable dt, IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                dt.Rows.Add(CreateRow(dt, entity));
            }
        }

        /// <summary>
        /// 创建行
        /// </summary>
        private static DataRow CreateRow<T>(DataTable dt, T entity)
        {
            DataRow row = dt.NewRow();
            var type = typeof(T);
            foreach (var property in type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
            {
                row[property.Name] = property.GetValue(entity) ?? DBNull.Value;
            }
            return row;
        }
    }
}
