using AntdUI;
using Insnex.Vision2D.Common;
using Insnex.Vision2D.Core;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Xml.Linq;
using VDEApp.Commons;
using VDEApp.Configs;
using VDEApp.Controllers;
using VDEApp.Infrastructure;
using VDEApp.Models.TaskNodes;
using static VDEApp.Program;
using Message = AntdUI;

namespace VDEApp.Views.Task
{
    public partial class WinCalibration : AntdUI.Window, ILocalizableForm
    {
        private Models.TaskNodes.CalibrationNode _calibrationNode;
        public Models.TaskNodes.CalibrationNode CalibrationNode
        {
            get { return _calibrationNode; }
            set
            {
                _calibrationNode = value;
                insToolEditorControl1.Subject = value?.InsToolBlock;
            }
        }

        private Dictionary<string, object> DataDic = new Dictionary<string, object>();
        private BindingList<DataItem> DataList = new BindingList<DataItem>();

        #region Init

        public WinCalibration()
        {
            InitializeComponent();
            InitTable();
            RefreshLanguage();
            string name = GlobalConfig.Instance.CurrentProject.CurrentTask.Name + GlobalConfig.Instance.GlobalLocalizer.GetString("标定节点");
            this.pageHeader1.Text = name;
            if(ServiceLocator.ProjectController.CurrentProject.IsRunning)
            {
                this.btnSave.Enabled = false;
                this.btnRestore.Enabled = false;
            }
            this.FormClosing += WinCalibration_FormClosing;
        }

        private void InitTable()
        {
            tableParams.Columns.Clear();
            tableParams.Columns = new ColumnCollection{
                new Column ("Name" , GlobalConfig.Localizer.GetString("tableParams_column_name")).SetAlign(ColumnAlign.Center),
                new Column ("Type" , GlobalConfig.Localizer.GetString("tableParams_column_type")).SetAlign(ColumnAlign.Center),
                new Column ("Value", GlobalConfig.Localizer.GetString("tableParams_column_value")).SetAlign(ColumnAlign.Center),
                new Column ("Btns" , GlobalConfig.Localizer.GetString("tableParams_column_btns")).SetAlign(ColumnAlign.Center)
            };
            DataDic = GlobalConfig.Instance.CurrentProject.CurrentTask.Parameter ?? new Dictionary<string, object>();
            foreach (var key in DataDic.Keys.ToList())
            {
                if (DataDic[key] is Int64 i)
                {
                    DataDic[key] = (Int32)i;
                }
            }
            DataList = new BindingList<DataItem>(DicToList(DataDic));
            RefreshTable();
            tableParams.EditMode = TEditMode.DoubleClick;
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
            tableParams.CellBeginEdit -= TableParams_CellBeginEdit;
            tableParams.CellButtonClick -= TableParams_CellButtonClick;
            tableParams.CellEndEdit -= TableParams_CellEndEdit;

            // 绑定数据源
            tableParams.DataSource = new BindingList<DataItem>(DataList);

            // 重新绑定事件
            tableParams.CellBeginEdit += TableParams_CellBeginEdit;
            tableParams.CellButtonClick += TableParams_CellButtonClick;
            tableParams.CellEndEdit += TableParams_CellEndEdit;
        }

        #endregion

        #region Event

        private void WinCalibration_FormClosing(object sender, FormClosingEventArgs e)
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
                    string path = GlobalConfig.Instance.CurrentProject.CurrentTask.GetCalibrationToolBlockPath();
                    CalibrationNode.Save(path);
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
                string path = GlobalConfig.Instance.CurrentProject.CurrentTask.GetCalibrationToolBlockPath();
                this.CalibrationNode.Load(path);
                //insToolEditorControl1.Subject = this.CalibrationNode.InsToolBlock;
                //insToolEditorControl1.Update();
            }
            this.FormClosing -= WinCalibration_FormClosing;
        }

        #region Button
        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string path = GlobalConfig.Instance.CurrentProject.CurrentTask.GetCalibrationToolBlockPath();

                CalibrationNode.Save(path);
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
                string path = GlobalConfig.Instance.CurrentProject.CurrentTask.GetCalibrationToolBlockPath();
                this.CalibrationNode.Load(path);
                insToolEditorControl1.Subject = this.CalibrationNode.InsToolBlock;
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
        private void BtnNewParam_Click(object sender, EventArgs e)
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
        private void BtnSaveParams_Click(object sender, EventArgs e)
        {
            try
            {
                //修改当前任务parameter
                var ctask = GlobalConfig.Instance.CurrentProject.CurrentTask;
                ctask.Parameter = ListToDic(DataList);
                //修改当前项目TaskGroup中的parameter
                foreach (TaskModel t in GlobalConfig.Instance.CurrentProject.TaskGroup)
                {
                    if (t.Guid == ctask.Guid)
                    {
                        t.Parameter = ListToDic(DataList);
                    }
                }
                //参数同步到ToolBlock
                if (CalibrationNode.InsToolBlock.Inputs.Contains("parameter"))
                {
                    CalibrationNode.InsToolBlock.Inputs["parameter"].Value = ListToDic(DataList);
                }
                else
                {
                    CalibrationNode.InsToolBlock.Inputs.Add(new InsToolBlockTerminal("parameter", ListToDic(DataList), typeof(Dictionary<string, object>)));
                }
                //保存项目
                ServiceLocator.ProjectController.SaveCurrentProject();
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

        #region Param_Edit
        private bool TableParams_CellBeginEdit(object sender, TableEventArgs e)
        {
            if (e.Column == null)
            {
                return true;
            }

            return e.Column.Key == "Value"||e.Column.Key == "Name";
        }
        private bool TableParams_CellEndEdit(object sender, AntdUI.TableEndEditEventArgs e)
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

                        object convertedValue;
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
                        if (DataList.Any(x => x.Name == name))
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
        private void TableParams_CellButtonClick(object sender, TableButtonEventArgs e)
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
                    AntdUI.Message.success(this,content);
                }
                else
                {
                    AntdUI.Message.warn(this,content);
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
            if (dic != null) {
                foreach (var item in dic)
                {
                    list.Add(new DataItem { Name = item.Key, Object=item.Value ,Btns = CreateOpsButtons() });
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
}
