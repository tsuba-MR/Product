## Arduino_SwitchVisible
> バージョン
Unity 2020.3.7f1
> 使用ライブラリ, モジュールなど
Arduino, System.IO.Ports
> 注意点
そのままunitypackageをインポートすると
The type or namespace name 'Ports' does not exist in the namespace 'System.IO'（System.IO中にPortsがない）
とエラーが出る可能性があるため、
edit -> Project Settings -> Player -> Other Settings -> Configuration -> Api Compatibility Level
を、 .NET 4.x
になっていることをご確認ください。
> ファイル，ディレクトリの説明
### Code
- Switch_Visible.ino
実際にArduinoに書き込むファイルです。
入力によって０か１をシリアル通信します。
- SerialHandler.cs
Arduinoのシリアル通信をUnityで受け取るために使うファイルです。
以下の記事を参考にし、タイムアウト処理を追加して実行が時間経過で止まるようにしました。
https://qiita.com/yjiro0403/items/54e9518b5624c0030531
- visible_switch.cs
受け取った信号によってUnity内のオブジェクトの描画を切り替えます。

### UnityPackage
- Arduino_QuizButton.unitypackage
本製作の情報をまとめたunitypackageです。
これのインポートと"Api Compatibility Level"の変更、Arduinoの準備をしていただくと動作可能です。
