## Arduino_QuizButton
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
- Chatterディレクトリ
自作ライブラリです。チャタリング対策関数を含んだオブジェクトをボタンの数だけ生成するために作成しました。
- Button_Chattering.ino
実際にArduinoに書き込むファイルです。
ボタンの数だけChatterインスタンスを生成し、チャタリング対策を施したボタン入力をシリアル通信します。
- SerialHandler.cs
Arduinoのシリアル通信をUnityで受け取るために使うファイルです。
以下の記事を参考にし、タイムアウト処理を追加して実行が時間経過で止まるようにしました。
https://qiita.com/yjiro0403/items/54e9518b5624c0030531
- SerialReceive_Button.cs
押されたボタンによって対応するCubeが光るような処理をするファイルです。
押されたボタンによってマテリアルをEmissionのものに変更し子オブジェクトであるスポットライトを点灯させます。
クイズ番組であるような、回答者の席が光るようなシステムを目指し製作しました。

### UnityPackage
- Arduino_QuizButton.unitypackage
本製作の情報をまとめたunitypackageです。
これのインポートと"Api Compatibility Level"の変更、Arduinoの準備をしていただくと動作可能です。

### その他
- quiz_button.png
Arduinoの配線図を記録した画像ファイルです。