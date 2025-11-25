# 任务管理

## 1.任务管理

### 1.1 任务列表与任务管理

在程序主界面的左侧顶部，可以看到任务列表区域。这里会列出当前项目中所有已创建的任务。

![](/Image/Task/TaskManageButtom.png)

- **当前任务**：被选中的任务是当前活动任务。
- **切换任务**：在任务列表中单击您想要操作的其他任务名称，即可将其设为当前活动任务。

在程序主界面的左侧顶部，可以看到任务管理按钮。在任务管理窗口中，您可以对项目中所有任务进行增加、删除、复制、重命名。

![](/Image/Task/TaskManageView.png)

### 1.2 管理任务

点击在程序主界面的左侧顶部的任务管理按钮，在弹出的对话框中，点击新建任务，系统会自动生成一条名为"newTask"的任务。任务列表区域，您可以进行以下管理操作：

- **重命名**：双击任意任务名后，即可修改任务名称，按回车应用修改，需要注意的是，不可以修改为已有任务名。重命名后，与该任务关联的所有配置文件和存图目录也会被自动更新，无需手动操作。
- **复制任务**：点击任意任务的操作栏中的"复制"按钮，可以快速创建一个与源任务配置完全相同的副本。新任务的默认名称位是“原任务名_复制”。
- **删除任务**：点击任意任务的操作栏中的"删除"按钮，将从项目中**永久移除**该任务及其所有相关配置（不包括存图目录）。请注意：删除操作**不可逆**！在删除前，请务必确认您不再需要这些数据。


## 2.节点编辑


### 2.1 取像节点

在取像节点中可以将配置好的相机与任务进行绑定，点击配置按钮可快捷进入当前已选择相机的配置页。

也可以在取像节点中配置取像数量、超时和曝光时间，在这里配置的参数将与任务本身绑定，在任务触发取像时将参数设置到相机中，再进行取像。

注意：取像节点不能绑定没有连接的相机。

![](/Image/Task/AcquireNode.png)

### 2.2 标定节点

1. 参数编辑

+ 选择参数类型 -> 输入参数名 -> 新建参数

+ 点击对应单元格的值进行修改，也支持修改变量名

+ 点击应用，将参数列表同步至ToolBlock

![](/Image/Task/Parameters.png)

2. 参数使用

+ 同步后的参数列表将作为ToolBlock的终端输入项保存在ToolBlock中。

![](/Image/Task/TerminalParameter_Calibration.png)

+ 输入终端的使用需要在ToolBlock脚本中进行。为了方便使用，我们的参数列表以字典(Dictionary<string,Object>)的形式保存。

    ```csharp
    var dic = mToolBlock.Inputs["parameter"].value as Dictionary<string,Object>;
    ```

+ 注：从字典中获取值的时候要进行类型转换(原类型为object)

    ```csharp
    int k = (int) dic["卷积核大小"];
    ```

![](/Image/Task/DictionaryUsage.png)

3. ToolBlock编辑规则

+ 标定节点根据BitchSize自动在输入终端中添加输入图像接口，用户使用直接拉线即可。

+ 如果需要将某些数据传递到下一个节点(检测节点)，请以连线或者添加的方式，将数据/图像添加到输出终端中。

![](/Image/Task/OutputTerminal_Inspection.png)

### 2.3 检测节点

1. 缺陷定义

+ 在SPEC表中新建缺陷项

+ 当选择Range类型数据时，双击对应单元格内的值时，将会弹出一个新的界面，提示输入下限(Low)及上限(High)

+ 点击应用，将缺陷列表同步至ToolBlock

![](/Image/Task/TerminalParameter_Inspection.png)

2. 缺陷使用

+ 使用方法与参数基本相同。

+ 需要注意的是，Range类型保存在字典当中是以元组(Tuple<double,double>)的形式保存，Item1是下限，Item2是上限。

    ```csharp
    var spec1 = (Tuple<double,double>)dic["Name"];
    double low  = spec1.Item1;
    double high = spec1.Item2;
    ```

![](/Image/Task/DictionaryUsage_Range.png)

3. ToolBlock编辑规则

+ 前序节点(标定节点)的输出终端会自动添加到本节点(检测节点)的输入终端，包括参数列表也会同步。

+ 检测节点的输出终端预先设置了DetectionImages作为缺陷小图，用户可以根据自己的需求，选择性在脚本中向该List添加。

![](/Image/Task/OutputTerminal_Inspection.png)
