using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Model生成器
{
    public partial class CreateModelForm : Form
    {
        /// <summary>
        /// xls文件路径
        /// </summary>
        private static string StrXlsPath { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public CreateModelForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 获取xls文件路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenXls_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "表格文件(*.xls)|*.xls";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                StrXlsPath = ofd.FileName;
            }
            labXlsPath.Text = StrXlsPath;
        }

        /// <summary>
        /// 创建model
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateModel_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(StrXlsPath))
            {
                MessageBox.Show("请先选择xls文件");
                return;
            }
            try
            {
                DataTable dt = new ExcelHelper(StrXlsPath).ExcelToDataTable("Sheet1", true);
                string strFileName = Path.GetFileNameWithoutExtension(StrXlsPath);
                string strModel = CreateMemberModel(dt, strFileName);
                strModel = strModel + "\r\n" + GetAddCode(dt) + "\r\n" + GetBatchAddCode(dt) + "\r\n" + GetUpdateCode(dt);
                using (StreamWriter sw = new StreamWriter(StrXlsPath.Replace("xls", "cs")))
                {
                    sw.Write(strModel);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            MessageBox.Show("生成成功");
        }

        /// <summary>
        /// 创建数据model
        /// </summary>
        /// <param name="dtData">datatable</param>
        /// <param name="strName">model的类名</param>
        /// <returns>model</returns>
        private string CreateMemberModel(DataTable dtData, string strName)
        {
            if (dtData == null)
            {
                return null;
            }
            StringBuilder strRes = new StringBuilder();
            strRes.Append("/// <summary>\r\n");
            strRes.Append("/// \r\n");
            strRes.Append("/// </summary>\r\n");
            strRes.Append("[DataContract]\r\n");
            strRes.AppendFormat("public class {0}", strName);
            strRes.Append("{\r\n");
            foreach (DataRow row in dtData.Rows)
            {
                strRes.Append(GetMemberData(row));
            }
            strRes.Append("}\r\n");
            return strRes.ToString();
        }

        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="drData">DataRow</param>
        /// <returns>单条数据</returns>
        private string GetMemberData(DataRow drData)
        {
            StringBuilder strData = new StringBuilder();
            strData.Append("/// <summary>\r\n");
            strData.AppendFormat("/// {0}\r\n", drData["Name"]);
            strData.Append("/// </summary>\r\n");
            strData.Append("[DataMember]\r\n");
            string strType = drData["Data Type"].ToString();
            strData.AppendFormat("public {0} {1} ", GetMemberType(strType), drData["Code"]);
            strData.Append("{get;set;}\r\n\r\n");
            return strData.ToString();
        }

        /// <summary>
        /// 数据类型转换
        /// </summary>
        /// <param name="strType">数据类型</param>
        /// <returns>数据类型</returns>
        private string GetMemberType(string strType)
        {
            if (string.IsNullOrWhiteSpace(strType))
            {
                return null;
            }
            if (strType.Contains("bigint"))
            {
                return "long";
            }
            if (strType.Contains("varchar"))
            {
                return "string";
            }
            if (strType.Contains("bool"))
            {
                return "bool";
            }
            if (strType.Contains("char"))
            {
                return "char";
            }
            if (strType.Contains("date"))
            {
                return "DateTime";
            }
            if (strType.Contains("dec"))
            {
                return "decimal";
            }
            if (strType.Contains("double"))
            {
                return "double";
            }
            if (strType.Contains("float"))
            {
                return "float";
            }
            if (strType.Contains("int"))
            {
                return "int";
            }
            if (strType.Contains("long"))
            {
                return "long";
            }
            if (strType.Contains("timestamp"))
            {
                return "timestamp";
            }
            if (strType.Contains("tinyint"))
            {
                return "int";
            }
            return null;
        }

        /// <summary>
        /// 获取属性集合
        /// </summary>
        /// <param name="dtData">datatable</param>
        /// <returns>属性集合</returns>
        private List<string> GetProp(DataTable dtData) => (from DataRow dr in dtData.Rows select dr["Code"].ToString()).ToList();

        /// <summary>
        /// 获取类型集合
        /// </summary>
        /// <param name="dtData"></param>
        /// <returns></returns>
        private List<string> GetType(DataTable dtData)
        {
            List<string> res = new List<string>();
            foreach (DataRow dr in dtData.Rows)
            {
                res.Add(dr["Data Type"].ToString());
            }
            return res;
        } 

        /// <summary>
        /// 生成添加代码
        /// </summary>
        /// <param name="dtData">数据源</param>
        /// <returns>添加代码</returns>
        private string GetAddCode(DataTable dtData)
        {
            List<string> propList = GetProp(dtData);
            StringBuilder addCode = new StringBuilder();
            string tbName = Path.GetFileNameWithoutExtension(StrXlsPath);
            addCode.AppendFormat("public long Add({0} entity)\r\n", tbName);
            addCode.Append("{\r\n");
            addCode.Append("this.ClearParameters();\r\n");
            addCode.Append("StringBuilder sql = new StringBuilder();\r\n");
            addCode.AppendFormat("sql.Append(\"INSERT INTO {0} (\");\r\n", tbName);
            int i = 1;
            foreach (string prop in propList)
            {
                if (i == 1)
                {
                    i++;
                    continue;
                }
                if (i == propList.Count)
                {
                    addCode.AppendFormat("sql.Append(\" {0} \");\r\n", prop);
                }
                else
                {
                    addCode.AppendFormat("sql.Append(\" {0}, \");\r\n", prop);
                }
                i++;
            }
            addCode.Append("sql.Append(\") VALUES(\");\r\n");
            i = 1;
            foreach (string prop in propList)
            {
                if (i == 1)
                {
                    i++;
                    continue;
                }
                if (i == propList.Count)
                {
                    addCode.AppendFormat("sql.Append(\" @{0} \");\r\n", prop);
                }
                else
                {
                    addCode.AppendFormat("sql.Append(\" @{0}, \");\r\n", prop);
                }
                i++;
            }
            addCode.Append("sql.Append(\"); \");\r\n");
            addCode.AppendFormat("sql.Append(\"SELECT @{0}:= LAST_INSERT_ID(); \");\r\n", propList[0]);
            addCode.Append("\r\n");
            i = 1;
            foreach (string prop in propList)
            {
                if (i == 1)
                {
                    i++;
                    continue;
                }
                addCode.AppendFormat("this.AddParameter(\"@{0}\", entity.{0});\r\n", prop);
                i++;
            }
            addCode.Append("\r\n");
            addCode.Append("object obj = this.ExecuteScalar(sql.ToString());\r\n");
            addCode.Append("var result =  obj == null ? 0 : Convert.ToInt64(obj);\r\n");
            addCode.Append("if (result <= 0)\r\n");
            addCode.Append("{\r\n");
            addCode.Append("throw new CustomException(\"添加失败\");\r\n");
            addCode.Append("}\r\n");
            addCode.Append("return result;\r\n");
            addCode.Append("}\r\n");

            return addCode.ToString();
        }

        /// <summary>
        /// 创建批量添加代码
        /// </summary>
        /// <param name="dtData">数据源</param>
        /// <returns>批量添加代码</returns>
        private string GetBatchAddCode(DataTable dtData)
        {
            List<string> propList = GetProp(dtData);
            StringBuilder addCode = new StringBuilder();
            string tbName = Path.GetFileNameWithoutExtension(StrXlsPath);
            addCode.AppendFormat("public void BatchAdd(List<{0}> entitys)\r\n", tbName);
            addCode.Append("{\r\n");
            addCode.Append("this.ClearParameters();\r\n");
            addCode.Append("StringBuilder sql = new StringBuilder();\r\n");
            addCode.AppendFormat("sql.Append(\"INSERT INTO {0} (\");\r\n", tbName);
            int i = 1;
            foreach (string prop in propList)
            {
                if (i == 1)
                {
                    i++;
                    continue;
                }
                if (i == propList.Count)
                {
                    addCode.AppendFormat("sql.Append(\" {0} \");\r\n", prop);
                }
                else
                {
                    addCode.AppendFormat("sql.Append(\" {0}, \");\r\n", prop);
                }
                i++;
            }
            addCode.Append("sql.Append(\") VALUES \");\r\n");
            addCode.Append("for (int i = 0; i < entitys.Count; i++)\r\n");
            addCode.Append("{\r\n");
            addCode.Append("sql.Append(\"(\");\r\n");
            i = 1;
            foreach (string prop in propList)
            {
                if (i == 1)
                {
                    i++;
                    continue;
                }
                if (i == propList.Count)
                {
                    addCode.AppendFormat("sql.AppendFormat(\" @{0}{{0}}\", i);\r\n", prop);
                }
                else
                {
                    addCode.AppendFormat("sql.AppendFormat(\" @{0}{{0}},\", i);\r\n", prop);
                }
                i++;
            }
            addCode.Append("sql.Append(\"),\");\r\n");
            i = 1;
            foreach (string prop in propList)
            {
                if (i == 1)
                {
                    i++;
                    continue;
                }
                addCode.AppendFormat("this.AddParameter(string.Format(\"@{0}{{0}}\", i), entitys[i].{0});\r\n", prop);
                i++;
            }
            addCode.Append("}\r\n");
            addCode.Append("int res = ExecuteNonQuery(sql.ToString().TrimEnd(','));\r\n");
            addCode.Append("if (res <= 0)\r\n");
            addCode.Append("{\r\n");
            addCode.Append("throw new CustomException(\"批量插入数据失败\");\r\n");
            addCode.Append("}\r\n");
            addCode.Append("}\r\n");

            return addCode.ToString();
        }

        /// <summary>
        /// 创建更新代码
        /// </summary>
        /// <param name="dtData"></param>
        /// <returns></returns>
        private string GetUpdateCode(DataTable dtData)
        {
            List<string> propList = GetProp(dtData);
            List<string> typeList = GetType(dtData);
            StringBuilder updateCode = new StringBuilder();
            string tbName = Path.GetFileNameWithoutExtension(StrXlsPath);
            updateCode.AppendFormat("public bool update({0} entity)\r\n", tbName);
            updateCode.Append("{\r\n");
            updateCode.Append("this.ClearParameters();\r\n");
            updateCode.Append("StringBuilder sql = new StringBuilder();\r\n");
            updateCode.AppendFormat("sql.Append(\"UPDATE {0} SET \");\r\n", tbName);
            int i = 1;
            foreach (string prop in propList)
            {
                if (i == 1)
                {
                    i++;
                    continue;
                }
                if (i == propList.Count)
                {
                    updateCode.AppendFormat("sql.Append(\" {0} = @{0} \");\r\n", prop);
                }
                else
                {
                    updateCode.AppendFormat("sql.Append(\" {0} = @{0}, \");\r\n", prop);
                }
                i++;
            }
            updateCode.AppendFormat("sql.Append(\" WHERE {0} = @{0}; \");\r\n", propList[0]);
            updateCode.Append("\r\n");
            i = 0;
            foreach (string prop in propList)
            {
                if (typeList[i].Contains("varchar"))
                {
                    updateCode.AppendFormat("this.AddParameter(\"@{0}\", entity.{0}??string.Empty);\r\n", prop);
                }
                else
                {
                    updateCode.AppendFormat("this.AddParameter(\"@{0}\", entity.{0});\r\n", prop);
                }
                i++;
            }
            updateCode.Append("\r\n");
            updateCode.Append("var res = this.ExecuteNonQuery(sql.ToString());\r\n");
            updateCode.Append("if (res <= 0)\r\n");
            updateCode.Append("{\r\n");
            updateCode.Append("throw new CustomException(\"更新失败\");\r\n");
            updateCode.Append("}\r\n");
            updateCode.Append("return true;\r\n");
            updateCode.Append("}\r\n");
            return updateCode.ToString();
        }
    }
}
