   M S B A M L   `   `   `  ���� H  DIP_calculator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:IP_calculatorIP_calculator      IP_calculator.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     -localclr-namespace:IP_calculator     -     � Title$  IP калькулятор��$	��600��$	��850�� � WindowStartupLocation$ CenterScreen=� � 
ResizeMode$ NoResize=� � WindowStyle$
 None=����#212226.���� .+�� $��0,0,0,20q�.+��� $��18(�$��Center=�$��0,10,0,0q�$��Center=�
�������.��IP калькулятор�� -    ��Name$ btnHide��   btnHide#��[  $��Right=�$��0,5,45,0q�$��40��$��40���� -   $ btnClose��  btnClose#��[ $��Right=�$��0,5,5,0q�$��40��$��40����  �� Orientation$ 
Horizontal=�.+��� $��	120,0,0,0q�.+��� 
�������$��14(�$��IP-адрес�� � ToolTip _� ���#9e53c9
�������.��75Через точку в формате "192.168.0.1"�� -    ��Name$ tb_IpAddress��$��0,10,0,0q�$	��200��$��30��
�������$��16(�$��Center=��� $��60,0,0,0q�.+��� 
�������$��14(�.��Маска подсети�� -    ��Name$ cb_SubnetMask��$��0,10,0,0q�$	��200��$��30��
�������$��16(�$��Center=��� -   $ btn_Calculate��$��
60,33,40,0q�$	��120��$��35��  btn_Calculate#��[ .��Расчитать�� -   	 ��Name$	 lbl_VerificationInfo��$��	116,0,0,0q�$��12(����#bb95f0$��25��� -   
 �Name$
 
gr_IpTable��$��0,40,0,0q�$	��700��$	��370��� .+��� -	   $ 	btn_About�����#9e53c9$��Center=�  	btn_About#��[ .��О программе