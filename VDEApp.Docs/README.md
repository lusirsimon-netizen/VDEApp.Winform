# VDE最小框架应用文档 / VDE Minimum Framework App Documentation


## 构建说明

### 使用UV


创建虚拟环境：

```sh
uv venv
```

构建页面：

- Linux
    ```sh
    make html
    ```
- Windows
    使用Powershell：
    ```powershell
    .\make.ps1 html
    ```

### 使用全局Python

下载依赖：

```sh
pip install -r requirements.txt
```

构建页面：

```sh
sphinx-build -M html source build
```


