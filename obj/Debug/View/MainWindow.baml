   M S B A M L   `   `   `  ����E  AAutoschool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:Autoschool.ViewAutoschool.View  5
   	        Autoschool.View.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   _ [MaterialDesignThemes.Wpf, Version=4.9.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5	   	   /localclr-namespace:Autoschool.View     5
   	   -   5          �Name$  window��5   	   $	��480��5   	   $	��600��5   	    � Title$ 
MainWindow��5      $	��800��6   $
��1300��6&    � WindowStartupLocation$ Manual=�63    � Icon$ Logos/car1.jpg��6R   $��Calama)�6h   ��5      �� 5   
    j� TileMode$
 Tile=�6    �� ImageSource$# /View/Backgrounds/backgr1.png��6%    �� RelativeTransform 6Q   ��  �� ElementName$ window�� �� Mode$ OneWay=�.��5      (� .��5   
   � $	��391��6   $	��527��6   	 � RowDefinitions	 5      �� 5      6"   �� 5      6"   �� 5      6"   �� 5      6"   �� 5      6"   �� 5      6"   �� 5      6"   
 � ColumnDefinitions
 5      �� 5      6%   �� 5      6%   �� 5       6%   .+�5"      �� ���	#FFF4F4F5$��Center=�6)   $	��399��6F   
���   �$��Center=�6b   $	��395��6}   $��6��6�    J� Opacity$
 0.85Q�6�   $��1��6�   $��3��6�   $��	-4,1,0,52q�6�   �� 5#      $��Автошкола��6   $��Center=�6(   $��Center=�6L   $��
-4,36,0,35q�6i   $��1��6}   $��Center=�6�   $
��Auto��6�   $	��395��6�   $��60(�6�   $��2��6�   $��3��6�   ��6�   Z� �� 5$      $��Войти как��6   $��Center=�6(   $��Center=�6L   $��
-4,29,0,56q�6i   $��2��6}   $��Center=�6�   $
��Auto��6�   $	��395��6�   $��48(�6�   $��2��6�   $��3��6�   ��6�   Z� �� 5%      $��Студент��6   $��Center=�6'   $��Center=�6D   $	��255��6_   $��3��6k   $��
74,24,62,6q�6   $��3��6�   $��46��6�   $��20(�6�   �� 5&      $ ��Преподаватель��6   $��Center=�6-   $��Center=�6J   $	��255��6e   $��3��6q   $��74,10,62,19q�6�   $��4��6�   $��46��6�   $��20(�6�   �� 5'      $ ��Администратор��6   $��Center=�6-   $��Center=�6J   $	��255��6e   $��3��6q   $��
74,0,62,29q�6�   $��5��6�   $��46��6�   $��20(�6�   5)      5+      5,      