# LittleTest_lemonjam

题目：
用Unity3D实现:场景某处有个3D Cube，有个2D Sprite绕着这个Cube以y轴为轴心圆周移动，屏幕右上角有一个按键,鼠标点击它后,2D Sprite在Y轴方向跳起,跳起之后会落下回到之前的y位置,跳起不影响它继续圆周运动。 3D Cube可以被拖动,拖动时会变半透,根据鼠标位置移动到相应的位置,y轴位置不变.

右上角的按键可以被拖动,可以拖动到任何屏幕中的位置(无法拖到屏幕外),松手时会根据之前的动作判断是否会有惯性滑动(模拟越真实越好). 当与3D Cube重叠时,优先响应Cube的拖动.

要求:
只能使用一个Camera
不使用任何第三方插件
不用UGUI与GUI实现
2D Sprite以0.1秒的间隔播放帧动画
帧动画请自行选择合适的素材图
