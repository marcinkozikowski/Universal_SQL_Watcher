   M S B A M L   `   `   `  ����   Universal SQL Watecher'    !Universal_SQL_Watecher.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$  Universal SQL Watcher ��$	��700��$	��900�� � WindowStartupLocation$ CenterScreen=� � Icon$ Drawable/watcherIcon.png�� � 
ResizeMode$ CanMinimize=�.��� .+��� -    ��Name$ 
tabControl��$
��Left=�$	��671��$	��Top=�$	��894��.@��� $��Lista zapytań��$��14(�$	��200��$��0,0,0,-2q�$��30��
���   �.��� ���	#FFE5E5E5.+��� ���	#FFF4F4F5$
��Left=�$	��613��$��
615,10,0,0q�
���   �$	��Top=�$	��263���� -    ��Name$ listView��$
��Left=�$	��557��$��	10,66,0,0q�$	��Top=�$	��600�� �� View �� .P��� $��Nazwa Przypadku�� �� Width$	 590���� -    ��Name$ wybierzListeButton��$��Wybierz��$
��Left=�$��660,140,0,0q�$	��Top=�$	��180��$��60��	 � Cursor$
	 Hand���� -   $��Usuń��$
��Left=�$��660,230,0,0q�$	��Top=�$	��180��$��60��$
	 Hand���� ���	#FFF4F4F5$
��Left=�$��51��$��	10,10,0,0q�
���   �$	��Top=�$	��600���� $#��Lista zdefiniowanych zapytań��$��	10,10,0,0q�$��53��$	��600��$
��Left=�$	��Top=�$��Center=�$��Center=�$��18(���Z� �� -   $��Wyczyść liste��$
��Left=�$��660,320,0,0q�$	��Top=�$	��180��$��60��$
	 Hand���� -   $��Zapisz liste��$
��Left=�$��660,410,0,0q�$	��Top=�$	��180��$��60��$
	 Hand���� $
��Left=�$��60��$��633,140,0,0q�$	��Top=�$��60��$��Drawable/pick.png���� $
��Left=�$��60��$��633,230,0,0q�$	��Top=�$��60��$��Drawable/deleteicon.png���� $
��Left=�$��60��$��633,410,0,0q�$	��Top=�$��60��$��Drawable/save.png���� $
��Left=�$��60��$��633,320,0,0q�$	��Top=�$��60��$��Drawable/clear.png���� -   
 ��Name$
 tabZapytanie��$��Opcje zapytania��$��14(�$	��200��$��0,0,0,-2q�$��30��
���   �.��� ���	#FFE5E5E5.+��� ���	#FFF4F4F5$
��Left=�$	��122��$��
635,90,0,0q�
���   �$	��Top=�$	��222���� ���	#FFF4F4F5$
��Left=�$��54��$��
10,217,0,0q�
���   �$	��Top=�$	��868���� ���	#FFF4F4F5$
��Left=�$	��202��$��	10,10,0,0q�
���   �$	��Top=�$	��868���� -    ��Name$ 
loginLabel��$��Login��$
��Left=�$��
34,130,0,0q�$	��Top=� J� RenderTransformOrigin$ 0.568,1.862&��� -	   $ 	passLabel��$��Password��$
��Left=�$��320,130,0,0q�$	��Top=��� -
    ��Name$ 	loginText��$
��Left=�$��30��$��
82,130,0,0q� �� TextWrapping$
 Wrap=�$	��Top=�$	��200��$		 Pen���� -   $ passText��$
��Left=�$��30��$��393,130,0,0q�$
 Wrap=�$	��Top=�$	��200��$		 Pen���� -   $ sqlQuestionText��$
��Left=�$��30��$��
88,229,0,0q�$
 Wrap=�$	��Top=�$	��769��$L��Fselect "AgreementToCheckBIKDB","SalesIncomeTax2YearsBack" from "Loans"���� $	��SQL��$
��Left=�$��
34,230,0,0q�$	��Top=�$ 0.486,-0.414&��� ���	#FFF4F4F5$
��Left=�$	��271��$��
10,276,0,0q�
���   �$	��Top=�$	��868���� $��Server��$
��Left=�$��	34,38,0,0q�$	��Top=�$ -4.86,0.828&��� -   $ 
serverText��$
��Left=�$��30��$��	88,38,0,0q�$
 Wrap=�$	��Top=�$	��194��$		 Pen���� $
��Port��$
��Left=�$��
320,39,0,0q�$	��Top=��� -   $ portText��$
��Left=�$��30��$��
360,38,0,0q�$
 Wrap=�$	��Top=�$	��100��$		 Pen���� $��Database name��$
��Left=�$��
541,38,0,0q�$	��Top=�$��30���� -   $ databaseText��$
��Left=�$��30��$��
656,38,0,0q�$
 Wrap=�$	��Top=�$	��151��$		 Pen���� -    ��Name$ 
comboWybor��$
��Left=�$��656,130,0,0q�$	��Top=�$	��151��$��30��$
	 Hand���� $��Kolumna��$
��Left=�$��218,315,0,0q�$	��Top=�$��30���� $��Kolumna��$
��Left=�$��218,375,0,0q�$	��Top=�$ 1.186,2.776&�$��30���� $��
Wyrażenie��$
��Left=�$��578,315,0,0q�$	��Top=�$ 6.105,0.552&�$��30���� $��	Wartość��$
��Left=�$��578,375,0,0q�$	��Top=�$��30���� -   $ kolumnaIfText��$
��Left=�$��30��$��287,315,0,0q�$
 Wrap=�$	��Top=�$	��190�� � ToolTip$ " Bez apostrofów " ��$		 Pen���� -   $ kolumnaThenText��$
��Left=�$��30��$��287,375,0,0q�$
 Wrap=�$	��Top=�$	��190��$ " Bez apostrofów "��$		 Pen���� -   $ wyrazenieIfText��$
��Left=�$��30��$��665,315,0,0q�$
 Wrap=�$	��Top=�$	��172��$# Operator logiczny + wartość��$		 Pen���� -   $ wartoscThenText��$
��Left=�$��30��$��665,375,0,0q�$
 Wrap=�$	��Top=�$	��172��$ Sama wartość��$		 Pen���� $��where��$
��Left=�$��501,309,0,0q�$	��Top=�$��20(����	#FFF90000$ 0.125,0.604&��� $��=��$
��Left=�$��522,369,0,0q�$	��Top=�$��20(�
���  ��$ -0.194,0.514&��� $
��then��$
��Left=�$��134,369,0,0q�$	��Top=�$��20(����	#FFF90000�� -   $��
Rozpocznij��$
��Left=�$��747,460,0,0q�$	��Top=�$��90��$��50��$
	 Hand��$��Center=��� -    ��Name$ 
timeSlider��$
��Left=�$��
64,460,0,0q�$	��Top=�$��40��$	��588�� � Maximum$ 
1000000000Q� �� TickPlacement$ BottomRight=� �� TickFrequency$ 1000000Q� � SmallChange$ 1Q� �� AutoToolTipPlacement$ TopLeft=����	#FFBFBFBF�� -   $ timeText��$
��Left=�$��30��$��497,498,0,0q�$
 Wrap=�$��0��$	��Top=�$	��150��$		 Pen���� ���	#FFF4F4F5$
��Left=�$��71��$��
10,552,0,0q�
���   �$	��Top=�$	��868���� $��
Interwał:��$
��Left=�$��
15,426,0,0q�$	��Top=�$ -2.512,0.402&�$* $Co jaki czas należy sprawdzać baze���� -   $ secondsText��$
��Left=�$��30��$��134,500,0,0q�$
 Wrap=�$	��Top=�$	��100��$��0��$		 Pen���� $��Ticks:��$
��Left=�$��449,498,0,0q�$	��Top=�$��30���� $��Sekundy:��$
��Left=�$��
64,500,0,0q�$	��Top=�$��30���� -   $ nazwaPrzypadkuText��$
��Left=�$��30��$��149,579,0,0q�$
 Wrap=�$	��Top=�$	��250��$		 Pen���� $��Nazwa przypadku: ��$
��Left=�$��
22,579,0,0q�$	��Top=�$��30���� -   $ DodajPrzypadek��$��Dodaj przypadek do listy��$
��Left=�$��667,562,0,0q�$	��Top=�$	��170��$��50��$
	 Hand���� $
��Left=�$��50��$��621,562,0,0q�$	��Top=�$��50��$��Drawable/add.png���� $
��Left=�$��705,460,0,0q�$	��Top=�$��Drawable/start1.png��$��50��$��50��$
��Fill=��� $��Aktualne wątki��$
��Left=�$��30��$	��Top=�$	��200��$��14(�
���   �.��� ���	#FFE5E5E5.+��� ���	#FFF4F4F5$
��Left=�$	��613��$��
645,10,0,0q�
���   �$	��Top=�$	��233���� -   $ listaWatkowView��$
��Left=�$	��613��$��	10,10,0,0q�$	��Top=�$	��630�� �� .P��� $��Nazwa Wątku��$	 440�� �� DisplayMemberBinding �� *
nazwaWatku+�� $��Czy działa?��$	 200�� �� *dziala+�� -   $ zatrzymajWatekButton��$��	Zatrzymaj��$
��Left=�$��
685,70,0,0q�$	��Top=�$	��180��$��60��$
	 Hand���� -   $ wznowButton��$��Wznów��$
��Left=�$��685,245,0,0q�$	��Top=�$	��180��$��60��$
	 Hand���� -   $ usunWatekButton��$��Usuń��$
��Left=�$��685,420,0,0q�$	��Top=�$	��180��$��60��$
	 Hand���� $
��Left=�$��60��$��
655,73,0,0q�$	��Top=�$��60��$��Drawable/stopThread.png���� $
��Left=�$��60��$��655,421,0,0q�$	��Top=�$��60��$��Drawable/deleteicon.png���� $
��Left=�$��60��$��655,245,0,0q�$	��Top=�$��60��$
��Fill=�$��Drawable/start.png���� -   $ stopAllButton��$��Zatrzymaj wszystkie��$
��Left=�$��685,160,0,0q�$	��Top=�$	��180��$��60��$
	 Hand���� $
��Left=�$��60��$��655,160,0,0q�$	��Top=�$��60��$��Drawable/stopAll.png���� -    $ wznowWszystkieButton��$��Wznów wszystkie��$
��Left=�$��685,330,0,0q�$	��Top=�$	��180��$��60��$
	 Hand���� $
��Left=�$��60��$��655,330,0,0q�$	��Top=�$��60��$��Drawable/startAll.png���� $��Tabela Watku cache ��$
��Left=�$��30��$	��Top=�$	��120��$��14(�
���   �.��� ���	#FFE5E5E5.+��� -!   $ listViewCache��$
��Left=�$	��533��$��	40,26,0,0q�$	��Top=�$	��534�� �� .P��� �� -"   $��Odswiez��$
��Left=�$��
738,90,0,0q�$	��Top=�$��75��