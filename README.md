# WPF-Elementary-ExToolKit
 WPF Extend Tool Kit

新的自定义控件库

	'RequestDll'文件夹下文件为实现GIFImage控件所需要的DLL


	此Toolkit仅适用于Elementary系列，Practise系列使用![这个](https://github.com/IPpaTsuEr/WPF-Practise-ToolLib)
	
## ExWindow 自定义WIndows类
	
	FunctionBar [object] 用于放置标题栏控件

## ExPopBar 适用于ExWindow的FunctionBar的控件
 继承自HeaderedItemsControl
	IsSubPoped   [bool] 是否弹出
	PopBackgroud [SolidColorBrush] 设置背景画刷
	Command       [ICommand] 命令
	CommandParameter [object] 命令参数
	CommandTarget   [IInputElement] 命令目标

## ExPopBarItem 作为ExPopBar的Item的Container
 继承自ContentControl
	Icon [object] 显示icon
	Command       [ICommand] 命令
	CommandParameter [object] 命令参数
	CommandTarget   [IInputElement] 命令目标

## ScrollPanel 用于实现类似于浏览器页面过多时出现的滚动页标签的容器
 继承自ItemsControl

## GIFImage GIF控件
 实现GIF文件的显示与播放 继承自Image
	GIFData [string] 指向gif文件的路径
	Rate    [double] 切换图像的速率，单位：毫秒
	Index   [int]    图像帧序列
	Play    [bool]   是否播放GIF动画
	
	以下属性仅供读取，设置不会起任何效果
	Source  用于获取当前显示的GIF帧
	
## ArcProgress 弧形进度条
 实现圆弧形的进度条 [方案源自互联网]
	Content    [object] 用于放置内容控件
	StartAngle [double] 起始角度
	EndAngle   [double] 终止角度
	StrokeThickness [double] 弧线宽度
	Center     [Point]  中心坐标
	Radius     [double] 圆弧半径
	
	以下属性仅供读取，设置不会起任何效果
	ArcSize    [size] 获取弧大小
	EndPoint   [point] 获取弧终止点
	StartPoint [point] 获取弧起始点
	LargeArc   [bool] 优弧或是劣弧
	
## Exbrush 动态笔刷
 实现类似windows10 设置界面按钮的边框动态效果 继承自MarkupExtension [方案源自互联网]
	HighLightColor [Color] 高光颜色，默认为白色
	NormalColor [Color] 非高光颜色，默认为透明
	GradientRadius [double] 笔刷高光半径，默认为80
	BrushOpacity [double] 笔刷透明度，默认值为0.8
	BrushTransform [Transform] 变换
	RelativeBrushTransform [Transform] 相对变换
	GloabalEffect [bool] 当鼠标移出后是否依旧响应鼠标移动事件来动态更新笔刷