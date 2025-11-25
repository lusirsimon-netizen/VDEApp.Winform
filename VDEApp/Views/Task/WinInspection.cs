using AntdUI;
using Insnex.Vision2D.Common;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Xml.Linq;
using VDEApp.Commons;
using VDEApp.Configs;
using VDEApp.Controllers;
using VDEApp.Infrastructure;
using VDEApp.Models.TaskNodes;
using static VDEApp.Views.Task.WinCalibration;

namespace VDEApp.Views.Task
{
    public partial class WinInspection : AntdUI.Window, ILocalizableForm
    {
        private InspectionNode _inspectionNode;
        public InspectionNode InspectionNode
        {
            get { return _inspectionNode; }
            set
            {
                _inspectionNode = value;
                insToolEditorControl1.Subject = value?.InsToolBlock;
            }
        }

        private Dictionary<string, object> DataDic = new Dictionary<string, object>();
        private BindingList<DataItem> DataList = new BindingList<DataItem>();

        #region Init

        public WinInspection()
        {
            InitializeComponent();
            InitTable();
            RefreshLanguage();
            string name = GlobalConfig.Instance.CurrentProject.CurrentTask.Name + GlobalConfig.Instance.GlobalLocalizer.GetString("检测节点");
            this.pageHeader1.Text = name;
            if (ServiceLocator.ProjectController.CurrentProject.IsRunning)
            {
                this.btnSave.Enabled = false;
                this.btnRestore.Enabled = false;
            }
            this.FormClosing += WinInspection_FormClosing;
        }

        private void InitTable()
        {
            tableSPEC.Columns.Clear();
            tableSPEC.Columns = new ColumnCollection{
                new Column ("Name" , GlobalConfig.Localizer.GetString("tableParams_column_name")).SetAlign(ColumnAlign.Center),
                new Column ("Type" , GlobalConfig.Localizer.GetString("tableParams_column_type")).SetAlign(ColumnAlign.Center),
                new Column ("Value", GlobalConfig.Localizer.GetString("tableParams_column_value")).SetAlign(ColumnAlign.Center),
                new Column ("Btns" , GlobalConfig.Localizer.GetString("tableParams_column_btns")).SetAlign(ColumnAlign.Center)
            };
            DataDic = GlobalConfig.Instance.CurrentProject.CurrentTask.Spec ?? new Dictionary<string, object>();
            foreach (var key in DataDic.Keys.ToList())
            {
                if (DataDic[key] is JObject jObj)
                {
                    var item1 = double.Parse(jObj["Item1"]?.ToString());
                    var item2 = double.Parse(jObj["Item2"]?.ToString());
                    DataDic[key] = Tuple.Create(item1, item2);
                }
                else if (DataDic[key] is Int64 i)
                {
                    DataDic[key] = (Int32)i;
                }
            }
            DataList = new BindingList<DataItem>(DicToList(DataDic));
            RefreshTable();
            tableSPEC.EditMode = TEditMode.DoubleClick;
        }

        private static CellButton[] CreateOpsButtons() => new CellButton[]
        {
            new CellButton("delete",null, TTypeMini.Error  ).SetIcon("DeleteOutlined"),
        };

        #endregion

        #region Refresh

        public void RefreshLanguage()
        {
            LanguageController.AutoRefreshLanguage(this, GlobalConfig.Instance.GlobalLocalizer);
        }

        private void RefreshTable()
        {
            // 解除事件绑定
            tableSPEC.CellBeginEdit -= TableSPEC_CellBeginEdit;
            tableSPEC.CellButtonClick -= TableSPEC_CellButtonClick;
            tableSPEC.CellEndEdit -= TableSPEC_CellEndEdit;

            // 绑定数据源
            tableSPEC.DataSource = new BindingList<DataItem>(DataList);

            // 重新绑定事件
            tableSPEC.CellBeginEdit += TableSPEC_CellBeginEdit;
            tableSPEC.CellButtonClick += TableSPEC_CellButtonClick;
            tableSPEC.CellEndEdit += TableSPEC_CellEndEdit;
        }

        #endregion

        #region Event

        private void WinInspection_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ServiceLocator.ProjectController.CurrentProject.IsRunning)
                return;
            DialogResult r = MessageBox.Show(
                GlobalConfig.Localizer.GetString("tolb_save_tip", "Do you want to save the current ToolBlock?"),
                GlobalConfig.Localizer.GetString("tolb_save_confirm", "Save Confirmation"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (r == DialogResult.Yes)
            {
                try
                {
                    string path = GlobalConfig.Instance.CurrentProject.CurrentTask.GetInspectionToolBlockPath();
                    InspectionNode.Save(path);
                    SafeShowMessage(GlobalConfig.Localizer.GetString("tolb_save_success"), true);
                }
                catch (Exception ex)
                {
                    SafeShowMessage(string.Format(GlobalConfig.Localizer.GetString("tolb_save_fail"), ex.Message));
                    return; // 保存失败时不关闭对话框
                }
            }
            else
            {
                string path = GlobalConfig.Instance.CurrentProject.CurrentTask.GetInspectionToolBlockPath();
                this.InspectionNode.Load(path);
                //insToolEditorControl1.Subject = this.InspectionNode.InsToolBlock;
                //insToolEditorControl1.Update();
            }
            this.FormClosing -= WinInspection_FormClosing;
        }

        #region Button

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string path = GlobalConfig.Instance.CurrentProject.CurrentTask.GetInspectionToolBlockPath();
                InspectionNode.Save(path);
                MessageBox.Show(
                    string.Format(GlobalConfig.Localizer.GetString("tolb_save_path"), path),
                    GlobalConfig.Localizer.GetString("tolb_save_success", "Save Successfully"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format(GlobalConfig.Localizer.GetString("tolb_save_error"), ex.Message),
                    GlobalConfig.Localizer.GetString("tolb_save_failed", "Save Failed"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
        private void BtnRestore_Click(object sender, EventArgs e)
        {
            try
            {
                string path = GlobalConfig.Instance.CurrentProject.CurrentTask.GetInspectionToolBlockPath();
                this.InspectionNode.Load(path);
                insToolEditorControl1.Subject = this.InspectionNode.InsToolBlock;
                insToolEditorControl1.Update();
                DataList = new BindingList<DataItem>(DicToList(DataDic));
                RefreshTable();
                MessageBox.Show(
                    GlobalConfig.Localizer.GetString("tolb_restore_success", "Tolb has restored!"),
                    GlobalConfig.Localizer.GetString("tolb_restore_tip", "Tip"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format(GlobalConfig.Localizer.GetString("tolb_restore_fail"), ex.Message),
                    GlobalConfig.Localizer.GetString("tolb_restore_error", "Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return; // 保存失败时不关闭对话框
            }
        }
        private void BtnNewSPEC_Click(object sender, EventArgs e)
        {
            // 验证输入
            if (string.IsNullOrWhiteSpace(inputName.Text))
            {
                SafeShowMessage(GlobalConfig.Localizer.GetString("tolb_param_input_tip1"));
                return;
            }
            string name = inputName.Text.Trim();
            // 检查名称是否已存在
            if (DataList != null && DataList.Any(x => x.Name == name))
            {
                SafeShowMessage(GlobalConfig.Localizer.GetString("tolb_param_input_tip2"));
                return;
            }
            if (selectType.SelectedIndex == -1)
            {
                SafeShowMessage(GlobalConfig.Localizer.GetString("tolb_param_select_tip"));
                return;
            }
            string type = selectType.SelectedValue.ToString();

            try
            {
                // 直接创建对应类型的实例
                object newObject = type switch
                {
                    "String" => "null",
                    "Int" => 0,
                    "Double" => 0.0,
                    "Boolean" => false,
                    "Range" => Tuple.Create(0.0, 1.0),
                    _ => throw new ArgumentException($"不支持的类型: {type}")
                };

                // 创建新的 DataItem
                var newItem = new DataItem
                {
                    Name = name,
                    Object = newObject,
                    Btns = CreateOpsButtons()
                };

                // 添加到列表，保持类型一致
                DataList.Add(newItem);

                // 刷新表格
                RefreshTable();

                // 清空输入
                inputName.Text = "";

                SafeShowMessage(GlobalConfig.Localizer.GetString("tolb_param_new_success"), true);
            }
            catch (Exception ex)
            {
                SafeShowMessage(string.Format(GlobalConfig.Localizer.GetString("tolb_param_new_fail"), ex.Message));
            }
        }
        private void BtnSaveSPEC_Click(object sender, EventArgs e)
        {
            try
            {
                //修改当前任务parameter
                var ctask = GlobalConfig.Instance.CurrentProject.CurrentTask;
                ctask.Spec = ListToDic(DataList);
                //修改当前项目TaskGroup中的parameter
                foreach (TaskModel t in GlobalConfig.Instance.CurrentProject.TaskGroup)
                {
                    if (t.Guid == ctask.Guid)
                    {
                        t.Spec = ListToDic(DataList);
                    }
                }
                //参数同步到ToolBlock
                if (InspectionNode.InsToolBlock.Inputs.Contains("Spec"))
                {
                    InspectionNode.InsToolBlock.Inputs["Spec"].Value = ListToDic(DataList);
                }
                else
                {
                    InspectionNode.InsToolBlock.Inputs.Add(new InsToolBlockTerminal("Spec", ListToDic(DataList), typeof(Dictionary<string, object>)));
                }
                //保存项目
                MessageBox.Show(
                    GlobalConfig.Localizer.GetString("tolb_param_save_success"),
                    GlobalConfig.Localizer.GetString("tolb_save_success"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format(GlobalConfig.Localizer.GetString("tolb_param_save_failed"), ex.Message),
                    GlobalConfig.Localizer.GetString("tolb_save_failed"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        #endregion

        #region SPEC_Edit

        private bool TableSPEC_CellBeginEdit(object sender, TableEventArgs e)
        {
            if (e.Column == null)
            {
                return false;
            }

            if (e.Column.Key == "Value")
            {
                var item = e.Record as DataItem;
                if (item.Object is Tuple<double, double>)
                {
                    //编辑范围的值
                    try
                    {
                        Tuple<double, double> existingRange = item.Object is Tuple<double, double> range ? range : new Tuple<double, double>(1.0, 2.0);
                        using var rangeForm = new WinSPECInput(existingRange)
                        {
                            Owner = AppModuleSingleton.MainFormInstance,
                            StartPosition = FormStartPosition.CenterParent
                        };
                        if (rangeForm.ShowDialog() == DialogResult.OK)
                        {

                            item.Object = rangeForm.Result;
                            DataDic[item.Name] = rangeForm.Result;

                            var listItem = DataList.FirstOrDefault(x => x.Name == item.Name);
                            if (listItem != null)
                            {
                                listItem.Object = rangeForm.Result;
                            }
                            SafeShowMessage(string.Format(GlobalConfig.Localizer.GetString("tolb_param_edit_value_update"), item.Name), true);
                            RefreshTable();

                        }
                        return false;
                    }
                    catch (Exception ex)
                    {
                        SafeShowMessage(string.Format(GlobalConfig.Localizer.GetString("tolb_param_edit_value_fail"), ex.Message));
                    }

                }
                return true;
            }
            else if (e.Column.Key == "Name")
            {
                return true;
            }
            return false;

        }

        private bool TableSPEC_CellEndEdit(object sender, AntdUI.TableEndEditEventArgs e)
        {
            if (e.RowIndex > 0 && e.ColumnIndex == 2) // Value列
            {
                try
                {
                    // 获取编辑后的值
                    var item = e.Record as DataItem;
                    var newValue = e.Value?.ToString();
                    var name = item.Name;
                    var type = item.Type;
                    if (newValue == null)
                    {
                        SafeShowMessage(GlobalConfig.Localizer.GetString("tolb_param_edit_value_tip1"));
                        return false;
                    }

                    if (!string.IsNullOrEmpty(name))
                    {
                        // 获取对象的实际类型，而不是使用Type属性
                        var actualType = item.Object?.GetType();
                        if (actualType == null)
                        {
                            SafeShowMessage(GlobalConfig.Localizer.GetString("tolb_param_edit_value_tip2"));
                            return false;
                        }

                        object convertedValue = null;
                        try
                        {
                            // 根据类型转换值
                            if (actualType == typeof(string))
                            {
                                convertedValue = newValue;
                            }
                            else if (actualType == typeof(int))
                            {
                                convertedValue = int.Parse(newValue);
                            }
                            else if (actualType == typeof(double))
                            {
                                convertedValue = double.Parse(newValue);
                            }
                            else if (actualType == typeof(bool))
                            {
                                convertedValue = bool.Parse(newValue);
                            }
                            else
                            {
                                // 对于其他类型，尝试使用类型转换器
                                convertedValue = Convert.ChangeType(newValue, actualType);
                            }
                        }
                        catch (FormatException)
                        {
                            SafeShowMessage(string.Format(GlobalConfig.Localizer.GetString("tolb_param_edit_value_tip3"), newValue, actualType.Name));
                            return false;
                        }

                        // 更新字典和数据项
                        if (DataDic.ContainsKey(name))
                        {
                            var listItem = DataList.FirstOrDefault(x => x.Name == name);
                            if (listItem != null)
                            {
                                listItem.Object = convertedValue;
                            }
                            SafeShowMessage(string.Format(GlobalConfig.Localizer.GetString("tolb_param_edit_value_update"), name), true);
                            RefreshTable();
                        }
                        else
                        {
                            SafeShowMessage(string.Format(GlobalConfig.Localizer.GetString("tolb_param_edit_value_tip4"), name));
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    SafeShowMessage(string.Format(GlobalConfig.Localizer.GetString("tolb_param_edit_value_fail"), ex.Message));
                    return false;
                }
                return true;
            }
            else if (e.RowIndex > 0 && e.ColumnIndex == 0) //Name列
            {
                try
                {
                    var item = e.Record as DataItem;
                    var newName = e.Value?.ToString();
                    var value = item.Value;
                    var type = item.Type;
                    if (newName == null)
                    {
                        SafeShowMessage(GlobalConfig.Localizer.GetString("tolb_param_edit_name_tip1"));
                        return false;
                    }
                    bool isDuplicate = DataList.Any(x => x.Name == newName && !x.Object.Equals(item.Object));
                    if (isDuplicate)
                    {
                        SafeShowMessage(string.Format(GlobalConfig.Localizer.GetString("tolb_param_edit_name_tip2"), newName));
                        return false;
                    }
                    if (!string.IsNullOrEmpty(value))
                    {
                        var listItem = DataList.FirstOrDefault(x => x.Object.Equals(item.Object));
                        if (listItem != null)
                        {
                            listItem.Name = newName;
                        }
                        SafeShowMessage(GlobalConfig.Localizer.GetString("tolb_param_edit_name_success"), true);
                        RefreshTable();
                    }
                }
                catch (Exception ex)
                {
                    SafeShowMessage(string.Format(GlobalConfig.Localizer.GetString("tolb_param_edit_name_fail"), ex.Message));
                    return false;
                }
                return true;
            }
            return false;
        }

        private void TableSPEC_CellButtonClick(object sender, TableButtonEventArgs e)
        {
            // 检查是否是按钮列且不是标题行
            if (e.RowIndex > 0 && e.ColumnIndex == 3) // 第3列是按钮列
            {
                try
                {
                    var item = e.Record as DataItem;
                    var name = item.Name;
                    if (MessageBox.Show(
                        string.Format(GlobalConfig.Localizer.GetString("tolb_param_delete_tip"), name),
                        GlobalConfig.Localizer.GetString("tolb_param_delete_confirm"),
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (DataList.Any(x => x.Name == name))
                        {
                            // 从列表中移除
                            DataList.Remove(item);
                        }

                        SafeShowMessage(GlobalConfig.Localizer.GetString("tolb_param_delete_success"), true);
                        RefreshTable();
                    }
                }
                catch (Exception ex)
                {
                    SafeShowMessage(string.Format(GlobalConfig.Localizer.GetString("tolb_param_delete_fail"), ex.Message));
                }
            }
        }

        #endregion

        #endregion

        #region Other

        private void SafeShowMessage(string content, bool isSuccess = false)
        {
            try
            {
                // 尝试使用 AntdUI Message
                if (isSuccess)
                {
                    AntdUI.Message.success(this, content);
                }
                else
                {
                    AntdUI.Message.warn(this, content);
                }
            }
            catch
            {
                // 如果 AntdUI Message 失败，使用系统 MessageBox
                MessageBox.Show(content, "提示",
                    MessageBoxButtons.OK,
                    isSuccess ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            }
        }

        public List<DataItem> DicToList(Dictionary<string, object> dic)
        {
            var list = new List<DataItem>();
            if (dic != null)
            {
                foreach (var item in dic)
                {
                    list.Add(new DataItem { Name = item.Key, Object = item.Value, Btns = CreateOpsButtons() });
                }
            }
            return list;
        }

        public Dictionary<string, object> ListToDic(BindingList<DataItem> list)
        {
            var dictionary = new Dictionary<string, object>();

            if (list != null)
            {
                foreach (var item in list)
                {
                    if (item != null && !string.IsNullOrEmpty(item.Name))
                    {
                        // 如果字典中已存在相同的键，覆盖已存在的键
                        dictionary[item.Name] = item.Object;
                    }
                }
            }

            return dictionary;
        }

        #endregion

    }

    public class DataItem
    {
        //private string _name { get; set; }
        public object Object { get; set; }
        public string Name { get; set; }
        public string Type { get => this.Object == null ? "null" : FormatObjectType(Object); }
        public string Value { get => this.Object == null ? "null" : FormatObjectValue(Object); set; }
        public CellButton[] Btns { get; set; }
        // 格式化对象显示值
        private string FormatObjectValue(object obj)
        {
            if (obj is Tuple<double, double> range)
            {
                return $"{range.Item1}~{range.Item2}";
            }
            return obj?.ToString() ?? "null";
        }
        private string FormatObjectType(object obj)
        {
            if (obj is Tuple<double, double> range)
            {
                return $"Range";
            }
            return obj?.GetType().Name;
        }
    }

}
