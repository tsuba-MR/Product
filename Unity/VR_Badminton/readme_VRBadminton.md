## Arduino_QuizButton
> バージョン
Unity 2020.3.7f1
> 使用ライブラリ, モジュールなど
SteamVR, HTC Vive
> 注意点
※unitypackageを追加しただけではdllNotFoundErrorが出るため、これに加えアセットストアよりSteamVR Pluginをインポートしてください
> ファイル，ディレクトリの説明
### Code
- BallAccelator.cs
Viveコントローラーの速度を計測し、速度と当たった面に応じて返球します。
VelocityEstimatorコンポーネントを使ってコントローラー本体の速度と角速度の2つを取得し最終的なシャトルの速度を決定します。
球を飛ばす方向は接触時の法線ベクトルを取ることで決定しています。
- BallShoot_DL_copy.cs
シャトルの軌道計算を行うコードです。
ResistandLift関数により空気抵抗と揚力を計算し、シャトルのRigidbody.velocityに反映します。
千葉大と東北大の論文を参照し、飛距離が現実のものに近づいたか否かを判断しながらコードを作成しました。
- Button.cs
VR空間でデバッグするために作成したコードです。
トリガーを押してシャトルのスタート、グリップボタンを押してシーンのリセットを行います。

### UnityPackage
- Arduino_QuizButton.unitypackage
本製作の情報をまとめたunitypackageです。
これのインポートとSteamVR Pluginのインポートをしていただくと動作可能です。
