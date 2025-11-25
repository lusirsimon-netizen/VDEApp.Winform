using System;
using System.Collections.Generic;
using VDEApp.Controllers;

namespace VDEApp
{
    public partial class FromExample : AntdUI.Window, ILocalizableForm
    {
        public FromExample()
        {
            InitializeComponent();

            InitTableData();
        }
        /// <summary>
        /// 初始化表格数据
        /// </summary>
        public void InitTableData()
        {
            //添加列
            table1.Columns = new AntdUI.ColumnCollection {
                //自定义绘制下拉框
                new AntdUI.Column("Item1","相机类型").SetRender((val,roc,index)=>
                {
                    var testItem=(ExampleClass)roc;
                    var btn=new AntdUI.CellButton("item1",testItem.Item1.ToString()).SetArrow();
                    btn.DropDownItems=new List<object>{
                        new AntdUI.SelectItem("aaa",1),
                        new AntdUI.SelectItem("bbb", 2)
                    };
                    btn.DropDownValueChanged=(value)=>{
                            testItem.Item1=(int)value;
                        btn.Text=testItem.Item2;
                    };
                    return btn;
                })
                .SetLocalizationTitleID("Table.Column."),
                //普通文本列
                new AntdUI.Column("Item2", "姓名").SetLocalizationTitleID("Table.Column."),
            };
            //绑定数据
            var items = new List<ExampleClass>();
            items.Add(new ExampleClass() { Item1 = 1, Item2 = "1111" });
            items.Add(new ExampleClass() { Item1 = 2, Item2 = "2222" });
            table1.DataSource = items;
        }

        public void RefreshLanguage()
        {
            throw new NotImplementedException();
        }
    }
}
