using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using VDEApp.Commons;
using VDEApp.Configs;
using VDEApp.LogModule;
using VDEApp.Models;

namespace VDEApp.Controllers
{
    public static class LanguageController
    {
        /// <summary>
        /// 用于临时存储所有收集到的语言Key（确保唯一性）
        /// </summary>
        private static HashSet<string> _languageKeys = new HashSet<string>();
        /// <summary>
        /// 自动更新窗口及所有子控件的多语言文本
        /// </summary>
        /// <param name="form">需要更新的窗口</param>
        /// <param name="localizer">本地化工具实例</param>
        public static void AutoRefreshLanguage(Form form, Localizer localizer)
        {
            if (form == null || localizer == null)
                return;
            _languageKeys.Clear();
            string formKeyPrefix = form.GetType().Name;
            string formTitleKey = $"{formKeyPrefix}_Title";
            _languageKeys.Add(formTitleKey);
            string formTitleValue = localizer.GetString(formTitleKey);
            if (!string.IsNullOrEmpty(formTitleValue))
            {
                form.Text = formTitleValue;
            }
            UpdateControls(form.Controls, formKeyPrefix, localizer);
            WriteKeystoFile();
        }

        /// <summary>
        /// 递归更新控件集合的多语言文本
        /// </summary>
        private static void UpdateControls(Control.ControlCollection controls, string formKeyPrefix, Localizer localizer)
        {
            foreach (Control control in controls)
            {
                if (string.IsNullOrWhiteSpace(control.Name) && !control.HasChildren)
                    continue;
                string controlKey = $"{formKeyPrefix}_{control.Name}";
                bool hasWritabileText = hasWritabileTextProperty(control);
                if (hasWritabileText)
                {
                    _languageKeys.Add(controlKey);
                    UpdateControlText(control, controlKey, localizer);
                }
                if (control.HasChildren)
                {
                    UpdateControls(control.Controls, formKeyPrefix, localizer);
                }
            }
        }

        /// <summary>
        /// 判断控件是否具有可写的Text属性
        /// </summary>
        private static bool hasWritabileTextProperty(Control control)
        {
            if (control == null)
                return false;
            PropertyInfo textProperty = control.GetType().GetProperty("Text");
            return textProperty != null && textProperty.CanWrite;
        }

        /// <summary>
        /// 根据控件类型更新文本属性（支持常见控件类型）
        /// </summary>
        private static void UpdateControlText(Control control, string controlKey, Localizer localizer)
        {
            string localizedText = localizer.GetString(controlKey);
            if (string.IsNullOrEmpty(localizedText))
                return;
            switch (control)
            {
                // AntdUI控件（根据实际控件库调整）
                case AntdUI.Button antdBtn:
                    antdBtn.Text = localizedText;
                    break;
                case AntdUI.Label antdLbl:
                    antdLbl.Text = localizedText;
                    break;
                case AntdUI.TabPage antdTab:
                    antdTab.Text = localizedText;
                    break;
                case AntdUI.PageHeader antdHeader:
                    antdHeader.Text = localizedText;
                    break;


                // 标准WinForm控件
                case Button btn:
                    btn.Text = localizedText;
                    break;
                case Label lbl:
                    lbl.Text = localizedText;
                    break;
                case TabPage tab:
                    tab.Text = localizedText;
                    break;
                case GroupBox gbx:
                    gbx.Text = localizedText;
                    break;
                case CheckBox cbx:
                    cbx.Text = localizedText;
                    break;
                case RadioButton rbtn:
                    rbtn.Text = localizedText;
                    break;


                default:
                    PropertyInfo textProperty = control.GetType().GetProperty("Text");
                    if (textProperty != null && textProperty.CanWrite)
                    {
                        textProperty.SetValue(control, localizedText);
                    }
                    break;

            }
            if (control is ToolStrip toolStrip)
            {
                foreach (ToolStripItem item in toolStrip.Items)
                {
                    if (!string.IsNullOrWhiteSpace(item.Name))
                    {
                        string itemKey = $"{controlKey}_{item.Name}";
                        _languageKeys.Add(itemKey); // 收集ToolStripItem的Key
                        string itemText = localizer.GetString(itemKey);
                        if (!string.IsNullOrEmpty(itemText))
                        {
                            item.Text = itemText;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 收集所有已收集到的语言Key（确保唯一性）
        /// </summary>
        private static void WriteKeystoFile()
        {
            try
            {
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LanguageKey.Txt");
                HashSet<string> allKeys = new HashSet<string>();
                if (File.Exists(filePath))
                {
                    string[] existingKeys = File.ReadAllLines(filePath, Encoding.UTF8);
                    foreach (string key in existingKeys)
                    {
                        if (!string.IsNullOrWhiteSpace(key))
                        {
                            allKeys.Add(key);
                        }
                    }
                }

                foreach (string newKey in _languageKeys)
                {
                    allKeys.Add(newKey);
                }

                using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
                {
                    foreach (string key in allKeys.OrderBy(k => k)) // 保持排序一致
                    {
                        writer.WriteLine(key);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error($"写入语言Key文件失败：{ex.Message}");
            }
        }

        /// <summary>
        /// 切换语言并刷新所有窗口
        /// </summary> 
        public static void SwitchLanguage(string langCode)
        {
            GlobalConfig.Instance.CurrentLang = langCode;
            GlobalConfig.Instance.UserOperation.CurrentLanguage = langCode;
            GlobalConfig.Instance.SaveOperation();
            GlobalConfig.Instance.GlobalLocalizer.LoadLanguage(langCode);
            // 触发语言切换事件
            AppModuleSingleton.LanguageSwitchEvent?.Invoke(null, EventArgs.Empty);
            // 刷新所有打开的窗口
            foreach (Form form in Application.OpenForms)
            {
                if (form is ILocalizableForm localizableForm)
                {
                    localizableForm.RefreshLanguage();
                }
            }
        }
    }

    /// <summary>
    /// 所有需要支持多语言的窗口都实现此接口
    /// </summary>
    public interface ILocalizableForm
    {
        void RefreshLanguage();
    }
}
