## 2DGame
> バージョン<br>
Unity 2019.4.0f1<br>
## ファイル，ディレクトリの説明
### Code
自作した部分を含むコードのみ選択しました．
- PlayerController.cs
大小のジャンプを追加したプレイヤーの実装コードです．<br>
89行目でジャンプの初速度を設定し，スペースを押す長さに応じてジャンプ力を変更します．
- HomingController.cs
プレイヤーを追尾する敵の動きを実装したコードです．<br>
38行目でプレイヤーと自身の座標から方向ベクトルを作成し，指定したスピード分移動させます．
### Head_Stamp_build
- Head Stamp.exe
動作を見ていただくための作品です．
>操作方法
            A, D：キャラクターの左右の移動
           Space：キャラクターのジャンプ
### UnityPackage
- 2DGame.unitypackage
本製作の情報をまとめたunitypackageです．
