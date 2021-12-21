# MyEngin2

玩具终归只是玩具，不必深究。

## 文件目录

| 名称                       | 注释                   |
| -------------------------- | ---------------------- |
| Example                    | 配置文件模板           |
| MyEngine2.Common           | 公共组件               |
| MyEngine2.Test.Common      | 公共组件测试           |
| MyEngine2.Entrance.Console | 控制台入口             |
| TestScript                 | 部分 Python 测试用脚本 |

论功能比 MyEngine 也就多个长连接（其他工具可忽略不计），中间重写了相当一部分逻辑（不包括转译部分），插件暂时还没做。借 Dotnet 之手跨个平台，之前 MyEngine 的 Native 层懒得写了，主要是调试麻烦。