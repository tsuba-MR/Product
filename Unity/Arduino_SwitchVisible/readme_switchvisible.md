## Arduino_SwitchVisible
> バージョン<br>
Unity 2020.3.7f1<br>
> 使用ライブラリ, モジュールなど<br>
Arduino, System.IO.Ports<br>
> 注意点<br>
そのままunitypackageをインポートすると<br>
**The type or namespace name 'Ports' does not exist in the namespace 'System.IO'**<br>
とエラーが出る可能性があるため、<br>
**edit -> Project Settings -> Player -> Other Settings -> Configuration -> Api Compatibility Level**<br>
が、<br>
**.NET 4.x** <br>になっていることをご確認ください。<br>
## ファイル，ディレクトリの説明<br>
### Code
- Switch_Visible.ino<br>
実際にArduinoに書き込むファイルです。<br>
入力によって０か１をシリアル通信します。<br>
- SerialHandler.cs<br>
Arduinoのシリアル通信をUnityで受け取るために使うファイルです。<br>
以下の記事を参考にし、タイムアウト処理を追加して実行が時間経過で止まるようにしました。<br>
https://qiita.com/yjiro0403/items/54e9518b5624c0030531
- visible_switch.cs<br>
受け取った信号によってUnity内のオブジェクトの描画を切り替えます。

### UnityPackage
- Arduino_QuizButton.unitypackage
本製作の情報をまとめたunitypackageです。<br>
これのインポートと"Api Compatibility Level"の変更、Arduinoの準備をしていただくと動作可能です。
