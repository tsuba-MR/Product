## Arduino_QuizButton
> バージョン<br>
Unity 2020.3.7f1<br>
> 使用ライブラリ, モジュールなど<br>
SteamVR, HTC Vive<br>
> 注意点<br>
※unitypackageを追加しただけではdllNotFoundErrorが出るため、これに加えアセットストアよりSteamVR Pluginをインポートしてください<br>
## ファイル，ディレクトリの説明
### Code
- BallAccelator.cs<br>
Viveコントローラーの速度を計測し、速度と当たった面に応じて返球します。<br>
VelocityEstimatorコンポーネントを使ってコントローラー本体の速度と角速度の2つを取得し最終的なシャトルの速度を決定します。<br>
球を飛ばす方向は接触時の法線ベクトルを取ることで決定しています。<br>
- BallShoot_DL_copy.cs<br>
シャトルの軌道計算を行うコードです。<br>
ResistandLift関数により空気抵抗と揚力を計算し、シャトルのRigidbody.velocityに反映します。<br>
千葉大と東北大の論文を参照し、飛距離が現実のものに近づいたか否かを判断しながらコードを作成しました。<br>
- Button.cs<br>
VR空間でデバッグするために作成したコードです。<br>
トリガーを押してシャトルのスタート、グリップボタンを押してシーンのリセットを行います。<br>

### UnityPackage
- Arduino_QuizButton.unitypackage<br>
本製作の情報をまとめたunitypackageです。<br>
これのインポートとSteamVR Pluginのインポートをしていただくと動作可能です。<br>
